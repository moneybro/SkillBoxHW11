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
        long selectedClientId;

        public EmployeePage(Employee employee)
        {
            ep = this;
            InitializeComponent();
            workEmployee = new WorkEmployee(employee, this);
            this.DataContext = workEmployee;
            ClientsDG.ItemsSource = workEmployee.Clients;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var client = (Client)ClientsDG.SelectedItem;
            if (client != null)
            {
                ClientSelectChangedEvent(client);
                selectedClientId = workEmployee.SelectedClient.ID;
                userValue.Text = ClientsDG.SelectedItem.ToString();
            }
        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!EditUserEventSuccess())
            {
                MessageBox.Show("Не удалось отредактировать пользователя.");
                return;
            }

            //LoadClientsToGUI();

            ClientsDG.Items.SortDescriptions.Clear();
            ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("ID", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();
        }

        private void addNewClientBtn_Click(object sender, RoutedEventArgs e)
        {
            AddNewClientEventSuccess();
            ClientsDG.Items.SortDescriptions.Clear();
            ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("ID", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();
        }

        private void removeClientBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!RemoveClientEventSuccess())
            {
                MessageBox.Show("Не удалось удалить клиента.");
            }
            //List<Client> list = new List<Client>();
            //list.AddRange(Clients);
            //var updatedClientList = employee.DeleteClient(list, selectedClientId);

            //Clients.Clear();
            //foreach (var item in updatedClientList)
            //{
            //    Clients.Add(item);
            //}
            ClientsDG.Items.SortDescriptions.Clear();
            ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("ID", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            ep.Visibility = Visibility.Collapsed;
            ep = null;
            NavigationService.GoBack();
        }
    }
}
