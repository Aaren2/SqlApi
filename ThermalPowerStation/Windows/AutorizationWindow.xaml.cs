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
    /// Логика взаимодействия для AutorizationWindow.xaml
    /// </summary>
    public partial class AutorizationWindow : Window
    {
        HttpClient client = new HttpClient();
        public AutorizationWindow()
        {
            client.BaseAddress = new Uri("http://localhost:5057/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            Button button = sender as Button;
            button.IsEnabled = false; 
            this.GetAutorizatio();
        }
        private class Root
        {
            public string Column1 { get; set; }
            public int IdEmployee { get; set; }
        }
        private async void GetAutorizatio()
        {
            var respones = await client.GetStringAsync("Employee/"+ HttpUtility.UrlEncode(TBNumber.Text) +"&"+ HttpUtility.UrlEncode(TBPassword.Password));
            var jsonResult = JsonConvert.DeserializeObject(respones).ToString();
            var students = JsonConvert.DeserializeObject<List<Root>>(jsonResult);
           
            if (students.Count() == 0) 
            {
                MessageBox.Show("Телефон или пароль указаны неверно");
                BTN.IsEnabled = true;
                return;
            }
            else if (students.First().Column1 == "Главный инженер")
            {
                EngineerWindow engineerWindow = new EngineerWindow(students.First().IdEmployee);
                engineerWindow.Show();
                this.Close();
            }
            else if (students.First().Column1 == "Сотрудник отдела контроля качества")
            {
                ControleWindow controleWindow = new ControleWindow(students.First().IdEmployee);
                controleWindow.Show();
                this.Close();
            }
            else if (students.First().Column1 == "Системный администратор")
            {
                AdminWindow AdminWindow = new AdminWindow(students.First().IdEmployee);
                AdminWindow.Show();
                this.Close();
            }
            else 
            { 
                MessageBox.Show(students.First().Column1);
            }
            BTN.IsEnabled = true;
        }

        private void TB_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text == "Номер телефона" ) 
            {
                tb.Text = null;
            }
        }

        private void TB_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Replace(" ", "") == "")
            {
                if (tb.Name == "TBNumber") 
                { 
                    tb.Text = "Номер телефона";
                }
            }
        }
        
    }
}
