﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using static SqlApi.Controllers.ClassHelper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SqlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        SqlConnection con = new SqlConnection(SQL);
        [HttpGet("{number}&{password}")]
        public string Get(string number,string password)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select dbo.FN_AutorizationEmplyee('"+ number +"','"+ password + "'), IdEmployee From Employee Where Number = '" + number + "' and Password ='" + password + "' ", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("{id}")]
        public string Get(int id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select ConCat(FirstName,' ',LastName,' ',MiddleName) From Employee Where IdEmployee = "+id, con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }

    }
}