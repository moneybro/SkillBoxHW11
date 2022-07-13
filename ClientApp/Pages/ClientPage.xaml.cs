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
using ClassLibrary.Classes;
using ClassLibrary.Methods;
using ClientApp.ViewModels;

namespace ClientApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        ClientPage? cp;
        WorkClient workClient;
        public event Action OpenMainAccBtnClicked;
        public event Action OpenDepoAccBtnClicked;
        public event Action<BankAccForClient> CloseAccBtnClicked;
        public event Action<object> AddMoneyBtnClicked;
        public event Action<object> TfrMoneyBtnClicked;

        public ClientPage(Client client)
        {
            cp = this;
            InitializeComponent();
            workClient = new WorkClient(client, this);
            this.DataContext = workClient;
            ClientFIO.Text = workClient.fullName;
        }

        private void ClosePage(object sender, RoutedEventArgs e)
        {
            cp = null;
            NavigationService.GoBack();
        }
        private void OpenMainAcc(object sender, RoutedEventArgs e)
        {
            OpenMainAccBtnClicked();
        }
        private void CloseMainAcc(object sender, RoutedEventArgs e)
        {
            CloseAccBtnClicked(workClient.mainAcc);
        }
        private void OpenDepoAcc(object sender, RoutedEventArgs e)
        {
            OpenDepoAccBtnClicked();
        }
        private void CloseDepoAcc(object sender, RoutedEventArgs e)
        {
            CloseAccBtnClicked(workClient.depoAcc);
        }

        private void PutMoneyToMainAccBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMoneyBtnClicked(sender);
        }            
        private void TransferMoneyBtn_Click(object sender, RoutedEventArgs e)
        {
            TfrMoneyBtnClicked(sender);
        }        
    }
}
