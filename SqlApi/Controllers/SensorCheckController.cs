using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SqlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorCheckController : ControllerBase
    {

        SqlConnection con = new SqlConnection(@"server=DESKTOP-9135E6U;database=DBThermalPowerStation;Integrated Security=true;");
        // GET api/<SensorCheckController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select * From FN_EffectivenessMainEquipment((Select IdStation From Employee Where IdEmployee = '"+id+"'))", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
    }
}
