using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace SchoolManagementSystem
{
    public partial class AddTeachersForm : UserControl
    {
        MySqlConnection connect = new MySqlConnection("Server=localhost;Database=school_system;Uid=root;Pwd=092423;");
        private string imagePath;

        public AddTeachersForm()
        {
            InitializeComponent();
            TeacherDisplayData();
        }

        public void TeacherDisplayData()
        {
            AddTeachersData addTD = new AddTeachersData();
            teacher_gridData.DataSource = addTD.TeacherData();
        }

        private void Teacher_addBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(teacher_id.Text)
                || string.IsNullOrWhiteSpace(teacher_name.Text)
                || string.IsNullOrWhiteSpace(teacher_gender.Text)
                || string.IsNullOrWhiteSpace(teacher_address.Text)
                || string.IsNullOrWhiteSpace(teacher_status.Text)
                || teacher_image.Image == null
                || string.IsNullOrWhiteSpace(imagePath))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    connect.Open();
                    string checkTeacherID = "SELECT COUNT(*) FROM teachers WHERE teacher_id = @teacherID";

                    using (MySqlCommand checkTID = new MySqlCommand(checkTeacherID, connect))
                    {
                        checkTID.Parameters.AddWithValue("@teacherID", teacher_id.Text.Trim());
                        int count = Convert.ToInt32(checkTID.ExecuteScalar());

                        if (count >= 1)
                        {
                            MessageBox.Show("Teacher ID: " + teacher_id.Text.Trim() + " already exists", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            DateTime today = DateTime.Today;
                            string insertData = "INSERT INTO teachers " +
                                "(teacher_id, teacher_name, teacher_gender, teacher_address, " +
                                "teacher_image, teacher_status, date_insert) " +
                                "VALUES(@teacherID, @teacherName, @teacherGender, @teacherAddress," +
                                "@teacherImage, @teacherStatus, @dateInsert)";

                            string directoryPath = Path.GetDirectoryName(imagePath);
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            File.Copy(imagePath, Path.Combine(directoryPath, Path.GetFileName(imagePath)), true);

                            using (MySqlCommand cmd = new MySqlCommand(insertData, connect))
                            {
                                cmd.Parameters.AddWithValue("@teacherID", teacher_id.Text.Trim());
                                cmd.Parameters.AddWithValue("@teacherName", teacher_name.Text.Trim());
                                cmd.Parameters.AddWithValue("@teacherGender", teacher_gender.Text.Trim());
                                cmd.Parameters.AddWithValue("@teacherAddress", teacher_address.Text.Trim());
                                cmd.Parameters.AddWithValue("@teacherImage", imagePath);
                                cmd.Parameters.AddWithValue("@teacherStatus", teacher_status.Text.Trim());
                                cmd.Parameters.AddWithValue("@dateInsert", today.ToString());

                                cmd.ExecuteNonQuery();

                                TeacherDisplayData();

                                MessageBox.Show("Added successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ClearFields();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting Database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        private void ClearFields()
        {
            teacher_id.Text = "";
            teacher_name.Text = "";
            teacher_gender.SelectedIndex = -1;
            teacher_address.Text = "";
            teacher_status.SelectedIndex = -1;
            teacher_image.Image = null;
            imagePath = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image files (*.jpg; *.png)|*.jpg;*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                imagePath = open.FileName;
                teacher_image.ImageLocation = imagePath;
            }
        }

        private void Teacher_clearBtn_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void Teacher_updateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(teacher_id.Text)
                || string.IsNullOrWhiteSpace(teacher_name.Text)
                || string.IsNullOrWhiteSpace(teacher_gender.Text)
                || string.IsNullOrWhiteSpace(teacher_address.Text)
                || string.IsNullOrWhiteSpace(teacher_status.Text)
                || teacher_image.Image == null
                || string.IsNullOrWhiteSpace(imagePath))
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    connect.Open();
                    DialogResult check = MessageBox.Show("Are you sure you want to Update Teacher ID: "
                        + teacher_id.Text.Trim() + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (check == DialogResult.Yes)
                    {
                        DateTime today = DateTime.Today;
                        string updateData = "UPDATE teachers SET " +
                            "teacher_name = @teacherName, teacher_gender = @teacherGender" +
                            ", teacher_address = @teacherAddress, teacher_status = @teacherStatus" +
                            ", date_update = @dateUpdate WHERE teacher_id = @teacherID";

                        using (MySqlCommand cmd = new MySqlCommand(updateData, connect))
                        {
                            cmd.Parameters.AddWithValue("@teacherName", teacher_name.Text.Trim());
                            cmd.Parameters.AddWithValue("@teacherGender", teacher_gender.Text.Trim());
                            cmd.Parameters.AddWithValue("@teacherAddress", teacher_address.Text.Trim());
                            cmd.Parameters.AddWithValue("@teacherStatus", teacher_status.Text.Trim());
                            cmd.Parameters.AddWithValue("@dateUpdate", today.ToString());
                            cmd.Parameters.AddWithValue("@teacherID", teacher_id.Text.Trim());

                            cmd.ExecuteNonQuery();

                            TeacherDisplayData();

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
                    MessageBox.Show("Error connecting Database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        /*private void Teacher_deleteBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(teacher_id.Text))
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to Delete Teacher ID: "
                    + teacher_id.Text + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        connect.Open();
                        DateTime today = DateTime.Today;
                        string deleteData = "UPDATE teachers SET date_delete = @dateDelete " +
                            "WHERE teacher_id = @teacherID";

                        using (MySqlCommand cmd = new MySqlCommand(deleteData, connect))
                        {
                            cmd.Parameters.AddWithValue("@dateDelete", today.ToString());
                            cmd.Parameters.AddWithValue("@teacherID", teacher_id.Text.Trim());

                            cmd.ExecuteNonQuery();

                            TeacherDisplayData();

                            MessageBox.Show("Deleted successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ClearFields();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting Database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }*/

        private void Teacher_gridData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = teacher_gridData.Rows[e.RowIndex];
                teacher_id.Text = row.Cells[1].Value.ToString();
                teacher_name.Text = row.Cells[2].Value.ToString();
                teacher_gender.Text = row.Cells[3].Value.ToString();
                teacher_address.Text = row.Cells[4].Value.ToString();
                imagePath = row.Cells[5].Value.ToString();
                string imageData = row.Cells[5].Value.ToString();

                if (imageData != null && imageData.Length > 0)
                {
                    teacher_image.Image = Image.FromFile(imageData);
                }
                else
                {
                    teacher_image.Image = null;
                }

                teacher_status.Text = row.Cells[6].Value.ToString();
            }
        }
    }
}
