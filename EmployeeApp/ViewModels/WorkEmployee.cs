using ClassLibrary.Classes;
using ClassLibrary.Classes.Exceptions;
using ClassLibrary.Interfaces;
using ClassLibrary.Methods.ExtensionMethods;
using EmployeeApp.Classes;
using EmployeeApp.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeApp.ViewModels
{
    internal class WorkEmployee : INotifyPropertyChanged
    {
        Employee _employee;
        EmployeePage _employeePage;
        Client _selectedClient;
        ObservableCollection<Client> _clients;
        public event PropertyChangedEventHandler PropertyChanged;
        NotifyCollectionChangedEventArgs e;
        bool _addNewClientBtnEnabled = false;
        bool _removeClientBtnEnabled = false;
        bool _editClientBtnEnabled = false;
        bool _putMoneyBtnEnabled = false;
        bool _tfrMoneyBtnEnabled = false;
        bool _openDepoAccBtnEnabled = false;
        bool _closeAccBtnEnabled = false;
        decimal _accsSumm = 0;
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
        public string AccsSumm
        {
            get
            {
                return $"Доступно: {_accsSumm.ToString()}";
            }
            set
            {
                decimal.TryParse(value, out _accsSumm);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.AccsSumm)));
            }
        }
        public Client? SelectedClient
        {
            get
            {
                return _selectedClient;
            }
            set
            {
                selectClient(value);
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
        private event Action AccsAmountSummChanged;
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

            if (_employee.GetType() == typeof(Consultant))
            {
                AddNewClientBtnEnabled = false;
                OpenDepoAccBtnEnabled = false;
            }


            Clients = new ObservableCollection<Client>();
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
            AccsAmountSummChanged += SetAccsAmountSumm;

            summToPutStorage = new SummToPutStorage();
        }

        #region работа с клиентом
        private void GetClients()
        {
            Clients.Clear();
            var cls = _employee.GetClients();
            cls.sortByLastName();
            foreach (var item in cls)
            {
                Clients.Add(item);
            }
        }
        private void selectClient(Client? client)
        {
            
            if (client == null)
            {
                EditClientBtnEnabled = false;
                RemoveClientBtnEnabled = false;
                OpenDepoAccBtnEnabled = false;
            }
            else
            {
                EditClientBtnEnabled = true;
                if (_employee.GetType() == typeof(Manager)) RemoveClientBtnEnabled = true;
                _selectedClient = client;

                var accs = _employee.EmployeeActions.BankAccActions.GetClientAccs(client.ID);
                ClientAccs.Clear();
                foreach (var acc in accs)
                {
                    try
                    {
                        ClientAccs.Add((BankAccMain)acc);
                        OpenDepoAccBtnEnabled = _employee.CanAddRemoveDepoAcc;
                    }
                    catch
                    {
                        ClientAccs.Add((BankAccDepo)acc);
                        OpenDepoAccBtnEnabled = false;
                    }
                }
                
                
            }

            AccsAmountSummChanged();

            AccTransactions.Clear();
            PutMoneyBtnEnabled = false;
            TfrMoneyBtnEnabled = false;
            CloseAccBtnEnabled = false;
        }
        private bool changeClient()
        {
            var clientBeforeEdit = _selectedClient.Clone();
            _employee.ChangeClient(_selectedClient);
            string changes = "";

            changes += _selectedClient.LastName != clientBeforeEdit.LastName ? $"фамилия было: {clientBeforeEdit.LastName}, стало:{_selectedClient.LastName} | " : "";

            changes += _selectedClient.FirstName != clientBeforeEdit.FirstName ? $"имя было: {clientBeforeEdit.FirstName}, стало:{_selectedClient.FirstName} | " : "";

            changes += _selectedClient.Patronymic != clientBeforeEdit.Patronymic ? $"отчество было: {clientBeforeEdit.Patronymic}, стало:{_selectedClient.Patronymic} | " : "";

            changes += _selectedClient.MobPhone != clientBeforeEdit.MobPhone ? $"мобильный телефон было: {clientBeforeEdit.MobPhone}, стало:{_selectedClient.MobPhone} | " : "";

            changes += _selectedClient.PaspSeria != clientBeforeEdit.PaspSeria ? $"серия паспорта было: {clientBeforeEdit.PaspSeria}, стало:{_selectedClient.PaspSeria} | " : "";

            changes += _selectedClient.PaspNum != clientBeforeEdit.PaspNum ? $"номер паспорта было: {clientBeforeEdit.PaspNum}, стало:{_selectedClient.PaspNum}" : "";



            if (changes != "")
            {
                //clientBeforeEdit.EmployeeType = _employee.Type;
                    //Clients.Remove(clientBeforeEdit);
                    //Clients.Add(_selectedClient);
                    GetClients();
                    WorkEmployeeEvent($"обновление данных клиента:  {changes}, обновил данные {_employee.Type} {_employee.LastName} {_employee.FirstName}");
                return true;
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
                WorkEmployeeEvent($"{_employee.Type}  {_employee.LastName} {_employee.FirstName} создал пользователя ID:{result.ID} {result.LastName} {result.FirstName}");
                WorkEmployeeEvent($"создан счет {_employee.EmployeeActions.BankAccActions.GetClientAccs(result.ID)[0].AccNumber}");
                Clients.Add(result);
                SelectedClient = result;
                AccsAmountSummChanged();
                GetClients();
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
                    //Clients.sortByLastName();
                    // закрываем все счета клиента
                    foreach (var acc in ClientAccs)
                    {
                        WorkEmployeeEvent($"счет №{acc.AccNumber} закрыт при удалении клента ID:{_selectedClient.ID} {_selectedClient.LastName} {_selectedClient.FirstName} сотрудником {_employee.LastName} {_employee.FirstName}");

                        _employee.EmployeeActions.BankAccActions.CloseAcc(acc.AccNumber);
                    }
                    ClientAccs.Clear();
                    GetClients();
                    AccTransactions.Clear();
                    SelectedClient = null;
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
        private void SetAccsAmountSumm()
        {
            try
            {
                decimal summ = 0;
                if (ClientAccs.Count == 2) summ = ClientAccs[0] + ClientAccs[1];
                if (ClientAccs.Count == 1) summ = ClientAccs[0] + null;
                if (ClientAccs.Count == 0) AccsSumm = "0";
                AccsSumm = summ.ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion
        #region работа с банковскими счетами
        //private void getAccTransactions(BankAccForClient accNum)
        //{
        //    var trs = _employee.EmployeeActions.BankAccActions.GetAccTransactions(accNum.AccNumber);
        //    AccTransactions.Clear();
        //    foreach (var tr in trs)
        //    {
        //        //AccTransactions.Add(tr);
        //    }
        //}
        private void getAccTransactionsFull(BankAccForClient accNum)
        {
            if (accNum != null)
            {
            var trs = _employee.EmployeeActions.TransactionsActions.GetBankAccTransactionsFull(accNum.AccNumber);
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
                CloseAccBtnEnabled = _employee.CanAddRemoveDepoAcc;
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
                    if (_employee.EmployeeActions.BankAccActions.SaveAcc(selectedAcc, dateTime))
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
                        _employee.EmployeeActions.TransactionsActions.SaveTransaction(bankAccTansaction);
                        AccTransactions.Add(new BankAccTransactionFull() { Acc = selectedAcc, Tr = bankAccTansaction }); // так просто добавляем в коллекцию и данные актуальны и базу не грузим
                        MessageBox.Show($"Пополнение счета выполнено успешно.");
                    }
                    else
                    {
                        selectedAcc.Amount -= summ;
                        MessageBox.Show($"Не удалось сохранить данные счета.");
                    }
                    AccsAmountSummChanged();
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
                    var transferSuccess = transferStorage.TransferMoney(summStorage, dateTime, Employee.EmployeeActions.BankAccActions);
                    BankAccTransaction bankAccTansaction = new BankAccTransaction();
                    if (transferSuccess)
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
                        _employee.EmployeeActions.TransactionsActions.SaveTransaction(bankAccTansaction);
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
            try
            {
                var result = _employee.GetNewDepoAcc(_selectedClient.ID);
                if (result != null)
                {
                    ClientAccs.Add(result);
                    WorkEmployeeEvent($"{_employee.Type}  {_employee.LastName} {_employee.FirstName} создал для клиента ID:{_selectedClient.ID} {_selectedClient.LastName} {_selectedClient.FirstName} депозитный счет №{result.AccNumber}");
                    OpenDepoAccBtnEnabled = false;
                    selectedAcc = ClientAccs[0];
                    TfrMoneyBtnEnabled = true;
                }
            }
            catch (EmployeeExeption ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void closeDepoAcc()
        {
            if (selectedAcc.GetType() == typeof(BankAccDepo))
            {
                if (_employee.EmployeeActions.BankAccActions.CloseAcc(selectedAcc.AccNumber))
                {   
                    WorkEmployeeEvent($"{_employee.Type} {_employee.LastName} {_employee.FirstName} закрыл депозитный счет №{selectedAcc.AccNumber} клиента ID:{_selectedClient.ID} {_selectedClient.LastName} {_selectedClient.FirstName}");

                    ClientAccs.Remove(selectedAcc);
                    selectedAcc = ClientAccs.FirstOrDefault();
                    selectedAccChanged(selectedAcc);
                    OpenDepoAccBtnEnabled = _employee.CanAddRemoveDepoAcc;
                    TfrMoneyBtnEnabled = false;
                    getAccTransactionsFull(selectedAcc);
                    SetAccsAmountSumm();
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
            GlobalVarsAndActions.LogInfo(msg);
        }
        #endregion
    }
}
