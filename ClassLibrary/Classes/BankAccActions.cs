using ClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class BankAccActions :
        IGetAllBankAccs,
        IGetClientAccs,
        IBankAccCreateNewMain,
        IBankNewAccGetNumber,
        ISaveAcc,
        IBankAccClose,
        ITransferMoney
    {
        BankAccActionsJSON actions = new BankAccActionsJSON();
        public BankAccMain GetNewMainAcc(long clId)
        {
            return actions.GetNewMainAcc(clId);
        }
        public BankAccDepo GetNewDepoAcc(long clId)
        {
            return actions.GetNewDepoAcc(clId);
        }
        public long GetNewAccNumber()
        {
            return actions.GetNewAccNumber();
        }
        public bool SaveAcc<T>(T bankAcc)
        {
            return actions.SaveAcc(bankAcc);
        }
        public bool SaveAcc<T>(T bankAcc, string repoPath)
        {
            return actions.SaveAcc(bankAcc, repoPath);
        }
        public bool CloseAcc(long accNum)
        {
            return actions.CloseAcc(accNum);
        }
        public List<BankAcc> GetAllBankAccs()
        {
            return actions.GetAllBankAccs();
        }
        public List<BankAcc> GetClientAccs(long clId)
        {
            return actions.GetClientAccs(clId);
        }

        public bool transferMoney<T>(T acc1, T acc2, decimal summ) where T : BankAcc
        {
            if (acc1 != null && acc2 != null && summ != 0)
            {
                acc1.Amount = acc1.Amount - summ;
                acc2.Amount = acc2.Amount + summ;
                if(SaveAcc(acc1) && SaveAcc(acc2)) return true;
                else return false;
            }
            else return false;
        }
    }
}
