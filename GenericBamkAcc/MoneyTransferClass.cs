using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBankAcc
{
    internal class MoneyTransferClass
    {
        internal void transferMoney<T> (T acc1, T acc2, decimal summ) where T : BankAcc
        {
            acc1.Amount = acc1.Amount - summ;
            acc2.Amount = acc2.Amount + summ;   
        }
    }
}
