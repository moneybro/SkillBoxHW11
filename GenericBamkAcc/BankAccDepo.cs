using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GenericBankAcc
{
    internal class BankAccDepo : BankAcc, IBank<BankAcc>
    {
        [JsonConstructor]
        internal BankAccDepo(
            long _clientId,
            long _accNumber,
            decimal _amount,
            bool _active,
            DateTime _createDate,
            DateTime _updateDate
        )
        {
            this.ClientId = _clientId;
            this.AccNumber = _accNumber;
            this.Amount = _amount;
            this.Active = _active;
            this.CreateDate = _createDate;
            this.UpdateDate = _updateDate;
        }

        bool _isDepo = true;
        public bool IsDepo { get { return _isDepo; } }
        public override string ToString()
        {
            return $"ClientId: {ClientId}\n" +
                $"AccNumber: {AccNumber}\n" +
                $"Acc type: Depo\n" +
                $"Amount: {Amount}\n" +
                $"Active: {Active}\n" +
                $"Create date: {CreateDate}\n";
        }

        public BankAcc PushMoneyToAcc(decimal summ)
        {
            this.Amount += summ + (summ * 10 / 100);
            return this;
        }
    }
}
