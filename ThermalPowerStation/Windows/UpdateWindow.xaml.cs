using Newtonsoft.Json;
using System;
using System.Collections;
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
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        HttpClient client = new HttpClient();

        int idEmp;
        string table;
        string id;
        object students1;
        string data;
        public UpdateWindow(string table, string id,int idEmp)
        {

            this.idEmp = idEmp;
            this.table = table;
            this.id = id;
            client.BaseAddress = new Uri("http://localhost:5057/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            WindowState = WindowState.Maximized;
            Get();            
        }
        private async void Get()
        {
            var respones1 = await client.GetStringAsync("Admin/Select/" + HttpUtility.UrlEncode(table)+"&"+ HttpUtility.UrlEncode(id));
            var jsonResult1 = JsonConvert.DeserializeObject(respones1).ToString();

            switch (table)
            {
                case "Gender":
                    students1 = JsonConvert.DeserializeObject<List<Gender>>(jsonResult1);
                    break;
                case "City":
                    students1 = JsonConvert.DeserializeObject<List<City>>(jsonResult1);
                    break;
                case "Post":
                    students1 = JsonConvert.DeserializeObject<List<Post>>(jsonResult1);
                    break;
                case "ThermalPowerStation":
                    students1 = JsonConvert.DeserializeObject<List<Classes.ThermalPowerStation>>(jsonResult1);
                    break;
                case "EmployeePost":
                    students1 = JsonConvert.DeserializeObject<List<EmployeePost>>(jsonResult1);
                    break;
                case "TypeEquipment":
                    students1 = JsonConvert.DeserializeObject<List<TypeEquipment>>(jsonResult1);
                    break;
                case "MainEquipment":
                    students1 = JsonConvert.DeserializeObject<List<MainEquipment>>(jsonResult1);
                    break;
                case "sysdiagrams":
                    students1 = JsonConvert.DeserializeObject<List<sysdiagrams>>(jsonResult1);
                    break;
                case "TypeMaintenance":
                    students1 = JsonConvert.DeserializeObject<List<TypeMaintenance>>(jsonResult1);
                    break;
                case "Maintenance":
                    students1 = JsonConvert.DeserializeObject<List<Maintenance>>(jsonResult1);
                    break;
                case "EmployeeMaintenance":
                    students1 = JsonConvert.DeserializeObject<List<EmployeeMaintenance>>(jsonResult1);
                    break;
                case "TypeSensor":
                    students1 = JsonConvert.DeserializeObject<List<TypeSensor>>(jsonResult1);
                    break;
                case "SensorReadings":
                    students1 = JsonConvert.DeserializeObject<List<SensorReadings>>(jsonResult1);
                    break;
                case "Equipment":
                    students1 = JsonConvert.DeserializeObject<List<Equipment>>(jsonResult1);
                    break;
                case "Sensor":
                    students1 = JsonConvert.DeserializeObject<List<Sensor>>(jsonResult1);
                    break;
                case "Employee":
                    students1 = JsonConvert.DeserializeObject<List<Employee>>(jsonResult1);
                    break;
            }


            if (table == "SensorReadings")
            {
                List<SensorReadings> sensorReadings = (List<SensorReadings>)students1;
                SensorReadings sensorReading = sensorReadings.FirstOrDefault();
                TBIdSensor.Visibility = Visibility.Visible;
                TBIdSensor.Text = sensorReading.IdSensor;
                TBReadings.Visibility = Visibility.Visible;
                TBReadings.Text = sensorReading.Readings + "";
                TBDataType.Visibility = Visibility.Visible;
                TBDataType.Text = sensorReading.DataType;
                DPReadingsDate.Visibility = Visibility.Visible;
                DPReadingsDate.Text = sensorReading.ReadingsDate + "";
            }
            else if (table == "Sensor")
            {
                List<Sensor> sensors = (List<Sensor>)students1;
                Sensor sensor = sensors.FirstOrDefault();
                TBIdTypeSensor.Visibility = Visibility.Visible;
                TBIdTypeSensor.Text = sensor.IdType + "";
                TBIdEquipment.Visibility = Visibility.Visible;
                TBIdEquipment.Text = sensor.IdEquipment;
            }
            else if (table == "TypeSensor")
            {
                List<Type> types = (List<Type>)students1;
                Type type = types.FirstOrDefault();
                TBNameType.Visibility = Visibility.Visible;
                TBNameType.Text = type.Name;
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


                data = TBIdSensor.Text + "," + TBReadings.Text.Replace(",",".") + "," + TBDataType.Text + "," + DPReadingsDate.Text;

                

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

            var respones1 = await client.GetStringAsync("Admin/Update/" + HttpUtility.UrlEncode(data + "") + "&" + HttpUtility.UrlEncode(table)+"&"+ HttpUtility.UrlEncode(id));

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
