//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimetableCreationTool
{
    using System;
    using System.Collections.Generic;
    
    public partial class Timetable
    {
        public int id { get; set; }
        public int courseId { get; set; }
        public int lecturerId { get; set; }
        public int roomId { get; set; }
        public System.TimeSpan startTime { get; set; }
        public string day { get; set; }
        public int duration { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public virtual Room Room { get; set; }
    }
}
