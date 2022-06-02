using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    internal interface IBankAccActions
    {
        internal interface IBankAccClose
        {
            public bool CloseAcc(long accNum);
        }

        internal interface IGetAllBankAccs
        {
            List<BankAcc> GetAllBankAccs();
        }
        public interface IBankAccCreateNew
        {
            BankAcc GetNewBankAcc();
        }
        public interface IBankAccCreateNewMain
        {
            BankAccMain GetNewMainAcc(long clId);
        }
        interface IBankAccGetNumberById
        {
            BankAcc GetAccNumberById(long accNumber);
        }
        public interface IBankNewAccGetNumber
        {
            public long GetNewAccNumber();
        }
        internal interface IGetClientAccs
        {
            List<BankAcc> GetClientAccs(long clId);
        }
        internal interface ISaveAcc
        {
            bool SaveAcc<T>(T acc, string repoPath);
        }
        internal interface ITransferMoney
        {
            bool transferMoney<T>(T acc1, T acc2, decimal summ) where T : BankAcc;
        }
    }
}
