using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Classes;
using ClassLibrary.Methods;


namespace SkillBoxHW13
{
    public class Consultant : Employee
    {
        public Consultant()
        {
            this.FirstName = default;
            this.LastName = default;
            this.Age = default;
            this.Salary = default;
        }
        public Consultant(string fn, string ln, int age, int salary)
        {
            this.FirstName = fn;
            this.LastName = ln;
            this.Age = age;
            this.Salary = salary;
        }
        public override string Type => "consultant";
        public override bool AddNewClient()
        {
            return false;
        }

        public override bool ChangeClient(long _client)
        {
            Client client = getClientById(_client);
            EditClient editClient = new EditClient(client, this);
            if(editClient.ShowDialog() == true) client = editClient.editedClient;
            editClient.Close();
            return SaveEditedClient(client, this);
        }

        public override List<Client> DeleteClient(List<Client> clients, long client)
        {
            return null;
        }

        public override List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            clients.AddRange(ClientCommonMethods.GetClientsAllData());
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].PaspSeria = 0;
                clients[i].PaspNum = 0;
            }
            return clients;
        }

        public override List<Client> RefreshClientsView(List<Client> clients)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].PaspSeria = 0;
                clients[i].PaspNum = 0;
            }
            return clients;
        }
    }
}
