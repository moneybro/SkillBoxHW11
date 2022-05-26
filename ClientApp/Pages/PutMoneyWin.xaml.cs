using ClientApp.ViewModels;
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

namespace ClientApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для PutMoneyWin2.xaml
    /// </summary>
    public partial class PutMoneyWin : Window
    {
        decimal putSumm = 0;
        public string AccNum { get; set; }
        public string Balance { get; set; }
        WorkClient _workClient;
        internal PutMoneyWin(
            string accNum,
            string balance,
            WorkClient workClient
            )
        {
            InitializeComponent();
            this.DataContext = this;
            AccNum = accNum;
            Balance = balance;
            _workClient = workClient;
        }

        private void putMon(object sender, RoutedEventArgs e)
        {
            decimal.TryParse(summTB.Text, out putSumm);
            if (putSumm > 0)
            {
                _workClient.summ = putSumm;
                this.DialogResult = true;
            }
        }
    }
}
