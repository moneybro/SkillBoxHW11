using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Unicode;
using ClassLibrary.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Classes
{
    public class Client : IEquatable<Client?>, IComparable<Client>
    {
        public long ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string? MobPhone { get; set; }
        public int? PaspSeria { get; set; }
        public long? PaspNum { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public string? ChangeType { get; set; }
        public string? EmployeeType { get; set; }
        public string? Status { get; set; } // активен = active, удален = deleted и т.п.
        [NotMapped]
        public IBankAccActions BankAccActions { get; set; }
        [NotMapped]
        public ITransactionsActions TransactionsActions { get; set; }

        // пустой конструктор для десирализации, так как если не прописать инициализацию интерфейсов, то оно ругается на то, что нельзя создать объект интерфейса или абстрактного класса
        public Client()
        {
            this.BankAccActions = new BankAccActions();
            this.TransactionsActions = new BankAccActions();
        }
        public Client(long ID)
        {
            this.ID = ID;
            this.BankAccActions = new BankAccActions();
            this.TransactionsActions = new BankAccActions();
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Client);
        }
        public bool Equals(Client? other)
        {
            return other != null &&
                   ID == other.ID;
        }
        public override string ToString()
        {
            return $"ФИО: {LastName} {FirstName} {Patronymic}, мобильный телефон: {MobPhone}";
        }
        public Client Clone()
        {
            return (Client)MemberwiseClone();
        }
        public int CompareTo(Client? other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;

            else
                return this.ID.CompareTo(other.ID);
        }
    }
}
