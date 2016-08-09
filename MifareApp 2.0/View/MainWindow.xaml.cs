using GalaSoft.MvvmLight.Messaging;
using MifareApp_2._0.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MifareApp_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }


        private void NotificationMessageReceived(NotificationMessage msg)
        {
            if (msg.Notification == Constants.SHOW_INITIALIZE_CARD_WINDOW)
            {
                new InitializeCardWindow().Show();
            }

            if (msg.Notification == Constants.SHOW_CHARGE_DISCHARGE_WINDOW)
            {
                new ChargeDischargeWindow().Show();
            }
        }

    }
}
