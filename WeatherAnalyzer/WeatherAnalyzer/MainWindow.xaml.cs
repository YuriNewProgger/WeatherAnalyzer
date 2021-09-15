using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherAnalyzer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //https://weather-analyzer-prod.herokuapp.com/weather/moscow
        //https://weather-analyzer-prod.herokuapp.com/weather/london
        //https://weather-analyzer-prod.herokuapp.com/weather/paris

        private Dictionary<string, string> Cities = new Dictionary<string, string>()
        {
            {"Moscow",  "https://weather-analyzer-prod.herokuapp.com/weather/moscow"},
            {"London",  "https://weather-analyzer-prod.herokuapp.com/weather/london"},
            {"Paris",  "https://weather-analyzer-prod.herokuapp.com/weather/paris"},
        };

        public MainWindow()
        {
            InitializeComponent();

            ListCities.ItemsSource = new List<string>()
            {
                "Moscow", "Paris", "London"
            };
            ListCities.SelectedIndex = 0;
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
    }
}
