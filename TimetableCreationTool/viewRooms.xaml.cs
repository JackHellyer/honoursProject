using System;
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
    /// Interaction logic for viewRooms.xaml
    /// </summary>
    public partial class viewRooms : Window
    {
        public viewRooms()
        {
            InitializeComponent();
        }
        private timetableCreationEntities2 dbcontext;
        private System.Windows.Data.CollectionViewSource roomsViewSource;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.dbcontext = new timetableCreationEntities2();
            this.roomsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("roomsViewSource")));

            var query = from Room in this.dbcontext.Rooms
                        orderby Room.roomCode
                        select Room;
            this.roomsViewSource.Source = query.ToList();
        }
    }
}
