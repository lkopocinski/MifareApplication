using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareApp_2._0.Model
{
    public class ServicesDaoImpl : ServicesDao
    {
        public List<Service> ServicesList { get; set; }

        public ServicesDaoImpl()
        {
            ServicesList = new List<Service>();
            fillServicesList();
        }

        private void fillServicesList()
        {
            ServicesDB.fillDB();

            foreach (ServicesDB.ServiceDB element in ServicesDB.ServicesDBList)
            {
                ServicesList.Add(new Service(element.Name, element.SerialNumber, element.SectorNumber));
            }
        }

        public Service GetService(string name)
        {
            foreach (Service element in ServicesList)
            {
                if (element.Name.Equals(name))
                {
                    return element;
                }
            }

            return null;
        }

        public string[] GetServicesNames()
        {
            List<string> servicesNames = new List<string>();

            foreach (Service element in ServicesList)
            {
                servicesNames.Add(element.Name);
            }

            return servicesNames.ToArray();
        }

        public List<Service> ExcludeServicesByCard(CardReader cardReader, byte[] cardId)
        {
            List<Service> ExcludedServicesList = new List<Service>(ServicesList);
            List<Service> toRemove = new List<Service>();
            Keys keys;
            byte electronicWalletBlockNumber;
            string status;

            foreach (Service element in ExcludedServicesList)
            {
                keys = new Keys(cardId, Convert.ToByte(element.SectorNumber));
                electronicWalletBlockNumber = Convert.ToByte((Convert.ToInt32(element.SectorNumber) * Constants.BLOCKS_IN_SECTOR + 1));

                cardReader.Connect(out status);
                cardReader.LoadKey(0, Conversions.ToString(keys.getB()), out status);
                cardReader.Authentication(electronicWalletBlockNumber, 0, Constants.KEY_B, out status);

                if (!cardReader.IsAuthenticated)
                {
                    toRemove.Add(element);
                }
            }

            foreach (Service element in toRemove)
            {
                ExcludedServicesList.Remove(element);
            }

            return ExcludedServicesList;
        }
    }
}
