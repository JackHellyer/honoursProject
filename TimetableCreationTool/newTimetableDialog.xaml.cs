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

namespace TimetableCreationTool
{
    /// <summary>
    /// Interaction logic for newTimetableDialog.xaml
    /// </summary>
    public partial class newTimetableDialog : Window
    {
        
        public newTimetableDialog()
        {
            InitializeComponent();
        }

        public void createTimetable_Click(object sender, RoutedEventArgs e)
        {
            string timetableName = timetableNameTextBox.Text;
            Window1 win1 = new Window1(timetableName);
            win1.Show();
            this.Close();
            window.Close();
           

        }
    }
}
