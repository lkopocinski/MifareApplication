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
    public class InitializeCardViewModel : ViewModelBase
    {
        #region Connection

        public List<CardReader> Readers { get; set; }

        public CardReader SelectedReader { get; set; }

        public String Key { get; set; }

        public bool IsMadCard { get; set; }

        public String UID { get; set; }

        public RelayCommand ConnectCommand { get; private set; }

        #endregion

        #region MAD

        public RelayCommand InitializeMadCommand { get; private set; }

        public String MadBlockContent { get; set; }

        #endregion

        #region Service Activation

        public List<Service> Services { get; set; }

        public Service SelectedService { get; set; }

        public String KeyA { get; set; }

        public String AccessBits { get; set; }

        public String KeyB { get; set; }

        public RelayCommand SaveServiceCommand { get; private set; }

        #endregion

        public InitializeCardViewModel()
        {
            Readers = new List<CardReader>(new CardReader[] { new CardReader("Czytnik 1"), new CardReader("Czytnik 2") });
            Key = "FFFFFFFFFFFF";            

            ConnectCommand = new RelayCommand(ConnectMethod);
            InitializeMadCommand = new RelayCommand(InitializeMadMethod);
            SaveServiceCommand = new RelayCommand(SaveServiceMethod);
        }

        
        private void ConnectMethod()
        {
            UID = "930911";
        }

        private void InitializeMadMethod()
        {

        }

        private void SaveServiceMethod()
        {
            
        }
    }
}
