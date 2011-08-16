using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using LiveSynergy;
using LiveSynergy.Data;

namespace LiveSynergy.Pages
{
    public partial class ViewDevicePage : PhoneApplicationPage
    {

        private Device ThisDevice;
        DateTime startTime;
        public ViewDevicePage()
        {
            InitializeComponent();
        }

        // timeSpan, counted in minutes, 24*60 represents 1 day
        private void InitializeLabel(int timeSpan)
        {
            int MaxEnergy = ThisDevice.EnergyConsumption.Max();
            /*
             * ylabel
                <TextBlock Text="50" Canvas.Left="-30" Canvas.Top="-80"/>
                <TextBlock Text="100" Canvas.Left="-30" Canvas.Top="-160"/>
                <TextBlock Text="150" Canvas.Left="-30" Canvas.Top="-240"/>
                <TextBlock Text="200" Canvas.Left="-30" Canvas.Top="-320"/>
             */
            int yDelta = 0, ylabelNum = 5;
            for (int i = 0; i < ylabelNum; i++)
            {
                yDelta += MaxEnergy / ylabelNum;
                TextBlock yLabelPoint = new TextBlock();
                yLabelPoint.Text = yDelta.ToString();
                ylabel.Children.Add(yLabelPoint);
                Canvas.SetLeft(yLabelPoint, -35);
                Canvas.SetTop(yLabelPoint, -yDelta * 280 / MaxEnergy);
            }
            
            
            /*  <TextBlock  Text="kW" Canvas.Left="-20" Canvas.Top="-330"/> */
            TextBlock ylabelName = new TextBlock();
            ylabelName.Text = "kW";
            ylabel.Children.Add(ylabelName);
            Canvas.SetLeft(ylabelName, -20);
            Canvas.SetTop(ylabelName, -330);

            
            /*  <TextBlock x:Name="xlabelName" Canvas.Left="360" Canvas.Top="0"/>   */
            TextBlock xlabelName = new TextBlock();
            xlabel.Children.Add(xlabelName);
            Canvas.SetLeft(xlabelName, 360);
            Canvas.SetTop(xlabelName, 0);
            /*
             * xlabel
                <TextBlock Text="0" Canvas.Left="-10" Canvas.Top="0"/>
                <TextBlock Text="5" Canvas.Left="45" Canvas.Top="0"/>
                <TextBlock Text="10" Canvas.Left="95" Canvas.Top="0"/>
                <TextBlock Text="15" Canvas.Left="145" Canvas.Top="0"/>
                <TextBlock Text="20" Canvas.Left="195" Canvas.Top="0"/>
                <TextBlock Text="25" Canvas.Left="245" Canvas.Top="0"/>
                <TextBlock Text="30" Canvas.Left="295" Canvas.Top="0"/>
            */
            
            if (timeSpan == 60)
            {
                int xDelta = 0, xlabelNum = 6, xlabelTotalTime = 60;
                for (int i = 0; i < xlabelNum; i++)
                {
                    xDelta += 50;
                    TextBlock xLabelPoint = new TextBlock();
                    xLabelPoint.Text = (xDelta / 50 * xlabelTotalTime / xlabelNum).ToString();
                    xlabel.Children.Add(xLabelPoint);
                    Canvas.SetLeft(xLabelPoint, xDelta - 5);
                    Canvas.SetTop(xLabelPoint, 0);
                }
                xlabelName.Text = "min";
            }
            else
            {
                int xDelta = 0, xlabelNum = 6, xlabelTotalTime = 24;
                for (int i = 0; i < xlabelNum; i++)
                {
                    xDelta += 50;
                    TextBlock xLabelPoint = new TextBlock();
                    xLabelPoint.Text = (xDelta / 50 * xlabelTotalTime / xlabelNum).ToString();
                    xlabel.Children.Add(xLabelPoint);
                    Canvas.SetLeft(xLabelPoint, xDelta - 5);
                    Canvas.SetTop(xLabelPoint, 0);
                }
                xlabelName.Text = "hour";
            }
        }

