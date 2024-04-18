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

        //POST : /Teacher/Update
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, decimal Salary)
        {
            Teacher TeacherInfo = new Teacher();
            TeacherInfo.FirstName = TeacherFname;
            TeacherInfo.LastName = TeacherLname;
            TeacherInfo.EmployeeNumber = EmployeeNumber;
            TeacherInfo.HireDate = HireDate;
            TeacherInfo.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);

            return RedirectToAction("Show/" + id);
        }

        //GET : /Teacher/Update/{id}
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }
    }
}