using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using ClassLibrary.Methods;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmployeeApp.Classes
{
    public abstract class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public abstract string Type { get; }
        public abstract Client AddNewClient();
        public abstract bool ChangeClient(Client client);
        public abstract bool DeleteClient(Client client);
        public abstract List<Client> GetClients();
        public bool CanAddRemoveDepoAcc { get; set; }

        // работа со счетами, метод нужен для обеспечения разрешения на доступ к общему методу в интерфейсе
        public abstract BankAccDepo GetNewDepoAcc(long clId);

        EmployeeActions employeeActions = new EmployeeActions(GlobalVarsAndActions.StorageType);
        internal EmployeeActions EmployeeActions 
        {
            get { return employeeActions; }
        }

        public enum SortedCriterion
        {
            Age,
            LastName
        }
        public static IComparer<Employee> SortedBy(SortedCriterion criterion)
        {
            if (criterion == SortedCriterion.Age) return new SortedByAge();
            if (criterion == SortedCriterion.LastName) return new SortedByLastName();
            return new SortedByLastName();
        }

        /// <summary>
        /// сортировка по фамилии
        /// </summary>
        class SortedByLastName : IComparer<Employee>
        {
            public int Compare(Employee? x, Employee? y)
            {
                Employee X = x;
                Employee Y = y;

                return x.LastName.CompareTo(y.LastName);
            }
        }
        /// <summary>
        /// сортировка по возрасту
        /// </summary>
        class SortedByAge : IComparer<Employee>
        {
            public int Compare(Employee? x, Employee? y)
            {
                Employee? X = x;
                Employee? Y = y;

                if (X.Age == Y.Age) return 0;
                else if (X.Age > Y.Age) return 1;
                else return -1;
            }
        }
        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }
    }
}
