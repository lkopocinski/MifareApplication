using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareApp_2._0.Model
{
    public class Sector
    {
        public string Number { get; set; }

        public string Id { get; set; }
        
        public Sector(string number)
        {
            Number = number;
        }

        public Sector(int number)
        {
            if(number > 0)
            {
                Number = number.ToString();
            }
            else
            {
                throw new RankException("Sector number must be positive integer");
            }            
        }
    }
}
