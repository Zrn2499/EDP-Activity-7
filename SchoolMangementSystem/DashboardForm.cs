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

namespace SchoolManagementSystem
{
    public partial class DashboardForm : UserControl
    {
        MySqlConnection connect = new MySqlConnection("Server=localhost;Database=school_system;Uid=root;Pwd=092423;");

        public DashboardForm()
        {
            InitializeComponent();

            DisplayTotalES();
            DisplayTotalTT();
            DisplayTotalGS();
            DisplayEnrolledStudentToday();
        }

        public void DisplayTotalES()
        {
            try
            {
                connect.Open();
                string selectData = "SELECT COUNT(id) FROM students WHERE student_status = 'Enrolled' AND date_delete IS NULL";

                using (MySqlCommand cmd = new MySqlCommand(selectData, connect))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        total_ES.Text = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        public void DisplayTotalTT()
        {
            try
            {
                connect.Open();
                string selectData = "SELECT COUNT(id) FROM teachers WHERE teacher_status = 'Active' AND date_delete IS NULL";

                using (MySqlCommand cmd = new MySqlCommand(selectData, connect))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        total_TT.Text = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        public void DisplayTotalGS()
        {
            try
            {
                connect.Open();
                string selectData = "SELECT COUNT(id) FROM students WHERE student_status = 'Graduated' AND date_delete IS NULL";

                using (MySqlCommand cmd = new MySqlCommand(selectData, connect))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        total_GS.Text = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }

        public void DisplayEnrolledStudentToday()
        {
            AddStudentData asData = new AddStudentData();
            dataGridView1.DataSource = asData.DashboardStudentData();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {

        }
    }
}
