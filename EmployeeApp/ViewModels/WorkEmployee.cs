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
        BankAccForClient? _selectedAcc;
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
        public BankAccForClient? SelectedAcc
        {
            get
            {
                return _selectedAcc;
            }
            set
            {
                _selectedAcc = value;
                selectedAccChanged(_selectedAcc);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SelectedAcc)));
            }
        }
        public Employee Employee 
        { 
            get { return _employee; }
            set { _employee = value; }
        }
        internal EmployeePage EmployeePage { get; set; }
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Client> ClientsView { get; set; }
        public ObservableCollection<BankAccForClient> ClientAccs { get; set; } 
        public ObservableCollection<BankAccTransactionFull> AccTransactions { get; set; }

        
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
            ClientsView = new ObservableCollection<Client>();
            ClientAccs = new ObservableCollection<BankAccForClient>();
            AccTransactions = new ObservableCollection<BankAccTransactionFull>();

            GetClients();
            EditClientBtnEnabled = false;

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
            clientsViewFillOrRefresh();
        }
        private void clientsViewFillOrRefresh()
        {
            this.ClientsView.Clear();
            if (this.Employee is Manager)
            {
                foreach (var item in Clients)
                {
                    this.ClientsView.Add(item);
                }
            }
            if (this.Employee is Consultant)
            {
                foreach (var item in Clients)
                {
                    var n = item.Clone();
                    n.PaspNum = 0;
                    n.PaspSeria = 0;
                    this.ClientsView.Add(n);
                }
                //ClientsView.sortByLastName();
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

        #region команда добавление нового клиента
        // команда добавления нового объекта
        private RelayCommand addNewClientCommand;
        public RelayCommand AddNewClientCommand
        {
            get
            {
                return addNewClientCommand ??
                  (addNewClientCommand = new RelayCommand(obj =>
                  {
                      addNewClient();
                  }));
            }
        }
        private void addNewClient()
        {
            var result = _employee.AddNewClient();
            if (result != null)
            {
                WorkEmployeeEvent($"{_employee.Type}  {_employee.LastName} {_employee.FirstName} создал пользователя ID:{result.ID} {result.LastName} {result.FirstName}");
                WorkEmployeeEvent($"создан счет {_employee.EmployeeActions.BankAccActions.GetClientAccs(result.ID)[0].AccNumber}");
                Clients.Add(result);
                AccsAmountSummChanged();
                GetClients();
                SelectedClient = result;
            }
            else
            {
                MessageBox.Show("Не удалось создать клиента");
            }
        }


        #endregion

        #region команда редактирования клиента
        private RelayCommand editClientCommand;
        public RelayCommand EditClientCommand
        {
            get
            {
                return editClientCommand ??
                  (editClientCommand = new RelayCommand(obj =>
                  {
                      changeClient();
                  }));
            }
        }

        private bool changeClient()
        {
            if (_selectedClient == null) return false;
            Client changedClientFromDB; // сюда сохраняем обновленного клиента, которого изменили на форме и его перечитываем из базы

            var clientBeforeEdit = Clients.FirstOrDefault(c => c.ID == _selectedClient.ID).getClientCopy(); // делаем копию данных (из коллекции, где полные данные, чтобы закрытые поля не терялись и не перезаписывались) клиента для определения какие поля изменились и сохранения изменений в логи
            var IDeditClient = _selectedClient.ID;  // сохраняем ID клиента, которого редактируем
            var success = _employee.ChangeClient(Clients.FirstOrDefault(c => c.ID == _selectedClient.ID)); // редактировение, возравт успешности редактирования
            if (success) changedClientFromDB = _employee.GetClients().Find(c => c.ID == IDeditClient);      // если ред-е успешно, то перечитываем этого клиента из базы, потому что сюда его обновленный экземпляр не возвращается
            else return false; // если ред-е не удалось, то выходим
            string changes = "";

            changes += changedClientFromDB.LastName != clientBeforeEdit.LastName ? $"фамилия было: {clientBeforeEdit.LastName}, стало:{changedClientFromDB.LastName} | " : "";

            changes += changedClientFromDB.FirstName != clientBeforeEdit.FirstName ? $"имя было: {clientBeforeEdit.FirstName}, стало:{changedClientFromDB.FirstName} | " : "";

            changes += changedClientFromDB.Patronymic != clientBeforeEdit.Patronymic ? $"отчество было: {clientBeforeEdit.Patronymic}, стало:{changedClientFromDB.Patronymic} | " : "";

            changes += changedClientFromDB.MobPhone != clientBeforeEdit.MobPhone ? $"мобильный телефон было: {clientBeforeEdit.MobPhone}, стало:{changedClientFromDB.MobPhone} | " : "";

            changes += changedClientFromDB.PaspSeria != clientBeforeEdit.PaspSeria ? $"серия паспорта было: {clientBeforeEdit.PaspSeria}, стало:{changedClientFromDB.PaspSeria} | " : "";

            changes += changedClientFromDB.PaspNum != clientBeforeEdit.PaspNum ? $"номер паспорта было: {clientBeforeEdit.PaspNum}, стало:{changedClientFromDB.PaspNum}" : "";

            //clientBeforeEdit = null;  
            if (changes != "")
            {
                GetClients();
                SelectedClient = changedClientFromDB;
                WorkEmployeeEvent($"обновление данных клиента:  {changes}, обновил данные {_employee.Type} {_employee.LastName} {_employee.FirstName}");
                return true;
            }
            else return false;
        }
        #endregion

        #region команда удаления клиента

        // команда удаления объекта
        private RelayCommand removeClientCommand;
        public RelayCommand RemoveClientCommand
        {
            get
            {
                return removeClientCommand ??
                    (removeClientCommand = new RelayCommand(obj =>
                    {
                        // оставил в качестве примера передачи параметра в команду
                        //Client client = obj as Client;
                        //if (client != null)
                        //{
                        //    removeClient();
                        //}
                        removeClient(); // параметр можно не проверять, так как существующий метод предусматривает проверку выделенного клиента (подлежащий удалению)
                    },
                    (obj) => Clients.Count > 0)); // если в коллекции нет элементов, то команда не может быть выполнена
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
        #endregion

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
        private void selectedAccChanged(BankAccForClient acc)
        {
            if (acc == null)
            {
                AccTransactions.Clear();
                return;
            }
            PutMoneyBtnEnabled = true;
            CloseAccBtnEnabled = false;
            getAccTransactionsFull(acc);
            if (ClientAccs.Count == 2)
            {
                TfrMoneyBtnEnabled = true;
            }
            if (_selectedAcc.GetType() == typeof(BankAccDepo))
            {
                CloseAccBtnEnabled = _employee.CanAddRemoveDepoAcc;
            }
        }

        #region команда пополнения счета
        // команда пополнения счета
        private RelayCommand putMoneyCommand;
        public RelayCommand PutMoneyCommand
        {
            get
            {
                return putMoneyCommand ??
                    (putMoneyCommand = new RelayCommand(obj =>
                    {
                        putMoney();
                    },
                    (obj) => ClientAccs.Count >= 1)); // если в коллекции нет элементов, то команда не может быть выполнена
            }
        }
        private void putMoney()
        {
            PutMoneyWin putMoneyWin = new PutMoneyWin(
                _selectedAcc.AccNumber.ToString(), _selectedAcc.Amount.ToString(),
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
                    _selectedAcc.PushMoneyToAcc(summ);
                    if (_employee.EmployeeActions.BankAccActions.SaveAcc(_selectedAcc, dateTime))
                    {
                        
                        bankAccTansaction.Date = dateTime;
                        //bankAccTansaction.TransType = BankAccTansaction.TransactionType.Income;
                        bankAccTansaction.AccNumberSource = 0;
                        bankAccTansaction.AccNumberTarget = _selectedAcc.AccNumber;
                        bankAccTansaction.Summ = summ;
                        bankAccTansaction.EmployeeId = _employee.Id;
                        bankAccTansaction.ClientId = 0;
                        bankAccTansaction.OperatorName = $"{_employee.LastName} {_employee.FirstName}";
                        bankAccTansaction.Description = "";
                        _employee.EmployeeActions.TransactionsActions.SaveTransaction(bankAccTansaction);
                        AccTransactions.Add(new BankAccTransactionFull() { Acc = _selectedAcc, Tr = bankAccTansaction }); // так просто добавляем в коллекцию и данные актуальны и базу не грузим
                        MessageBox.Show($"Пополнение счета выполнено успешно.");
                    }
                    else
                    {
                        _selectedAcc.Amount -= summ;
                        MessageBox.Show($"Не удалось сохранить данные счета.");
                    }
                    AccsAmountSummChanged();
                }
            }
        }
        #endregion

        #region команда перевода среств между счетами
        // команда перевода среств между счетами
        private RelayCommand transferMoneyCommand;
        public RelayCommand TransferMoneyCommand
        {
            get
            {
                return transferMoneyCommand ??
                    (transferMoneyCommand = new RelayCommand(obj =>
                    {
                        tfrMoney();
                    },
                    (obj) => ClientAccs.Count >= 2));
            }
        }
        private void tfrMoney()
        {
            if (_selectedAcc != null)
            {
                var targetAcc = ClientAccs.FirstOrDefault(a => a.AccNumber != _selectedAcc.AccNumber);
                string accSourceNumber = _selectedAcc.AccNumber.ToString();
                string accSourceAmount = _selectedAcc.Amount.ToString();
                string accTargetNumber = targetAcc.AccNumber.ToString();
                string accTargetAmount = targetAcc.Amount.ToString();

                IStorageTransferMoney<BankAccForClient> transferStorage = new BankAccTransferStorage<BankAccBase>();

                transferStorage.addAcc = _selectedAcc;
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
                        AccTransactions.Add(new BankAccTransactionFull() { Acc = _selectedAcc, Tr = bankAccTansaction }); // так просто добавляем в коллекцию и данные актуальны и базу не грузим
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
        #endregion

        #region команда открытия счета депо
        private RelayCommand openDepoAccCommand;
        public RelayCommand OpenDepoAccCommand
        {
            get
            {
                return openDepoAccCommand ??
                    (openDepoAccCommand = new RelayCommand(obj =>
                    {
                        openDepoAcc();
                    },
                    (obj) => ClientAccs.Count >= 1));
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
                    _selectedAcc = ClientAccs[0];
                    TfrMoneyBtnEnabled = true;
                }
            }
            catch (EmployeeExeption ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region команда закрытия счета депо
        private RelayCommand closeDepoAccCommand;
        public RelayCommand CloseDepoAccCommand
        {
            get
            {
                return closeDepoAccCommand ??
                    (closeDepoAccCommand = new RelayCommand(obj =>
                    {
                        closeDepoAcc();
                    },
                    (obj) => ClientAccs.Count >= 2));
            }
        }
        private void closeDepoAcc()
        {
            if (_selectedAcc.GetType() == typeof(BankAccDepo))
            {
                if (_employee.EmployeeActions.BankAccActions.CloseAcc(_selectedAcc.AccNumber))
                {   
                    WorkEmployeeEvent($"{_employee.Type} {_employee.LastName} {_employee.FirstName} закрыл депозитный счет №{_selectedAcc.AccNumber} клиента ID:{_selectedClient.ID} {_selectedClient.LastName} {_selectedClient.FirstName}");

                    ClientAccs.Remove(_selectedAcc);
                    _selectedAcc = ClientAccs.FirstOrDefault();
                    selectedAccChanged(_selectedAcc);
                    OpenDepoAccBtnEnabled = _employee.CanAddRemoveDepoAcc;
                    TfrMoneyBtnEnabled = false;
                    getAccTransactionsFull(_selectedAcc);
                    SetAccsAmountSumm();
                }
            }
            else
            {
                MessageBox.Show("Этот счет отдельно закрыть нельзя.");
                return;
            }
        }
        #endregion
        private void logInfo(string msg)
        {
            GlobalVarsAndActions.LogInfo(msg);
        }
        #endregion
    }
}
