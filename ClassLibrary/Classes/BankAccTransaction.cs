using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class BankAccTransaction
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime Date { get; set; }
        //public TransactionType TransType { get; set; }
        public long AccNumberSource { get; set; } // номер счета источника
        public long AccNumberTarget { get; set; } // номер счета приемника
        public decimal Summ { get; set; }



        // кто выполнил операцию пополнения сотрудник или клиент
        public int EmployeeId { get; set; } 
        public long ClientId { get; set; }
        public string OperatorName { get; set; }
        // конец ------ кто выполнил операцию пополнения сотрудник или клиент



        public string Description { get; set; }

        //public enum TransactionType
        //{
        //    Income, // приход
        //    Outgo   // расход
        //}
    }
}
