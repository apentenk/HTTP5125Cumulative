using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

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
                if (ResultSet["teacherid"].GetType() != typeof(DBNull)) Course.TeacherID = (long)ResultSet["teacherid"];
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
                if (ResultSet["teacherid"].GetType() != typeof(DBNull)) Course.TeacherID = (long)ResultSet["teacherid"];
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
                if (ResultSet["teacherid"].GetType() != typeof(DBNull)) Course.TeacherID = (long)ResultSet["teacherid"];
                Course.StartDate = (DateTime)ResultSet["startdate"];
                Course.FinishDate = (DateTime)ResultSet["finishdate"];
                Course.ClassName = (string)ResultSet["classname"]; ;
            }

            //Close the connection between the web server and database
            Conn.Close();
            
            return Course;
        }

        /// <summary>
        /// Adds an Class to the School Database.
        /// </summary>
        /// <param name="NewCourse">An object with fields that map to the columns of the classes table. Non-Deterministic.</param>
        /// <example>
        /// POST api/ClassData/AddClass 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ClassCode":"http5101",
        ///	"ClassName":"Web Application Development",
        ///	"TeacherID":"1",
        ///	"StartDate":"2018-09-04"
        ///	"FinishDate":"2018-12-14"
        /// }
        /// </example>
        [HttpPost]
        [Route("api/ClassData/AddClass/")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddCourse([FromBody] Course NewCourse)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into classes (className, classCode, teacherID, startDate, finishDate) values (@ClassName, @ClassCode, @TeacherID, @StartDate, @FinishDate)";
            cmd.Parameters.AddWithValue("@ClassName", NewCourse.ClassName);
            cmd.Parameters.AddWithValue("@ClassCode", NewCourse.ClassCode);
            cmd.Parameters.AddWithValue("@TeacherID", NewCourse.TeacherID);
            cmd.Parameters.AddWithValue("@StartDate", NewCourse.StartDate);
            cmd.Parameters.AddWithValue("@FinishDate", NewCourse.FinishDate);
            cmd.Prepare();

            //Execute Query
            cmd.ExecuteNonQuery();

            //Close the connection between the web server and database
            Conn.Close();

        }

        /// <summary>
        /// Deletes a Class from the School Database if the ID of that class exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the Class.</param>
        /// <example>POST /api/ClassData/DeleteClass/3</example>
        [HttpDelete]
        [Route("api/ClassData/DeleteClass/{id}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteClass(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from classes where classid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Execute Query
            cmd.ExecuteNonQuery();

            //Close the connection between the web server and database
            Conn.Close();


        }

        /// <summary>
        /// Updates a Class from the School Database. 
        /// </summary>
        /// <param name="id">the class's ID in the database</param>
        /// <param name="ClassInfo">An object with fields that map to the columns of the classes table.</param>
        /// <example>
        /// POST api/ClassData/UpdateClass/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ClassCode":"http5125",
        ///	"ClassName":"Back-end Web Development 1",
        ///	"StartDate":"2015-05-15 00:00:00",
        ///	"FinishDate":"2015-08-15 00:00:00"
        ///	"TeacherID":T012
        /// }
        /// </example>
        [HttpPost]
        [Route("api/ClassData/UpdateClass/{id}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateCourse(int id, [FromBody] Course ClassInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Debug.WriteLine(ClassInfo.AuthorFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update classes set classCode=@ClassCode, className=@ClassName, startDate=@StartDate, finishDate=@FinishDate, teacherId=@TeacherID  where classid=@ClassId";
            cmd.Parameters.AddWithValue("@ClassCode", ClassInfo.ClassCode);
            cmd.Parameters.AddWithValue("@ClassName", ClassInfo.ClassName);
            cmd.Parameters.AddWithValue("@StartDate", ClassInfo.StartDate);
            cmd.Parameters.AddWithValue("@FinishDate", ClassInfo.FinishDate);
            cmd.Parameters.AddWithValue("@TeacherID", ClassInfo.TeacherID);
            cmd.Parameters.AddWithValue("@ClassId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }

    }
}