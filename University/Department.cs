using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace University
{
    public partial class Department : Form
    {
        private string connString = "Server=MSI\\SQLEXPRESS;Database=CollegeDatabase;Trusted_Connection=True;";



        public Department()
        {
            InitializeComponent();
            LoadDepartments();
            LoadCollegesIntoComboBox();
            txtDeptID.Enabled = false; 
        }


        private void LoadDepartments()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Department", conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvDepartments.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading departments: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCollegesIntoComboBox()
        {
            string query = "SELECT CollegeID, CollegeName FROM College";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Dictionary<int, string> colleges = new Dictionary<int, string>();

                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                colleges.Add(id, name);
                            }

                            cmbCollege.DataSource = new BindingSource(colleges, null);
                            cmbCollege.DisplayMember = "Value"; 
                            cmbCollege.ValueMember = "Key";    
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading colleges: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearFields()
        {
            cmbCollege.SelectedIndex = -1; 
            txtDeptName.Clear();
            txtDeptCode.Clear();
            chkIsActive.Checked = false;
        }


        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (cmbCollege.SelectedValue == null)
            {
                MessageBox.Show("Please select a college before adding a department.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDeptName.Text) || string.IsNullOrWhiteSpace(txtDeptCode.Text))
            {
                MessageBox.Show("Please enter both Department Name and Department Code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO Department (DepartmentName, DepartmentCode, CollegeID, IsActive) VALUES (@Name, @Code, @CollegeID, @IsActive)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtDeptName.Text);
                        cmd.Parameters.AddWithValue("@Code", txtDeptCode.Text);
                        cmd.Parameters.AddWithValue("@CollegeID", cmbCollege.SelectedValue); 
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Department added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDepartments(); 
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding department: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeptID.Text))
            {
                MessageBox.Show("Please select a department to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCollege.SelectedValue == null)
            {
                MessageBox.Show("Please select a college.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Department SET DepartmentName = @Name, DepartmentCode = @Code, CollegeID = @CollegeID, IsActive = @IsActive WHERE DepartmentID = @ID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtDeptID.Text));
                        cmd.Parameters.AddWithValue("@Name", txtDeptName.Text);
                        cmd.Parameters.AddWithValue("@Code", txtDeptCode.Text);
                        cmd.Parameters.AddWithValue("@CollegeID", cmbCollege.SelectedValue);
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Department updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDepartments(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating department: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeptID.Text))
            {
                MessageBox.Show("Please select a department to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this department? This cannot be undone.",
                                                  "Confirm Deletion",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Department WHERE DepartmentID = @ID";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtDeptID.Text));
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Department deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDepartments(); 
                                ClearFields();
                            }
                            else
                            {
                                MessageBox.Show("No department was deleted. Please check if the record exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (SqlException ex) when (ex.Number == 547) 
                {
                    MessageBox.Show("This department is linked to other records and cannot be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting the department: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dgvDepartments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvDepartments.Rows[e.RowIndex];

                    txtDeptID.Text = row.Cells["DepartmentID"].Value?.ToString() ?? "";
                    txtDeptName.Text = row.Cells["DepartmentName"].Value?.ToString() ?? "";
                    txtDeptCode.Text = row.Cells["DepartmentCode"].Value?.ToString() ?? "";

                    // Handle CollegeID null case
                    object collegeIDValue = row.Cells["CollegeID"].Value;
                    if (collegeIDValue != DBNull.Value)
                    {
                        cmbCollege.SelectedValue = collegeIDValue;
                    }
                    else
                    {
                        cmbCollege.SelectedIndex = -1; // Set to no selection
                    }

                    // Handle IsActive null case
                    object isActiveValue = row.Cells["IsActive"].Value;
                    if (isActiveValue != DBNull.Value)
                    {
                        chkIsActive.Checked = Convert.ToBoolean(isActiveValue);
                    }
                    else
                    {
                        chkIsActive.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while selecting a department: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Department_Load(object sender, EventArgs e)
        {

        }

        private void chkIsActive_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtDeptID_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }

        private void cmbCollege_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}