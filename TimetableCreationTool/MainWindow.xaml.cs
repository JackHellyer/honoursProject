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
        private timetableCreationEntities dbcontext;
        
        public string userMyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public MainWindow()
        {
            InitializeComponent();
        }
        
        public void newTimetable_Click(object sender, RoutedEventArgs e)
        {
            newTimetableDialog nt = new newTimetableDialog();
            nt.Show();
            
            
           
        }

        public void loadTimetable_Click(object sender, RoutedEventArgs e)
        {

        }


        public void menuExit_Click(object Sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            
        }

        public void menuInsertRoomCSV_Click(object Sender, RoutedEventArgs e)
        {
            insertRoomCsv irc = new insertRoomCsv();

            bool? result = irc.ShowDialog();
 
            System.IO.Directory.CreateDirectory(userMyDocumentsPath + "/Timetable App");
            createExampleCSVFile();
             
        }

       



        public void createExampleCSVFile()
        {
            string pathToCreateCSVFile = userMyDocumentsPath + "/Timetable App/rooms.txt";
           
            
                if (File.Exists(pathToCreateCSVFile))
                {

                }
                else
                {

                    try
                    {
                        using (FileStream fs = File.Create(pathToCreateCSVFile))
                        {
                        Byte[] info = new UTF8Encoding(true).GetBytes("roomCode,capacity,lab");
                        fs.Write(info, 0, info.Length);

                        }
                        
                        
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());

                    }
                    
                    
                }

        }
    
       

        


    }
}
