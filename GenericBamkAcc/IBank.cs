using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBankAcc
{
    internal interface IBank<out T> where T : Bank
    {
        T PushMoneyToAcc(decimal summ);
    }
}
