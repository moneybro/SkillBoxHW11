using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    internal interface ISaveAcc
    {
        bool SaveAcc<T>(T acc, string repoPath) where T : BankAcc;
    }
}
