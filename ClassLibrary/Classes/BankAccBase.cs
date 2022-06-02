using ClassLibrary.Interfaces;
using ClassLibrary.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes 
{
    public class BankAccBase : IBankAccActions
    {
        #region поля
        long _clientId;
        long _accNumber;
        decimal _amount;
        bool _active;
        DateTime _createDate = DateTime.Now;
        DateTime _updateDate;

        #endregion

        #region свойства
        public long ClientId { get { return _clientId; } set { _clientId = value; } }
        public long AccNumber
        {
            get { return _accNumber; }
            set { _accNumber = value; }
        }
        public decimal Amount 
        { get { return _amount; }
            set { _amount = value; }
        }
        public bool Active { get { return _active; } set { _active = value; } }
        public DateTime CreateDate { get { return _createDate; } set { _createDate = value; } }
        public DateTime UpdateDate { get { return _updateDate; } set { _updateDate = value; } }
        #endregion
    }
}
