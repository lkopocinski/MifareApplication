using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace MifareApp_2._0.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand CreateServiceCommand { get; private set; }
        public RelayCommand ChargeDischargeCommand { get; private set; }
        public RelayCommand InitializeCardCommand { get; private set; }

        public MainViewModel()
        {

            InitializeCardCommand = new RelayCommand(InitializeCardMethod);
            ChargeDischargeCommand = new RelayCommand(ChargeDischargeMethod);
            CreateServiceCommand = new RelayCommand(CreateServiceMethod);

        }

        private void InitializeCardMethod()
        {
            throw new NotImplementedException();
        }

        private void ChargeDischargeMethod()
        {
            throw new NotImplementedException();
        }

        private void CreateServiceMethod()
        {
            throw new NotImplementedException();
        }
    }
}