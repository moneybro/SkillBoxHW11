using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Classes;
using ClassLibrary.Methods;
using EmployeeApp.Views;

namespace EmployeeApp
{
    public class Manager : Employee
    {
        public Manager()
        {
            this.FirstName = default;
            this.LastName = default;
            this.Age = default;
            this.Salary = default;
        }
        public Manager(string fn, string ln, int age, int salary)
        {
            this.FirstName = fn;
            this.LastName = ln;
            this.Age = age;
            this.Salary = salary;
        }
        public override string Type => "manager";
        public override Client AddNewClient()
        {
            Client newClient;
            long newClID = ClientCommonMethods.getNewClientId();
            EditClient createNewClientWin = new EditClient(newClID);
            if (createNewClientWin.ShowDialog() == true)
            {
                newClient = createNewClientWin.editedClient;
                if (newClient != null)
                {
                    createNewClientWin.Close();
                    base.SaveEditedClient(newClient, this);
                    return newClient;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public override bool ChangeClient(Client _client)
        {
            //Client client = getClientById(_client);
            EditClient editClient = new EditClient(_client, this);
            if (editClient.ShowDialog() == true) _client = editClient.editedClient;
            editClient.Close();
            return base.SaveEditedClient(_client, this);
        }
        public override bool DeleteClient(Client client)
        {
            Client clientToDel = client;
            if (clientToDel != null)
            {
                clientToDel.Status = "deleted";
                if (base.SaveEditedClient(clientToDel, this))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        //public override List<Client> DeleteClient(List<Client> clients, long client)
        //{
        //    Client clientToDel = clients.Find(c => c.ID == client);
        //    if (clientToDel != null)
        //    {
        //        clientToDel.Status = "deleted";
        //        if (base.SaveEditedClient(clientToDel, this))
        //        {
        //            clients.RemoveAll(c => c.ID == client);
        //            return clients;
        //        }
        //        else
        //        {
        //            return clients;
        //        }
        //    }
        //    else
        //    {
        //        return clients;
        //    }
        //}
        public override List<Client> GetClients()
        {
            return ClientCommonMethods.GetClientsAllData();
        }
        //public override List<Client> RefreshClientsView(List<Client> clients)
        //{
        //    return clients;
        //}
    }
}
