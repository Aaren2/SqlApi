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
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using ThermalPowerStation.Classes;

namespace ThermalPowerStation.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        HttpClient client = new HttpClient();

        List<Root> students;

        private class Root
        {
            public string TABLE_NAME { get; set; }
        }

        int id;
        string idreading;
        IEnumerable students1;

        public AdminWindow(int id)
        {
            this.id = id;

            client.BaseAddress = new Uri("http://localhost:5057/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            WindowState = WindowState.Maximized;
            GetAutorizatio();
            GetEmployee();

        }
        private class Root1
        {
            public string Column1 { get; set; }
        }
        private async void GetEmployee()
        {
            var respones = await client.GetStringAsync("Employee/" + id);
            var jsonResult = JsonConvert.DeserializeObject(respones).ToString();
            var students = JsonConvert.DeserializeObject<List<Root1>>(jsonResult);
            TBHolder.Text += students.First().Column1;
        }
        private async void GetAutorizatio()
        {

            var respones = await client.GetStringAsync("Admin/Select");
            var jsonResult = JsonConvert.DeserializeObject(respones).ToString();
            students = JsonConvert.DeserializeObject<List<Root>>(jsonResult);

            foreach (Root root in students)
            {
                CBSensor.Items.Add(root.TABLE_NAME);
            }

        }

        private async void GetSensor(Root root)
        {
            var respones1 = await client.GetStringAsync("Admin/Select/" + HttpUtility.UrlEncode(root.TABLE_NAME));
            var jsonResult1 = JsonConvert.DeserializeObject(respones1).ToString();
            
            switch (root.TABLE_NAME)
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

            DG.ItemsSource = students1;
        }


        private void CBSensor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Root root = students.Where(f => f.TABLE_NAME.Equals(CBSensor.SelectedValue)).FirstOrDefault();

            if (root != null)
            {
                GetSensor(root);
            }

            idreading = null;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(CBSensor.SelectedValue + "", id);
            addWindow.Show();
            this.Close();
        }

        private void BtnUpd_Click(object sender, RoutedEventArgs e)
        {
            if (idreading != null)
            {
                UpdateWindow updateWindow = new UpdateWindow(CBSensor.SelectedValue + "", idreading, id);
                updateWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Данные не выбраны");
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (idreading != null)
            {
                if (MessageBox.Show("Точно хотите удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
                {
                    GetDel();
                }
            }
            else
            {
                MessageBox.Show("Данные не выбраны");
            }
        }
        private async void GetDel()
        {
            var respones2 = await client.GetStringAsync("Admin/Delete/" + idreading + "&"+CBSensor.SelectedValue);
            Root root = students.Where(f => f.TABLE_NAME.Equals(CBSensor.SelectedValue)).FirstOrDefault();

            if (root != null)
            {
                GetSensor(root);
            }
            DG.SelectedIndex = -1;
            idreading = null;
        }

        private void DG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!DG.SelectedIndex.Equals(-1))
            {
                TextBlock x = DG.Columns[0].GetCellContent(DG.Items[DG.SelectedIndex]) as TextBlock;
                idreading = x?.Text+"";
                DG.SelectedIndex = -1;
            }

        }









        //sUOiko0YsA
    }
}