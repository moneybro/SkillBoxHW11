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

        internal Client _workClient;
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

        SummToPutStorage summToPutStorage;
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
            _workClient = cl;
            _clientPage = clientPage;
            _workClient.LastName = cl.LastName;
            _workClient.FirstName = cl.FirstName;
            _workClient.Patronymic = cl.Patronymic;
            _workClient.MobPhone = cl.MobPhone;
            _workClient.PaspSeria = cl.PaspSeria;
            _workClient.PaspNum = cl.PaspNum;
            _workClient.LastChangeDate = DateTime.Now;
            fullName = $"{LastName} {FirstName} {Patronymic}";
            accList = _workClient.BankAccActions.GetClientAccs(cl.ID);
            mainAcc = (BankAccMain?)accList.Find(a => a.GetType() == typeof(BankAccMain));
            depoAcc = (BankAccDepo?)accList.Find(a => a.GetType() == typeof(BankAccDepo));
            LogAction += myLogAction;
            _clientPage.OpenMainAccBtnClicked += createNewMainAcc;
            _clientPage.OpenDepoAccBtnClicked += createNewDepoAcc;
            _clientPage.CloseAccBtnClicked += closeAcc;
            _clientPage.AddMoneyBtnClicked += putMoney;
            _clientPage.TfrMoneyBtnClicked += tfrMoney;
            summToPutStorage = new SummToPutStorage();
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
                BankAccMain bankAcc = _workClient.BankAccActions.GetNewMainAcc(this.ID);
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
        internal void createNewDepoAcc()
        {
            if (depoAcc == null)
            {
                var bankAcc = _workClient.BankAccActions.GetNewDepoAcc(this.ID);

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
        internal void closeAcc(BankAccForClient acc)
        {
            var accNumToClose = acc;
            if (acc != null && _workClient.BankAccActions.CloseAcc(acc.AccNumber))
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
        }/// <summary>
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
        internal void putMoney(object sender)
        {
            IPushMoney<BankAccBase> accChargeAction = mainAcc;

            var s = (Button)sender;
            var senderName = s.Name;
            BankAccForClient? accToCharge = new BankAccForClient();
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
                new PutMoneyWin(modernAccView(accToCharge.AccNumber), accToCharge.Amount.ToString(), summToPutStorage);

            if (putMoneyWin.ShowDialog() == true)
            {
                decimal summ = -1;
                decimal.TryParse(putMoneyWin.summTB.Text, out summ);
                var dateTime = DateTime.Now;
                BankAccTransaction bankAccTansaction = new BankAccTransaction();
                if (summ > 0)
                {
                    //accToCharge.Amount += summ;
                    //accChargeAction = accToCharge;
                    accChargeAction.PushMoneyToAcc(summ);
                    if (_workClient.BankAccActions.SaveAcc(accToCharge, dateTime))
                    {
                        refreshAccFields();


                        bankAccTansaction.Date = dateTime;
                        //bankAccTansaction.TransType = BankAccTansaction.TransactionType.Income;
                        bankAccTansaction.AccNumberSource = 0;
                        bankAccTansaction.AccNumberTarget = accToCharge.AccNumber;
                        bankAccTansaction.Summ = summ;
                        bankAccTansaction.EmployeeId = 0;
                        bankAccTansaction.ClientId = _workClient.ID;
                        bankAccTansaction.OperatorName = $"{_workClient.LastName} {_workClient.FirstName}";
                        bankAccTansaction.Description = "";
                        _workClient.TransactionsActions.SaveTransaction(bankAccTansaction);



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
                    var dateTime = DateTime.Now;
                    var transfeSuccess = transferStorage.TransferMoney(summ, dateTime);
                    BankAccTransaction bankAccTansaction = new BankAccTransaction();
                    if (transfeSuccess)
                    {
                        refreshAccFields();


                        bankAccTansaction.Date = dateTime;
                        bankAccTansaction.AccNumberSource = long.Parse(accSourceNumber);
                        bankAccTansaction.AccNumberTarget = long.Parse(accTargetNumber);
                        bankAccTansaction.Summ = summ;
                        bankAccTansaction.EmployeeId = 0;
                        bankAccTansaction.ClientId = _workClient.ID;
                        bankAccTansaction.OperatorName = $"{_workClient.LastName} {_workClient.FirstName}";
                        bankAccTansaction.Description = "перевод между счетами";
                        _workClient.TransactionsActions.SaveTransaction(bankAccTansaction);


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
        void myLogAction(string msg)
        {
            Log.Information(msg);
        }
    }
}
