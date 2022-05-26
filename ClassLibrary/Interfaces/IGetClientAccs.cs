using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    internal interface IGetClientAccs
    {
        List<BankAcc> GetClientAccs(long clId);
    }
}
