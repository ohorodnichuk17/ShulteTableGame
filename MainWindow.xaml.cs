using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HomeWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int max_lose_time= 250;
        int lose_time = 250;
        int seconds = 0 ;
        int prev=0;
        public Difficulty Diff { get; set; } = new Difficulty { Diff = 1 };
        DispatcherTimer timer = new DispatcherTimer();
  
       
        bool isStarted = false;
        List<int> existingElems = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            this.DataContext = this;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lose_time = max_lose_time / (int)Diff.Diff;
            if (seconds == lose_time)
            {
                StopGame();
                MessageBox.Show("You lose");
            }
            secLabel.Content = $"{++seconds} / {lose_time}";
        }

        private Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }
        private void StopGame()
        {
            isStarted = false;
            existingElems.Clear();
            prev = 0;
            timer.Stop();
            secLabel.Content = "0";
            seconds = 0;
            gameProgress.Value = 0;
            StateButton.Content = "Start";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isStarted == true)
            {
                StopGame();
                return;
            }
            isStarted = true;
            StateButton.Content = "Stop";
            foreach (UIElement ui in elemsGrid.Children)
            {
                if ((ui as Button) != null)
                {
                    Random rnd = new Random();
                    int num  = rnd.Next(1, 25); ;
                    while (existingElems.Contains(num))
                    {
                        num = rnd.Next(1, 25); 
                    }
                    Button btn = ui as Button;
                    btn.Content = num;
                    
                    Brush br = PickBrush();
                    if (br == Brushes.Transparent || br == Brushes.Black)
                    {
                        br = Brushes.Red;
                    }
                    btn.Background =  br;
                    existingElems.Add(num);
                }
            }
            timer.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (isStarted)
            {
                Button btn = (sender as Button);
                int number = Int32.Parse((sender as Button).Content.ToString());
                if (number == prev + 1)
                {
                    ++prev;
                    ColorAnimation ca = new ColorAnimation(Colors.Red, Colors.LightGreen, new Duration(TimeSpan.FromSeconds(0.5)));
                    Storyboard.SetTarget(ca, btn);
                    Storyboard.SetTargetProperty(ca, new PropertyPath("Background.Color"));
                    gameProgress.Value += 1;
                    Storyboard stb = new Storyboard();
                    stb.Children.Add(ca);
                    stb.Begin();
                    if (number == 24)
                    {
                        MessageBox.Show("You won");
                        StopGame();
                    }
                }
               
                else
                {
                    
                    TranslateTransform trans = new TranslateTransform();
                    btn.RenderTransform = trans;
                    DoubleAnimation animX = new DoubleAnimation(0, 20, TimeSpan.FromSeconds(0.5));
                    trans.BeginAnimation(TranslateTransform.XProperty, animX);
                    animX = new DoubleAnimation(20, 0, TimeSpan.FromSeconds(0.5));
                    trans.BeginAnimation(TranslateTransform.XProperty, animX);
                }
            }
            
            
        }

        private void difSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider s = sender as Slider;
            if (s != null)
            {
                s.Value = Math.Round(s.Value);
            }
        }
    }
}
