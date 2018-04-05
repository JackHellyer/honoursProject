using Microsoft.VisualBasic.FileIO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

            //bool? result = irc.ShowDialog();
            string csvFilePath = @"C:\Users\Jack\Desktop\test.txt";
            DataTable csvData = getDataTableCSVFile(csvFilePath);

            //DataTable csvData = getDataTableCSVFile(@"C:\Users\Jack\Desktop\test.txt");
            InsertDataTableToSQL(csvData);
            listView.ItemsSource = csvData.DefaultView;
          
            




            
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
                }
            }
            catch( Exception ex)
            {
                MessageBox.Show("not working");
                MessageBox.Show(ex.Message);

            }
            return csvData;
        }

        static void InsertDataTableToSQL(DataTable csvFileData)
        {
            using (SqlConnection dbConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB;  Initial Catalog = timetableCreation; Integrated Security = True; Connect Timeout = 30"))
            {

                
                dbConnection.Open();
                if(dbConnection.State == ConnectionState.Open)
                {

                    MessageBox.Show("success");
                    using (SqlBulkCopy sbc = new SqlBulkCopy(dbConnection))
                    {
                        // change this method later to have a string parameter which will hold the destination table
                        sbc.DestinationTableName = "dbo.Room";

                        foreach (var column in csvFileData.Columns)
                     
                        sbc.ColumnMappings.Add(column.ToString(), column.ToString());
                        sbc.WriteToServer(csvFileData);
                        
                        

                    }
                }
                else
                {
                    MessageBox.Show("failed");
                }
                    
                
                
                
                
            }
        }

        
    }
}
