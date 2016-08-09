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
    public class ChargeDischargeViewModel : ViewModelBase
    {
        #region Connection

        public List<CardReader> Readers { get; set; }

        public CardReader SelectedReader { get; set; }

        public String UID { get; set; }

        public RelayCommand ConnectCommand { get; private set; }

        #endregion

        #region Electronic Wallet

        public List<Service> Services { get; set; }

        public Service SelectedService { get; set; }

        public String Saldo { get; set; }

        public String ValueToSave { get; set; }

        public RelayCommand DecrementCommand { get; private set; }

        public RelayCommand IncrementCommand { get; private set; }

        public RelayCommand ConfirmCommand { get; private set; }

        #endregion

        public ChargeDischargeViewModel()
        {
            Readers = new List<CardReader>(new CardReader[] { new CardReader("Czytnik 1"), new CardReader("Czytnik 2") });

            Saldo = "0";
            ValueToSave = "0";

            ConnectCommand = new RelayCommand(ConnectMethod);
            DecrementCommand = new RelayCommand(DecrementMethod);
            IncrementCommand = new RelayCommand(IncrementMethod);
            ConfirmCommand = new RelayCommand(ConfirmMethod);
        }

        private void ConnectMethod()
        {
            UID = "930911";
        }

        private void DecrementMethod()
        {
            ValueToSave =  (Int64.Parse(ValueToSave) - 1).ToString();
        }

        private void IncrementMethod()
        {
            ValueToSave = (Int64.Parse(ValueToSave) + 1).ToString();
        }

        private void ConfirmMethod()
        {
            Saldo = (Int64.Parse(Saldo) + Int64.Parse(ValueToSave)).ToString();
        }
    }
}
