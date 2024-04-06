using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Cors;

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

        /// <summary>
        /// Adds an Teacher to the School Database.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teachers table. Non-Deterministic.</param>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"John",
        ///	"TeacherLname":"Doe",
        ///	"EmployeeNumber":"T402",
        ///	"HireDate":"2015-05-15 00:00:00"
        ///	"salary":63.84
        /// }
        /// </example>
        [HttpPost]
        [Route("api/TeacherData/AddTeacher/")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            Debug.WriteLine(NewTeacher.FirstName);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname,@TeacherLname,@EmployeeNumber, @HireDate, @Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.FirstName);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.LastName);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            //Execute Query
            cmd.ExecuteNonQuery();

            //Close the connection between the web server and database
            Conn.Close();



        }
        /// <summary>
        /// Deletes an Teacher from the School Database if the ID of that teacher exists. Maintains relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/3</example>
        [HttpDelete]
        [Route("api/TeacherData/DeleteTeacher/{id}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();


            cmd.ExecuteNonQuery();

            //Updating Courses
            cmd.CommandText = "Update classes set teacherid =  null where teacherid = @tid";
            cmd.Parameters.AddWithValue("@tid", id);
            cmd.Prepare();

            //Execute Query
            cmd.ExecuteNonQuery();

            //Close the connection between the web server and database
            Conn.Close();


        }

    }

}