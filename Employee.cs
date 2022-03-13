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
        protected abstract string Type { get; }
        public abstract List<Client> GetClients();
        public abstract Client ChangeClient(Client client);
        public abstract List<Client> RefreshClientsView(List<Client> clientsShort);
        public void DeleteClient(Client client) { }
        protected bool SaveEditedClient(Client client, Employee employee)
        {
            client.ChangeType = "edited";
            client.EmployeeType = employee.Type;
            client.LastChangeDate = DateTime.Now;
            if (client.Status == null)
            {
                client.Status = "active";
            }
            // какие данные изменены будут определяться сравнением
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
        protected virtual List<Client> getClientsAllData()
        {
            var ser = new Newtonsoft.Json.JsonSerializer();
            var streamreader = new StreamReader("clients.json", new UTF8Encoding());
            List<Client> clients = new List<Client>();

            using (var reader = new JsonTextReader(streamreader))
            {
                while (reader.Read())
                {
                    reader.CloseInput = false;
                    reader.SupportMultipleContent = true;
                    clients.Add(ser.Deserialize<Client>(reader));
                }
            }
            //return clients;
            List<Client> cl = new List<Client>();
            foreach (var client in clients)
            {
                if (!cl.Contains(client))
                {
                    cl.Add(client);
                }
                else
                {
                    var oldCl = cl.Find(c => c.ID == client.ID);
                    if (oldCl.LastChangeDate == null || client.LastChangeDate > oldCl.LastChangeDate)
                    {
                        cl.Remove(oldCl);
                        cl.Add(client);
                    }
                }
            }
            return cl;
        }

        
    }
}
