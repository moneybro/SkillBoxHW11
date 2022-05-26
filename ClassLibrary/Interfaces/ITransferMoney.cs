using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    internal interface ITransferMoney
    {
        bool transferMoney<T>(T acc1, T acc2, decimal summ) where T : BankAcc;
    }
}
