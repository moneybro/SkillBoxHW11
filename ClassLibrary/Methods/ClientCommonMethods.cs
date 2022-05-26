using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Classes;

namespace ClassLibrary.Methods
{
    public static class ClientCommonMethods
    {
        public static Client GetClientById(long clientId)
        {
            return GetClientsAllData().Find(c => c.ID == clientId);
        }
        public static List<Client> GetClientsAllData()
        {
            var c = getClientsRawData();
            return filteredClients(c);
        }

        static List<Client> getClientsRawData()
        {
            var clientsDbPath = DbPaths.getClientsPath();

            if (!File.Exists(clientsDbPath)) File.CreateText(clientsDbPath).Close();
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
            }
            return clients;
        }
        static List<Client> filteredClients(List<Client> clients)
        {
            // блок предназначен для выбора клиента по ID, и того, который был отредактирован последним (с этим ID)
            // также исключаем удаленных
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
                cl.RemoveAll(c => c.Status == "deleted");
            }
            return cl;
        }

        public static long getNewClientId()
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
    }
}
