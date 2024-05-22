namespace ThermalPowerStation.Classes
{
    public partial class SensorReadings
    {
        public int IdSensorReadings { get; set; }
        public string IdSensor { get; set; }
        public decimal Readings { get; set; }
        public string DataType { get; set; }
        public System.DateTime ReadingsDate { get; set; }
    }
}
