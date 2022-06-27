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

namespace EmployeeApp
{
    public abstract class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public abstract string Type { get; }
        public abstract List<Client> GetClients();
        public abstract Client AddNewClient();
        public abstract bool ChangeClient(Client client);
        public IBankAccActions BankAccActions { get; set; }

        /// <summary>
        /// сохранить измененного клиента
        /// </summary>
        /// <param name="client"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        protected bool SaveEditedClient(Client client, Employee employee)
        {
            var clientsDb = DbPaths.getClientsPath();
            client.ChangeType = "edited";
            client.EmployeeType = employee.Type;
            client.LastChangeDate = DateTime.Now;
            if (client.Status == null)
            {
                client.Status = "active";
            }
            // todo: какие данные изменены будет определяться сравнением (в будущем)
            string clientsPath = Path.Combine(clientsDb);
            var options = new JsonSerializerOptions 
            {
                WriteIndented = true, //фрматированный json
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            if (File.Exists(clientsPath))
            {
                using (FileStream fs = new FileStream(clientsPath, FileMode.Append))
                {
                    System.Text.Json.JsonSerializer.SerializeAsync<Client>(fs, client, options);
                }
                return true;
            }
            else
            {
                using (FileStream fs = new FileStream(clientsPath, FileMode.Create))
                {
                    System.Text.Json.JsonSerializer.SerializeAsync<Client>(fs, client, options);
                }
                return true;
            }
            return false;
        }
        public abstract bool DeleteClient(Client client);
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
                Employee X = (Employee)x;
                Employee Y = (Employee)y;

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
                Employee? X = (Employee)x;
                Employee? Y = (Employee)y;

                if (X.Age == Y.Age) return 0;
                else if (X.Age > Y.Age) return 1;
                else return -1;
            }
        }
    }
}
