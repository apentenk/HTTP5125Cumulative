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

        //GET Student/New
        public ActionResult New()
        {
            return View();
        }

        //GET : /Student/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudent(id);


            return View(NewStudent);
        }

        //POST : /Student/Update
        [HttpPost]
        public ActionResult Update(int id, string StudentFname, string StudentLname, string StudentNumber, DateTime HireDate)
        {
            Student StudentInfo = new Student();
            StudentInfo.FirstName = StudentFname;
            StudentInfo.LastName = StudentLname;
            StudentInfo.StudentNumber = StudentNumber;
            StudentInfo.EnrolDate = HireDate;

            StudentDataController controller = new StudentDataController();
            controller.UpdateStudent(id, StudentInfo);

            return RedirectToAction("Show/" + id);
        }

        //GET : /Student/Update/{id}
        public ActionResult Update(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student SelectedStudent = controller.FindStudent(id);

            return View(SelectedStudent);
        }
    }
}