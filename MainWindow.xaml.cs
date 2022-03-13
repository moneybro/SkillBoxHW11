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

namespace SkillBoxHW11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        Employee employee;
        Consultant consultant = new Consultant();
        Manager manager = new Manager();
        Client client;
        bool dataFirstLoad = true;

        public MainWindow()
        {
            InitializeComponent();
            employee = manager;
            if (dataFirstLoad)
            {
                foreach (var item in employee.GetClients())
                {
                    Clients.Add(item);
                }
                dataFirstLoad = false;
            }
            else
            {
                List<Client> clientsShort = new List<Client>();
                clientsShort.AddRange(Clients);
                dataFirstLoad = false;
                Clients.Clear();
                foreach (var item in employee.RefreshClientsView(clientsShort))
                {
                    Clients.Add(item);
                }
            }
            ClientsDG.ItemsSource = Clients;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            client = (Client)ClientsDG.SelectedItem;
            userValue.Content = ClientsDG.SelectedItem;
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
                    break;
                case "менеджер":
                    employee = manager;
                    break;
            }
            for (int i = 0; i < ClientsDG.Items.Count; i++)
            {
                ClientsDG.Columns[5].
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Client tmpCl = employee.ChangeClient(client);
            Clients.Remove(client);
            Clients.Add(tmpCl);

            ClientsDG.Items.SortDescriptions.Clear();
            ClientsDG.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("ID", System.ComponentModel.ListSortDirection.Ascending));
            ClientsDG.Items.Refresh();

            //(ClientsDG.ItemsSource as DataView).Sort = "ID";
            //System.Data.DataView dv = (System.Data.DataView)ClientsDG.ItemsSource;
            //dv.Sort = "ID";
            //if (client != null)
            //{
            //    EditClient editClient = new EditClient(client, employee);
            //    editClient.Show();
            //}
        }

        private void ClientsDG_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            MessageBox.Show("ClientsDG_RowEditEnding");
        }
    }
}
