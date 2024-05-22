using System;

namespace ThermalPowerStation.Classes
{
    public partial class sysdiagrams
    {
        public string name { get; set; }
        public int principal_id { get; set; }
        public int diagram_id { get; set; }
        public Nullable<int> version { get; set; }
        public byte[] definition { get; set; }
    }
}
