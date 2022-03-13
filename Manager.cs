using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBoxHW11
{
    internal class Manager : Employee
    {
        public override string Type => "manager";

        public override Client ChangeClient(long _client)
        {
            Client client = getClientById(_client);
            EditClient editClient = new EditClient(client, this);
            if (editClient.ShowDialog() == true) client = editClient.editedClient;
            editClient.Close();
            base.SaveEditedClient(client, this);
            return client;
        }

        public override List<Client> GetClients()
        {
            return base.getClientsAllData();
        }

        public override List<Client> RefreshClientsView(List<Client> clients)
        {
            return clients;
        }
    }
}
