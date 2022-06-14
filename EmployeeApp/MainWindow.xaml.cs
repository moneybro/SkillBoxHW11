﻿using EmployeeApp.Views;
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

namespace EmployeeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Page workPage;
        public Page WorkPage
        {
            get { return workPage; }
            set { workPage = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            AuthPage authWin = new AuthPage();
            MainWindowFrame.Content = authWin;
        }
    }
}
