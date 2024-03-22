using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;

namespace Cumulative1.Controllers
{
    public class StudentDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();


        /// <summary>
        /// Returns a list of Students in the School database
        /// </summary>
        /// <param name="id">The key to search through student names</param>
        /// <returns>
        /// A list of student objects.
        /// </returns>
        /// <example>GET api/StudentData/ListStudents -> -> [{"EnrolDate":"2018-06-18T00:00:00","FirstName":"Sarah","LastName":"Valdez","StudentId":"1","StudentNumber":"N1678"},{"EnrolDate":"2018-08-02T00:00:00","FirstName":"Jennifer","LastName":"Faulkner","StudentId":"2","StudentNumber":"N1679"}]</example>
        /// <example>GET api/StudentData/ListStudents/Sarah -> [{"EnrolDate":"2018-06-18T00:00:00","FirstName":"Sarah","LastName":"Valdez","StudentId":"1","StudentNumber":"N1678"}]<example>
        [HttpGet]
        [Route("api/StudentData/ListStudents/{SearchKey?}")]
        public IEnumerable<Student> ListAllStudentData(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Students where lower(studentfname) like lower(@key) or lower(studentlname) like lower(@key) or lower(concat(studentfname, ' ', studentlname)) like lower(@key)";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey +"%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Student> StudentsData = new List<Student> { };

            while (ResultSet.Read())
            {
                Student Student = new Student();
                Student.FirstName = (string)ResultSet["studentfname"];
                Student.StudentId = (uint)ResultSet["studentid"];
                Student.LastName = (string)ResultSet["studentlname"];
                Student.StudentNumber = (string)ResultSet["studentnumber"];
                Student.EnrolDate = (DateTime)ResultSet["enroldate"];
                StudentsData.Add(Student);
            }

            //Close the connection between the web server and database
            Conn.Close();

            return StudentsData;
        }

        /// <summary>
        /// Returns an individual student from the database by specifying the primary key studentid
        /// </summary>
        /// <param name="id">the student's ID in the database</param>
        /// <returns>An student object</returns>
        /// <example>GET /api/StudentData/FindStudent/1 -> {"EnrolDate":"2018-06-18T00:00:00","FirstName":"Sarah","LastName":"Valdez","StudentId":"1","StudentNumber":"N1678"}</example>
        [HttpGet]
        [Route("api/StudentData/ListStudent/{id}")]
        public Student FindStudent(int id)
        {
            Student Student = new Student();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Students where studentid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                Student.FirstName = (string)ResultSet["studentfname"];
                Student.StudentId = (uint)ResultSet["studentid"];
                Student.LastName = (string)ResultSet["studentlname"];
                Student.StudentNumber = (string)ResultSet["studentnumber"];
                Student.EnrolDate = (DateTime)ResultSet["enroldate"];
            }

            //Close the connection between the web server and database
            Conn.Close();

            return Student;
        }

    }
}