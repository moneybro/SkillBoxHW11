using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Classes;
using ClassLibrary.Classes.Exceptions;
using ClassLibrary.Methods;
using EmployeeApp.Views;

namespace EmployeeApp.Classes
{
    public class Consultant : Employee
    {
        public Consultant()
        {
            FirstName = default;
            LastName = default;
            Age = default;
            Salary = default;
            CanAddRemoveDepoAcc = false;
        }
        public Consultant(int id, string fn, string ln, int age, int salary)
        {
            Id = id;
            FirstName = fn;
            LastName = ln;
            Age = age;
            Salary = salary;
            CanAddRemoveDepoAcc = false;
        }
        public override string Type => "consultant";
        public override Client AddNewClient()
        {
            return null;
        }
        public override bool ChangeClient(Client _client)
        {
            EditClientPage editClient = new EditClientPage(_client, this);
            if (editClient.ShowDialog() == true) editClient.Close();
            return EmployeeActions.ClientActions.UpdateClient(_client);
        }
        public override bool DeleteClient(Client client)
        {
            return false;
        }
        public override List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            clients.AddRange(EmployeeActions.ClientActions.GetClients());
            //for (int i = 0; i < clients.Count; i++)
            //{
            //    clients[i].PaspSeria = 0;
            //    clients[i].PaspNum = 0;
            //}
            return clients;
        }

        public override BankAccDepo GetNewDepoAcc(long clId)
        {
            throw new EmployeeExeption(1);
        }
    }
}
