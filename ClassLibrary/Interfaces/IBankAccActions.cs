using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    internal interface IBankAccActions
    {
        List<BankAccForClient> GetClientAccs(long clId);
        BankAccMain GetNewMainAcc(long clId);
        BankAccDepo GetNewDepoAcc(long clId);
        bool SaveAcc<T>(T acc, string repoPath) where T : BankAccForClient;
        public bool CloseAcc(long accNum);
    }
}
