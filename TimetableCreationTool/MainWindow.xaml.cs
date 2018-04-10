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

        public void newTimetable_Click(object sender, RoutedEventArgs e)
        {
            newTimetableDialog nt = new newTimetableDialog();
            nt.Show();
            //this.Close();
            
           
        }
        
        string temp;
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
                    bool ifValid = ifVaildLoadFile(fbd.SelectedPath);
                    if(ifValid)
                    {
                        DataTable roomCSV = irc.getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/" + tName + "/" + "rooms.txt");
                        irc.InsertDataTableToSQL(roomCSV);
                        irc.selectIntoDistinct();
                        irc.truncateTempAfterCSVInsert();
                        DataTable lecturerCSV = irc.getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/" + tName + "/" + "lecturers.txt");
                        ilc.InsertDataTableToSQL(lecturerCSV);
                        DataTable courseCSV = icc.getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/" + tName + "/" + "courses.txt");
                        icc.InsertDataTableToSQL(courseCSV);

                        
                        Window1 win1 = new Window1(tName);
                        win1.Show();
                        irc.Close();
                        ilc.Close();
                        icc.Close();
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

    }
}
