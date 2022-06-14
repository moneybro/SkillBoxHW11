using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBankAcc
{
    internal class BankAcc : Bank
    {
        //public BankAcc()
        //{
        //    base.Amount = this.Amount;
        //}

        long _clientId;
        long _accNumber;
        //decimal _amount;
        bool _active;
        DateTime _createDate;
        DateTime _updateDate;
        public long ClientId {
            get { return _clientId; }
            set { _clientId = value; }
        }
        public long AccNumber
        {
            get { return _accNumber; }
            set { _accNumber = value; }
        }
        //public decimal Amount
        //{
        //    get { return _amount; }
        //    set { _amount = value; }
        //}
        public bool Active { get { return _active; } set { _active = value; } }
        public DateTime CreateDate { get { return _createDate; } set { _createDate = value; } }
        public DateTime UpdateDate { get { return _updateDate; } set { _updateDate = value; } }

        public override string ToString()
        {
            return $"ClientId: {ClientId}\n" +
                $"AccNumber: {AccNumber}\n" +
                $"Amount: {Amount}\n" +
                $"Active: {Active}\n" +
                $"Create date: {CreateDate}\n";
        }
    }
}
