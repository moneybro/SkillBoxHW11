using ClassLibrary.Classes;
using EmployeeApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeApp.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        WorkEmployee workEmployee;
        
        EmployeePage ep;
        public event Action<Client> ClientSelectChangedEvent;
        public event Func<bool> EditUserEventSuccess;
        public event Action AddNewClientEventSuccess;
        public event Func<bool> RemoveClientEventSuccess;
        public event Action<BankAccForClient> BankAccSelectEvent;
        public event Action PutMoneyBtnEvent;
        public event Action TfrMoneyBtnEvent;
        public event Action OpenDepoAccBtnEvent;
        public event Action CloseDepoAccBtnEvent;

        long selectedClientId;

        public EmployeePage(Employee employee)
        {
            ep = this;
            InitializeComponent();
            workEmployee = new WorkEmployee(employee, this);
            this.DataContext = workEmployee;
            ClientsDG.ItemsSource = workEmployee.Clients;
            bankAccsListBox.ItemsSource = workEmployee.ClientAccs;
            bankAccTransactions.ItemsSource = workEmployee.AccTransactions;
            employeeValue.Text = $"{employee.FirstName} {employee.LastName}";
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var client = ClientsDG.SelectedItem as Client;
            if (client != null)
            {
                ClientSelectChangedEvent(client);
                selectedClientId = workEmployee.SelectedClient.ID;
            }
        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!EditUserEventSuccess())
            {
                MessageBox.Show("Не удалось отредактировать пользователя.");
                return;
            }
            ClientsDG.Items.SortDescriptions.Clear();
            //ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("LastName", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();
        }

        private void addNewClientBtn_Click(object sender, RoutedEventArgs e)
        {
            AddNewClientEventSuccess();
            ClientsDG.Items.SortDescriptions.Clear();
            //ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("LastName", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();
        }

        private void removeClientBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!RemoveClientEventSuccess())
            {
                MessageBox.Show("Не удалось удалить клиента.");
            }
            ClientsDG.Items.SortDescriptions.Clear();
            //ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("ID", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            ep.Visibility = Visibility.Collapsed;
            ep = null;
            NavigationService.GoBack();
        }

        private void OnClientBankAccSelected(object sender, RoutedEventArgs e)
        {
            if (((System.Windows.Controls.SelectionChangedEventArgs)e).AddedItems.Count > 0) // при выборе другого клиента срабатывает это событие и тогда массив пуст, что вызывает ошибку
            {
                var item = ((System.Windows.Controls.SelectionChangedEventArgs)e).AddedItems[0];

                var res = item as BankAccMain;
                var res2 = item as BankAccDepo;

                if (res != null) BankAccSelectEvent(res);
                if (res2 != null) BankAccSelectEvent(res2);
            }
        }

        private void PutMoneyToMainAccBtn_Click(object sender, RoutedEventArgs e)
        {
            PutMoneyBtnEvent();
        }

        private void TransferMoneyFromMainAccBtn_Click(object sender, RoutedEventArgs e)
        {
            TfrMoneyBtnEvent();
        }

        private void OpenDepoAccBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenDepoAccBtnEvent();
        }

        private void CloseAccBtn_Click(object sender, RoutedEventArgs e)
        {
            CloseDepoAccBtnEvent();
        }

        private void clientAccsGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bankAccsListBox.Height = clientAccsGrid.ActualHeight - 25;
        }
    }
}
