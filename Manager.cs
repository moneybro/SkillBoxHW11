using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkillBoxHW11
{
    internal class Manager : Employee
    {
        public override string Type => "manager";

        public override bool AddNewClient()
        {
            Client newClient;
            long newClID = CommonMethods.getNewClientId();
            EditClient createNewClientWin = new EditClient(newClID);
            if (createNewClientWin.ShowDialog() == true)
            {
                newClient = createNewClientWin.editedClient;
                if (newClient != null)
                {
                    createNewClientWin.Close();
                    base.SaveEditedClient(newClient, this);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            { 
                newClient = null;
                return false;
            }
        }

        public override bool ChangeClient(long _client)
        {
            Client client = getClientById(_client);
            EditClient editClient = new EditClient(client, this);
            if (editClient.ShowDialog() == true) client = editClient.editedClient;
            editClient.Close();
            return base.SaveEditedClient(client, this);
        }

        public override List<Client> DeleteClient(List<Client> clients, long client)
        {
            Client clientToDel = clients.Find(c => c.ID == client);
            if (clientToDel != null)
            {
                clientToDel.Status = "deleted";
                if (base.SaveEditedClient(clientToDel, this))
                {
                    clients.RemoveAll(c => c.ID == client);
                    return clients;
                }
                else
                {
                    return clients;
                }
            }
            else
            {
                return clients;
            }
        }

        public override List<Client> GetClients()
        {
            return CommonMethods.GetClientsAllData();
        }

        public override List<Client> RefreshClientsView(List<Client> clients)
        {
            return clients;
        }
    }
}
