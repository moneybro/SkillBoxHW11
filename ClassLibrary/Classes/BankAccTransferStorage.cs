using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Interfaces;

namespace ClassLibrary.Classes
{
    public class BankAccTransferStorage<T> : IStorageTransferMoney<T> where T : BankAccBase
    {
        List<T> db;
        BankAccActions accActions = new BankAccActions();
        public BankAccTransferStorage()
        {
            db = new List<T>();
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

        public bool TransferMoney(decimal summ)
        {
            if (db.Count == 2)
            {
                var accSource = accActions.GetAccByNum(db[0].AccNumber);
                var accTarget = accActions.GetAccByNum(db[1].AccNumber);

                db[0].Amount -= summ;
                db[1].Amount += summ;
                accSource.Amount -= summ;
                accTarget.Amount += summ;

                // если информация об счетах не сохранилась, то возвращаем баланс 
                var successSource = accActions.SaveAcc(accSource);
                var successTarget = accActions.SaveAcc(accTarget);

                if (!successSource || !successTarget)
                {
                    db[0].Amount += summ;
                    db[1].Amount -= summ;
                    accSource.Amount += summ;
                    accTarget.Amount -= summ;
                    accActions.SaveAcc(accSource);
                    accActions.SaveAcc(accTarget);
                    return false;
                }
                else return true;
                
            }
            else
            {
                return false;
            }
        }
    }
}
