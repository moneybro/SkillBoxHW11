using ClassLibrary.Interfaces;
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
        SummToPutStorage _summToPutStorage;
        internal PutMoneyWin(
            string accNum,
            string balance,
            SummToPutStorage summToPutStorage
            )
        {
            InitializeComponent();
            this.DataContext = this;
            AccNum = accNum;
            Balance = balance;
            _summToPutStorage = summToPutStorage;
        }

        private void putMon(object sender, RoutedEventArgs e)
        {
            decimal.TryParse(summTB.Text, out putSumm);
            if (putSumm > 0)
            {
                _summToPutStorage.summ = putSumm;
                this.DialogResult = true;
            }
        }
    }
}
