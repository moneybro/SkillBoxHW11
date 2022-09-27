using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class BankAccTransactionFull
    {
        public BankAccTransaction Tr { get; set; }
        public BankAccForClient Acc { get; set; }

        public override string ToString()
        {
            string fullString;
            string descr = "";
            

            // пополнение и перевод сюда
            if (Acc.AccNumber == Tr.AccNumberTarget)
            {
                if (Acc.AccNumber == Tr.AccNumberTarget && Tr.AccNumberSource == 0)
                {
                    descr = $"пополнение выполнил {Tr.OperatorName}";
                }
                else
                {
                    descr = $"перевод со счета {Tr.AccNumberSource} выполнил {Tr.OperatorName}";
                }
            }


            // перевод отсюда
            if (Acc.AccNumber == Tr.AccNumberSource && Tr.AccNumberTarget != 0)
            {
                descr = $"перевод на счет {Tr.AccNumberTarget}";
            }

            var trString = $"ID:{Tr.Id} {Acc.UpdateDate} {descr}, сумма {Tr.Summ}, баланс: {Acc.Amount}"; // отладочная строка
            //var trString = $"ID:{Tr.Id} {Acc.UpdateDate} {descr}, сумма {Tr.Summ}, баланс: {Acc.Amount}"; // отладочная строка

            return trString;
        }
    }
}
