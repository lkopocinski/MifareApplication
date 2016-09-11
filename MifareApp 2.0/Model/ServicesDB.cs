using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareApp_2._0.Model
{
    public class ServicesDB
    {
        public static List<ServiceDB> ServicesDBList = new List<ServiceDB>();

        public ServicesDB() { }

        public static void fillDB()
        {
            ServicesDBList.Add(new ServiceDB("Biblioteka", 0x01, 0x01));
            ServicesDBList.Add(new ServiceDB("Parking", 0x02, 0x02));
            ServicesDBList.Add(new ServiceDB("Lodogryf", 0x03, 0x03));
            ServicesDBList.Add(new ServiceDB("Tramwaj", 0x04, 0x04));
            ServicesDBList.Add(new ServiceDB("Filharmonia", 0x05, 0x05));
            ServicesDBList.Add(new ServiceDB("Lodziarnia", 0x06, 0x06));
            ServicesDBList.Add(new ServiceDB("Centrum zabaw dla niedzieci", 0x07, 0x07));
            ServicesDBList.Add(new ServiceDB("Kino", 0x08, 0x08));
            ServicesDBList.Add(new ServiceDB("Gokarty", 0x09, 0x09));
            ServicesDBList.Add(new ServiceDB("Coyote", 0x0A, 0x0A));
            ServicesDBList.Add(new ServiceDB("Grey", 0x0B, 0x0B));

        }

        public class ServiceDB
        {
            public string Name;
            public byte SerialNumber;
            public byte SectorNumber;

            public ServiceDB(string name, byte serialNumber, byte sectorNumber)
            {
                this.Name = name;
                this.SerialNumber = serialNumber;
                this.SectorNumber = sectorNumber;
            }
        }
    }
}
