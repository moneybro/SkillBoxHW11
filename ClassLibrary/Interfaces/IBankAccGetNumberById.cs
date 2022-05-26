using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    interface IBankAccGetNumberById
    {
        BankAcc GetAccNumberById(long accNumber);
    }
}
