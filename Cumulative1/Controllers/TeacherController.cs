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
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET: Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController Controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = Controller.ListAllTeacherData(SearchKey);
            return View(Teachers);
        }

        //GET Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController TeacherController = new TeacherDataController();
            Teacher Teacher = TeacherController.FindTeacher(id);
            ClassDataController CourseController = new ClassDataController();
            IEnumerable<Course> Courses = CourseController.ListTeacherCourses((long)id);
            TeacherCourseList CourseList = new TeacherCourseList();
            CourseList.Courses = Courses;
            CourseList.Teacher = Teacher;
            return View(CourseList);
        }

        //GET Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //GET : /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);


            return View(NewTeacher);
        }
    }
}