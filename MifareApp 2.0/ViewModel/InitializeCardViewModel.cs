using GalaSoft.MvvmLight;
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
    public class InitializeCardViewModel : ViewModelBase
    {

        public List<CardReader> Readers { get; set; }

        public CardReader SelectedReader { get; set; }


        public InitializeCardViewModel()
        {
            Readers = new List<CardReader>(new CardReader[] { new CardReader("Czytnik 1"), new CardReader("Czytnik 2") });
        }

    }
}
