using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using static SqlApi.Controllers.ClassHelper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SqlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorReadingsController : ControllerBase
    {
        SqlConnection con = new SqlConnection(SQL);
        // GET: api/<SensorReadingsController>


 
        [HttpGet("Select/{id}&{idsensor}")]
        public string Get(int id, string idsensor)
        {
            SqlDataAdapter adapter = new("Select sr.* From SensorReadings sr join Sensor s on sr.IdSensor = s.IdSensor join Equipment e on s.IdEquipment = e.IdEquipment join ThermalPowerStation tps on e.IdStation = tps.IdStation join Employee em on tps.IdStation = em.IdStation where IdEmployee = " + id + " and sr.IdSensor = '"+ idsensor+"'", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Select/IdEmployee={idEmp}")]
        public string Get(int idEmp,int asd)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select sr.IdSensor From SensorReadings sr join Sensor s on sr.IdSensor = s.IdSensor join Equipment e on s.IdEquipment = e.IdEquipment join ThermalPowerStation tps on e.IdStation = tps.IdStation join Employee em on tps.IdStation = em.IdStation where IdEmployee = " + idEmp+ " Group by sr.IdSensor ", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Select/IdSensorReadings={id}")]
        public string Get(int id,decimal asd)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select * From SensorReadings where IdSensorReadings  = " + id, con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Select/{id}")]
        public string Get(int id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select sr.* From SensorReadings sr join Sensor s on sr.IdSensor = s.IdSensor join Equipment e on s.IdEquipment = e.IdEquipment join ThermalPowerStation tps on e.IdStation = tps.IdStation join Employee em on tps.IdStation = em.IdStation where IdEmployee = " + id, con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }


        [HttpGet("Insert/{idsensor}&{readings}&{data}")]
        public string Get(string idsensor, decimal readings, string data)
        {
            SqlDataAdapter adapter = new("Insert into SensorReadings (IdSensor,Readings,DataType,ReadingsDate)Values('" + idsensor + "'," + readings + ",(Select Case When IdType = 1 Then 'бар' When IdType = 2 Then 'ватт-час' When IdType = 3 Then 'градусов цельси' When IdType = 4 Then 'паскаль' When IdType = 5 Then 'оборотов в минуту' Else 'киловатт-час' END AS DataTypeText From Sensor  Where IdSensor = '" + idsensor + "'),'" + data + "')", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Update/{idsensor}&{readings}&{data}")]
        public string Get(int idsensor, decimal readings, string data)
        {
            SqlDataAdapter adapter = new("Update SensorReadings set Readings = " + (readings + "").Replace(',', '.') + " , ReadingsDate = '" + data + "' Where IdSensorReadings = '" + idsensor + "'", con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        [HttpGet("Delete/{id}")]
        public string Get(int id, bool asd)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Delete from SensorReadings Where IdSensorReadings = " + id, con);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return JsonConvert.SerializeObject(dataTable);
        }
        
        
    }
}
