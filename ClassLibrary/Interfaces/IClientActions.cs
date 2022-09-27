using ClassLibrary.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    public interface IClientActions
    {
        List<Client> GetClients();  // sql + json
        long getNewClientId();      // sql + json
        bool UpdateClient(Client client);
        bool DeleteClient(Client client, string employeeType);
        bool SaveNewClient(Client client);
    }
}
