using EmployeeApp.ViewModels;
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
using ClassLibrary.Classes.Exceptions;
using EmployeeApp.Classes;

namespace EmployeeApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthWin2.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        List<Employee> employees = new List<Employee>();
        Employee man1 = new Manager(1, "Иванов", "Иван", 20, 10000);
        Employee man2 = new Manager(2, "Сидоров", "Сидор", 25, 10000);
        Employee errUser = new Manager(5, "злоумышленный", "манагер", 999, 9999999);
        Employee cons1 = new Consultant(3, "Блондинка", "Элла", 18, 7000);
        Employee cons2 = new Consultant(4, "Блондинка2", "Элла2", 18, 7000);
        
        Employee selectedEmployee;
        public AuthPage()
        {
            InitializeComponent();
            employees.Add(man1);
            employees.Add(man2);
            employees.Add(cons1);
            employees.Add(cons2);
            employees.Add(errUser);
            selectedEmployee = null;
            employeeCbox.ItemsSource = employees;
        }

        private void ClientBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(selectedEmployee != null)
                {
                    if (selectedEmployee.Id == 5)
                    {
                        throw new EmployeeAppExeption(1);
                    }
                    EmployeePage employeePage = new EmployeePage(selectedEmployee);
                    NavigationService.Navigate(employeePage);
                }
            }
            catch(EmployeeAppExeption ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void employeeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (employeeCbox.SelectedItem != null) selectedEmployee = (Employee)employeeCbox.SelectedItem;
        }
    }
}
