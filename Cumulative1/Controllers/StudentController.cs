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
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET : /Student/List
        public ActionResult List(string SearchKey = null)
        {
            StudentDataController Controller = new StudentDataController();
            IEnumerable<Student> Students = Controller.ListAllStudentData(SearchKey);
            return View(Students);
        }

        //GET : /Student/Show/{id}
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student Students = controller.FindStudent(id);
            return View(Students);
        }
    }
}