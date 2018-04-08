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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
           
        }
       
            ProjectControl SelectedProjectControl = new ProjectControl();
        ReportUserControl SelectedHistoryControl = new ReportUserControl();


        public void OpenProjectControl()
            {
            MainBorder.Child = SelectedProjectControl;
            }

          
            public void OpenHomeControl()
        {
            MainBorder.Child = new HomeControl();
            

        }
        public void OpenHistoryControl()
        {
            MainBorder.Child = SelectedHistoryControl;
        }
        private void MainWindow_Loaded(Object sender, RoutedEventArgs e)
        {

            Application.Current.MainWindow=this;
        }
        
    }
}
