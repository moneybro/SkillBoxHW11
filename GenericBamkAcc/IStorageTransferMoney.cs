using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBankAcc
{
    internal interface IStorageTransferMoney<in T> where T : Bank
    {
        T addAcc { set; }
        void TransferMoney(decimal summ);
    }
}
