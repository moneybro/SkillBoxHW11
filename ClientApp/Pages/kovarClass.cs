using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Pages
{
    internal class kovarClass
    {
        void pm()
        {
            BankAccMain mainAcc = new BankAccMain(100);
            IPushMoney<BankAccBase> bankAcc = mainAcc;
        }
    }
}
