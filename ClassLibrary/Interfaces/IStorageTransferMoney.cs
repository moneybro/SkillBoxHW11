using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    // контрвариантный интерфейс
    public interface IStorageTransferMoney<in T> where T : BankAccBase
    {
        T addAcc { set; }
        bool TransferMoney(decimal summ);
    }
}
