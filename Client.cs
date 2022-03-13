using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Unicode;
using Newtonsoft.Json;

namespace SkillBoxHW11
{
    public class Client : IEquatable<Client?>
    {
        public long ID { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string? MobPhone { get; set; }
        public int? PaspSeria { get; set; }
        public long? PaspNum { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public string? ChangeType { get; set; }
        public string? EmployeeType { get; set; }
        public string? Status { get; set; } // активен = active, удален = deleted и т.п.

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
            return $"LastName: {LastName} | Name: {Name} | Patronimyc: {Patronymic} | mobphone: {MobPhone}";
        }

        /// <summary>
        /// сначала сделал этот метод, который проверяет какой оператор смотрит данные и возвращает подготовленные данные
        /// метод перенесен в класс работника в учебных целях
        /// </summary>
        /// <param name="employee">тип работника (консультант или менеджер)</param>
        /// <returns>подготовленный список клиентов</returns>
        //public static List<Client> getClients(Employee employee)
        //{
        //    var ser = new Newtonsoft.Json.JsonSerializer();
        //    var streamreader = new StreamReader("clients.json", new UTF8Encoding());
        //    List<Client> clients = new List<Client>();

        //    using (var reader = new JsonTextReader(streamreader))
        //    {
        //        while (reader.Read())
        //        {
        //            reader.CloseInput = false;
        //            reader.SupportMultipleContent = true;
        //            clients.Add(ser.Deserialize<Client>(reader));
        //        }
        //    }


        //    switch (employee.GetType().Name)
        //    {
        //        case "Consultant":
        //        default:
        //            for (int i = 0; i < clients.Count; i++)
        //            {
        //                clients[i].PaspSeria = 0;
        //                clients[i].PaspNum = 0;
        //            }
        //            break;
        //        case "Manager":
        //            break;
        //    }

        //    return clients;
        //}
    }
}
