using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //private Model1Container dbContext;
        
        string userMyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string timetableName;
        private string dbConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;  Initial Catalog = timetableCreation; Integrated Security = True; Connect Timeout = 30";
        //private System.Windows.Data.CollectionViewSource roomsViewSource;

        public Window1(string tName)
        {
            InitializeComponent();
            timetableName = tName;
            this.Title = timetableName;
           
        }
        
        
        public void menuSave_Click(object sender, RoutedEventArgs e)
        {
            saveDbToCSVFile("roomCode,capacity, lab", "rooms.txt", "dbo.Room");
            saveDbToCSVFile("lecturerName,lecturerDept,modulesTaught", "lecturers.txt", "dbo.Lecturer");

        }

       
        public void menuExit_Click(object sender, RoutedEventArgs e)
        {
            
            Application.Current.Shutdown();

        }

        public void menuViewRooms_Click(object sender, RoutedEventArgs e)
        {
            viewRooms vr = new viewRooms();
            vr.Owner = this;
            vr.Show();
            //Window_Loaded(sender, e);




        }

        public void menuViewLecturers_Click(object sender, RoutedEventArgs e)
        {
            viewLecturers vl = new viewLecturers();
            vl.Owner = this;
            vl.Show();
        }

        public void menuInsertRoomCSV_Click(object Sender, RoutedEventArgs e)
        {
            
            insertRoomCsv irc = new insertRoomCsv(timetableName);
            irc.Owner = this;
            string fileName = "rooms.txt";
            string tableColumns = "roomCode,capacity,lab";
            createExampleCSVFile(fileName, tableColumns);
            bool? result = irc.ShowDialog();

            

        }

        private void menuLecturersCSV_Click(object sender, RoutedEventArgs e)
        {
            insertLecturerCSV ilc = new insertLecturerCSV(timetableName);
            ilc.Owner = this;
            string fileName = "lecturers.txt";
            string tableColumns = "lecturerName,lecturerDept,modulesTaught";
            createExampleCSVFile(fileName, tableColumns);
            ilc.Show();
        }

        public void saveDbToCSVFile(string columns, string fName, string tableName)
        {
            SqlConnection dbConnection = new SqlConnection(dbConnectionString);
            dbConnection.Open();
            
            //string command = "SELECT" + 
            SqlCommand selectRooms = new SqlCommand("SELECT " + columns + " FROM " + tableName + ";", dbConnection);
            SqlDataReader sdr = selectRooms.ExecuteReader();

            string fileName = userMyDocumentsPath + "/Timetable App/" + timetableName + "/" + fName;
            StreamWriter sw = new StreamWriter(fileName);
            object[] output = new object[sdr.FieldCount];

            for (int i = 0; i < sdr.FieldCount; i++)
                output[i] = sdr.GetName(i);

            sw.WriteLine(string.Join(",", output));

            while (sdr.Read())
            {
                sdr.GetValues(output);
                sw.WriteLine(string.Join(",", output));
            }

            sw.Close();
            sdr.Close();
            dbConnection.Close();
        }

        public void truncateAllTables()
        {
            string queryString = "DELETE FROM dbo.Room DBCC CHECKIDENT ('timetableCreation.dbo.Room', RESEED, 0)";
            using (SqlConnection dbConnection = new SqlConnection(dbConnectionString))
            {

                SqlCommand command = new SqlCommand(queryString, dbConnection);
                dbConnection.Open();

                SqlDataReader reader = command.ExecuteReader();

                dbConnection.Close();


            }
        }

        

        public void createExampleCSVFile(string fileName, string tableColumns)
        {
            string pathToCreateCSVFile = userMyDocumentsPath + "/Timetable App/" + timetableName +"/" + fileName;


            if (!File.Exists(pathToCreateCSVFile))
            {
                try
                {
                    using (FileStream fs = File.Create(pathToCreateCSVFile))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(tableColumns);
                        fs.Write(info, 0, info.Length);

                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());

                }
            }
           

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save before exiting", "Exit", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                
            }
            else
            {
                saveDbToCSVFile("roomCode,capacity, lab", "rooms.txt", "dbo.Room");
                saveDbToCSVFile("lecturerName,lecturerDept,modulesTaught", "lecturers.txt", "dbo.Lecturer");
            }
        }

        
    }
}
