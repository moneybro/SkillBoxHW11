using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SkillBoxHW11
{
    public abstract class Employee
    {
        public abstract string Type { get; }
        public abstract List<Client> GetClients();
        public abstract bool AddNewClient();
        public abstract bool ChangeClient(long client);
        public abstract List<Client> RefreshClientsView(List<Client> clientsShort);
        protected bool SaveEditedClient(Client client, Employee employee)
        {
            client.ChangeType = "edited";
            client.EmployeeType = employee.Type;
            client.LastChangeDate = DateTime.Now;
            if (client.Status == null)
            {
                client.Status = "active";
            }
            // todo: какие данные изменены будут определяться сравнением (в будущем)
            string clientsPath = Path.Combine("clients.json");
            var options = new JsonSerializerOptions { WriteIndented = true };
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
        public abstract List<Client> DeleteClient(List<Client> list, long client);
        protected virtual Client getClientById(long _id)
        {
            return CommonMethods.GetClientsAllData().Find(c => c.ID == _id);
        }

        
    }
}
