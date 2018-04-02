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
    /// Interaction logic for ReportUserControl.xaml
    /// </summary>
    public partial class ReportUserControl : UserControl
    {
        public ReportUserControl()
        {
            InitializeComponent();
        }

        private void ReportUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible == true)
            {
                ReportDataGrid.ItemsSource = CrawlerQueries.GetCrawlerDetails();
            }
            }
        private void btnGoToHome_Click(object sender, RoutedEventArgs e)
        {
            var win = new System.Windows.Window();
            win.Content = new HomeControl();
            win.Show();
        }
    }
}
