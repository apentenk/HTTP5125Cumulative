using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cumulative1.Controllers
{
    public class CourseController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(string SearchKey = null)
        {
            CourseDataController Controller = new CourseDataController();
            IEnumerable<Course> Courses = Controller.ListAllCourseData(SearchKey);
            return View(Courses);
        }

        public ActionResult Show(int id)
        {
            CourseDataController Controller = new CourseDataController();
            Course Course = Controller.FindCourse(id);
            return View(Course);
        }
    }
}