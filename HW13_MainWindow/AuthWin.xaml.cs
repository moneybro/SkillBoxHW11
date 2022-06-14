using ClientApp;
using ClientApp.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace HW13_MainWindow
{
    /// <summary>
    /// Логика взаимодействия для AuthWin.xaml
    /// </summary>
    public partial class AuthWin : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public AuthWin()
        {
            InitializeComponent();
            if (wp == null) this.Visibility = Visibility.Visible;
        }
        Page wp;
        public Page WorkPage 
        {
            get
            {
                return wp;
            }
            set
            {
                wp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(wp)));
            }
        }

        private void ClientBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthClientPage());
        }
    }
}