        private void InitializeViewEnergy(int timeSpan)
        {
            /*<Path x:Name="DataPlot"  Stroke="Red" StrokeThickness="3"/>*/
            Path DataPlot = new Path();
            DataPlot.StrokeThickness = 3;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
            DataPlot.Stroke = mySolidColorBrush;
            EnergyCanvas.Children.Add(DataPlot);

            int MaxEnergy = ThisDevice.EnergyConsumption.Max();

            PathFigure pathFigure = new PathFigure();
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = new PathFigureCollection();

            mySolidColorBrush.Color = Color.FromArgb(255, 204, 204, 255);

            int xDelta = 340;
            int EnergySampleData;
            
            for (int i = 0, j=ThisDevice.EnergyConsumption.Count-1; i < 6 ; i++, j = j - timeSpan/6)
            {
                xDelta -= 50;
                EnergySampleData = ThisDevice.EnergyConsumption[j];
                if (i == 0)
                {
                    pathFigure.StartPoint = new Point(xDelta, EnergySampleData * 280 / MaxEnergy);
                }   
                else
                {
                    LineSegment lineSegment = new LineSegment();
                    lineSegment.Point = new Point(xDelta, EnergySampleData * 280 / MaxEnergy);
                    pathFigure.Segments.Add(lineSegment);
                }

                /*
                 *  <Ellipse Width="20" Height="20" Fill="Red" Canvas.Left="40" Canvas.Top="90"/>
					<Ellipse Width="20" Height="20" Fill="Red" Canvas.Left="90" Canvas.Top="190"/>
					<Ellipse Width="20" Height="20" Fill="Red" Canvas.Left="140" Canvas.Top="340"/>
					<Ellipse Width="20" Height="20" Fill="Red" Canvas.Left="190" Canvas.Top="330"/>
					<Ellipse Width="20" Height="20" Fill="Red" Canvas.Left="240" Canvas.Top="290"/>
					<Ellipse Width="20" Height="20" Fill="Red" Canvas.Left="290" Canvas.Top="150"/>
					<Ellipse Width="20" Height="20" Fill="Red" Canvas.Left="340" Canvas.Top="110"/>
                */
                Ellipse thePoint = new Ellipse();
                Rectangle thePointForTouch = new Rectangle();
                
                thePoint.Width = 20; thePoint.Height = 20;
                thePointForTouch.Width = 50;
                thePointForTouch.Height = 50;
                thePointForTouch.Fill = mySolidColorBrush;
                thePointForTouch.Opacity = 0;
                thePointForTouch.IsHitTestVisible = true;
                
                thePoint.Fill = mySolidColorBrush;
                
                thePointForTouch.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(thePoint_ManipulationStarted);


                EnergyCanvas.Children.Add(thePointForTouch);
                Canvas.SetLeft(thePointForTouch, xDelta - thePointForTouch.Width / 2);
                Canvas.SetTop(thePointForTouch, (EnergySampleData - thePointForTouch.Height / 2) * 280 / MaxEnergy);


                EnergyCanvas.Children.Add(thePoint);
                Canvas.SetLeft(thePoint, xDelta - thePoint.Width / 2);
                Canvas.SetTop(thePoint, (EnergySampleData - thePoint.Height / 2) * 280 / MaxEnergy);

            }
            pathGeometry.Figures.Add(pathFigure);
            DataPlot.Data = pathGeometry;
        }


        void thePoint_ManipulationStarted(object sender, ManipulationStartedEventArgs args)
        {
            Rectangle thePointForTouch = sender as Rectangle;
            thePointForTouch.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(thePoint_ManipulationCompleted);
        }

        void thePoint_ManipulationCompleted(object sender, ManipulationCompletedEventArgs args)
        {
            int MaxEnergy = ThisDevice.EnergyConsumption.Max();
            Rectangle thePointForTouch = sender as Rectangle;
            thePointForTouch.ManipulationCompleted -= new EventHandler<ManipulationCompletedEventArgs>(thePoint_ManipulationCompleted);
            MessageBox.Show((Canvas.GetLeft(thePointForTouch) + thePointForTouch.Width / 2).ToString() + "," + (Canvas.GetTop(thePointForTouch) * MaxEnergy / 360 + thePointForTouch.Height / 2).ToString());
        }

        void ExcuteCommand(object sender, EventArgs args)
        {
            if (commandList.SelectedIndex == -1)
                return;

            MessageBoxResult mCommandExcute = MessageBox.Show("the command: " + commandList.SelectedItem.ToString() + " will be excuted", "Command Excute", MessageBoxButton.OKCancel);
            if (mCommandExcute == MessageBoxResult.OK)
            {
                if (commandList.SelectedItem.ToString() == "view energy consumption")
                {
                    commandList.SelectedIndex = -1;
                    devicePivot.SelectedIndex = 1;
                }
                else
                {
                    // http post to Jeff to excute that command
                    MessageBox.Show("Excute Commands: " + commandList.SelectedItem.ToString());
                    commandList.SelectedIndex = -1;
                }
            }
            else
            {
                commandList.SelectedIndex = -1;
            }
        }

        void CreateTrigger(object sender, EventArgs args)
        {
            if (eventList.SelectedIndex == -1)
                return;
        }

        void NavigateToChildPage(object sender, EventArgs args)
        {

        }
        

        private void OnClickViewHour(object sender, RoutedEventArgs e)
        {
            xlabel.Children.Clear();
            ylabel.Children.Clear();
            EnergyCanvas.Children.Clear();
            InitializeLabel(60);
            InitializeViewEnergy(60);
        }
        private void OnClickViewDay(object sender, RoutedEventArgs e)
        {
            xlabel.Children.Clear();
            ylabel.Children.Clear();
            EnergyCanvas.Children.Clear();
            InitializeLabel(60 * 24);
            InitializeViewEnergy(60 * 24);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // i need a loading page here
            // the information below needs updating every time

            base.OnNavigatedTo(e);
            string index;
            if (NavigationContext.QueryString.TryGetValue("Index", out index))
            {
                ThisDevice = App.ViewModel.AllTheDevice[Int32.Parse(index)];
                devicePivot.Title = ThisDevice.DeviceName.ToUpper();
                devicePivot.DataContext = ThisDevice;
                
                
                Random _rand = new Random(1000);
                ThisDevice.EnergyConsumption = new List<int>();
                for (int i = 0; i < 24 * 60; i++)
                {
                    ThisDevice.EnergyConsumption.Add(_rand.Next(0, 500));
                }

                InitializeLabel(60);
                InitializeViewEnergy(60);
                startTime = DateTime.Now;
                lasthour.IsChecked = true;
            }
        }

    }
}