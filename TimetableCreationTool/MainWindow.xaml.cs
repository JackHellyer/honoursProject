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
        private string dbConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;  Initial Catalog = timetableCreation; Integrated Security = True; Connect Timeout = 30";
        private string userMyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public MainWindow()
        {
            InitializeComponent();
        }


        public void menuExit_Click(object Sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            
        }

        public void menuInsertRoomCSV_Click(object Sender, RoutedEventArgs e)
        {
            insertRoomCsv irc = new insertRoomCsv();

            bool? result = irc.ShowDialog();
            
            DataTable csvData = getDataTableCSVFile(userMyDocumentsPath + "/Timetable App/rooms.txt");

           
            //DataTable csvData = getDataTableCSVFile(@"C:\Users\Jack\Desktop\test.txt");
            InsertDataTableToSQL(csvData);
            selectIntoDistinct();
            truncateTempAfterCSVInsert();
            listView.ItemsSource = csvData.DefaultView;

            string userName2 = Environment.UserName;
            MessageBox.Show(userMyDocumentsPath);
            System.IO.Directory.CreateDirectory(userMyDocumentsPath + "/Timetable App");
            createExampleCSVFile();
             
        }

        public void openExternalCSVFile_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(userMyDocumentsPath + "/Timetable App/rooms.txt");
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
    
        public DataTable getDataTableCSVFile(string filePath)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(filePath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] columnFields = csvReader.ReadFields();
                    foreach(string column in columnFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while(!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        // making empty value as null
                        for(int i=0; i < fieldData.Length; i++)
                        {
                            if(fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }

                        }
                        csvData.Rows.Add(fieldData);
                        
                    }
                    csvReader.Close();
                }
                
            }
            catch( Exception ex)
            {
                MessageBox.Show("not working");
                MessageBox.Show(ex.Message);

            }
            return csvData;
        }
        

        public void InsertDataTableToSQL(DataTable csvFileData)
        {
            using (SqlConnection dbConnection = new SqlConnection(dbConnectionString))
            {

                
                dbConnection.Open();
                if(dbConnection.State == ConnectionState.Open)
                {

                    MessageBox.Show("connection success");
                    using (SqlBulkCopy sbc = new SqlBulkCopy(dbConnection))
                    {
                        // change this method later to have a string parameter which will hold the destination table
                        sbc.DestinationTableName = "dbo.roomTemp";

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

        public void selectIntoDistinct()
        {
            string queryString = "INSERT dbo.Room(roomCode,capacity,lab) SELECT roomCode,capacity,lab FROM dbo.roomTemp rt WHERE not exists(SELECT * FROM dbo.Room r WHERE rt.roomCode = r.roomCode);";
            using (SqlConnection dbConnection = new SqlConnection(dbConnectionString))
            {

                SqlCommand command = new SqlCommand(queryString, dbConnection);
                dbConnection.Open();

                SqlDataReader reader = command.ExecuteReader();
                    
                dbConnection.Close();
               
                
            }
        }
        public void truncateTempAfterCSVInsert()
        {
            string queryString = "TRUNCATE TABLE dbo.roomTemp;";
            using (SqlConnection dbConnection = new SqlConnection(dbConnectionString))
            {

                SqlCommand command = new SqlCommand(queryString, dbConnection);
                dbConnection.Open();

                SqlDataReader reader = command.ExecuteReader();

                dbConnection.Close();


            }
        }



    }
}
