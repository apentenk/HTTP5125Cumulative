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

        
        [HttpGet]
        public Student MakeStudent(MySqlDataReader Row)
        { 
            Student student = new Student();
            
            student.FirstName = (string)Row["studentfname"];
            student.StudentId = (uint)Row["studentid"];
            student.LastName = (string)Row["studentlname"];
            student.StudentNumber = (string)Row["studentnumber"];
            student.EnrolDate = (DateTime)Row["enroldate"];
            return student;
        }

        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>GET api/StudentData/ListStudents</example>
        /// <returns>
        /// A list of student objects.
        /// </returns>
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
                Student student = MakeStudent(ResultSet);
                StudentsData.Add(student);
            }

            Conn.Close();

            return StudentsData;
        }

        /// <summary>
        /// Returns an individual student from the database by specifying the primary key studentid
        /// </summary>
        /// <param name="id">the student's ID in the database</param>
        /// <returns>An student object</returns>
        [HttpGet]
        public Student FindStudent(int id)
        {
            Student FoundStudent = new Student();

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
                FoundStudent = MakeStudent(ResultSet);
            }

            Conn.Close();
            return FoundStudent;
        }

    }
}