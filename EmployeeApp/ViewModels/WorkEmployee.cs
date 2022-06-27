using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using EmployeeApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeApp.ViewModels
{
    internal class WorkEmployee : INotifyPropertyChanged
    {
        Employee _employee;
        EmployeePage _employeePage;
        Client _selectedClient;
        public event PropertyChangedEventHandler PropertyChanged;
        bool _addNewClientBtnEnabled = false;
        bool _removeClientBtnEnabled = false;
        bool _editClientBtnEnabled = false;
        public bool AddNewClientBtnEnabled {
            get
            {
                return _addNewClientBtnEnabled;
            }
            set 
            {
                _addNewClientBtnEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AddNewClientBtnEnabled)));
            } 
        }
        public bool RemoveClientBtnEnabled 
        {
            get
            {
                return _removeClientBtnEnabled;
            }
            set
            {
                _removeClientBtnEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.RemoveClientBtnEnabled)));
            }
        }
        public bool EditClientBtnEnabled {
            get
            {
                return _editClientBtnEnabled;
            }
            set
            {
                _editClientBtnEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.EditClientBtnEnabled)));
            }
        }
        public Client SelectedClient
        {
            get
            {
                return _selectedClient;
            }
            set
            {
                _selectedClient = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SelectedClient)));
            }
        }
        internal Employee Employee 
        { 
            get { return _employee; }
            set { _employee = value; }
        }
        internal EmployeePage EmployeePage { get; set; }
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        public ObservableCollection<BankAccForClient> ClientAccs { get; set; } = new ObservableCollection<BankAccForClient>();
        public ObservableCollection<BankAccForClient> AccTransactions { get; set; } = new ObservableCollection<BankAccForClient>();

        BankAccForClient selectedAcc;
        SummToPutStorage summToPutStorage;


        internal WorkEmployee(Employee employee, EmployeePage employeePage)
        {
            _employee = employee;
            _employeePage = employeePage;
            if (_employee.GetType() == typeof(Manager))
            {
                AddNewClientBtnEnabled = true;
                RemoveClientBtnEnabled = true;
            }
            else
            {
                AddNewClientBtnEnabled = false;
                RemoveClientBtnEnabled = false;
            }
            GetClients();
            EditClientBtnEnabled = false;
            _employeePage.ClientSelectChangedEvent += selectClient;

            _employeePage.BankAccSelectEvent += getAccTransactions;
            _employeePage.BankAccSelectEvent += selectedAccChanged;

            _employeePage.EditUserEventSuccess += changeClient;
            _employeePage.AddNewClientEventSuccess += addNewClient;
            _employeePage.RemoveClientEventSuccess += removeClient;


            _employeePage.PutMoneyBtnEvent += putMoney;
            summToPutStorage = new SummToPutStorage();
        }

        
        #region работа с клиентом
        private void GetClients()
        {
            Clients.Clear();
            foreach (var item in _employee.GetClients())
            {
                Clients.Add(item);
            }
        }
        private void selectClient(Client client)
        {
            EditClientBtnEnabled = true;
            SelectedClient = client;
            ClientAccs.Clear();
            AccTransactions.Clear();
            var accs = _employee.BankAccActions.GetClientAccs(client.ID);
            foreach (var acc in accs)
            {
                try
                {
                    ClientAccs.Add((BankAccMain)acc);
                }
                catch
                {
                    ClientAccs.Add((BankAccDepo)acc);
                }
            }
        }
        private bool changeClient()
        {
            bool result = false;
            if (_selectedClient != null)
            {
                result = _employee.ChangeClient(_selectedClient);
            }            
            return result;
        }
        private void addNewClient()
        {
            var result = _employee.AddNewClient();
            if (result != null)
            {
                Clients.Add(result);
            }
            else
            {
                MessageBox.Show("Не удалось создать клиента");
            }
        }
        private bool removeClient()
        {
            if (_selectedClient != null)
            {
                if (_employee.DeleteClient(_selectedClient))
                {
                    Clients.Remove(_selectedClient);
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
        #endregion
        #region работа с банковскими счетами
        private void getAccTransactions(BankAccForClient accNum)
        {
            var trs = _employee.BankAccActions.GetAccTransactions(accNum.AccNumber);
            AccTransactions.Clear();
            foreach (var tr in trs)
            {
                AccTransactions.Add(tr);
            }
        }
        private void selectedAccChanged(BankAccForClient accNum)
        {
            selectedAcc = accNum;
        }
        private void putMoney()
        {
            PutMoneyWin putMoneyWin = new PutMoneyWin(
                selectedAcc.AccNumber.ToString(), selectedAcc.Amount.ToString(),
                summToPutStorage);
            if (putMoneyWin.ShowDialog() == true)
            {
                decimal summ = -1;
                decimal.TryParse(putMoneyWin.summTB.Text, out summ);
                if (summ > 0)
                {
                    selectedAcc.PushMoneyToAcc(summ);
                    if (_employee.BankAccActions.SaveAcc(selectedAcc))
                    {
                        getAccTransactions(selectedAcc); // для обновления списка транзакций
                        MessageBox.Show($"Пополнение счета выполнено успешно.");
                    }
                    else
                    {
                        selectedAcc.Amount -= summ;
                        MessageBox.Show($"Не удалось сохранить данные счета.");
                    }
                }
            }
        }
        #endregion
    }
}
