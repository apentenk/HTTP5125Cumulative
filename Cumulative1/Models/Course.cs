using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cumulative1.Models
{
    public class Course
    {
        public int ClassId { get; set; }
        public string ClassCode { get; set; }
        public int TeacherID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string ClassName { get; set; }
    }
}