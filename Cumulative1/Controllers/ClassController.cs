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
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Class/List
        public ActionResult List(string SearchKey = null)
        {
            ClassDataController Controller = new ClassDataController();
            IEnumerable<Course> Courses = Controller.ListAllCourseData(SearchKey);
            return View(Courses);
        }

        //GET: /Class/Show/{id}
        public ActionResult Show(int id)
        {
            ClassDataController Controller = new ClassDataController();
            Course Course = Controller.FindCourse(id);
            return View(Course);
        }

        //GET Class/New
        public ActionResult New()
        {
            return View();
        }


        //GET : /Class/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            ClassDataController controller = new ClassDataController();
            Course NewCourse = controller.FindCourse(id);


            return View(NewCourse);
        }

        //POST : /Course/Update
        [HttpPost]
        public ActionResult Update(int id, string ClassCode, string ClassName, DateTime StartDate, DateTime FinishDate, long TeacherID)
        {
            Course CourseInfo = new Course();
            CourseInfo.ClassCode = ClassCode;
            CourseInfo.ClassName = ClassName;
            CourseInfo.StartDate = StartDate;
            CourseInfo.FinishDate = FinishDate;
            CourseInfo.TeacherID = TeacherID;

            ClassDataController controller = new ClassDataController();
            controller.UpdateCourse(id, CourseInfo);

            return RedirectToAction("Show/" + id);
        }

        //GET : /Course/Update/{id}
        public ActionResult Update(int id)
        {
            ClassDataController controller = new ClassDataController();
            Course SelectedClass = controller.FindCourse(id);

            return View(SelectedClass);
        }
    }
}