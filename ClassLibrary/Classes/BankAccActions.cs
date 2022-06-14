﻿using ClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class BankAccActions :
        IBankAccActions
        //,IBankAccGetByAccNum
    {
        BankAccActionsJSON actions = new BankAccActionsJSON();
        public List<BankAccForClient> GetClientAccs(long clId)
        {
            return actions.GetClientAccs(clId);
        }
        public BankAccMain GetNewMainAcc(long clId)
        {
            return actions.GetNewMainAcc(clId);
        }
        public BankAccDepo GetNewDepoAcc(long clId)
        {
            return actions.GetNewDepoAcc(clId);
        }
        public BankAccForClient GetAccByNum(long accNumber)
        {
            return actions.GetAccByNum(accNumber);
        }
        public bool SaveAcc<T>(T bankAcc) where T : BankAccForClient
        {
            return actions.SaveAcc(bankAcc);
        }
        public bool SaveAcc<T>(T bankAcc, string repoPath) where T : BankAccForClient
        {
            return actions.SaveAcc(bankAcc, repoPath);
        }
        public bool CloseAcc(long accNum)
        {
            return actions.CloseAcc(accNum);
        }
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
