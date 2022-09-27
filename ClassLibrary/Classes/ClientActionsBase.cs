using ClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public class ClientActionsBase
    {
        public IClientActions ClientActions { get; set; }
        public ClientActionsBase(IClientActions _actions)
        {
            ClientActions = _actions;
        }
    }
}
