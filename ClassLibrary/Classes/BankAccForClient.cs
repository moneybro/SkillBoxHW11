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

        #endregion
    }
}
