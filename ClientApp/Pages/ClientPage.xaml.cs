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
using ClassLibrary.Classes;
using ClassLibrary.Methods;
using ClientApp.ViewModels;

namespace ClientApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        ClientPage? cp;
        WorkClient workClient;

        public ClientPage(Client client)
        {
            cp = this;
            InitializeComponent();
            workClient = new WorkClient(client);
            this.DataContext = workClient;
            ClientFIO.Text = workClient.fullName;
        }

        private void ClosePage(object sender, RoutedEventArgs e)
        {
            cp = null;
            NavigationService.GoBack();
        }
        private void OpenMainAcc(object sender, RoutedEventArgs e)
        {
            if (workClient.mainAcc == null)
            {
                workClient.createNewMainAcc();
                MessageBox.Show($"Счет {workClient.MainAccNumber} открыт");
            }
            else
            {
                MessageBox.Show("Главный счет уже открыт, можно иметь только 1 главный счет");
            }
        }
        private void CloseMianAcc(object sender, RoutedEventArgs e)
        {
            var accNumToClose = workClient.MainAccNumber;
            if (workClient.mainAcc != null && workClient.closeAcc(workClient.mainAcc))
            {
                MessageBox.Show($"Счет {accNumToClose} закрыт.");
            }
            else
            {
                MessageBox.Show("Счет отсутствует.");
            }
        }
        private void OpenDepoAcc(object sender, RoutedEventArgs e)
        {
            if (workClient.depoAcc == null)
            {
                workClient.createNewDepoAcc();
                MessageBox.Show($"Счет {workClient.DepoAccNumber} открыт");
            }
            else
            {
                if (workClient.accList.Where(a => a.GetType() == typeof(BankAccMain)).Count() > 0)
                {
                    MessageBox.Show("Депозитный счет уже открыт, можно иметь только 1 депозитный счет");
                }
            }
        }
        private void CloseDepoAcc(object sender, RoutedEventArgs e)
        {
            string accNumToCloseString = workClient.DepoAccNumber;
            if (workClient.depoAcc != null && workClient.closeAcc(workClient.depoAcc))
            {
                MessageBox.Show($"Счет {accNumToCloseString} закрыт.");
            }
            else
            {
                MessageBox.Show("Счет отсутствует.");
            }
        }
        private void AddMoneyBtn_Click(object sender, RoutedEventArgs e)
        {
            var s = (Button)sender;
            var senderName = s.Name;
            string accNum = "",
                accAmount = "";
            BankAcc? accToCharge = new BankAcc();
            if (senderName == "PutMoneyToMainAccBtn")
            {
                if (workClient.mainAcc == null) { return; }
                accToCharge = workClient.mainAcc;
                accNum = workClient.MainAccNumber;
                accAmount = workClient.MainAccAmount;
            }
            if (senderName == "PutMoneyToDepoAccBtn")
            {
                if (workClient.depoAcc == null) { return; }
                accToCharge = workClient.depoAcc;
                accNum = workClient.DepoAccNumber;
                accAmount = workClient.DepoAccAmount;
            }
            PutMoneyWin putMoneyWin = new PutMoneyWin(accNum, accAmount, workClient);
                if(putMoneyWin.ShowDialog() == true)
            {
                if (workClient.pushMoneyToAcc(accToCharge))
                {
                    MessageBox.Show("Пополнение выполнено успешно.");
                }
            }
        }            
        private void TransferMoneyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (workClient.mainAcc != null && workClient.depoAcc != null)
            {
                var s = (Button)sender;
                var senderName = s.Name;
                string acc1Num = "",
                    acc2Num = "",
                    acc1Amount = "",
                    acc2Amount = "";
                BankAcc accSource = new BankAcc();
                BankAcc accTarget = new BankAcc();
                if (senderName == "TransferMoneyFromMainAccBtn")
                {
                    acc1Num = workClient.MainAccNumber;
                    acc1Amount = workClient.MainAccAmount;
                    acc2Num = workClient.DepoAccNumber;
                    acc2Amount = workClient.DepoAccAmount;
                    accSource = workClient.mainAcc;
                    accTarget = workClient.depoAcc;
                }
                if (senderName == "TransferMoneyFromDepoAccBtn")
                {
                    acc1Num = workClient.DepoAccNumber;
                    acc1Amount = workClient.DepoAccAmount;
                    acc2Num = workClient.MainAccNumber;
                    acc2Amount = workClient.MainAccAmount;
                    accSource = workClient.depoAcc;
                    accTarget = workClient.mainAcc;
                }

                MoneyTransferWin moneyTransferWin = new MoneyTransferWin(
                    acc1Num,
                    acc1Amount,
                    acc2Num,
                    acc2Amount,
                    workClient
                    );
                if (moneyTransferWin.ShowDialog() == true)
                {
                    if (workClient.tfrMoney(accSource, accTarget))
                    {
                        MessageBox.Show("Перевод выполнен успешно.");
                    }
                }
            }
            
        }        
    }
}
