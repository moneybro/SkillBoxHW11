using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using ClientApp.ViewModels;

namespace ClientApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MoneyTransferWin.xaml
    /// </summary>
    public partial class MoneyTransferWin : Window
    {
        decimal transferSumm = 0;
        public string Acc1Num { get; set; }
        public string Acc1Amount { get; set; }
        public string Acc2Num { get; set; }
        public string Acc2Amount { get; set; }
        public decimal TransferSumm { get { return transferSumm; } set { transferSumm = value; } }
        WorkClient _workClient;
        internal MoneyTransferWin(
            string acc1,
            string acc1Amount,
            string acc2,
            string acc2Amount,
            WorkClient client
            )
        {
            InitializeComponent();
            _workClient = client;
            Acc1Num = acc1;
            Acc2Num = acc2;
            Acc1Amount = acc1Amount;
            Acc2Amount = acc2Amount;
        }

        private void tnsMon(object sender, RoutedEventArgs e)
        {
            decimal.TryParse(transferSummTBox.Text, out transferSumm);
            if (transferSumm != 0 && transferSumm > 0)
            {
                _workClient.summ = transferSumm;
                this.DialogResult = true;
            }
        }

        private void onClose(object sender, EventArgs e)
        {

        }
    }
}
