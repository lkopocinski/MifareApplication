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
        public bool IsConnected { get; set; }

        public List<CardReader> Readers { get; set; }

        public CardReader SelectedReader { get; set; }

        public String Key { get; set; }

        public bool IsMadCard { get; set; }

        public String UID { get; set; }

        public RelayCommand ConnectCommand { get; private set; }

        #endregion

        #region MAD        

        public RelayCommand InitializeMadCommand { get; private set; }

        public String UserPin { get; set; }

        public String MadBlockContent { get; set; }
        
        #endregion

        #region Service Activation        

        public List<Service> Services { get; set; }

        public Service SelectedService { get; set; }

        public String KeyA { get; set; }

        public String AccessBits { get; set; }

        public String KeyB { get; set; }

        public RelayCommand SaveServiceCommand { get; private set; }

        public ServicesDaoImpl ServicesDaoImplement { get; set; }

        #endregion

        public string status = "";

        public InitializeCardViewModel()
        {
            Readers = new CardReader().getListReaders();
            Key = Constants.VIRGIN_MIFARE_KEY;
            
            IsConnected = false;

            ConnectCommand = new RelayCommand(ConnectMethod);
            InitializeMadCommand = new RelayCommand(InitializeMadMethod);
            SaveServiceCommand = new RelayCommand(SaveServiceMethod);
        }

        
        private void ConnectMethod()
        {
            SelectedReader.sCardEstablishContext(out status);

            if (SelectedReader.GetStatusChange(out status) == 0)
            {
                SelectedReader.Connect(out status);
                SelectedReader.LoadKey(0, Key, out status);
                SelectedReader.Authentication(3, 0, Constants.KEY_B, out status);
                IsConnected = (status == "> General Authenticate" + "   Successful \n") ? true : false;

                string uid = (SelectedReader.Read(0, out status));
                if (uid != "")
                {
                    UID = uid.Substring(0, 8);

                    ServicesDaoImplement = new ServicesDaoImpl();
                    Services = ServicesDaoImplement.ServicesList;
                }
                else
                {
                    // Case when card is not non-personalized
                    // or personalized not by Sambor & Kopocinski Company 
                    // "FFFFFFFFFFFF" is not B key
                    // Further personalization cannot be done
                    UID = Constants.NOT_NONPERSONALIZED_CARD;
                }
            }
            else
            {
                UID = Constants.CARD_RM;
            }
        }

        private void InitializeMadMethod()
        {
            // DONE:
            // Init block 3:
            // Init A key equals A0A1A2A3A4A5
            // Init B key equals FFFFFFFFFFFF (Shall be Set in Constants Class)
            // Init access bits equals 0C33CFC1
            // Init block 2 by PIN code, which can be read only with key B. Known for Card user and Base stations.
            // Init Block 1 by our uniqe value. It shall indicate sth like Sambor&Kopocinski Company ID, eg. "6B6F6368616D206D6F6A61206D616D65"

            MadBlockContent = Constants.MAD_INITIAL_SECTOR_TRAILER;
            //string userPin = ((new Random()).Next(10000, 99999)).ToString();
            string userPin = "12345";
            string secondBlockContent = Constants.EMPTY_BLOCK.Remove(0, 5) + userPin;

            UserPin = userPin;
            SelectedReader.Write(3, out status, Conversions.toHexByteArrayFromString(Constants.MAD_INITIAL_SECTOR_TRAILER));
            SelectedReader.Write(2, out status, Conversions.toHexByteArrayFromString(secondBlockContent));
            SelectedReader.Write(1, out status, Conversions.toHexByteArrayFromString(Constants.COMPANY_ID));

            SelectedReader.Disconnect(out status);
        }

        private void SaveServiceMethod()
        {
            // DONE:
            // It gets UID, Type of Service, Sector of that Service and MasterKey
            // Using Keys class, generates A and B keys for above pointed Type of Service

            // Set empty Wallet (00000000FFFFFFFF0000000010EF10EF) in block 1
            // Block 0 is left empty (Entire block filled by 0)
            // Block 2 shall be filled by ID of Service

            // Set block 3: A + 08778F00 + B

            byte[] uid = Conversions.toHexByteArrayFromString(UID);
            Keys keys;
            byte sectorNumber;
            int trailerBlockNumber;
            string serviceId;
            string blockWithServiceId = Constants.EMPTY_BLOCK.Remove(0, 2);

            sectorNumber = ServicesDaoImplement.GetService(SelectedService.Name).SectorNumber;
            serviceId = (Convert.ToInt32(SelectedService.ID)).ToString();
            if (Convert.ToInt32(SelectedService.ID) > 9)
            {
                blockWithServiceId += serviceId;
            }
            else
            {
                blockWithServiceId += "0" + serviceId;
            }

            keys = new Keys(uid, sectorNumber);
            trailerBlockNumber = Convert.ToInt32((ServicesDaoImplement.GetService(SelectedService.Name).SectorNumber)) * Constants.BLOCKS_IN_SECTOR + Constants.TRAILER_BLOCK_NUMBER;

            SelectedReader.Connect(out status);
            SelectedReader.LoadKey(0, Constants.VIRGIN_MIFARE_KEY, out status);
            SelectedReader.Authentication(Convert.ToByte(trailerBlockNumber), 0, Constants.KEY_B, out status);

            KeyA = Conversions.ToString(keys.getA());
            KeyB = Conversions.ToString(keys.getB());   
            AccessBits = "08778F00";
            string sectorTrailerBlockVal = KeyA + AccessBits + KeyB;

            // Empty block
            SelectedReader.Write(Convert.ToByte(trailerBlockNumber - 3), out status, Conversions.toHexByteArrayFromString(Constants.EMPTY_BLOCK));
            // Empty Electronic Wallet
            SelectedReader.Write(Convert.ToByte(trailerBlockNumber - 2), out status, Conversions.toHexByteArrayFromString(Constants.EMPTY_ELECTRONIC_WALLET));
            // Service's ID
            SelectedReader.Write(Convert.ToByte(trailerBlockNumber - 1), out status, Conversions.toHexByteArrayFromString(blockWithServiceId));
            // A + Access Bites + B
            SelectedReader.Write(Convert.ToByte(trailerBlockNumber), out status, Conversions.toHexByteArrayFromString(sectorTrailerBlockVal)); 
        }
    }
}
