using System;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;

namespace SchoolManagementSystem
{
    public partial class AddStudentForm : UserControl
    {
        MySqlConnection connect = new MySqlConnection("Server=localhost;Database=school_system;Uid=root;Pwd=092423;");
        public AddStudentForm()
        {
            InitializeComponent();
            DisplayStudentData();
        }

        public void DisplayStudentData()
        {
            AddStudentData adData = new AddStudentData();
            student_studentData.DataSource = adData.StudentData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(student_id.Text)
                || string.IsNullOrWhiteSpace(student_name.Text)
                || string.IsNullOrWhiteSpace(student_gender.Text)
                || string.IsNullOrWhiteSpace(student_address.Text)
                || string.IsNullOrWhiteSpace(student_grade.Text)
                || string.IsNullOrWhiteSpace(student_section.Text)
                || string.IsNullOrWhiteSpace(student_status.Text)
                || student_image.Image == null
                || string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        string checkStudentID = "SELECT COUNT(*) FROM students WHERE student_id = @studentID";

                        using (MySqlCommand checkSID = new MySqlCommand(checkStudentID, connect))
                        {
                            checkSID.Parameters.AddWithValue("@studentID", student_id.Text.Trim());
                            int count = Convert.ToInt32(checkSID.ExecuteScalar());

                            if (count >= 1)
                            {
                                MessageBox.Show("Student ID: " + student_id.Text.Trim() + " is already exist"
                                    , "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime today = DateTime.Today;
                                string insertData = "INSERT INTO students (student_id, student_name" +
                                    ", student_gender, student_address, student_grade, student_section" +
                                    ", student_image, student_status, date_insert) " +
                                    "VALUES(@studentID, @studentName, @studentGender, @studentAddress" +
                                    ", @studentGrade, @studentSection, @studentImage, @studentStatus" +
                                    ", @dateInsert)";

                                string path = Path.Combine(@"C:\Users\WINDOWS 10\source\repos\SchoolMangementSystem\SchoolMangementSystem\Student_Directory\", student_id.Text.Trim() + ".jpg");
                                string directoryPath = Path.GetDirectoryName(path);

                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                File.Copy(imagePath, path, true);

                                using (MySqlCommand cmd = new MySqlCommand(insertData, connect))
                                {
                                    cmd.Parameters.AddWithValue("@studentID", student_id.Text.Trim());
                                    cmd.Parameters.AddWithValue("@studentName", student_name.Text.Trim());
                                    cmd.Parameters.AddWithValue("@studentGender", student_gender.Text.Trim());
                                    cmd.Parameters.AddWithValue("@studentAddress", student_address.Text.Trim());
                                    cmd.Parameters.AddWithValue("@studentGrade", student_grade.Text.Trim());
                                    cmd.Parameters.AddWithValue("@studentSection", student_section.Text.Trim());
                                    cmd.Parameters.AddWithValue("@studentImage", path);
                                    cmd.Parameters.AddWithValue("@studentStatus", student_status.Text.Trim());
                                    cmd.Parameters.AddWithValue("@dateInsert", today.ToString("yyyy-MM-dd"));

                                    cmd.ExecuteNonQuery();

                                    DisplayStudentData();

                                    MessageBox.Show("Added successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    ClearFields();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting Database: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        public void ClearFields()
        {
            student_id.Text = "";
            student_name.Text = "";
            student_gender.SelectedIndex = -1;
            student_address.Text = "";
            student_grade.SelectedIndex = -1;
            student_section.SelectedIndex = -1;
            student_status.SelectedIndex = -1;
            student_image.Image = null;
            imagePath = "";
        }

        public string imagePath;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image files (*.jpg; *.png)|*.jpg;*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                imagePath = open.FileName;
                student_image.ImageLocation = imagePath;
            }
        }

        private void Student_updateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(student_id.Text)
                || string.IsNullOrWhiteSpace(student_name.Text)
                || string.IsNullOrWhiteSpace(student_gender.Text)
                || string.IsNullOrWhiteSpace(student_address.Text)
                || string.IsNullOrWhiteSpace(student_grade.Text)
                || string.IsNullOrWhiteSpace(student_section.Text)
                || string.IsNullOrWhiteSpace(student_status.Text)
                || student_image.Image == null
                || string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();
                        DialogResult check = MessageBox.Show("Are you sure you want to Update Student ID: "
                            + student_id.Text.Trim() + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (check == DialogResult.Yes)
                        {
                            DateTime today = DateTime.Today;
                            String updateData = "UPDATE students SET student_name = @studentName, " +
                                "student_gender = @studentGender, student_address = @studentAddress, " +
                                "student_grade = @studentGrade, student_section = @studentSection, " +
                                "student_status = @studentStatus, date_update = @dateUpdate " +
                                "WHERE student_id = @studentID";

                            using (MySqlCommand cmd = new MySqlCommand(updateData, connect))
                            {
                                cmd.Parameters.AddWithValue("@studentName", student_name.Text.Trim());
                                cmd.Parameters.AddWithValue("@studentGender", student_gender.Text.Trim());
                                cmd.Parameters.AddWithValue("@studentAddress", student_address.Text.Trim());
                                cmd.Parameters.AddWithValue("@studentGrade", student_grade.Text.Trim());
                                cmd.Parameters.AddWithValue("@studentSection", student_section.Text.Trim());
                                cmd.Parameters.AddWithValue("@studentStatus", student_status.Text.Trim());
                                cmd.Parameters.AddWithValue("@dateUpdate", today.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@studentID", student_id.Text.Trim());

                                cmd.ExecuteNonQuery();

                                DisplayStudentData();

                                MessageBox.Show("Updated successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ClearFields();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cancelled.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ClearFields();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting Database: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void Student_studentData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = student_studentData.Rows[e.RowIndex];
                student_id.Text = row.Cells[1].Value.ToString();
                student_name.Text = row.Cells[2].Value.ToString();
                student_gender.Text = row.Cells[3].Value.ToString();
                student_address.Text = row.Cells[4].Value.ToString();
                student_grade.Text = row.Cells[5].Value.ToString();
                student_section.Text = row.Cells[6].Value.ToString();
                imagePath = row.Cells[5].Value.ToString();
                string imageData = row.Cells[7].Value.ToString();

                if (imageData != null && imageData.Length > 0)
                {
                    student_image.Image = Image.FromFile(imageData);
                }
                else
                {
                    student_image.Image = null;
                }

                student_status.Text = row.Cells[8].Value.ToString();
            }
        }

        private void AddStudentForm_Load(object sender, EventArgs e)
        {

        }

        /*private void Student_deleteBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(student_id.Text))
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    DialogResult check = MessageBox.Show("Are you sure you want to Delete Student ID: "
                        + student_id.Text + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (check == DialogResult.Yes)
                    {
                        try
                        {
                            connect.Open();
                            DateTime today = DateTime.Today;
                            string deleteData = "UPDATE students SET date_delete = @dateDelete " +
                                "WHERE student_id = @studentID";

                            using (MySqlCommand cmd = new MySqlCommand(deleteData, connect))
                            {
                                cmd.Parameters.AddWithValue("@dateDelete", today.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@studentID", student_id.Text.Trim());

                                cmd.ExecuteNonQuery();
                                DisplayStudentData();
                                MessageBox.Show("Deleted successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ClearFields();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error connecting Database: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cancelled.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }*/
    }
}
