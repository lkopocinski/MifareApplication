using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareApp_2._0.Model
{
    public class Service
    {
        public string Name { get; set; }

        public byte ID { get; set; }

        public byte SectorNumber { get; set; }

        public Service(string name, byte id, byte sectorNumber)
        {
            Name = name;
            ID = id;
            SectorNumber = sectorNumber;
        }
    }
}
