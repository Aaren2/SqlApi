using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ThermalPowerStation.Classes;

namespace ThermalPowerStation.Windows
{
    /// <summary>
    /// Логика взаимодействия для UpdSensorReadingsWindow.xaml
    /// </summary>
    public partial class UpdSensorReadingsWindow : Window
    {
        HttpClient client = new HttpClient();
        string id;
        int idreadings;
        int idEmp;
        public UpdSensorReadingsWindow(string id, int idreadings, int idEmp)
        {
            this.idEmp = idEmp;
            this.id = id;
            this.idreadings = idreadings;
            client.BaseAddress = new Uri("http://localhost:5057/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            GetCheckSensor();
            WindowState = WindowState.Maximized;
            
        }

        private async void GetCheckSensor()
        {
            var respones2 = await client.GetStringAsync("SensorReadings/Select/IdSensorReadings=" + idreadings);
            var jsonResult2 = JsonConvert.DeserializeObject(respones2).ToString();
            var students2 = JsonConvert.DeserializeObject<List<SensorReadings>>(jsonResult2);
            TBSensorReadings.Text = students2.First().Readings+"";
            DP.Text = students2.First().ReadingsDate + "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetUpd();
        }

        private async void GetUpd()
        {
            if (string.IsNullOrWhiteSpace(DP.Text))
            {
                MessageBox.Show("Дата не может быть пустой");
                return;
            }
            bool result = Decimal.TryParse(TBSensorReadings.Text, out var number);
            if (result != true)
            {
                MessageBox.Show("Показания должены быть заполнены числами");
                return;
            }
            var respones1 = await client.GetStringAsync("SensorReadings/Update/" + HttpUtility.UrlEncode(idreadings+"") + "&" + HttpUtility.UrlEncode(TBSensorReadings.Text.Replace(',','.')) + "&" + HttpUtility.UrlEncode(DP.Text));
            EngineerWindow engineerWindow = new EngineerWindow(idEmp);
            engineerWindow.Show();
            this.Close();
        }


        private void TB_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text == "Показания")
            {
                tb.Text = null;
            }
        }

        private void TB_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Replace(" ", "") == null)
            {
                tb.Text = "Показания";
            }
        }
    }
}
