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

namespace ClientApp.ViewModels
{
    internal class WorkClient : Client, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        internal List<BankAcc> accList;
        internal BankAcc? mainAcc;
        internal BankAcc? depoAcc;
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

        BankAccActions accActions;
        internal WorkClient(Client cl) : base(ID: cl.ID)
        {
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
            mainAcc = accList.Find(a => a.GetType() == typeof(BankAccMain));
            depoAcc = accList.Find(a => a.GetType() == typeof(BankAccDepo));
        }

        void refreshAccFields()
        {
                MainAccNumber += 0;
                MainAccAmount += 0;
                MainAccIsNotNull = MainAccIsNotNull;
                DepoAccNumber += 0;
                DepoAccAmount += 0;
                DepoAccIsNotNull = DepoAccIsNotNull;
        }
        internal BankAccMain createNewMainAcc()
        {
            if(mainAcc == null)
            {
                BankAccMain bankAcc = accActions.GetNewMainAcc(this.ID);
                if (bankAcc != null)
                {
                    accList.Add(bankAcc);
                    mainAcc = bankAcc;
                    refreshAccFields();
                }
                else
                {
                    return null;
                }
                return bankAcc;
            }
            return null;
        }
        internal bool closeAcc(BankAcc acc)
        {
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
                return true;
            }
            else return false;
        }
        internal BankAccDepo createNewDepoAcc()
        {
            if (depoAcc == null)
            {
                var bankAcc = accActions.GetNewDepoAcc(this.ID);

                if (bankAcc != null)
                {
                    depoAcc = bankAcc; 
                    accList.Add(depoAcc);
                    refreshAccFields();
                }
                else
                {
                    return null;
                }
                return bankAcc;
            }
            return null;
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
        internal bool tfrMoney(BankAcc accSource, BankAcc accTarget)
        {
            var success = accActions.transferMoney(accSource, accTarget, this.summ);
            if (success)
            {
                refreshAccFields();
                return true;
            }
            return false;
        }
        internal bool pushMoneyToAcc(BankAcc acc)
        {
            if (acc != null && summ > 0)
            {
                acc.Amount += this.summ;
                if (accActions.SaveAcc(acc))
                {
                    refreshAccFields();
                    return true;
                }
                else
                {
                    acc.Amount -= summ;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
