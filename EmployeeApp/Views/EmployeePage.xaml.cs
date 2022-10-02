using ClassLibrary.Classes;
using EmployeeApp.Classes;
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
        public EmployeePage(Employee employee)
        {
            ep = this;
            InitializeComponent();
            workEmployee = new WorkEmployee(employee, this);
            DataContext = workEmployee;
        }
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            ep.Visibility = Visibility.Collapsed;
            ep = null;
            workEmployee = null;
            NavigationService.GoBack();
        }
        private void clientAccsGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bankAccsListBox.Height = clientAccsGrid.ActualHeight - 25;
        }
    }
}
