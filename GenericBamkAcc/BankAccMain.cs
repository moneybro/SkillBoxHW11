using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBankAcc
{
    internal class BankAccMain : BankAcc, IBank<Bank>
    {
        [JsonConstructor]
        internal BankAccMain(
             long _clientId,
             long _accNumber,
             decimal _amount,
             bool _active,
             DateTime _createDate,
             DateTime _updateDate,
             string Name
         )
        {
            this.ClientId = _clientId;
            this.AccNumber = _accNumber;
            this.Amount = _amount;
            this.Active = _active;
            this.CreateDate = _createDate;
            this.UpdateDate = _updateDate;
            this.Name = Name;
        }

        bool _isDepo = false;
        string _name = "NameNameNameName";
        public bool IsDepo { get { return _isDepo; } }
        public string Name { get { return _name; } set { _name = value; } }
        public void printNoDepo()
        {
            Console.WriteLine("no depo acc");
        }
        public override string ToString()
        {
            return $"ClientId: {ClientId} \n" +
                $"AccNumber: {AccNumber} \n" +
                $"Acc type: No depo \n" +
                $"Amount: {Amount} \n" +
                $"Active: {Active} \n" +
                $"Create date: {CreateDate} \n";
        }

        public Bank PushMoneyToAcc(decimal summ)
        {
            this.Amount += summ;
            return this;
        }
    }
}
