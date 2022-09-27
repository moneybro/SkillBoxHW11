using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    static internal class BankAccFabricStat
    {
        internal static BankAccMain getNewMainAcc(long clId)
        {
            var newMainAcc = new BankAccMain(clId);
            return newMainAcc;
        }

        internal static BankAccDepo getNewDepoAcc(long clId)
        {
            var newDepoAcc = new BankAccDepo(clId);
            return newDepoAcc;
        }
    }
}
