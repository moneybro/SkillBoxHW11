using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using EmployeeApp.Views;
using Serilog;
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
        bool _putMoneyBtnEnabled = false;
        bool _tfrMoneyBtnEnabled = false;
        bool _openDepoAccBtnEnabled = false;
        bool _closeAccBtnEnabled = false;
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
        public bool PutMoneyBtnEnabled
        {
            get
            {
                return _putMoneyBtnEnabled;
            }
            set
            {
                _putMoneyBtnEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PutMoneyBtnEnabled)));
            }
        }
        public bool TfrMoneyBtnEnabled
        {
            get
            {
                return _tfrMoneyBtnEnabled;
            }
            set
            {
                _tfrMoneyBtnEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.TfrMoneyBtnEnabled)));
            }
        }
        public bool OpenDepoAccBtnEnabled
        {
            get
            {
                return _openDepoAccBtnEnabled;
            }
            set
            {
                _openDepoAccBtnEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.OpenDepoAccBtnEnabled)));
            }
        }
        public bool CloseAccBtnEnabled
        {
            get
            {
                return _closeAccBtnEnabled;
            }
            set
            {
                _closeAccBtnEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.CloseAccBtnEnabled)));
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
        public ObservableCollection<BankAccTransactionFull> AccTransactions { get; set; } = new ObservableCollection<BankAccTransactionFull>();

        BankAccForClient? selectedAcc;
        SummToPutStorage summToPutStorage;
        internal decimal summStorage;

        public event Action<string> WorkEmployeeEvent;

        internal WorkEmployee(Employee employee, EmployeePage employeePage)
        {
            _employee = employee;
            _employeePage = employeePage;
            if (_employee.GetType() == typeof(Manager))
            {
                AddNewClientBtnEnabled = true;
            }
            else
            {
                AddNewClientBtnEnabled = false;
                RemoveClientBtnEnabled = false;
            }
            GetClients();
            EditClientBtnEnabled = false;
            _employeePage.ClientSelectChangedEvent += selectClient;

            _employeePage.BankAccSelectEvent += getAccTransactionsFull;
            _employeePage.BankAccSelectEvent += selectedAccChanged;

            _employeePage.EditUserEventSuccess += changeClient;
            _employeePage.AddNewClientEventSuccess += addNewClient;
            _employeePage.RemoveClientEventSuccess += removeClient;


            _employeePage.PutMoneyBtnEvent += putMoney;
            _employeePage.TfrMoneyBtnEvent += tfrMoney;
            _employeePage.OpenDepoAccBtnEvent += openDepoAcc;
            _employeePage.CloseDepoAccBtnEvent += closeDepoAcc;

            WorkEmployeeEvent += logInfo;


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
            OpenDepoAccBtnEnabled = false;
            if (_employee.GetType() == typeof(Manager)) RemoveClientBtnEnabled = true;
            SelectedClient = client;
            PutMoneyBtnEnabled = false;
            TfrMoneyBtnEnabled = false;
            CloseAccBtnEnabled = false;

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
            if (accs.Count == 1) OpenDepoAccBtnEnabled = true;
        }
        private bool changeClient()
        {
            var editClient = _selectedClient.Clone();
            _employee.ChangeClient(editClient);
            string changes = "";

            changes += _selectedClient.LastName != editClient.LastName ? $"фамилия было: {_selectedClient.LastName}, стало:{editClient.LastName} | " : "";

            changes += _selectedClient.FirstName != editClient.FirstName ? $"имя было: {_selectedClient.FirstName}, стало:{editClient.FirstName} | " : "";

            changes += _selectedClient.Patronymic != editClient.Patronymic ? $"отчество было: {_selectedClient.Patronymic}, стало:{editClient.Patronymic} | " : "";

            changes += _selectedClient.MobPhone != editClient.MobPhone ? $"мобильный телефон было: {_selectedClient.MobPhone}, стало:{editClient.MobPhone} | " : "";

            changes += _selectedClient.PaspSeria != editClient.PaspSeria ? $"серия паспорта было: {_selectedClient.PaspSeria}, стало:{editClient.PaspSeria} | " : "";

            changes += _selectedClient.PaspNum != editClient.PaspNum ? $"номер паспорта было: {_selectedClient.PaspNum}, стало:{editClient.PaspNum}" : "";



            if (changes != "")
            {
                if(_employee.SaveEditedClient(editClient, _employee))
                {
                    Clients.Remove(_selectedClient);
                    Clients.Add(editClient);
                    WorkEmployeeEvent($"обновление данных клиента:  {changes}, обновил данные {_employee.Type} {_employee.LastName} {_employee.FirstName}");
                    return true;
                }
                else return false;
            }
            else return false;
        }
        //private bool changeClient()
        //{
        //    bool result = false;
        //    if (_selectedClient != null)
        //    {
        //        result = _employee.ChangeClient(_selectedClient);
        //    }
        //    return result;
        //}
        private void addNewClient()
        {
            var result = _employee.AddNewClient();
            if (result != null)
            {
                WorkEmployeeEvent($"сотрудник {_employee.LastName} {_employee.FirstName} создал пользователя ID:{result.ID} {result.LastName} {result.FirstName}");
                WorkEmployeeEvent($"создан счет {_employee.BankAccActions.GetClientAccs(result.ID)[0].AccNumber}");
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
                    WorkEmployeeEvent($"удаление клиента ID:{_selectedClient.ID} {_selectedClient.LastName} {_selectedClient.FirstName} , удалил {_employee.LastName} {_employee.FirstName}");

                    Clients.Remove(_selectedClient); // удаляем клиента из наблюдаемой коллекции

                    // закрываем все счета клиента
                    foreach (var acc in ClientAccs)
                    {
                        WorkEmployeeEvent($"счет №{acc.AccNumber} закрыт при удалении клента ID:{_selectedClient.ID} {_selectedClient.LastName} {_selectedClient.FirstName} сотрудником {_employee.LastName} {_employee.FirstName}");

                        _employee.BankAccActions.CloseAcc(acc.AccNumber);
                    }
                    ClientAccs.Clear();
                    AccTransactions.Clear();
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
                //AccTransactions.Add(tr);
            }
        }
        private void getAccTransactionsFull(BankAccForClient accNum)
        {
            if (accNum != null)
            {
            var trs = _employee.TransactionsActions.GetBankAccTransactionsFull(accNum.AccNumber);
            AccTransactions.Clear();
            foreach (var tr in trs)
            {
                AccTransactions.Add(tr);
            }
            }
        }
        private void selectedAccChanged(BankAccForClient accNum)
        {
            if (accNum == null)
            {
                return;
            }
            selectedAcc = accNum;
            PutMoneyBtnEnabled = true;
            CloseAccBtnEnabled = false;
            if (ClientAccs.Count == 2)
            {
                TfrMoneyBtnEnabled = true;
            }
            if (selectedAcc.GetType() == typeof(BankAccDepo))
            {
                CloseAccBtnEnabled = true;
            }
        }
        private void putMoney()
        {
            PutMoneyWin putMoneyWin = new PutMoneyWin(
                selectedAcc.AccNumber.ToString(), selectedAcc.Amount.ToString(),
                summToPutStorage);
            BankAccTransaction bankAccTansaction = new BankAccTransaction();
            if (putMoneyWin.ShowDialog() == true)
            {
                decimal summ = -1;
                decimal.TryParse(putMoneyWin.summTB.Text, out summ);
                if (summ > 0)
                {
                    var dateTime = DateTime.Now;
                    //string dateTime = DateTime.Now.ToString();
                    selectedAcc.PushMoneyToAcc(summ);
                    if (_employee.BankAccActions.SaveAcc(selectedAcc, dateTime))
                    {
                        
                        bankAccTansaction.Date = dateTime;
                        //bankAccTansaction.TransType = BankAccTansaction.TransactionType.Income;
                        bankAccTansaction.AccNumberSource = 0;
                        bankAccTansaction.AccNumberTarget = selectedAcc.AccNumber;
                        bankAccTansaction.Summ = summ;
                        bankAccTansaction.EmployeeId = _employee.Id;
                        bankAccTansaction.ClientId = 0;
                        bankAccTansaction.OperatorName = $"{_employee.LastName} {_employee.FirstName}";
                        bankAccTansaction.Description = "";
                        _employee.TransactionsActions.SaveTransaction(bankAccTansaction);
                        AccTransactions.Add(new BankAccTransactionFull() { Acc = selectedAcc, Tr = bankAccTansaction }); // так просто добавляем в коллекцию и данные актуальны и базу не грузим
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
        private void tfrMoney()
        {
            if (selectedAcc != null)
            {
                var targetAcc = ClientAccs.FirstOrDefault(a => a.AccNumber != selectedAcc.AccNumber);
                string accSourceNumber = selectedAcc.AccNumber.ToString();
                string accSourceAmount = selectedAcc.Amount.ToString();
                string accTargetNumber = targetAcc.AccNumber.ToString();
                string accTargetAmount = targetAcc.Amount.ToString();

                IStorageTransferMoney<BankAccForClient> transferStorage = new BankAccTransferStorage<BankAccBase>();

                transferStorage.addAcc = selectedAcc;
                transferStorage.addAcc = targetAcc;
                
                MoneyTransferWin moneyTransferWin = new MoneyTransferWin(
                    accSourceNumber,
                    accSourceAmount,
                    accTargetNumber,
                    accTargetAmount,
                    this);

                if (moneyTransferWin.ShowDialog() == true)
                {
                    //var success = accActions.transferMoney(accSource, accTarget, this.summ);
                    var dateTime = DateTime.Now;
                    var transfeSuccess = transferStorage.TransferMoney(summStorage, dateTime);
                    BankAccTransaction bankAccTansaction = new BankAccTransaction();
                    if (transfeSuccess)
                    {
                        bankAccTansaction.Date = dateTime;
                        //bankAccTansaction.TransType = BankAccTansaction.TransactionType.Income;
                        bankAccTansaction.AccNumberSource = long.Parse(accSourceNumber);
                        bankAccTansaction.AccNumberTarget = long.Parse(accTargetNumber);
                        bankAccTansaction.Summ = summStorage;
                        bankAccTansaction.EmployeeId = _employee.Id;
                        bankAccTansaction.ClientId = 0;
                        bankAccTansaction.OperatorName = $"{_employee.LastName} {_employee.FirstName}";
                        bankAccTansaction.Description = "перевод между счетами";
                        _employee.TransactionsActions.SaveTransaction(bankAccTansaction);
                        //getAccTransactionsFull(selectedAcc); // если будет обращение к большой бд, то это будет очень затратно по времени и ресурсам
                        AccTransactions.Add(new BankAccTransactionFull() { Acc = selectedAcc, Tr = bankAccTansaction }); // так просто добавляем в коллекцию и данные актуальны и базу не грузим
                        MessageBox.Show("Перевод выполнен успешно.");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось выполнить перевод.");
                    }
                }
                else
                {
                    MessageBox.Show("Что-то пошло не так.");
                }
            }
        }
        private void openDepoAcc()
        {
            if (ClientAccs.Count == 2)
            {
                MessageBox.Show("Депозитный счет уже есть, нельзя открыть еще один");
                return;
            }
            var result = _employee.BankAccActions.GetNewDepoAcc(_selectedClient.ID);
            if (result != null)
            {
                ClientAccs.Add(result);
                WorkEmployeeEvent($"сотрудник {_employee.LastName} {_employee.FirstName} создал для клиента ID:{_selectedClient.LastName} {_selectedClient.FirstName} депозитный счет №{result.AccNumber}");
            }
        }
        private void closeDepoAcc()
        {
            if (selectedAcc.GetType() == typeof(BankAccDepo))
            {
                if (_employee.BankAccActions.CloseAcc(selectedAcc.AccNumber))
                {   
                    WorkEmployeeEvent($"сотрудник {_employee.LastName} {_employee.FirstName} закрыл депозитный счет №{selectedAcc.AccNumber} клиента ID: {_selectedClient.LastName} {_selectedClient.FirstName}");

                    ClientAccs.Remove(selectedAcc);
                    selectedAcc = ClientAccs.FirstOrDefault();
                    selectedAccChanged(selectedAcc);
                    getAccTransactionsFull(selectedAcc);
                }
            }
            else
            {
                MessageBox.Show("Этот счет отдельно закрыть нельзя.");
                return;
            }
        }
        private void logInfo(string msg)
        {
            Log.Information(msg);
        }
        #endregion
    }
}
