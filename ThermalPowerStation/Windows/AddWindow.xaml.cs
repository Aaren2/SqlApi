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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        HttpClient client = new HttpClient();
        
        int idEmp;
        string table;
        string data;
        public AddWindow(string table,int idEmp)
        {
            
            this.idEmp = idEmp;
            this.table = table;
            client.BaseAddress = new Uri("http://localhost:5057/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            TBHolder.Text += table;
            DPReadingsDate.SelectedDate = DateTime.Now;
            WindowState = WindowState.Maximized;
            if (table == "SensorReadings")
            {
                TBIdSensor.Visibility = Visibility.Visible;
                TBReadings.Visibility = Visibility.Visible;
                TBDataType.Visibility = Visibility.Visible;
                DPReadingsDate.Visibility = Visibility.Visible;
            }
            else if (table == "Sensor")
            {
                TBIdTypeSensor.Visibility = Visibility.Visible;
                TBIdEquipment.Visibility = Visibility.Visible;
            }
            else if (table == "TypeSensor")
            {
                TBNameType.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetAdd();

        }

        private async void GetAdd()
        {

            if (table == "SensorReadings")
            {
                TBIdSensor.Visibility = Visibility.Visible;
                TBReadings.Visibility = Visibility.Visible;
                TBDataType.Visibility = Visibility.Visible;
                DPReadingsDate.Visibility = Visibility.Visible;
                if (string.IsNullOrWhiteSpace(TBIdSensor.Text))
                {
                    MessageBox.Show("Сэнсор не может быть пустой");
                    return;
                }
                if (string.IsNullOrWhiteSpace(TBReadings.Text))
                {
                    MessageBox.Show("Показания не могут быть пустыми");
                    return;
                }
                if (string.IsNullOrWhiteSpace(TBDataType.Text))
                {
                    MessageBox.Show("Тип показания не может быть пустым");
                    return;
                }
                if (string.IsNullOrWhiteSpace(DPReadingsDate.Text))
                {
                    MessageBox.Show("Дата не может быть пустой");
                    return;
                }
                bool result = Decimal.TryParse(TBReadings.Text, out var number);
                if (result != true)
                {
                    MessageBox.Show("Показания должены быть заполнены числами");
                    return;
                }

                data= TBIdSensor.Text+","+ TBReadings.Text+","+TBDataType.Text+","+DPReadingsDate.Text;

                

            }
            else if (table == "Sensor")
            {
                TBIdTypeSensor.Visibility = Visibility.Visible;
                TBIdEquipment.Visibility = Visibility.Visible;
                if (string.IsNullOrWhiteSpace(TBIdTypeSensor.Text))
                {
                    MessageBox.Show("Тип сенсора не может быть пустым");
                    return;
                }
                if (string.IsNullOrWhiteSpace(TBIdEquipment.Text))
                {
                    MessageBox.Show("Оборудование не может быть пустым");
                    return;
                }
                bool result = Int32.TryParse(TBIdSensor.Text, out var number);
                if (result != true)
                {
                    MessageBox.Show("Тип сенсора должен быть заполнен числами");
                    return;
                }
                result = Int32.TryParse(TBIdSensor.Text, out var number1);
                if (result != true)
                {
                    MessageBox.Show("Оборудование должено быть заполнено числами");
                    return;
                }

                data = TBIdTypeSensor.Text + "," + TBIdEquipment;

                
            }
            else if (table == "TypeSensor")
            {
                TBNameType.Visibility = Visibility.Visible;
                if (string.IsNullOrWhiteSpace(TBIdTypeSensor.Text))
                {
                    MessageBox.Show("Тип сенсора не может быть пустым");
                    return;
                }
                data = TBIdTypeSensor.Text;

                
            }

            var respones1 = await client.GetStringAsync("Admin/Insert/" + HttpUtility.UrlEncode(data + "") + "&" + HttpUtility.UrlEncode(table));

            AdminWindow adminWindow = new AdminWindow(idEmp);
            adminWindow.Show();
            this.Close();
        }


        private void TB_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text == "Тип сенсора" || tb.Text == "Оборудование" || tb.Text == "Тип показания" || tb.Text == "Показания" || tb.Text == "Сенсор")
            {
                tb.Text = null;
            }
        }

        private void TB_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Replace(" ", "") == "")
            {
                if (tb.Name == "TBIdSensor")
                {
                    tb.Text = "Сенсор";
                }
                if (tb.Name == "TBReadings")
                {
                    tb.Text = "Показания";
                }
                if (tb.Name == "TBDataType")
                {
                    tb.Text = "Тип показания";
                }
                if (tb.Name == "TBIdTypeSensor")
                {
                    tb.Text = "Оборудования";
                }
                if (tb.Name == "TBIdEquipment")
                {
                    tb.Text = "Тип показания";
                }
                if (tb.Name == "TBNameType")
                {
                    tb.Text = "Тип сенсора";
                }
            }
        }
    }
}