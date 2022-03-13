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

        public Client(long ID)
        {
            this.ID = ID;
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
            return $"LastName: {LastName} | Name: {Name} | Patronimyc: {Patronymic} | mobphone: {MobPhone}";
        }
    }
}
