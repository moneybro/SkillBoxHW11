using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
using ClassLibrary.Classes;

namespace SkillBoxHW13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        static public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        Employee employee;
        Consultant consultant = new Consultant();
        Manager manager = new Manager();
        Client client;
        long selectedClientId;

        EmployeePage ep;

        public EmployeePage()
        {
            ep = this;
            InitializeComponent();
            employee = consultant;
            operatorCB.SelectedIndex = 0;
            addNewClientBtn.IsEnabled = false;
            removeClientBtn.IsEnabled = false;
            LoadClientsToGUI();
            ClientsDG.ItemsSource = Clients;
        }
        void LoadClientsToGUI()
        {
            Clients.Clear();
            foreach (var item in employee.GetClients())
            {
                Clients.Add(item);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            client = (Client)ClientsDG.SelectedItem;
            if (client != null)
            {
                selectedClientId = client.ID;
                userValue.Text = ClientsDG.SelectedItem.ToString();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            var lowerCase = selectedItem.Content.ToString().ToLower();
            switch (lowerCase)
            {
                case "консультант":
                    employee = consultant;
                    addNewClientBtn.IsEnabled = false;
                    removeClientBtn.IsEnabled = false;
                    break;
                case "менеджер":
                    employee = manager;
                    addNewClientBtn.IsEnabled = true;
                    removeClientBtn.IsEnabled = true;
                    break;
            }
            Clients.Clear();
            foreach (var item in employee.GetClients())
            {
                Clients.Add(item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!employee.ChangeClient(selectedClientId))
            {
                MessageBox.Show("Не удалось отредактировать пользователя");
                return;
            }

            LoadClientsToGUI();

            ClientsDG.Items.SortDescriptions.Clear();
            ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("ID", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();
        }

        private void addNewClientBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!employee.AddNewClient())
            {
                MessageBox.Show("Не удалось создать нового пользователя");
                return;
            }
            LoadClientsToGUI();
            ClientsDG.Items.SortDescriptions.Clear();
            ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("ID", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();
        }

        private void removeClientBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Client> list = new List<Client>();
            list.AddRange(Clients);
            var updatedClientList = employee.DeleteClient(list, selectedClientId);
            
            Clients.Clear();
            foreach (var item in updatedClientList)
            {
                Clients.Add(item);
            }
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
