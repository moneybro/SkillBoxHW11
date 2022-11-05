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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Classes
{
    public class Client : IEquatable<Client?>, IComparable<Client>
    {
        public long ID { get; set; }
        [DisplayName("Фамилия")]
        public string LastName { get; set; }
        [DisplayName("Имя")]
        public string FirstName { get; set; }
        [DisplayName("Отчество")]
        public string Patronymic { get; set; }
        [DisplayName("Моб. тлф.")]
        public string? MobPhone { get; set; }
        [DisplayName("Серия пасп.")]
        public int? PaspSeria { get; set; }
        [DisplayName("Номер пасп.")]
        public long? PaspNum { get; set; }

        [DisplayName("Посл. изм-е")]
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

        //public Client(Client client)
        //{
        //    this.ID = 0;
        //    this.LastName = client.LastName;
        //    this.FirstName = client.FirstName;
        //    this.
        //}

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

        /// <summary>
        /// копия клиента без ID (с нулевым), для сравнения полей
        /// </summary>
        /// <returns>нового клиента с ID=0</returns>
        public Client getClientCopy()
        {
            var client = (Client)MemberwiseClone();
            client.ID = 0;
            return client;
        }

        /// <summary>
        /// клонируем клиентов для заполнения коллекций и изменением данных, которые не должны сохранится в БД, для отображения коллекции и скрытыми полями
        /// </summary>
        /// <returns></returns>
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
