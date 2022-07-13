using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class BankAccTransaction
    {
        public DateTime Date { get; set; }
        //public TransactionType TransType { get; set; }
        public long AccNumberSource { get; set; } // номер счета источника
        public long AccNumberTarget { get; set; } // номер счета приемника
        public decimal Summ { get; set; }



        // кто выполнил операция пополнения сотрудник или клиент
        public int EmployeeId { get; set; } 
        public long ClientId { get; set; }
        public string OperatorName { get; set; }
        // конец ------ кто выполнил операция пополнения сотрудник или клиент



        public string Description { get; set; }

        //public enum TransactionType
        //{
        //    Income, // приход
        //    Outgo   // расход
        //}
    }
}
