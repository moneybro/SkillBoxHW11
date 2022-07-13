using ClassLibrary.Interfaces;
using ClassLibrary.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class BankAccForClient : BankAccBase, IPushMoney<BankAccBase>
    {
        #region поля
        long _clientId;

        #endregion

        #region свойства
        public long ClientId { get { return _clientId; } set { _clientId = value; } }

        public BankAccBase PushMoneyToAcc(decimal summ)
        {
            this.Amount += summ;
            return this;
        }

        public static decimal operator +(BankAccForClient? acc1, BankAccForClient? acc2)
        {
            decimal a1 = acc1 != null ? acc1.Amount : 0;
            decimal a2 = acc2 != null ? acc2.Amount : 0;
            return a1 + a2;
        }
        #endregion
    }
}
