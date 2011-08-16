using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace LiveSynergy.LocalDB
{
    /// <summary>
    /// flags
    /// </summary>
    [Flags]
    public enum CacheAction
    {
        None = 0,
        Insert = 1,
        Delete = 2,
        Update = 4,
        Sync = 8
    }


    public class StringToTypeConverter : TypeConverter
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType.IsAssignableFrom(typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var typeName = value as string;
            if (String.IsNullOrEmpty(typeName))
                return null;
            var type = Type.GetType(typeName);
            return type;
        }

    }

    public class EntityConfiguration : IEntityConfiguration
    {
        [TypeConverter(typeof(StringToTypeConverter))]
        public Type EntityType { get; set; }
    }

    /// <summary>
    /// Interfaces
    /// </summary>
    public interface IInitializeAndCleanup
    {
        void Initialize();
        void Cleanup();
    }
    public interface IStorageProvider : IInitializeAndCleanup
    {
        List<IEntityConfiguration> StorageConfigurations { get; }

        IEnumerable<T> Select<T>() where T : class;

        void Insert<T>(T item) where T : class;

        void Delete<T>(T item) where T : class;

        void Update<T>(T item) where T : class;
    }
    public interface IEntityConfiguration
    {
        Type EntityType { get; set; }
    }
    public interface IEntityBase : INotifyPropertyChanged
    {
        bool Deleted { get; set; }
        DateTime LastUpdated { get; set; }
        bool IsChanged { get; set; }

        void UpdateFrom(IEntityBase newEntity);
    }
    public interface ISyncProvider : IInitializeAndCleanup
    {
        List<IEntityConfiguration> SyncConfigurations { get; }

        void RetrieveChangesFromServer<T>(DateTime lastUpdated, Action<T[]> ChangedEntitiesFromServer) where T : class, IEntityBase;

        void SaveChangesToServer<T>(KeyValuePair<T, CacheAction>[] EntitiesToSave, Action<KeyValuePair<T, CacheAction>[]> SaveChangesToServerCompleted) where T : class, IEntityBase;
    }


    
    public class TypeStorageContext<T> : IInitializeAndCleanup
    {
        private string FileName { get; set; }
        private IsolatedStorageFileStream FileStream { get; set; }
        private DataContractJsonSerializer Serializer { get; set; }

        public TypeStorageContext(string fileName)
        {
            FileName = fileName;
        }

        public void Initialize()
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            Serializer = new DataContractJsonSerializer(typeof(T));
            FileStream = isf.OpenFile(FileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
        }

        public void Cleanup()
        {
            FileStream.Close();
        }

        private void Write<T>(T item)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.WriteObject(ms, item);
                ms.Flush();
                var bytes = ms.ToArray();
                var lengthBytes = BitConverter.GetBytes(bytes.Length);
                FileStream.Write(lengthBytes, 0, 4);
                FileStream.Write(bytes, 0, bytes.Length);
            }
            FileStream.Flush();
        }

        private T Read<T>() where T : class
        {
            var lenghtBytes = new byte[4];
            FileStream.Read(lenghtBytes, 0, 4);
            var bytes = new byte[BitConverter.ToInt32(lenghtBytes, 0)];
            FileStream.Read(bytes, 0, bytes.Length);
            using (var ms = new MemoryStream(bytes))
            {
                var instance = Serializer.ReadObject(ms) as T;
                return instance;
            }
        }

        public IEnumerable<T> Select<T>() where T : class
        {
            while (FileStream.Position < FileStream.Length)
            {
                var instance = Read<T>();
                yield return instance;
            }
        }

        public void Insert<T>(T item) where T : class
        {
            var entity = item as SimpleEntityBase;
            entity.EntityId = Guid.NewGuid();

            FileStream.Seek(0, System.IO.SeekOrigin.End);
            Write(item);

#if DEBUG
            FileStream.Seek(0, System.IO.SeekOrigin.Begin);
            var reader = new StreamReader(FileStream);
            var txt = reader.ReadToEnd();
            Debug.WriteLine(txt);
#endif
        }

        [DataContract]
        public class SimpleEntityBase : IEntityBase
        {
            public Guid EntityId { get; set; }

            [DataMember]
            public bool Deleted { get; set; }
            [DataMember]
            public DateTime LastUpdated { get; set; }
            public bool IsChanged { get; set; }

            public virtual void UpdateFrom(IEntityBase newEntity) { }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                if (propertyName != "IsChanged")
                {
                    IsChanged = true;
                }
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        public void Delete<T>(T item) where T : class
        {
            var entity = item as SimpleEntityBase;
            var searchId = entity.EntityId;
            FileStream.Seek(0, System.IO.SeekOrigin.Begin);
            var lastEntityStartIndex = FileStream.Position;
            while (FileStream.Position < FileStream.Length)
            {
                var nextEntity = Read<T>() as SimpleEntityBase;
                if (nextEntity.EntityId == searchId)
                {
                    var buffer = new byte[1000];
                    int count;
                    var entitySize = FileStream.Position - lastEntityStartIndex;
                    while ((count = FileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        FileStream.Seek(-(entitySize + count), System.IO.SeekOrigin.Current);
                        FileStream.Write(buffer, 0, count);
                        FileStream.Seek(entitySize + count, System.IO.SeekOrigin.Current);
                    }
                    FileStream.SetLength(FileStream.Length - entitySize);
                }
                lastEntityStartIndex = FileStream.Position;
            }
        }

        public void Update<T>(T item) where T : class
        {
            Delete(item);

            FileStream.Seek(0, System.IO.SeekOrigin.End);
            Write(item);
        }
    }



    public class SimpleStorageProvider : IStorageProvider
    {

        public List<IEntityConfiguration> StorageConfigurations { get; private set; }
        public string StoreName { get; set; }

        public SimpleStorageProvider()
        {
            StorageConfigurations = new List<IEntityConfiguration>();
        }

        public Dictionary<Type, IInitializeAndCleanup> Contexts { get; set; }

        public void Initialize()
        {
            IsolatedStorageFile isf =
                          IsolatedStorageFile.GetUserStoreForApplication();
            // Create folder for the store           
            isf.CreateDirectory(StoreName);



            Contexts = new Dictionary<Type, IInitializeAndCleanup>();
            foreach (var config in StorageConfigurations)
            {
                string fileName = String.Format(@"{0}\{1}", StoreName, config.EntityType.Name);

                var storeType = typeof(TypeStorageContext<>).MakeGenericType(config.EntityType);
                var store = Activator.CreateInstance(storeType, fileName) as IInitializeAndCleanup;
                store.Initialize();
                Contexts[config.EntityType] = store;
            }
        }

        public void Cleanup()
        {
            foreach (var config in StorageConfigurations)
            {
                (Contexts[config.EntityType] as IInitializeAndCleanup).Cleanup();
            }
        }

        private TypeStorageContext<T> CurrentStore<T>()
        {
            return Contexts[typeof(T)] as TypeStorageContext<T>;
        }

        public IEnumerable<T> Select<T>() where T : class
        {
            var store = CurrentStore<T>();
            return store.Select<T>();
        }

        public void Insert<T>(T item) where T : class
        {
            var store = CurrentStore<T>();
            store.Insert(item);
        }

        public void Delete<T>(T item) where T : class
        {
            var store = CurrentStore<T>();
            store.Delete(item);
        }

        public void Update<T>(T item) where T : class
        {
            var store = CurrentStore<T>();
            store.Update(item);
        }
    }

    public interface IObjectCache : IInitializeAndCleanup
    {
        List<IObjectKeyMap> ObjectKeyMappings { get; }

        void Insert<T>(T item) where T : class, IEntityBase;

        IEnumerable<T> Select<T>() where T : class, IEntityBase;

        void Delete<T>(T item) where T : class, IEntityBase;

        bool Exists<T>(T item) where T : class, IEntityBase;

        IEnumerable<T> FindByKey<T>(object key) where T : class, IEntityBase;


        #region Persistent Storage
        IStorageProvider Store { get; set; }
        CacheAction CacheMode { get; set; }
        void Persist();
        #endregion

        #region Synchronization
        ISyncProvider SyncService { get; set; }
        void Sync<T>() where T : class, IEntityBase;
        #endregion
    }

    public interface IObjectKeyMap
    {
        Type EntityType { get; set; }
        string KeyPropertyName { get; set; }
    }

    public class ObjectKeyMap : IObjectKeyMap
    {
        [TypeConverter(typeof(StringToTypeConverter))]
        public Type EntityType { get; set; }
        public string KeyPropertyName { get; set; }
    }

    public class ObjectCache : IObjectCache
    {
        public List<IObjectKeyMap> ObjectKeyMappings { get; private set; }
        public ObjectCache()
        {
            ObjectKeyMappings = new List<IObjectKeyMap>();
        }

        private Dictionary<Type, PropertyInfo> EntityKeyPropertyCache { get; set; }
        private Dictionary<Type, Func<object, object>> EntityKeyFunctions { get; set; }
        private Dictionary<Type, List<IEntityBase>> InMemoryCache { get; set; }


        public IStorageProvider Store { get; set; }
        public CacheAction CacheMode { get; set; }
        private Dictionary<Type, bool> EntitiesLoaded { get; set; }
        private List<KeyValuePair<IEntityBase, CacheAction>> History { get; set; }

        public ISyncProvider SyncService { get; set; }
        private Dictionary<Type, DateTime> LastSynced { get; set; }

        public void Initialize()
        {
            if (Store != null)
            {
                Store.Initialize();
            }

            if (SyncService != null)
            {
                SyncService.Initialize();
            }


            History = new List<KeyValuePair<IEntityBase, CacheAction>>();
            InMemoryCache = new Dictionary<Type, List<IEntityBase>>();
            EntitiesLoaded = new Dictionary<Type, bool>();
            EntityKeyFunctions = new Dictionary<Type, Func<object, object>>();
            EntityKeyPropertyCache = new Dictionary<Type, PropertyInfo>();
            LastSynced = new Dictionary<Type, DateTime>();

            foreach (var registeredType in ObjectKeyMappings)
            {
                var entityType = registeredType.EntityType;
                var cache = new List<IEntityBase>();
                InMemoryCache[entityType] = cache;
                EntitiesLoaded[entityType] = false;
                LastSynced[entityType] = DateTime.MinValue;
                EntityKeyPropertyCache[entityType] = entityType.GetProperty(registeredType.KeyPropertyName);
                EntityKeyFunctions[entityType] = (entity) => EntityKeyPropertyCache[entity.GetType()].GetValue(entity, null);
            }
        }

        public void Cleanup()
        {
            if (SyncService != null)
            {
                SyncService.Cleanup();
            }

            if (Store != null)
            {
                Store.Cleanup();
            }
        }

        private List<IEntityBase> EntityCache<T>() where T : class, IEntityBase
        {
            var entityType = typeof(T);
            List<IEntityBase> cache = InMemoryCache[entityType];

            if (!EntitiesLoaded[typeof(T)])
            {
                var lastUpdated = DateTime.MinValue;
                foreach (var entity in Store.Select<T>().OfType<IEntityBase>())
                {
                    cache.Add(entity);
                    lastUpdated = (entity.LastUpdated > lastUpdated ? entity.LastUpdated : lastUpdated);
                }
                LastSynced[typeof(T)] = lastUpdated;
                EntitiesLoaded[typeof(T)] = true;
            }

            return cache;
        }

        public IEnumerable<T> Select<T>() where T : class, IEntityBase
        {
            var cacheList = EntityCache<T>();
            return cacheList.AsReadOnly().OfType<T>();
        }

        private Func<object, object> KeyFunction<T>()
        {
            Func<object, object> keyFunction;
            if (!EntityKeyFunctions.TryGetValue(typeof(T), out keyFunction))
            {
                keyFunction = (object keyItem) => (object)keyItem;
            }
            return keyFunction;
        }


        public IEnumerable<T> FindByKey<T>(object key) where T : class, IEntityBase
        {
            var keyFunction = KeyFunction<T>();
            var matches = from x in Select<T>()
                          where keyFunction(x).Equals(key)
                          select x;
            return matches;
        }

        private IEnumerable<T> FindMatches<T>(T item) where T : class, IEntityBase
        {
            var keyFunction = KeyFunction<T>();
            var key = keyFunction(item);
            return FindByKey<T>(key);
        }

        public bool Exists<T>(T item) where T : class, IEntityBase
        {
            var matches = FindMatches(item).FirstOrDefault();
            return matches != null;
        }

        public void Insert<T>(T item) where T : class, IEntityBase
        {
            if (Exists(item))
            {
                throw new EntityExistsException();
            }
            EntityCache<T>().Add(item);
            if (ShouldPersist(CacheAction.Insert))
            {
                Store.Insert(item);
            }
            else
            {
                History.Add(new KeyValuePair<IEntityBase, CacheAction>(item, CacheAction.Insert));
            }
            item.PropertyChanged += item_PropertyChanged<T>;
        }

        public void Delete<T>(T item) where T : class, IEntityBase
        {
            EntityCache<T>().Remove(item);
            item.Deleted = true;
            if (ShouldPersist(CacheAction.Delete))
            {
                Store.Update(item);
            }
            else
            {
                History.Add(new KeyValuePair<IEntityBase, CacheAction>(item, CacheAction.Delete));
            }
            item.PropertyChanged -= item_PropertyChanged<T>;
        }

        private bool ShouldPersist(CacheAction mode)
        {
            return (this.CacheMode & mode) > 0;
        }

        void item_PropertyChanged<T>(object sender, PropertyChangedEventArgs e) where T : class, IEntityBase
        {
            T senderObject = sender as T;
            if (ShouldPersist(CacheAction.Update))
            {
                Store.Update(senderObject);
            }
            else
            {
                History.Add(new KeyValuePair<IEntityBase, CacheAction>(sender as IEntityBase, CacheAction.Update));
            }
        }
        public void Persist()
        {
            while (History.Count > 0)
            {
                var action = History[0];
                switch (action.Value)
                {
                    case CacheAction.Insert:
                        Store.Insert(action.Key);
                        break;
                    case CacheAction.Delete:
                    case CacheAction.Update:
                        Store.Update(action.Key);
                        break;

                }
                History.RemoveAt(0);
            }

        }



        public void Sync<T>() where T : class, IEntityBase
        {
            // Touch the entity cache to make sure all entities are loaded from the persistent storage
            var cache = EntityCache<T>();

            DateTime lastupdated;
            if (!LastSynced.TryGetValue(typeof(T), out lastupdated)) return;
            SyncService.RetrieveChangesFromServer<T>(lastupdated, ChangedEntitiesFromServer);
        }

        private void ChangedEntitiesFromServer<T>(T[] entities) where T : class, IEntityBase
        {
            DateTime lastupdated = LastSynced[typeof(T)];

            foreach (var entity in entities)
            {
                lastupdated = (entity.LastUpdated > lastupdated) ? entity.LastUpdated : lastupdated;
                var existing = this.FindMatches(entity).FirstOrDefault();
                if (existing != null)
                {
                    if (!entity.Deleted)
                    {
                        existing.UpdateFrom(entity);
                    }
                    else
                    {
                        this.Delete(existing);
                    }
                    existing.IsChanged = false;
                }
                else if (!entity.Deleted)
                {
                    entity.IsChanged = false;
                    this.Insert(entity);
                }
            }

            this.Persist();
            LastSynced[typeof(T)] = lastupdated;

            var entitiesToSave = (from entity in Select<T>()
                                  where entity.IsChanged == true
                                  select new KeyValuePair<T, CacheAction>(
                                      entity,
                                      (entity.LastUpdated <= DateTime.MinValue ? CacheAction.Insert : (entity.Deleted ? CacheAction.Delete : CacheAction.Update))
                                      )).ToArray();
            SyncService.SaveChangesToServer<T>(entitiesToSave, ChangesSaved);

        }

        private void ChangesSaved<T>(KeyValuePair<T, CacheAction>[] savedEntities) where T : class, IEntityBase
        {
            var lastUpdated = LastSynced[typeof(T)];
            foreach (var entity in savedEntities)
            {
                if (lastUpdated < entity.Key.LastUpdated)
                {
                    lastUpdated = entity.Key.LastUpdated;
                }
            }
            LastSynced[typeof(T)] = lastUpdated;
            this.Persist();
        }
    }

    class EntityExistsException : Exception
    {
    }

    [DataContract]
    public class SimpleEntityBase : IEntityBase
    {
        public Guid EntityId { get; set; }

        [DataMember]
        public bool Deleted { get; set; }
        [DataMember]
        public DateTime LastUpdated { get; set; }
        public bool IsChanged { get; set; }

        public virtual void UpdateFrom(IEntityBase newEntity) { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (propertyName != "IsChanged")
            {
                IsChanged = true;
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


}
