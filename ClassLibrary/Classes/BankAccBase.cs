using ClassLibrary.Interfaces;
using ClassLibrary.Methods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Classes
{
    public abstract class BankAccBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region поля
        long id;
        long _accNumber;
        decimal _amount;
        bool _active;
        DateTime _createDate;
        DateTime _updateDate;

        #endregion

        #region свойства
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get { return id; } set { id = value; } }
        public long AccNumber
        {
            get { return _accNumber; }
            set { _accNumber = value; }
        }
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Amount)));
            }
        }
        public bool Active { get { return _active; } set { _active = value; } }
        public DateTime CreateDate { get { return _createDate; } set { _createDate = value; } }
        public DateTime UpdateDate { get { return _updateDate; } set { _updateDate = value; } }
        #endregion

        #region методы
        #endregion
    }
}
