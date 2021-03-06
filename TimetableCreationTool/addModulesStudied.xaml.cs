﻿using System;
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
        private timetableCreationEntities3 dbcontext;
        private System.Windows.Data.CollectionViewSource moduleViewSource;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            onRefresh();
            
        }

        public void onRefresh()
        {
            this.dbcontext = new timetableCreationEntities3();
            this.moduleViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("moduleViewSource")));
            int intCId = int.Parse(cId);

            //var query2 = dbcontext.Modules.Where(m => m.Courses.Any(c => c.courseId == intCId));

            var query = from Module in this.dbcontext.Modules
                        where Module.Courses.Any(c => c.courseId == intCId)
                        orderby Module.moduleName
                        select Module;
            this.moduleViewSource.Source = query.ToList();
        }

        public void bindComboBox(ComboBox comboBoxName)
        {
            string query = "select moduleId, moduleCode, moduleName from dbo.Module ORDER BY moduleName";
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
            //MessageBox.Show(comboBox.SelectedValue.ToString() + " " + comboBox.Text);
        }

        private void addModules_Click(object sender, RoutedEventArgs e)
        {
            if(comboBox.SelectedItem != null)
            {
                int courseId = int.Parse(cId);
                int moduleId = int.Parse(comboBox.SelectedValue.ToString());
                //string query = "INSERT INTO Course_Module (courseId, moduleId) VALUES(" + courseId + "," + moduleId +");";
                string query2 = "INSERT dbo.Course_Module (courseId, moduleId) SELECT " + courseId + "," +  moduleId + " WHERE NOT EXISTS( SELECT courseId, moduleId FROM dbo.Course_Module WHERE courseId = " + courseId + " AND moduleId = " + moduleId + ");";

               


                SqlConnection conn = new SqlConnection(dbConnectionString);
                conn.Open();
                SqlCommand command = new SqlCommand(query2, conn);
                command.ExecuteNonQuery();
                conn.Close();

                onRefresh();
            }
            else
            {
                MessageBox.Show("No module selected.");
            }
            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            object selected = this.moduleListView.SelectedItem;
            //if there isn't an order selected, show message and return
            if (selected == null)
            {
                MessageBox.Show("No module selected");
                return;
            }
            Module module = (Module)selected;


            int mId = module.moduleId;

            string query = "DELETE FROM Course_Module WHERE courseId = " + cId + "AND moduleId = " + mId + ";";

            SqlConnection conn = new SqlConnection(dbConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
            conn.Close();

            onRefresh();
            //MessageBox.Show(mId.ToString());

            onRefresh();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
