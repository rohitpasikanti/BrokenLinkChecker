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


namespace BrokenLinkChecker
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        public HomeControl()
        {
            InitializeComponent();
            
        }

      
        private void BtnBrokenLinkChecker_Click_1(object sender, RoutedEventArgs e)
        {
            var win = new System.Windows.Window();
            win.Content = new ProjectItemControl();
            win.Show();
            
        }
    }
}
