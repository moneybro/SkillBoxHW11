using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Interfaces
{
    internal interface IPushMoney<T> where T : BankAccForClient
    {
        T PushMoneyToAcc(decimal summ);
    }
}
