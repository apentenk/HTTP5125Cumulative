using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Cumulative1.Controllers
{
    public class ClassDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of Classes in the School database
        /// </summary>
        /// <param name="id">The key to search through class codes and names</param>
        /// <returns>
        /// A list of course objects.
        /// </returns>
        /// <example>GET api/ClassData/ListClasses -> -> [{"ClassCode":"http5101","ClassId":"1","ClassName":"Web Application Development","FinishDate":"2018-12-14T00:00:00","StartDate":"2018-09-04T00:00:00","TeacherID":"1"},{"ClassCode":"http5102","ClassId":"2","ClassName":"Project Management","FinishDate":"2018-12-14T00:00:00","StartDate":"2018-09-04T00:00:00","TeacherID":"2"}]</example>
        /// <example>GET api/ClasseData/ListClasses/5101 -> [{"ClassCode":"http5101","ClassId":"1","ClassName":"Web Application Development","FinishDate":"2018-09-04T00:00:00","StartDate":"2018-14-18T00:00:00","TeacherID":"1"}]<example>
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
                Course Course = new Course();
                Course.ClassId = (int)ResultSet["classid"];
                Course.ClassCode = (string)ResultSet["classcode"];
                Course.TeacherID = (long)ResultSet["teacherid"];
                Course.StartDate = (DateTime)ResultSet["startdate"];
                Course.FinishDate = (DateTime)ResultSet["finishdate"];
                Course.ClassName = (string)ResultSet["classname"];
                CoursesData.Add(Course);
            }

            Conn.Close();

            return CoursesData;
        }

        /// <summary>
        /// Returns a list of Classes in the School database that are taught by a specific teacher
        /// </summary>
        /// <param name="Teacherid">The ID representing the teacher for the classes</param>
        /// <returns>
        /// A list of course objects.
        /// </returns>
        /// <example>GET api/ClasseData/ListTeacherClasses/1 -> {"ClassCode":"http5101","ClassId":"1","ClassName":"Web Application Development","FinishDate":"2018-09-04T00:00:00","StartDate":"2018-14-18T00:00:00","TeacherID":"1"}<example>
        [HttpGet]
        [Route("api/ClassData/ListTeacherClasses/{SearchKey?}")]
        public IEnumerable<Course> ListTeacherCourses(long Teacherid)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Classes where classes.teacherid = "+Teacherid;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Course> CoursesData = new List<Course> { };

            while (ResultSet.Read())
            {
                Course Course = new Course();
                Course.ClassId = (int)ResultSet["classid"];
                Course.ClassCode = (string)ResultSet["classcode"];
                Course.TeacherID = (long)ResultSet["teacherid"];
                Course.StartDate = (DateTime)ResultSet["startdate"];
                Course.FinishDate = (DateTime)ResultSet["finishdate"];
                Course.ClassName = (string)ResultSet["classname"]; ;
                CoursesData.Add(Course);
            }

            //Close the connection between the web server and database
            Conn.Close();

            return CoursesData;
        }

        /// <summary>
        /// Returns an individual class from the database by specifying the primary key classid
        /// </summary>
        /// <param name="id">the course's ID in the database</param>
        /// <returns>A course object</returns>
        /// <example>GET api/ClasseData/ListTeacherClasses/1 -> {"ClassCode":"http5101","ClassId":"1","ClassName":"Web Application Development","FinishDate":"2018-09-04T00:00:00","StartDate":"2018-14-18T00:00:00","TeacherID":"1"}<example>
        [HttpGet]
        [Route("api/ClassData/ListClass/{id}")]
        public Course FindCourse(int id)
        {
            Course Course = new Course();

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
                Course.ClassId = (int)ResultSet["classid"];
                Course.ClassCode = (string)ResultSet["classcode"];
                Course.TeacherID = (long)ResultSet["teacherid"];
                Course.StartDate = (DateTime)ResultSet["startdate"];
                Course.FinishDate = (DateTime)ResultSet["finishdate"];
                Course.ClassName = (string)ResultSet["classname"]; ;
            }

            //Close the connection between the web server and database
            Conn.Close();
            
            return Course;
        }

    }
}