using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace MifareApp_2._0.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region Main Window

        public bool MainLabelVisibility { get; set; }
        public string DescriptionLabel { get; set; }
        public bool DescriptionLabelVisibility { get; set; }

        public ICommand MouseOverDescription { get; private set; }
        public ICommand MouseOverInitializeCard { get; private set; }
        public ICommand MouseOverChargeDischarge { get; private set; }
        public ICommand MouseOverCreateService { get; private set; }

        public RelayCommand CreateServiceCommand { get; private set; }
        public RelayCommand ChargeDischargeCommand { get; private set; }
        public RelayCommand InitializeCardCommand { get; private set; }
       
        #endregion

        public MainViewModel()
        {                               
            MouseOverDescription = new RelayCommand(MouseOverDescriptionMethod);
            MouseOverInitializeCard = new RelayCommand(MouseOverInitializeCardMethod);
            MouseOverChargeDischarge = new RelayCommand(MouseOverChargeDischargeMethod);
            MouseOverCreateService = new RelayCommand(MouseOverCreateServiceMethod);

            InitializeCardCommand = new RelayCommand(InitializeCardMethod);
            ChargeDischargeCommand = new RelayCommand(ChargeDischargeMethod);
            CreateServiceCommand = new RelayCommand(CreateServiceMethod);

            ShowMainLabel();
        }

        private void MouseOverDescriptionMethod()
        {
            ShowMainLabel();
        }

        private void MouseOverInitializeCardMethod()
        {
            ShowDescription("Application for defloration brandnew Mifare card and setting services on card");
        }

        private void MouseOverChargeDischargeMethod()
        {
            ShowDescription("Application uses electronic wallet");
        }

        private void MouseOverCreateServiceMethod()
        {
            ShowDescription("Application for creating new services");
        }

        private void ShowMainLabel()
        {
            MainLabelVisibility = true;
            DescriptionLabelVisibility = false;
        }

        private void ShowDescription(string description)
        {
            MainLabelVisibility = false;
            DescriptionLabelVisibility = true;

            DescriptionLabel = description;
        }

        private void InitializeCardMethod()
        {
            Messenger.Default.Send(new NotificationMessage(Constants.SHOW_INITIALIZE_CARD_WINDOW));
        }

        private void ChargeDischargeMethod()
        {
            Messenger.Default.Send(new NotificationMessage(Constants.SHOW_CHARGE_DISCHARGE_WINDOW));
        }

        private void CreateServiceMethod()
        {
            Messenger.Default.Send(new NotificationMessage(Constants.SHOW_CREATE_SERVICE_WINDOW));
        }        
    }
}