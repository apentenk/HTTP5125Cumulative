using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Cumulative1.Controllers
{
    public class CourseDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        
        [HttpGet]
        public Course MakeCourse(MySqlDataReader Row)
        { 
            Course course = new Course();
            course.ClassId = (int)Row["classid"];
            course.ClassCode = (string)Row["classcode"];
            course.TeacherID = (int)Row["teacherid"];
            course.StartDate = (DateTime)Row["startdate"];
            course.FinishDate = (DateTime)Row["finishdate"];
            course.ClassName = (string)Row["classname"];
            return course;
        }

        /// <summary>
        /// Returns a list of Courses in the system
        /// </summary>
        /// <example>GET api/ClassData/ListClasses</example>
        /// <returns>
        /// A list of class objects.
        /// </returns>
        [HttpGet]
        [Route("api/ClassData/ListClasses/{SearchKey?}")]
        public IEnumerable<Course> ListAllCourseData(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Classes where lower(classname) like lower(@key) or lower(classcode) like lower(@key) or lower(concat(classcode, ' ', classname)) like lower(@key)";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey +"%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Course> CoursesData = new List<Course> { };

            while (ResultSet.Read())
            {
                Course course = MakeCourse(ResultSet);
                CoursesData.Add(course);
            }

            Conn.Close();

            return CoursesData;
        }

        /// <summary>
        /// Returns an individual course from the database by specifying the primary key classid
        /// </summary>
        /// <param name="id">the course's ID in the database</param>
        /// <returns>An course object</returns>
        [HttpGet]
        public Course FindCourse(int id)
        {
            Course FoundCourse = new Course();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Classes where classid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                FoundCourse = MakeCourse(ResultSet);
            }

            Conn.Close();
            return FoundCourse;
        }

    }
}