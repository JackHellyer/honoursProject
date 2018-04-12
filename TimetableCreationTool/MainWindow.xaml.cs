using Microsoft.VisualBasic.FileIO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace TimetableCreationTool
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
        public string userMyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string dbConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;  Initial Catalog = timetableCreation; Integrated Security = True; Connect Timeout = 30";

        public void newTimetable_Click(object sender, RoutedEventArgs e)
        {
            newTimetableDialog nt = new newTimetableDialog();
            nt.Show();
            //this.Close();
            
           
        }
        
        
        public void loadTimetable_Click(object sender, RoutedEventArgs e)
        {
            
            
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.SelectedPath = userMyDocumentsPath + @"\" + @"Timetable App";
                fbd.ShowNewFolderButton = false;
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    int index = fbd.SelectedPath.LastIndexOf(@"\");
                    string tName = fbd.SelectedPath.Substring(index + 1);
                    //MessageBox.Show(tName);
                    insertRoomCsv irc = new insertRoomCsv(tName);
                    insertLecturerCSV ilc = new insertLecturerCSV(tName);
                    insertCourseCSV icc = new insertCourseCSV(tName);
                    insertModuleCSV imc = new insertModuleCSV(tName);
                    bool ifValid = ifVaildLoadFile(fbd.SelectedPath);
                    if(ifValid)
                    {
                        DataTable roomCSV = irc.getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/" + tName + "/" + "rooms.txt");
                        irc.InsertDataTableToSQL(roomCSV);
                        irc.selectIntoDistinct();
                        irc.truncateTempAfterCSVInsert();
                        DataTable lecturerCSV = irc.getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/" + tName + "/" + "lecturers.txt");
                        ilc.InsertDataTableToSQL(lecturerCSV);
                        ilc.selectIntoDistinct();
                        ilc.truncateTempAfterCSVInsert();
                        DataTable courseCSV = icc.getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/" + tName + "/" + "courses.txt");
                        icc.InsertDataTableToSQL(courseCSV);
                        icc.selectIntoDistinct();
                        icc.truncateTempAfterCSVInsert();
                        DataTable modulesCSV = imc.getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/" + tName + "/" + "modules.txt");
                        imc.InsertDataTableToSQL(modulesCSV);
                        imc.selectIntoDistinct();
                        imc.truncateTempAfterCSVInsert();

                        DataTable courseModuleCSV = imc.getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/" + tName + "/" + "coursemodules.txt");
                        InsertDataTableToSQL(courseModuleCSV);


                        Window1 win1 = new Window1(tName);
                        win1.Show();
                        irc.Close();
                        ilc.Close();
                        icc.Close();
                        imc.Close();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Timetable must be loaded from the Timetable App folder");
                    }
                    
                    
                    
                        
                        
                        


                }
                    


            }
                

        }
        private bool ifVaildLoadFile(string t)
        {
             bool ifValid = false;
             var dir = Directory.GetDirectories(userMyDocumentsPath + @"\Timetable App\");
             foreach (var i in dir)
             { 
                //MessageBox.Show("i:" + i);
                if (t == i)
                {
                    
                    
                    ifValid = true;
                    break;
                    
                
                }
                else
                {
                    ifValid = false;
                    
                    
                }
                    


             }
             return ifValid;
        }

        public void InsertDataTableToSQL(DataTable csvFileData)
        {
            using (SqlConnection dbConnection = new SqlConnection(dbConnectionString))
            {


                dbConnection.Open();
                if (dbConnection.State == ConnectionState.Open)
                {

                    //MessageBox.Show("connection success");
                    using (SqlBulkCopy sbc = new SqlBulkCopy(dbConnection))
                    {
                        // change this method later to have a string parameter which will hold the destination table
                        sbc.DestinationTableName = "dbo.Course_Module";

                        foreach (var column in csvFileData.Columns)

                            sbc.ColumnMappings.Add(column.ToString(), column.ToString());
                        sbc.WriteToServer(csvFileData);
                        dbConnection.Close();



                    }
                }
                else
                {
                    MessageBox.Show("connection failed");
                }





            }
        }

    }
}
