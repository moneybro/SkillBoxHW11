using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary.Classes;
using ClassLibrary.Methods;
using ClassLibrary.Interfaces;
using EmployeeApp.Views;

namespace EmployeeApp.Classes
{
    public class Manager : Employee
    {
        public override string Type => "manager";
        public Manager()
        {
            FirstName = default;
            LastName = default;
            Age = default;
            Salary = default;
            CanAddRemoveDepoAcc = true;
        }
        public Manager(int id, string fn, string ln, int age, int salary)
        {
            Id = id;
            FirstName = fn;
            LastName = ln;
            Age = age;
            Salary = salary;
            CanAddRemoveDepoAcc = true;
        }
        public override List<Client> GetClients()
        {
            return EmployeeActions.ClientActions.GetClients();
        }
        public override Client AddNewClient()
        {
            Client newClient;
            long newClID = EmployeeActions.ClientActions.getNewClientId(); 

            EditClientPage createNewClientWin = new EditClientPage(newClID);

            if (createNewClientWin.ShowDialog() == true)
            {
                var newMainAccForNewClient = GetNewMainAcc(newClID);
                newClient = createNewClientWin.editedClient;
                if (newClient != null)
                {
                    createNewClientWin.Close();
                    newClient.EmployeeType = this.Type;
                    // сохранение клиента
                    EmployeeActions.ClientActions.SaveNewClient(newClient);

                    DateTime now = DateTime.Now;

                    // сохранение его главного счета, одинаковое в ремя (с точностью до милисекнд) нужно для проверки были ли изменения счета или он только создался
                    EmployeeActions.BankAccActions.SaveAcc(newMainAccForNewClient, GlobalVarsAndActions.MainAccsRepoPath, now, now);

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
        public override bool ChangeClient(Client client)
        {
            EditClientPage editClientForm = new EditClientPage(client, this);
            if (editClientForm.ShowDialog() == true) editClientForm.Close();
            if (EmployeeActions.ClientActions.UpdateClient(client)) { return true; }
            else return false;
        }
        public override bool DeleteClient(Client client)
        {
            if (EmployeeActions.ClientActions.DeleteClient(client, this.Type))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        internal BankAccMain GetNewMainAcc(long clId)
        {
            return EmployeeActions.BankAccActions.GetNewMainAcc(clId);
        }
        public override BankAccDepo GetNewDepoAcc(long clId)
        {
            return EmployeeActions.BankAccActions.GetNewDepoAcc(clId);
        }
        public List<BankAccForClient> GetClientAccs(long clId)
        {
            return EmployeeActions.BankAccActions.GetClientAccs(clId);
        }
        bool CloseAcc(long accNum)
        {
            return EmployeeActions.BankAccActions.CloseAcc(accNum);
        }
    }
}
