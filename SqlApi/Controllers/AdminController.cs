using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SqlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        SqlConnection con = new SqlConnection(@"server=DESKTOP-9135E6U;database=DBThermalPowerStation;Integrated Security=true;");
        
        [HttpGet("Select/")]
        public string Get()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select TABLE_NAME From INFORMATION_SCHEMA.TABLES Where TABLE_TYPE = 'BASE TABLE' and TABLE_NAME != 'sysdiagrams'", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Select/{table}")]
        public string Get(string table)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select * From "+table, con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Select/{table}&{id}")]
        public string Get(string table, string id, int asd)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select DATA_TYPE,COLUMN_NAME From INFORMATION_SCHEMA.COLUMNS Where  TABLE_NAME = '" + table+"'", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            var a = dataTable.AsEnumerable().Select(r => r.Field<string>("DATA_TYPE")).ToList().FirstOrDefault();
            if (a.Replace(" ", "") != "decimal" && a.Replace(" ", "") != "int")
            {
                id = dataTable.AsEnumerable().Select(r => r.Field<string>("COLUMN_NAME")).ToList().FirstOrDefault()+" = '" + id + "',";
            }
            else
            {
                id = dataTable.AsEnumerable().Select(r => r.Field<string>("COLUMN_NAME")).ToList().FirstOrDefault() + " = " + id;
            }

            SqlDataAdapter adapter1 = new SqlDataAdapter("Select * From " + table+" Where "+id, con);
            DataTable dataTable1 = new DataTable();
            adapter1.Fill(dataTable1);
            return JsonConvert.SerializeObject(dataTable1);
        }
        [HttpGet("Insert/{data}&{table}")]
        public string Get(string data, string table)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select TABLE_NAME,String_Agg(COLUMN_NAME,', ') AS COLUMNS,String_Agg(DATA_TYPE,', ') as DATA_TYPE From INFORMATION_SCHEMA.COLUMNS Where  ORDINAL_POSITION != 1 and TABLE_NAME != 'sysdiagrams'  and TABLE_NAME not like 'VW%' group by TABLE_NAME", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            var Users = dataTable.AsEnumerable().Select(r => r.Field<string>("TABLE_NAME")).ToList();
            var Users1 = dataTable.AsEnumerable().Select(r => r.Field<string>("COLUMNS")).ToList();
            var Users2 = dataTable.AsEnumerable().Select(r => r.Field<string>("DATA_TYPE")).ToList();

            string[] a = Users2.ElementAtOrDefault(Users.FindIndex(x => x.Equals(table))).Split(',');
            string[] b = data.Split(',');
            data = "";
            for(int i = 0; i < a.Length; i++ )
            {
                if (a[i].Replace(" ", "") != "decimal"&& a[i].Replace(" ", "") != "int")
                {
                    data += "'" + b[i] + "',";
                }
                else
                { 
                    data += b[i]+",";
                }
            }
            //nchar, decimal, nvarchar, datetime
            //asd,20.00,asd,datetime)
            //'asd','20.00,'asd','datetime'
            data = data.Substring(0, data.Length - 1);
            SqlDataAdapter adapter1 = new SqlDataAdapter("Insert into " + table + " (" + Users1.ElementAtOrDefault(Users.FindIndex(x => x.Equals(table))) + ") Values(" + data + ")", con);
            DataTable dataTable1 = new DataTable();
            adapter1.Fill(dataTable1);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Update/{data}&{table}&{id}")]
        public string Get(string data, string table, string id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select TABLE_NAME,String_Agg(COLUMN_NAME,', ') AS COLUMNS,String_Agg(DATA_TYPE,', ') as DATA_TYPE From INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME != 'sysdiagrams'  and TABLE_NAME not like 'VW%' group by TABLE_NAME", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            var Users = dataTable.AsEnumerable().Select(r => r.Field<string>("TABLE_NAME")).ToList();
            var Users1 = dataTable.AsEnumerable().Select(r => r.Field<string>("COLUMNS")).ToList();
            var Users2 = dataTable.AsEnumerable().Select(r => r.Field<string>("DATA_TYPE")).ToList();

            string[] a = Users2.ElementAtOrDefault(Users.FindIndex(x => x.Equals(table))).Split(',');
            string[] c = Users1.ElementAtOrDefault(Users.FindIndex(x => x.Equals(table))).Split(',');
            string[] b = data.Split(',');
            data = "";
            if (a[0].Replace(" ", "") != "decimal" && a[0].Replace(" ", "") != "int")
            {
                id = "'" + id + "'";
            }
            else
            {
                
            }
            for (int i = 0; i < b.Length; i++)
            {
                if (a[i+1].Replace(" ", "") != "decimal"&& a[i + 1].Replace(" ", "") != "int")
                {
                    data += c[i+1]+"='" + b[i] + "',";
                }
                else
                {
                    data += c[i + 1] + "="+ b[i] + ",";
                }
            }
            //nchar, decimal, nvarchar, datetime
            //asd,20.00,asd,datetime)
            //'asd','20.00,'asd','datetime'


            data = data.Substring(0, data.Length - 1);
            SqlDataAdapter adapter1 = new("Update " + table + " set " + data + " Where " + c[0] + " = "+id, con);
            DataTable dataTable1 = new DataTable();
            adapter1.Fill(dataTable1);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Delete/{id}&{table}")]
        public string Get(string id, string table,decimal asd)
        {
            SqlDataAdapter adapter = new("Select DATA_TYPE,COLUMN_NAME From INFORMATION_SCHEMA.COLUMNS Where  TABLE_NAME = '" + table + "'", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            var a = dataTable.AsEnumerable().Select(r => r.Field<string>("DATA_TYPE")).ToList().FirstOrDefault();
            if (a.Replace(" ", "") != "decimal" && a.Replace(" ", "") != "int")
            {
                id = dataTable.AsEnumerable().Select(r => r.Field<string>("COLUMN_NAME")).ToList().FirstOrDefault() + " = '" + id + "',";
            }
            else
            {
                id = dataTable.AsEnumerable().Select(r => r.Field<string>("COLUMN_NAME")).ToList().FirstOrDefault() + " = " + id;
            }

            SqlDataAdapter adapter1 = new SqlDataAdapter("Delete From "+ table +" Where " + id, con);
            DataTable dataTable1 = new DataTable();
            adapter1.Fill(dataTable1);
            return JsonConvert.SerializeObject(dataTable1);
        }
    }
}
