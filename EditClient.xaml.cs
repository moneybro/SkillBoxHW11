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
using System.Windows.Shapes;

namespace SkillBoxHW11
{
    /// <summary>
    /// Логика взаимодействия для EditClient.xaml
    /// </summary>
    public partial class EditClient : Window
    {
        public Client editedClient;
        public EditClient()
        {
            InitializeComponent();
        }

        //public EditClient(Client client)
        //{
        //    InitializeComponent();
        //    editedClient = client;
        //    EditLastName.Text = editedClient.LastName;
        //    EditName.Text = editedClient.Name;
        //    EditPatronymic.Text = editedClient.Patronymic;
        //    EditMobPhone.Text = editedClient.MobPhone;
        //    EditPaspSeria.Text = editedClient.PaspSeria == 0 ? "****" : editedClient.PaspSeria.ToString();
        //    EditPaspNum.Text = editedClient.PaspNum == 0 ? "****" : editedClient.PaspNum.ToString();
        //    EditLastName.IsEnabled = false;
        //    EditName.IsEnabled = false;
        //    EditPatronymic.IsEnabled = false;
        //    EditPaspSeria.IsEnabled = false;
        //    EditPaspNum.IsEnabled = false;
        //}

        public EditClient(Client client, Employee employee)
        {
            InitializeComponent();
            editedClient = client;
            EditLastName.Text = editedClient.LastName;
            EditName.Text = editedClient.Name;
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

        //private Client Button_Click(object sender, RoutedEventArgs e)
        //{

        //    return editedClient;
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            editedClient.LastName = EditLastName.Text;
            editedClient.Name = EditName.Text;
            editedClient.Patronymic = EditPatronymic.Text;
            editedClient.MobPhone = EditMobPhone.Text;
            int paspSeria;
            if (int.TryParse(EditPaspSeria.Text, out paspSeria)) editedClient.PaspSeria = paspSeria;
            else editedClient.PaspSeria = 0;
            long paspNum;
            if (long.TryParse(EditPaspNum.Text, out paspNum)) editedClient.PaspNum = paspNum;
            else editedClient.PaspSeria = 0;
            this.DialogResult = true;
        }
    }
}
