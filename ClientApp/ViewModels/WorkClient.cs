using ClassLibrary;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using ClassLibrary.Methods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using ClientApp.Pages;
using System.Windows;
using System.Windows.Controls;

namespace ClientApp.ViewModels
{
    internal class WorkClient : Client, INotifyPropertyChanged
    {
        #region Properties and vars
        public event PropertyChangedEventHandler PropertyChanged;
        event Action<string> LogAction;

        internal ClientPage _clientPage;

        internal List<BankAccForClient> accList;
        internal BankAccMain? mainAcc;
        internal BankAccDepo? depoAcc;
        internal string fullName;

        /// <summary>
        /// сумма предназначенная для перевода или пополнения
        /// </summary>
        internal decimal summ = 0;
        public string MainAccNumber {
            get
            {
                if (mainAcc != null)
                {
                    return modernAccView(mainAcc.AccNumber);
                }
                else return "-";
            } 
            set {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MainAccNumber)));
            }
        }        
        public string MainAccAmount
        {
            get
            {
                return mainAcc != null ? mainAcc.Amount.ToString() : "-";
            }
            set {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MainAccAmount)));
            }
        }
        public bool MainAccIsNotNull
        {
            get
            {
                if (mainAcc != null) return true;
                else return false;
            }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MainAccIsNotNull)));
            }
        }
        public bool MainAccIsNull
        {
            get
            {
                if (mainAcc == null) return true;
                else return false;
            }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MainAccIsNull)));
            }
        }
        public string DepoAccNumber
        {
            get
            {
                if (depoAcc != null)
                {
                    return modernAccView(depoAcc.AccNumber);
                }
                else return "-";
            }
            set {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DepoAccNumber)));
            }
        }
        public string DepoAccAmount
        {
            get
            {
                return depoAcc != null ? depoAcc.Amount.ToString() : "-";
            }
            set { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DepoAccAmount))); }
        }
        public bool DepoAccIsNotNull
        {
            get
            {
                if (depoAcc != null) return true;
                else return false;
            }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DepoAccIsNotNull)));
            }
        }
        public bool DepoAccIsNull
        {
            get
            {
                if (depoAcc == null) return true;
                else return false;
            }
            set
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.DepoAccIsNull)));
            }
        }

        BankAccActions accActions;

        #endregion

        static WorkClient()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/clientAccsActions.log", rollingInterval: RollingInterval.Minute)
                .CreateLogger();
        }
        internal WorkClient(Client cl, ClientPage clientPage) : base(ID: cl.ID)
        {
            _clientPage = clientPage;
            accActions = new BankAccActions();
            this.LastName = cl.LastName;
            this.Name = cl.Name;
            this.Patronymic = cl.Patronymic;
            this.MobPhone = cl.MobPhone;
            this.PaspSeria = cl.PaspSeria;
            this.PaspNum = cl.PaspNum;
            this.LastChangeDate = DateTime.Now;
            fullName = $"{LastName} {Name} {Patronymic}";
            accList = accActions.GetClientAccs(cl.ID);
            mainAcc = (BankAccMain?)accList.Find(a => a.GetType() == typeof(BankAccMain));
            depoAcc = (BankAccDepo?)accList.Find(a => a.GetType() == typeof(BankAccDepo));
            LogAction += myLogAction;
            _clientPage.OpenMainAccBtnClicked += createNewMainAcc;
            _clientPage.OpenDepoAccBtnClicked += createNewDepoAcc;
            _clientPage.CloseAccBtnClicked += closeAcc;
            _clientPage.AddMoneyBtnClicked += pushMoneyToAcc;
            _clientPage.TfrMoneyBtnClicked += tfrMoney;
        }
        void refreshAccFields()
        {
            MainAccNumber += 0;
            MainAccAmount += 0;
            MainAccIsNotNull = MainAccIsNotNull;
            MainAccIsNull = MainAccIsNull;
            DepoAccNumber += 0;
            DepoAccAmount += 0;
            DepoAccIsNotNull = DepoAccIsNotNull;
            DepoAccIsNull = DepoAccIsNull;
        }

        internal void createNewMainAcc()
        {
            if (mainAcc == null)
            {
                BankAccMain bankAcc = accActions.GetNewMainAcc(this.ID);
                if (bankAcc != null)
                {
                    accList.Add(bankAcc);
                    mainAcc = bankAcc;
                    refreshAccFields();
                    LogAction($"createNewMainAcc {mainAcc}");
                    MessageBox.Show($"Счет {this.MainAccNumber} открыт");
                }
                else
                {
                    MessageBox.Show($"Что-то пошло не так. Главный счет не открыт");
                }
            }
            else
            { 
                MessageBox.Show($"Главный счет уже открыт, можно иметь только 1 главный счет"); 
            }
        }
        internal void closeAcc(BankAccForClient acc)
        {
            var accNumToClose = acc;
            if (acc != null && accActions.CloseAcc(acc.AccNumber))
            {
                accList.Remove(acc);
                if (acc is BankAccMain)
                {
                    mainAcc = null;
                }
                else
                {
                    depoAcc = null;
                }
                refreshAccFields();
                LogAction($"closeAcc {acc}");
                MessageBox.Show($"Счет {modernAccView(accNumToClose.AccNumber)} закрыт.");
            }
            else MessageBox.Show("Счет отсутствует.");
        }
        internal void createNewDepoAcc()
        {
            if (depoAcc == null)
            {
                var bankAcc = accActions.GetNewDepoAcc(this.ID);

                if (bankAcc != null)
                {
                    depoAcc = bankAcc; 
                    accList.Add(depoAcc);
                    refreshAccFields();
                    LogAction($"createNewDepoAcc {depoAcc}");
                    MessageBox.Show($"Счет {this.DepoAccNumber} открыт");
                }
                else
                {
                    MessageBox.Show($"Что-то пошло не так. Главный счет не открыт");
                }
            }
            else
            {
                MessageBox.Show($"Депозитный счет уже открыт, можно иметь только 1 депозитный счет");
            }            
        }
        /// <summary>
        /// преобразует вид банковского счета из цифрового в текстовый, дополняя нулями до 20 знаков
        /// </summary>
        /// <param name="accNum">номер счета long</param>
        /// <returns></returns>
        string modernAccView(long accNum)
        {
            string accString = accNum.ToString();
            string before = new string('0', 20 - accString.Length);
            return before + accString;
        }
        internal void tfrMoney(object sender)
        {
            if (this.mainAcc != null && this.depoAcc != null)
            {
                var s = (Button)sender;
                var senderName = s.Name;
                //BankAccForClient accSource = new BankAccForClient();
                //BankAccForClient accTarget = new BankAccForClient();
                string accSourceNumber = "0";
                string accSourceAmount = "0";
                string accTargetNumber = "0";
                string accTargetAmount = "0";

                IStorageTransferMoney<BankAccForClient> transferStorage = new BankAccTransferStorage<BankAccBase>();

                if (senderName == "TransferMoneyFromMainAccBtn")
                {
                    transferStorage.addAcc = this.mainAcc;
                    transferStorage.addAcc = this.depoAcc;
                    accSourceNumber = modernAccView(this.mainAcc.AccNumber);
                    accTargetNumber = modernAccView(this.depoAcc.AccNumber);
                    accSourceAmount = this.mainAcc.Amount.ToString();
                    accTargetAmount = this.depoAcc.Amount.ToString();
                }
                if (senderName == "TransferMoneyFromDepoAccBtn")
                {
                    transferStorage.addAcc = this.depoAcc;
                    transferStorage.addAcc = this.mainAcc;
                    accSourceNumber = modernAccView(this.depoAcc.AccNumber);
                    accTargetNumber = modernAccView(this.mainAcc.AccNumber);
                    accSourceAmount = this.mainAcc.Amount.ToString();
                    accTargetAmount = this.depoAcc.Amount.ToString();
                }
                MoneyTransferWin moneyTransferWin = new MoneyTransferWin(
                    accSourceNumber,
                    accSourceAmount,
                    accTargetNumber,
                    accTargetAmount,
                    this);

                if (moneyTransferWin.ShowDialog() == true)
                {
                    //var success = accActions.transferMoney(accSource, accTarget, this.summ);
                    var transfeSuccess = transferStorage.TransferMoney(summ);
                    if (transfeSuccess)
                    {
                        refreshAccFields();
                        LogAction($"transfer money from {accSourceNumber} to {accTargetNumber} summ = {this.summ}.");
                        MessageBox.Show("Перевод выполнен успешно.");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось выполнить перевод.");
                    }
                }
                else
                {
                    MessageBox.Show("Что-то пошло не так.");
                }
            }
        }
        internal void pushMoneyToAcc(object sender)
        {
            IPushMoney<BankAccBase> accChargeAction = mainAcc;

            var s = (Button)sender;
            var senderName = s.Name;
            BankAccForClient? accToCharge = new BankAccForClient();
            //BankAccBase accToCharge = new BankAccBase();
            if (senderName == "PutMoneyToMainAccBtn")
            {
                if (this.mainAcc == null) { MessageBox.Show("Нет главного счета."); }
                accToCharge = this.mainAcc;
                accChargeAction = this.mainAcc;
            }
            if (senderName == "PutMoneyToDepoAccBtn")
            {
                if (this.depoAcc == null) { MessageBox.Show("Нет депозитного счета."); }
                accToCharge = this.depoAcc;
                accChargeAction = this.depoAcc;
            }
            PutMoneyWin putMoneyWin = 
                new PutMoneyWin(modernAccView(accToCharge.AccNumber), accToCharge.Amount.ToString(), this);

            if (putMoneyWin.ShowDialog() == true)
            {
                decimal summ = -1;
                decimal.TryParse(putMoneyWin.summTB.Text, out summ);
                if (summ > 0)
                {
                    //accToCharge.Amount += summ;
                    //accChargeAction = accToCharge;
                    accChargeAction.PushMoneyToAcc(summ);
                    if (accActions.SaveAcc(accToCharge))
                    {
                        refreshAccFields();
                        LogAction($"push money to acc {accToCharge.AccNumber}, summ = {summ}");
                        MessageBox.Show($"Пополнение счета выполнено успешно.");
                    }
                    else
                    {
                        accToCharge.Amount -= summ;
                        MessageBox.Show($"Не удалось сохранить данные счета.");
                    }
                }
            }
        }
        void myLogAction(string msg)
        {
            Log.Information(msg);
        }
    }
}
