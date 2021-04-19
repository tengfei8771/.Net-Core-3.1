using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DBConfig
    {
        public string DBType { get; set; }
        public string MasterConnetion { get; set; }
        public int SlaveCount { get; set; }
        public int MaxHitLimit { get; set; }
        public List<Slaveconnetion> SlaveConnetions { get; set; }
    }

    public class Slaveconnetion
    {
        public int HitRate { get; set; }
        public string ConnectionString { get; set; }
        public int HitLimit { get; set; }
    }
}
