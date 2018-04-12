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
    /// Interaction logic for addModulesStudied.xaml
    /// </summary>
    public partial class addModulesStudied : Window
    {
        string cName;
        string cId;
        private string dbConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;  Initial Catalog = timetableCreation; Integrated Security = True; Connect Timeout = 30";
        public addModulesStudied(string courseName, string courseId)
        {
            InitializeComponent();
            cName = courseName;
            cId = courseId;
            bindComboBox(comboBox);

        }
        private timetableCreationEntities dbcontext;
        private System.Windows.Data.CollectionViewSource moduleViewSource;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.dbcontext = new timetableCreationEntities();
            this.moduleViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("moduleViewSource")));

            var query = from Module in this.dbcontext.Modules
                        orderby Module.moduleCode
                        select Module;
            var query2 = dbcontext.Modules.Where(s => s.Courses.Any(c => c.courseCode == cName));
            this.moduleViewSource.Source = query2.ToList();

            
        }

        public void bindComboBox(ComboBox comboBoxName)
        {
            string query = "select moduleId, moduleCode, moduleName from dbo.Module ORDER BY moduleCode";
            SqlConnection conn = new SqlConnection(dbConnectionString);
            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();
            sda.Fill(ds, "dbo.Module");
            comboBoxName.ItemsSource = ds.Tables[0].DefaultView;
            comboBoxName.DisplayMemberPath = ds.Tables[0].Columns["moduleName"].ToString();
            comboBoxName.SelectedValuePath = ds.Tables[0].Columns["moduleId"].ToString();
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            MessageBox.Show(comboBox.SelectedValue.ToString() + " " + comboBox.Text);
        }

        private void addModules_Click(object sender, RoutedEventArgs e)
        {
            int courseId = int.Parse(cId);
            int moduleId = int.Parse(comboBox.SelectedValue.ToString());
            string query = "INSERT INTO Course_Module (courseId, moduleId) VALUES (" + courseId + ","  + moduleId + ");";

            SqlConnection conn = new SqlConnection(dbConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
    }
}
