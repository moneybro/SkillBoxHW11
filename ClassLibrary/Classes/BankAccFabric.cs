using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    internal class BankAccFabric<T> where T : BankAcc
    {   
        public T acc;
        public BankAccFabric(T arg)
        {
            acc = arg;
        }
    }
}
