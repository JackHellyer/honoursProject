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

namespace TimetableCreationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private timetableCreationEntities dbcontext;
        public MainWindow()
        {
            InitializeComponent();
        }


        public void menuExit_Click(object Sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            
        }

        public void menuInsertRoomCSV_Click(object Sender, RoutedEventArgs e)
        {
            insertRoomCsv irc = new insertRoomCsv();

            bool? result = irc.ShowDialog();

            
        }

        
    }
}
