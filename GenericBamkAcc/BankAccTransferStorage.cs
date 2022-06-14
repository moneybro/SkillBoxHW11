using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBankAcc
{
    internal class BankAccTransferStorage<T> : IStorageTransferMoney<T> where T : Bank
    {
        List<Bank> db;

        public BankAccTransferStorage()
        {
            db = new List<Bank>();
        }
        public T addAcc { 
            set
            {
                if (db.Count < 2)
                {
                    if (db.Count == 0) Console.WriteLine($"transfer from acc {value.Amount}");
                    else Console.WriteLine($"transfer to acc {value.Amount}");
                    db.Add(value);
                }
                else
                {
                    Console.WriteLine("two accs added yet! can not more!");
                }
            }
        }

        public void TransferMoney(decimal summ)
        {
            if (db.Count == 2)
            {
                db[0].Amount -= summ;
                db[1].Amount += summ;
            }
        }
    }
}
