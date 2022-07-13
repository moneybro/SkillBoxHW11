using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes.Exceptions
{
    public class EmployeeAppExeption : Exception
    {
        string exDescr;
        public string Message
        {
            get { return exDescr; }
            set { exDescr = value; }
        }

        public EmployeeAppExeption()
        {
            exDescr = "Непредвиденная ошибка в приложении для сотрудников";
        }

        public EmployeeAppExeption(int exCode)
        {
            switch (exCode)
            {
                case 1:
                    exDescr = "Внимание! Злоумышленник, руки вверх!!! Доступ запрещен!";
                    break;
                case 2:
                    exDescr = "Код ошибки 2. Подробнее в документации";
                    break;
                default:
                    exDescr = "Непредвиденная ошибка в приложении для сотрудников";
                    break;
            }
            GlobalVarsAndActions.LogAlarm(exDescr);
        }
    }
}
