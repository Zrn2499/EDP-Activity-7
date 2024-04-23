using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem
{
    class AddStudentData
    {
        MySqlConnection connect = new MySqlConnection("Server=localhost;Database=school_system;Uid=root;Pwd=092423;");
        public int ID { set; get; }
        public string StudentID { set; get; }
        public string StudentName { set; get; }
        public string StudentGender { set; get; }
        public string StudentAddress { set; get; }
        public string StudentGrade { set; get; }
        public string StudentSection { set; get; }
        public string StudentImage { set; get; }
        public string Status { set; get; }
        public string DateInsert { set; get; }

        public List<AddStudentData> StudentData()
        {
            List<AddStudentData> listData = new List<AddStudentData>();
            if (connect.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string sql = "SELECT * FROM students WHERE date_delete IS NULL";

                    using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            AddStudentData addSD = new AddStudentData();
                            addSD.ID = (int)reader["id"];
                            addSD.StudentID = reader["student_id"].ToString();
                            addSD.StudentName = reader["student_name"].ToString();
                            addSD.StudentGender = reader["student_gender"].ToString();
                            addSD.StudentAddress = reader["student_address"].ToString();
                            addSD.StudentGrade = reader["student_grade"].ToString();
                            addSD.StudentSection = reader["student_section"].ToString();
                            addSD.StudentImage = reader["student_image"].ToString();
                            addSD.Status = reader["student_status"].ToString();
                            addSD.DateInsert = reader["date_insert"].ToString();

                            listData.Add(addSD);
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting Database: " + ex);
                }
                finally
                {
                    connect.Close();
                }
            }
            return listData;

        }

        public List<AddStudentData> DashboardStudentData()
        {
            List<AddStudentData> listData = new List<AddStudentData>();

            if (connect.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    connect.Open();
                    DateTime today = DateTime.Today;
                    string sql = "SELECT * FROM students WHERE date_insert = @dateInsert " +
                        "AND date_delete IS NULL";

                    using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                    {
                        cmd.Parameters.AddWithValue("@dateInsert", today.ToString("yyyy-MM-dd"));
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            AddStudentData addSD = new AddStudentData();
                            addSD.ID = (int)reader["id"];
                            addSD.StudentID = reader["student_id"].ToString();
                            addSD.StudentName = reader["student_name"].ToString();
                            addSD.StudentGender = reader["student_gender"].ToString();
                            addSD.StudentAddress = reader["student_address"].ToString();
                            addSD.StudentGrade = reader["student_grade"].ToString();
                            addSD.StudentSection = reader["student_section"].ToString();
                            addSD.StudentImage = reader["student_image"].ToString();
                            addSD.Status = reader["student_status"].ToString();
                            addSD.DateInsert = reader["date_insert"].ToString();

                            listData.Add(addSD);
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
                finally
                {
                    connect.Close();
                }
            }
            return listData;
        }
    }
}
