using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        /// <summary>
        /// Returns a list of Teachers in the School database
        /// </summary>
        /// <param name="id">The key to search through teachers names</param>
        /// <returns>
        /// A list of teacher objects.
        /// </returns>
        /// <example>GET api/TeacherData/ListTeachers -> -> [{"EmployeeNumber":"T378","FirstName":"Alexander","HireDate":"2016-08-05T00:00:00","LastName":"Bennett","Salary":"55.30","TeacherId":"1"},{"EmployeeNumber":"T381","FirstName":"Caitlin","HireDate":"2014-06-10T00:00:00","LastName":"Cummings","Salary":"62.77","TeacherId":"2"}]</example>
        /// <example>GET api/TeacherData/ListTeachers/Alex -> [{"EmployeeNumber":"T378","FirstName":"Alexander","HireDate":"2016-08-05T00:00:00","LastName":"Bennett","Salary":"55.30","TeacherId":"1"}]<example>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListAllTeacherData(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey +"%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Teacher> TeachersData = new List<Teacher> { };

            while (ResultSet.Read())
            {
                Teacher Teacher = new Teacher();
                Teacher.TeacherId = (int)ResultSet["teacherid"];
                Teacher.FirstName = (string)ResultSet["teacherfname"];
                Teacher.LastName = (string)ResultSet["teacherlname"];
                Teacher.EmployeeNumber = (string)ResultSet["employeenumber"];
                Teacher.HireDate = (DateTime)ResultSet["hiredate"];
                Teacher.Salary = (decimal)ResultSet["salary"];
                TeachersData.Add(Teacher);
            }

            //Close the connection between the web server and database
            Conn.Close();

            return TeachersData;
        }


        /// <summary>
        /// Returns an individual teacher from the database by specifying the primary key teacherid
        /// </summary>
        /// <param name="id">the teacher's ID in the database</param>
        /// <returns>An teacher object</returns>
        /// <example>GET api/TeacherData/FindTeacher/1 -> [{"EmployeeNumber":"T378","FirstName":"Alexander","HireDate":"2016-08-05T00:00:00","LastName":"Bennett","Salary":"55.30","TeacherId":"1"}<example>
        [HttpGet]
        [Route("api/TeacherData/ListTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            Teacher Teacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                Teacher.TeacherId = (int)ResultSet["teacherid"];
                Teacher.FirstName = (string)ResultSet["teacherfname"];
                Teacher.LastName = (string)ResultSet["teacherlname"];
                Teacher.EmployeeNumber = (string)ResultSet["employeenumber"];
                Teacher.HireDate = (DateTime)ResultSet["hiredate"];
                Teacher.Salary = (decimal)ResultSet["salary"];
            }

            //Close the connection between the web server and database
            Conn.Close();

            return Teacher;
        }

    }
}