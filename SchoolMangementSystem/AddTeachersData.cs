using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace SchoolManagementSystem
{
    class AddTeachersData
    {
        MySqlConnection connect = new MySqlConnection("Server=localhost;Database=school_system;Uid=root;Pwd=092423;");
        public int ID { set; get; }
        public string TeacherID { set; get; }
        public string TeacherName { set; get; }
        public string TeacherGender { set; get; }
        public string TeacherAddress { set; get; }
        public string TeacherImage { set; get; }
        public string Status { set; get; }
        public string DateInsert { set; get; }

        public List<AddTeachersData> TeacherData()
        {
            List<AddTeachersData> listData = new List<AddTeachersData>();
            if (connect.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string sql = "SELECT * FROM teachers WHERE date_delete IS NULL";

                    using (MySqlCommand cmd = new MySqlCommand(sql, connect))
                    {
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            AddTeachersData addTD = new AddTeachersData();
                            addTD.ID = Convert.ToInt32(reader["id"]);
                            addTD.TeacherID = reader["teacher_id"].ToString();
                            addTD.TeacherName = reader["teacher_name"].ToString();
                            addTD.TeacherGender = reader["teacher_gender"].ToString();
                            addTD.TeacherAddress = reader["teacher_address"].ToString();
                            addTD.TeacherImage = reader["teacher_image"].ToString();
                            addTD.Status = reader["teacher_status"].ToString();
                            addTD.DateInsert = reader["date_insert"].ToString();

                            listData.Add(addTD);
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
    }
}