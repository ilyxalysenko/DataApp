using System;
using System.Linq;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DataApp_New_.Model;
using static DataApp_New_.ViewModel.ViewModel;
using System.Diagnostics;
using DataApp_New_.ViewModel;
using System.Windows.Controls;


namespace DataApp_New_
{
    public partial class MainWindow : Window
    {

        public void SaveChangesButtonClick(object sender, RoutedEventArgs e) //кнопка сохранить
        {
            ((ViewModel.ViewModel)DataContext).SaveChangesToDatabase(); //вызывает метод SaveChangesToDatabase из объекта ViewModel
        }


        public MainWindow() // Конструктор класса MainWindow
        {
            
            InitializeComponent(); //инициализирующий компоненты интерфейса 
            DataContext = new ViewModel.ViewModel(); //и устанавливающий контекст данных в новый экземпляр ViewModel.

            var viewModel = (ViewModel.ViewModel)DataContext;
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(viewModel.DataChanged))
                {
                    Debug.WriteLine("DataChanged property changed.");
                    if (viewModel.DataChanged)
                    {
                        Debug.WriteLine("DataChanged is true.");
                    }
                }
            };
        }
    }
}
