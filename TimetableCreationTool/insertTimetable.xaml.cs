using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        
        private string dbConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;  Initial Catalog = timetableCreation; Integrated Security = True; Connect Timeout = 30";
        string cId;
        public insertTimetable(string day, string time, string courseId, string courseName)
        {
            InitializeComponent();

            dayTextBox.Text = day;
            timeTextBox.Text = time;
            cId = courseId;
            courseNameTextBox.Text = courseName;
            bindComboBox(moduleCombobox);
            bindComboBox1(roomCombobox);

        }

        public void bindComboBox(ComboBox comboBoxName)
        {
            string query = "select cm.moduleId, cm.courseId, m.moduleCode, m.moduleName from dbo.Module m, dbo.Course_Module cm WHERE m.moduleId = cm.moduleId AND cm.courseId = " + cId + " ORDER BY m.moduleName";
            SqlConnection conn = new SqlConnection(dbConnectionString);
            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();
            sda.Fill(ds, "dbo.Module");
            comboBoxName.ItemsSource = ds.Tables[0].DefaultView;
            comboBoxName.DisplayMemberPath = ds.Tables[0].Columns["moduleName"].ToString();
            comboBoxName.SelectedValuePath = ds.Tables[0].Columns["moduleId"].ToString();
        }

        public void bindComboBox1(ComboBox comboBoxName)
        {


            SqlConnection conn = new SqlConnection(dbConnectionString);
            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select r.roomId, r.roomCode from dbo.Room r, dbo.Course c WHERE c.courseId = @cId AND c.noOfStudents <= r.capacity ORDER BY r.roomCode;", conn);
            sda.SelectCommand.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@cId",
                    Value = cId
                    
            });
                //sda.SelectCommand.Parameters.AddWithValue("@day", dayTextBox.Text);
                DataSet ds = new DataSet();
                sda.Fill(ds, "dbo.Room");
                comboBoxName.ItemsSource = ds.Tables[0].DefaultView;
                comboBoxName.DisplayMemberPath = ds.Tables[0].Columns["roomCode"].ToString();
                comboBoxName.SelectedValuePath = ds.Tables[0].Columns["roomId"].ToString();
         



            // string query = "select r.roomId, r.roomCode from dbo.Room r, dbo.Course c, dbo.Timetable t WHERE c.courseId = " + cId + " AND c.noOfStudents <= r.capacity AND t.day <> " + dayTextBox.Text  + " ORDER BY roomCode;";

            
            
        }


    }
}
