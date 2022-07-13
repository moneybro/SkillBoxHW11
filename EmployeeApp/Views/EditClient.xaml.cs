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

namespace EmployeeApp.Views
{
    /// <summary>
    /// Логика взаимодействия для EditClient.xaml
    /// </summary>
    public partial class EditClient : Window
    {
        public Client editedClient;
        Employee bankOperator;
        public EditClient()
        {

        }
        public EditClient(long clId)
        {
            InitializeComponent();
            bankOperator = new Manager();
            editedClient = new Client(clId);
        }

        public EditClient(Client client, Employee employee)
        {
            InitializeComponent();
            editedClient = client;
            bankOperator = employee;
            EditLastName.Text = editedClient.LastName;
            EditName.Text = editedClient.FirstName;
            EditPatronymic.Text = editedClient.Patronymic;
            EditMobPhone.Text = editedClient.MobPhone;

            switch (employee.GetType().Name)
            {
                case "Consultant":
                    EditLastName.IsEnabled = false;
                    EditName.IsEnabled = false;
                    EditPatronymic.IsEnabled = false;
                    EditPaspSeria.IsEnabled = false;
                    EditPaspNum.IsEnabled = false;
                    EditPaspSeria.Text = "****";
                    EditPaspNum.Text = "******";

                    break;
                case "Manager":
                    EditPaspSeria.Text = editedClient.PaspSeria.ToString();
                    EditPaspNum.Text = editedClient.PaspNum.ToString();
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Client client = editedClient.Clone();
            int paspSeria;
            long paspNum;

            if (bankOperator.GetType() == typeof(Manager))
            {
                editedClient.LastName = EditLastName.Text;
                editedClient.FirstName = EditName.Text;
                editedClient.Patronymic = EditPatronymic.Text;
                editedClient.MobPhone = EditMobPhone.Text;
                if (int.TryParse(EditPaspSeria.Text, out paspSeria)) editedClient.PaspSeria = paspSeria;
                else editedClient.PaspSeria = 0;
                if (long.TryParse(EditPaspNum.Text, out paspNum)) editedClient.PaspNum = paspNum;
                else editedClient.PaspNum = 0;
            }

            if (bankOperator.GetType() == typeof(Consultant))
            {
                editedClient.MobPhone = EditMobPhone.Text;
            }
            this.DialogResult = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //this.DialogResult = false;
        }
    }
}
