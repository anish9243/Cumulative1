
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cumulative1.Models;
using MySql.Data.MySqlClient;

namespace Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        //The database context class which allows us to access our MySQL Database.

        private SchoolDbContext School = new SchoolDbContext();

        // This Controller interacts with the teacher table in our school database.
        /// <summary>
        /// Retrieves a list of all teachers in the system.
        /// </summary>
        /// <returns>A list of teachers.</returns>
        /// <example>To get the list of teachers, send a GET request to api/TeacherData/ListTeachers.</example>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key) or hiredate like @key or salary like @key";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;


                Teachers.Add(NewTeacher);
            }

            Conn.Close();

            return Teachers;
        }

        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from teachers where teacherid =" + @id;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                string EmployeeNumber = (string)ResultSet["employeenumber"];
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

            }
            Conn.Close();

            return NewTeacher;
        }
        /// <summary>
        /// Adds a new teacher to the database.
        /// </summary>
        /// <param name="NewTeacher">The teacher object to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <example>
        /// POST request to api/TeacherData/AddTeacher with a teacher object in the body.
        /// </example>
        [HttpPost]

        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();


            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            string query = "insert into teachers (teacherid, teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherId, @TeacherFname, @TeacherLname, @EmployeeNumber, @HireDate, @Salary)";

            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@TeacherId", NewTeacher.TeacherId);
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);


            cmd.Prepare();
            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// Deletes a teacher from the database based on their ID.
        /// </summary>
        /// <param name="TeacherId">The ID of the teacher to delete, which is the primary key.</param>
        /// <example>
        /// To delete a teacher with ID 2, send a POST request to: api/teacherdata/deleteteacher/2
        /// </example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            string query = "DELETE from teachers where teacherid=@id";
            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// Updates a teacher record in the database.
        /// </summary>
        /// <example>
        /// POST: api/teacherdata/updateteacher/{teacherid}
        /// </example>
        [HttpPost]
        [Route("api/teacherdata/updateteacher/{TeacherId}")]
        public void UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber=@EmployeeNumber, hiredate=@HireDate, salary=@Salary where teacherid=@TeacherId";

            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", TeacherInfo.HireDate);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@TeacherId", id);

            cmd.Prepare();
            cmd.ExecuteNonQuery();

            Conn.Close();


        }

    }
}
