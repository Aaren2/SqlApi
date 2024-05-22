using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using ThermalPowerStation.Classes;

namespace ThermalPowerStation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient(); 
        public MainWindow()
        {
            client.BaseAddress = new Uri("http://localhost:5057/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            this.GetSensorReadings();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.GetSensorReadings();
        }

        private async void GetSensorReadings() {
            var respones = await client.GetStringAsync("SensorReadings");
            var jsonResult = JsonConvert.DeserializeObject(respones).ToString();
            var students = JsonConvert.DeserializeObject<List<SensorReadings>>(jsonResult);
            dgSensorReadings.ItemsSource = students;
        }
    }
}
