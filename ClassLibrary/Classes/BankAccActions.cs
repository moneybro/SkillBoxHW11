using ClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class BankAccActions :
        IBankAccActions,
        ITransactionsActions,
        IBankAccGetByAccNum
    {
        internal IBankAccActions bankAccActions { get; set; }
        internal ITransactionsActions transactionsActions { get; set; }

        public BankAccActions()
        {
            if (GlobalVarsAndActions.StorageType == "json")
            {
                var jsconActions = new BankAccActionsJSON();
                bankAccActions = jsconActions;
                transactionsActions = jsconActions;
            }
            if (GlobalVarsAndActions.StorageType == "mssql")
            {
                var mssqlActions = new BankAccActionsSql();
                bankAccActions = mssqlActions;
                transactionsActions = mssqlActions;
            }
        }

        #region работа со счетами IBankAccActions
        public BankAccMain GetNewMainAcc(long clId)
        {
            return bankAccActions.GetNewMainAcc(clId);
        }
        public BankAccDepo GetNewDepoAcc(long clId)
        {
            return bankAccActions.GetNewDepoAcc(clId);
        }
        public List<BankAccForClient> GetClientAccs(long clId)
        {
            return bankAccActions.GetClientAccs(clId);
        }
        public BankAccForClient GetAccByNum(long accNumber)
        {
            return bankAccActions.GetAccByNum(accNumber);
        }
        public bool SaveAcc<T>(T bankAcc, DateTime dateTime) where T : BankAccForClient
        {
            return bankAccActions.SaveAcc(bankAcc, dateTime);
        }
        public bool SaveAcc<T>(T bankAcc, string repoPath, DateTime createDateTime, DateTime updateDateTime) where T : BankAccForClient
        {
            return bankAccActions.SaveAcc(bankAcc, repoPath, createDateTime, updateDateTime);
        }
        public bool CloseAcc(long accNum)
        {
            return bankAccActions.CloseAcc(accNum);
        }

        #region получение транзакций по номеру счета
        public List<BankAccForClient> GetAccTransactions(long accNum)
        {
            return bankAccActions.GetAccTransactions(accNum);
        }
        #endregion
        #endregion

        #region работа с транзакциями ITransactionsActions
            #region сохранение транзакции
            public bool SaveTransaction(BankAccTransaction tr)
            {
                return transactionsActions.SaveTransaction(tr);
            }
            #endregion
            #region получение полных транзакций по номеру счета
            public List<BankAccTransactionFull> GetBankAccTransactionsFull(long accNum)
            {
                return transactionsActions.GetBankAccTransactionsFull(accNum);
            }
            #endregion
        #endregion

        //public bool transferMoney<T>(T acc1, T acc2, decimal summ) where T : BankAccForClient
        //{
        //    if (acc1 != null && acc2 != null && summ != 0)
        //    {
        //        acc1.Amount = acc1.Amount - summ;
        //        acc2.Amount = acc2.Amount + summ;
        //        if(SaveAcc(acc1) && SaveAcc(acc2)) return true;
        //        else return false;
        //    }
        //    else return false;
        //}
    }
}
