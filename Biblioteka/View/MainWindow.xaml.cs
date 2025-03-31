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

namespace Biblioteka.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame? MainFrame { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Make _mainFrame accessible from the outside (to allow navigation from ViewModels):
            MainFrame = _mainFrame;

            // Navigate initially to LoginPage:
            _mainFrame.NavigationService.Navigate(new Uri("View/LoginPage.xaml", UriKind.Relative));
        }
    }
}
