using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBoxHW11
{
    internal class Manager : Employee
    {
        protected override string Type => "manager";

        public override Client ChangeClient(Client _client)
        {
            Client client = _client;
            EditClient editClient = new EditClient(client, this);
            if (editClient.ShowDialog() == true) client = editClient.editedClient;
            editClient.Close();
            base.SaveEditedClient(client, this);
            return _client;
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
