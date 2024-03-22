using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cumulative1.Models
{
    public class Teacher
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TeacherId {  get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

    }
}