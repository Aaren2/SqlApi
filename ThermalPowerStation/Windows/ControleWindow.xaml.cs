using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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

using Excel = Microsoft.Office.Interop.Excel;


namespace ThermalPowerStation.Windows
{
    /// <summary>
    /// Логика взаимодействия для ControleWindow.xaml
    /// </summary>
    public partial class ControleWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        HttpClient client = new HttpClient();

        List<Root> students;
        List<SensorReadings> students1;
        List<SensorReadings> students2;

        private class Root
        {
            public string IdSensor { get; set; }
        }

        private class Check
        {
            public string IdSensor { get; set; }
            public decimal Readings { get; set; }
            public string DataType { get; set; }
        }

        int id;
        public ControleWindow(int id)
        {
            this.id = id;

            client.BaseAddress = new Uri("http://localhost:5057/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
            WindowState = WindowState.Maximized;
            GetAutorizatio();
            GetCheckSensor();
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

            var respones = await client.GetStringAsync("SensorReadings/Select/IdEmployee=" + id);
            var jsonResult = JsonConvert.DeserializeObject(respones).ToString();
            students = JsonConvert.DeserializeObject<List<Root>>(jsonResult);

            var respones1 = await client.GetStringAsync("SensorReadings/Select/" + HttpUtility.UrlEncode(id + "") + "&" + HttpUtility.UrlEncode(students.First().IdSensor));
            var jsonResult1 = JsonConvert.DeserializeObject(respones1).ToString();
            students1 = JsonConvert.DeserializeObject<List<SensorReadings>>(jsonResult1);

            ChartValues<decimal> data = new ChartValues<decimal>();
            foreach (SensorReadings i in students1)
            {
                data.Add(i.Readings);
            }

            string lab = "";
            foreach (SensorReadings i in students1)
            {
                lab += (i.ReadingsDate + "").Split(' ')[0] + ",";
            }
            string[] labs = lab.Split(',');

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title =students.First().IdSensor,
                    Values = data,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 15
                }
            };
            Labels = labs;
            DataContext = this;
            left.Title = students1.First().DataType;

            foreach (Root root in students)
            {
                CBSensor.Items.Add(root.IdSensor);
            }


            DGSensor.ItemsSource = students1;
        }

        private async void GetSensor(Root root)
        {
            var respones1 = await client.GetStringAsync("SensorReadings/Select/" + HttpUtility.UrlEncode(id + "") + "&" + HttpUtility.UrlEncode(root.IdSensor));
            var jsonResult1 = JsonConvert.DeserializeObject(respones1).ToString();
            students1 = JsonConvert.DeserializeObject<List<SensorReadings>>(jsonResult1);

            ChartValues<decimal> data = new ChartValues<decimal>();
            foreach (SensorReadings i in students1)
            {
                data.Add(i.Readings);
            }

            string lab = "";
            foreach (SensorReadings i in students1)
            {
                lab += (i.ReadingsDate + "").Split(' ')[0] + ",";
            }
            string[] labs = lab.Split(',');


            SeriesCollection.Clear();
            SeriesCollection.Add(
                new LineSeries
                {
                    Title = root.IdSensor,
                    Values = data,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 15
                });


            DataContext = this;
            Labels = labs;
            DGSensor.ItemsSource = students1;
            left.Title = students1.First().DataType;
        }


        private void CBSensor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Root root = students.Where(f => f.IdSensor.Equals(CBSensor.SelectedValue)).FirstOrDefault();

            if (root != null)
            {
                GetSensor(root);
            }
        }

        private async void GetCheckSensor()
        {
            var respones2 = await client.GetStringAsync("SensorCheck/" + id + "");
            var jsonResult2 = JsonConvert.DeserializeObject(respones2).ToString();
            var students2 = JsonConvert.DeserializeObject<List<Check>>(jsonResult2);
            DGCheck.ItemsSource = students2;
        }

        private async void Get()
        {
            var respones2 = await client.GetStringAsync("SensorReadings/Select/" + id);
            var jsonResult2 = JsonConvert.DeserializeObject(respones2).ToString();
            students2 = JsonConvert.DeserializeObject<List<SensorReadings>>(jsonResult2);

            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;
            try
            {


                oXL = new Excel.Application();
                oXL.Visible = true;

                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;


                oSheet.Cells[1, 1] = "Счетчик";
                oSheet.Cells[1, 2] = "Показания";
                oSheet.Cells[1, 3] = "Тип показаний";
                oSheet.Cells[1, 4] = "Дата";


                for (int i = 0; i < students2.Count; i++)
                {
                    oSheet.Cells[i + 2, 1] = students2[i].IdSensor;
                    oSheet.Cells[i + 2, 2] = students2[i].Readings;
                    oSheet.Cells[i + 2, 3] = students2[i].DataType;
                    oSheet.Cells[i + 2, 4] = students2[i].ReadingsDate;
                }

                oSheet.get_Range("A1", "D1").Font.Bold = true;
                oSheet.get_Range("A1", "D1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                oRng = oSheet.get_Range("A1", "D1");
                oRng.EntireColumn.AutoFit();

                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {           
            Get();               
        }       
    }
}