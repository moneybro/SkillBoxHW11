using ClassLibrary.Classes;
using ClientApp.Interfaces;
using ClientApp.ViewModels;
using System;
using System.Collections.Generic;
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

namespace ClientApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthClientPage.xaml
    /// </summary>
    public partial class AuthClientPage : Page
    {
        Client client = null;
        public AuthClientPage()
        {
            InitializeComponent();
            var clients = new ClientsToAuth().Clients;
            ClientsToAuthCb.ItemsSource = clients;
        }

        private void ClientSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsToAuthCb.SelectedItem != null)
            {
                client = (Client)ClientsToAuthCb.SelectedItem;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                NavigationService.Navigate(new ClientPage(client));
            }
        }

        private void ClosePage(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
