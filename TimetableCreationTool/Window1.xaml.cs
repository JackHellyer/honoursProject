using System;
using System.Collections.Generic;
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
        
        public string userMyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string timetableName;
        public Window1(string tName)
        {
            InitializeComponent();
            timetableName = tName;
            this.Title = timetableName;
        }
        

        public void menuExit_Click(object Sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        public void menuInsertRoomCSV_Click(object Sender, RoutedEventArgs e)
        {
            insertRoomCsv irc = new insertRoomCsv(timetableName);
            string fileName = "rooms.txt";
            string tableColumns = "roomCode,capacity,lab";
            createExampleCSVFile(fileName, tableColumns);
            bool? result = irc.ShowDialog();

            

        }

        public void createExampleCSVFile(string fileName, string tableColumns)
        {
            string pathToCreateCSVFile = userMyDocumentsPath + "/Timetable App/" + timetableName +"/" + fileName;


            if (File.Exists(pathToCreateCSVFile))
            {

            }
            else
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
    }
}
