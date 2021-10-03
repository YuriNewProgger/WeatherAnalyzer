using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WeatherAnalyzer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        private Dictionary<string, string> Cities = new Dictionary<string, string>()
        {
            {"Moscow",  "https://weather-analyzer-prod.herokuapp.com/weather/moscow"},
            {"London",  "https://weather-analyzer-prod.herokuapp.com/weather/london"},
            {"Paris",  "https://weather-analyzer-prod.herokuapp.com/weather/paris"},
        };

        private Dictionary<string, string> Report = new Dictionary<string, string>()
        {
            {"Moscow",  "https://weather-analyzer-prod.herokuapp.com/weather/history/moscow"},
            {"London",  "https://weather-analyzer-prod.herokuapp.com/weather/history/london"},
            {"Paris",  "https://weather-analyzer-prod.herokuapp.com/weather/history/paris"},
        };

        public ObservableCollection<List<KeyValuePair<DateTime, int>>> common = new ObservableCollection<List<KeyValuePair<DateTime, int>>>();
        public List<KeyValuePair<DateTime, int>> t = new List<KeyValuePair<DateTime, int>>();
        public List<KeyValuePair<DateTime, int>> h = new List<KeyValuePair<DateTime, int>>();
        public List<KeyValuePair<DateTime, int>> s = new List<KeyValuePair<DateTime, int>>();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = common;

            ListCities.ItemsSource = new List<string>()
            {
                "Moscow", "Paris", "London"
            };
            ListCities.SelectedIndex = 0;

            CitiesReport.ItemsSource = new List<string>()
            {
                "Moscow", "Paris", "London"
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) => BlockDateTime.Text = DateTime.Now.ToString();


        private void Button_ClickЬMinimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        

        private void Button_ClickClose(object sender, RoutedEventArgs e) => Close();
        

        private void Button_ClickMaximize(object sender, RoutedEventArgs e) => 
            MessageBox.Show("Can`t resize.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

        private void ListCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TitleCity.Text = ListCities.SelectedItem.ToString();

            string references = Cities.Where(i => i.Key == TitleCity.Text).FirstOrDefault().Value;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(references);
            WebResponse response = webRequest.GetResponse();
            string values;

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                    values = reader.ReadToEnd();

            }


            var weather = JsonConvert.DeserializeObject<Weather>(values);

            HumidityBlock.Text = weather.humidity.ToString();
            TempBlock.Text = weather.temp.ToString();
            WindSpeedBlock.Text = weather.wind_speed.ToString();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();


        private void CheckBox_Checked_OnTemp(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            common.Add(t);
            //lineChart.DataContext = common;
        }

        private void CheckBox_Unchecked_OffTemp(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            common.Remove(t);
            //lineChart.DataContext = common;
        }

        private void CheckBox_Checked_OnHum(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            common.Add(h);
            //lineChart.DataContext = common;
        }

        private void CheckBox_Unchecked_OffHum(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            common.Remove(h);
            //lineChart.DataContext = common;
        }

        private void CheckBox_Checked_OnWin(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            common.Add(s);
            //lineChart.DataContext = common;
        }

        private void CheckBox_Unchecked_OffWin(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            common.Remove(s);
            //lineChart.DataContext = common;
        }

        private void CitiesReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if(t.Count > 0 || s.Count > 0 || h.Count > 0)
            {
                t.Clear();
                s.Clear();
                h.Clear();
            }

            TitleCity.Text = ListCities.SelectedItem.ToString();

            string references = Report.Where(i => i.Key == TitleCity.Text).FirstOrDefault().Value;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(references);
            WebResponse response = webRequest.GetResponse();
            string values;

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                    values = reader.ReadToEnd();

            }


            var weather = JsonConvert.DeserializeObject<List<Weather>>(values);

            var collection = weather.Where(i => i.createdAt.Date == startDate.SelectedDate.Value);


            foreach(var item in collection)
            {
                t.Add(new KeyValuePair<DateTime, int>(item.createdAt, (int)item.temp));
                h.Add(new KeyValuePair<DateTime, int>(item.createdAt, (int)item.humidity));
                s.Add(new KeyValuePair<DateTime, int>(item.createdAt, (int)item.wind_speed));
            }
            
            common.Add(t);
            common.Add(h);
            common.Add(s);
        }

    }
}
