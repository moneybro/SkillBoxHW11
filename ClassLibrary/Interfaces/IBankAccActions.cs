using ClassLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    public interface IBankAccActions
    {
        List<BankAccForClient> GetClientAccs(long clId);
        BankAccMain GetNewMainAcc(long clId);
        BankAccDepo GetNewDepoAcc(long clId);
        bool SaveAcc<T>(T acc, DateTime dateTime) where T : BankAccForClient;
        bool SaveAcc<T>(T acc, string repoPath, DateTime dateTime) where T : BankAccForClient;
        bool CloseAcc(long accNum);
        public List<BankAccForClient> GetAccTransactions(long accNum);

    }
}
