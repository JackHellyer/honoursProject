﻿using System;
using System.Collections.Generic;
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
    /// Interaction logic for viewCourses.xaml
    /// </summary>
    public partial class viewCourses : Window
    {
        public viewCourses()
        {
            InitializeComponent();
        }

        private timetableCreationEntities3 dbcontext;
        private System.Windows.Data.CollectionViewSource coursesViewSource;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.dbcontext = new timetableCreationEntities3();
            this.coursesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("coursesViewSource")));

            var query = from Course in this.dbcontext.Courses
                        orderby Course.courseCode
                        select Course;
            this.coursesViewSource.Source = query.ToList();

            

            
        }
    }
}
