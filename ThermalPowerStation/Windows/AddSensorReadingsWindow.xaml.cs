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

namespace ThermalPowerStation.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddSensorReadingsWindow.xaml
    /// </summary>
    public partial class AddSensorReadingsWindow : Window
    {
        HttpClient client = new HttpClient();
        string id;
        int idEmp;
        public AddSensorReadingsWindow(string id, int idEmp)
        {
            this.id = id;
            this.idEmp = idEmp;
            client.BaseAddress = new Uri("http://localhost:5057/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            DP.SelectedDate = DateTime.Now;
            WindowState = WindowState.Maximized;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetAdd();

        }

        private async void GetAdd()
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
            var respones1 = await client.GetStringAsync("SensorReadings/Insert/" + HttpUtility.UrlEncode(id + "") + "&" + HttpUtility.UrlEncode(TBSensorReadings.Text) + "&" + HttpUtility.UrlEncode(DP.Text));
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
