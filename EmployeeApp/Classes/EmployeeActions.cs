using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Classes
{
    internal class EmployeeActions
    {
        internal IBankAccActions BankAccActions { get; set; }
        internal ITransactionsActions TransactionsActions { get; set; }
        internal IClientActions ClientActions { get; set; }
        internal EmployeeActions(string type)
        {
            if (type == "json")
            {
                var jsconActions = new BankAccActionsJSON();
                BankAccActions = jsconActions;
                TransactionsActions = jsconActions;

                var clActions = new ClientActionsBase(new ClientActionsJson());
                ClientActions = clActions.ClientActions;
            }
            if (type == "mssql")
            {
                var mssqlActions = new BankAccActionsSql();
                BankAccActions = mssqlActions;
                TransactionsActions = mssqlActions;

                var clActions = new ClientActionsBase(new ClientActionsSql());
                ClientActions = clActions.ClientActions;
            }
        }
    }
}
