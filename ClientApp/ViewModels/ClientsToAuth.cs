using ClassLibrary.Classes;
using ClientApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Methods;

namespace ClientApp.ViewModels
{
    class ClientsToAuth : IClients
    {
        HashSet<Client>? _clients = new HashSet<Client>();
        public HashSet<Client> Clients 
        { 
            get
            {
                var clients = ClientCommonMethods.GetClientsAllData();
                foreach (var item in clients)
                {
                    _clients.Add(item);
                }
                return _clients;
            } 
            set{}
        }
    }
}
