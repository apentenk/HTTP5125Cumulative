using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cumulative1.Models
{
    public class TeacherCourseList
    {
        public Teacher Teacher { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}