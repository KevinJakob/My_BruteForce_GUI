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
using BruteForceGui.ViewModels;

namespace BruteForceGui.Views
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private MainViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainViewModel();
            DataContext = _vm;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            _vm.StartBruteForceAsync();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            _vm.Reset();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            _vm.ProgressStoped();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _vm.ProgressContinue();
        }
    }
}
