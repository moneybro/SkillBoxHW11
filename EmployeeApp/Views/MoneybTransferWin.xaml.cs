using EmployeeApp.ViewModels;
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
using System.Windows.Shapes;

namespace EmployeeApp.Views
{
    /// <summary>
    /// Логика взаимодействия для MoneybTransferWin2.xaml
    /// </summary>
    public partial class MoneyTransferWin : Window
    {
        decimal transferSumm = 0;
        public string Acc1Num { get; set; }
        public string Acc1Amount { get; set; }
        public string Acc2Num { get; set; }
        public string Acc2Amount { get; set; }
        public decimal TransferSumm { get { return transferSumm; } set { transferSumm = value; } }
        WorkEmployee _WorkEmployee;
        internal MoneyTransferWin(
            string acc1,
            string acc1Amount,
            string acc2,
            string acc2Amount,
            WorkEmployee workEmployee
            )
        {
            InitializeComponent();
            _WorkEmployee = workEmployee;
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
                _WorkEmployee.summStorage = transferSumm;
                this.DialogResult = true;
            }
        }

        private void onClose(object sender, EventArgs e)
        {

        }
    }
}
