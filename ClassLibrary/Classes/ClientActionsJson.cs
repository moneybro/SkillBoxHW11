using ClassLibrary.Contexts;
using ClassLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class ClientActionsJson : IClientActions
    {
        public List<Client> GetClients()
        {
            List<Client> cl = new List<Client>();
            foreach (var client in getClientsRawData())
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
                cl.RemoveAll(c => c.Status == "deleted");
            }
            return cl;
        }
        public long getNewClientId()
        {
            var cls = getClientsRawData();
            if (cls.Count > 0)
            {
                var clientId = cls.Max(c => c.ID);
                return clientId + 1;
            }
            else
            {
                return 1;
            }
        }
        public bool UpdateClient(Client client)
        {
            var cl = client;
            //cl.LastName = client.LastName;
            //cl.FirstName = client.FirstName;
            //cl.Patronymic = client.Patronymic;
            //cl.MobPhone = client.MobPhone;
            //cl.PaspSeria = client.PaspSeria;
            //cl.PaspNum = client.PaspNum;
            //cl.LastChangeDate = client.LastChangeDate;
            cl.ChangeType = "edited";
            //cl.EmployeeType = client.EmployeeType;
            //cl.Status = client.Status;
            if (writeClientToBase(cl))
            {
                return true;
            }
            else return false;
        }
        public bool DeleteClient(Client client, string employeeType)
        {
            client.Status = "deleted";
            client.EmployeeType = employeeType;
            if (UpdateClient(client))
            {
                return true;
            }
            else return false;
        }

        public bool SaveNewClient(Client client)
        {
            if (writeClientToBase(client))
            {
                return true;
            }
            else return false;
        }


        #region вспомогательные методы для реализации, не из родительского интерфейса
        bool writeClientToBase(Client client)
        {
            var clientsDb = DbPaths.getClientsPath();
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
                    System.Text.Json.JsonSerializer.SerializeAsync(fs, client, options);
                }
                return true;
            }
            else
            {
                using (FileStream fs = new FileStream(clientsPath, FileMode.Create))
                {
                    System.Text.Json.JsonSerializer.SerializeAsync(fs, client, options);
                }
                return true;
            }
        }
        static List<Client> getClientsRawData()
        {
            var clientsDbPath = DbPaths.getClientsPath();
            //if (!Directory.Exists(Path.GetDirectoryName(clientsDbPath)))
            //{
            //    Directory.CreateDirectory(Path.GetDirectoryName(clientsDbPath));
            //}
            //if (!File.Exists(clientsDbPath)) 
            //{ 
            //    File.CreateText(clientsDbPath).Close();
            //};
            var ser = new Newtonsoft.Json.JsonSerializer();
            var streamreader = new StreamReader(clientsDbPath, new UTF8Encoding());
            List<Client> clients = new List<Client>();

            using (var reader = new JsonTextReader(streamreader))
            {
                while (reader.Read())
                {
                    reader.CloseInput = false;
                    reader.SupportMultipleContent = true;
                    clients.Add(ser.Deserialize<Client>(reader));
                }
                reader.Close();
                streamreader.Close();
            }

            return clients;
        }
        #endregion
    }
}
