using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    // ковариантный интерфейс
    public interface IPushMoney<out T> where T : BankAccBase
    {
        T PushMoneyToAcc(decimal summ);
    }
}
