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
        
        public void newTimetable_Click(object sender, RoutedEventArgs e)
        {
            newTimetableDialog nt = new newTimetableDialog();
            nt.Show();
            this.Close();
            
           
        }

        public void loadTimetable_Click(object sender, RoutedEventArgs e)
        {

        }


       

       



       

        
    
       

        


    }
}
