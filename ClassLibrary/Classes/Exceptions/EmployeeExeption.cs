using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes.Exceptions
{
    public class EmployeeExeption : Exception
    {
        string exDescr;
        public string Message
        {
            get { return exDescr; }
            set { exDescr = value; }
        }

        public EmployeeExeption()
        {
            exDescr = "Непредвиденная ошибка в приложении для сотрудников";
        }

        public EmployeeExeption(int exCode)
        {
            switch (exCode)
            {
                case 1:
                    exDescr = "Отказано. Консультант не может создавать счета";
                    break;
                case 2:
                    exDescr = "";
                    break;
                default:
                    exDescr = "";
                    break;
            }
            GlobalVarsAndActions.LogAlarm(exDescr);
        }
    }
}
