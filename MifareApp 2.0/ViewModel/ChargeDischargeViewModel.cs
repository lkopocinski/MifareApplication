using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MifareApp_2._0.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MifareApp_2._0.ViewModel
{
    [ImplementPropertyChanged]
    public class ChargeDischargeViewModel : ViewModelBase
    {
        #region Connection

        public List<CardReader> Readers { get; set; }

        public CardReader SelectedReader { get; set; }

        public String UID { get; set; }

        public String UserPin { get; set; }

        public RelayCommand ConnectCommand { get; private set; }

        #endregion

        #region Electronic Wallet

        public List<Service> Services { get; set; }

        public Service SelectedService { get; set; }

        public String Saldo { get; set; }

        public String ValueToSave { get; set; }

        public Boolean IsDecrementEnabled { get; set; }

        public RelayCommand ServiceSelectedCommand { get; private set; }

        public RelayCommand DecrementCommand { get; private set; }

        public RelayCommand IncrementCommand { get; private set; }

        public RelayCommand ConfirmCommand { get; private set; }

        public ServicesDaoImpl ServicesDaoImplement { get; set; }

        #endregion

        public string status = "";

        public ChargeDischargeViewModel()
        {
            Readers = new CardReader().getListReaders();
            Saldo = "0";
            ValueToSave = "0";
            
            ConnectCommand = new RelayCommand(ConnectMethod);
            DecrementCommand = new RelayCommand(DecrementMethod);
            IncrementCommand = new RelayCommand(IncrementMethod);
            ConfirmCommand = new RelayCommand(ConfirmMethod);
            ServiceSelectedCommand = new RelayCommand(ServiceSelectedMethod);
        }
        

        private void ConnectMethod()
        {
            SelectedReader.sCardEstablishContext(out status);

            if (SelectedReader.GetStatusChange(out status) == 0)
            {
                SelectedReader.Connect(out status);
                SelectedReader.LoadKey(0, Constants.MAD_KEY, out status);
                SelectedReader.Authentication(3, 0, Constants.KEY_A, out status);

                string manBlockValue = (SelectedReader.Read(0, out status));
                if (!manBlockValue.Equals(""))
                {
                    UID = manBlockValue.Substring(0, 8);
                    byte[] uidValue = Conversions.toHexByteArrayFromString(UID);
                    Keys keys = new Keys(uidValue, 0x00);

                    SelectedReader.Connect(out status);
                    SelectedReader.LoadKey(0, Conversions.ToString(keys.getB()), out status);
                    SelectedReader.Authentication(2, 0, Constants.KEY_B, out status);
                    UserPin = (SelectedReader.Read(2, out status)).Substring(27, 5);

                    ServicesDaoImplement = new ServicesDaoImpl();
                    Services = ServicesDaoImplement.ExcludeServicesByCard(SelectedReader, Conversions.toHexByteArrayFromString(UID));
                }
                else
                {
                    UID = Constants.ACCESS_DENIED;
                }
            }

            SelectedReader.Disconnect(out status);
        }

        private void ServiceSelectedMethod()
        {
            string key = Conversions.ToString((new Keys(Conversions.toHexByteArrayFromString(UID), 
                                                        ServicesDaoImplement.GetService(SelectedService.Name).SectorNumber)).getB());
            byte electronicWalletBlockNumber = Convert.ToByte(Convert.ToInt32((ServicesDaoImplement.GetService(SelectedService.Name).SectorNumber)) * 
                                                     Constants.BLOCKS_IN_SECTOR + 
                                                     Constants.ELECTRONIC_WALLET_BLOCK_NUMBER);

            SelectedReader.Connect(out status);
            SelectedReader.LoadKey(0, key, out status);
            SelectedReader.Authentication(electronicWalletBlockNumber, 0, Constants.KEY_B, out status);

            string electronicWalletContent = SelectedReader.Read(electronicWalletBlockNumber, out status);
            int saldo = Convert.ToInt32(Convert.ToInt32(electronicWalletContent.Substring(0, 8), 16));
            Saldo = saldo.ToString();

            if (Saldo.Equals("0"))
            {
                IsDecrementEnabled = false;
            }
            else
            {
                IsDecrementEnabled = true;
            }

            SelectedReader.Disconnect(out status);
        }

        private void DecrementMethod()
        {
            ValueToSave =  (Int64.Parse(ValueToSave) - 1).ToString();

            if ((Int64.Parse(ValueToSave) + Int64.Parse(Saldo)) == 0)
            {
                IsDecrementEnabled = false;
            }
            else
            {
                IsDecrementEnabled = true;
            }
        }

        private void IncrementMethod()
        {
            ValueToSave = (Int64.Parse(ValueToSave) + 1).ToString();

            if ((Int64.Parse(ValueToSave) + Int64.Parse(Saldo)) == 0)
            {
                IsDecrementEnabled = false;
            }
            else
            {
                IsDecrementEnabled = true;
            }
        }

        private void ConfirmMethod()
        {
            Saldo = (Int64.Parse(Saldo) + Int64.Parse(ValueToSave)).ToString();

            string key = Conversions.ToString((new Keys(Conversions.toHexByteArrayFromString(UID),
                                                        ServicesDaoImplement.GetService(SelectedService.Name).SectorNumber)).getB());
            byte electronicWalletBlockNumber = Convert.ToByte(Convert.ToInt32((ServicesDaoImplement.GetService(SelectedService.Name).SectorNumber)) *
                                                     Constants.BLOCKS_IN_SECTOR +
                                                     Constants.ELECTRONIC_WALLET_BLOCK_NUMBER);

            SelectedReader.Connect(out status);
            SelectedReader.LoadKey(0, key, out status);
            SelectedReader.Authentication(electronicWalletBlockNumber, 0, Constants.KEY_B, out status);

            if (Int64.Parse(ValueToSave) > 0)
            {
                for (int i = 0; i < Int64.Parse(ValueToSave); ++i)
                {
                    SelectedReader.Increment(electronicWalletBlockNumber, out status);
                }
            }
            else if (Int64.Parse(ValueToSave) < 0)
            {
                for (int i = 0; i < Math.Abs(Int64.Parse(ValueToSave)); ++i)
                {
                    SelectedReader.Decrement(electronicWalletBlockNumber, out status);
                }
            }

            ValueToSave = "0";
            SelectedReader.Disconnect(out status);
        }
    }
}
