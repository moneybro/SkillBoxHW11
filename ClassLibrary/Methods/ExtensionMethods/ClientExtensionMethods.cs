using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Methods.ExtensionMethods
{
    public static class ClientExtensionMethods
    {
        public static void sortByLastName(this List<Client> clients)
        {
            clients.Sort(delegate (Client x, Client y)
            {
                if (x.LastName == null && y.LastName == null) return 0;
                else if (x.LastName == null) return -1;
                else if (y.LastName == null) return 1;
                else return x.LastName.CompareTo(y.LastName);
            });
        }
        public static void sortByFirstName(this List<Client> clients)
        {
            clients.Sort(delegate (Client x, Client y)
            {
                if (x.FirstName == null && y.FirstName == null) return 0;
                else if (x.FirstName == null) return -1;
                else if (y.FirstName == null) return 1;
                else return x.FirstName.CompareTo(y.FirstName);
            });
        }

        public static void sortByLastName(this ObservableCollection<Client> clients)
        {
            var storage = clients.ToList();
            storage.sortByLastName();
            clients.Clear();
            foreach (Client client in storage)
            {
                clients.Add(client);
            }
        }
    }
}
