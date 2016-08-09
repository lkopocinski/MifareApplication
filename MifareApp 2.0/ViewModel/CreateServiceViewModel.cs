using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MifareApp_2._0.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareApp_2._0.ViewModel
{
    [ImplementPropertyChanged]
    public class CreateServiceViewModel : ViewModelBase
    {
        public String ServiceName { get; set; }

        public List<Sector> Sectors { get; set; }

        public Sector SelectedService { get; set; }

        public RelayCommand SaveServiceCommand { get; private set; }


        public CreateServiceViewModel()
        {
            Sectors = new List<Sector>(new Sector[] { new Sector("1"), new Sector(5) });

            SaveServiceCommand = new RelayCommand(SaveServiceMethod);
        }

        private void SaveServiceMethod()
        {
            
        }
    }
}
