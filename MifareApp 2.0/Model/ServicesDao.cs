using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareApp_2._0.Model
{
    interface ServicesDao
    {
        Service GetService(string name);

        string[] GetServicesNames();

        List<Service> ExcludeServicesByCard(CardReader cardReader, byte[] cardId);
    }
}
