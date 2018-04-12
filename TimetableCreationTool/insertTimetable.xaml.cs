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
    /// Interaction logic for insertTimetable.xaml
    /// </summary>
    public partial class insertTimetable : Window
    {
        //string currentDay;
        //string currentTime;
        public insertTimetable(string day, string time, string courseId, string courseName)
        {
            InitializeComponent();

            dayTextBox.Text = day;
            timeTextBox.Text = time;
            courseNameTextBox.Text = courseName;


        }
    }
}
