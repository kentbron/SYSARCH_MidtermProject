using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace University
{
    public partial class College : Form
    {
        private string connString = "Server=MSI\\SQLEXPRESS;Database=CollegeDatabase;Trusted_Connection=True;";



        public College()
        {
            InitializeComponent();
            LoadColleges();
            txtCollegeID.Enabled = false; 
        }

        private void LoadColleges()
        {
            string query = "SELECT * FROM College";
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvColleges.DataSource = dt; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvColleges_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvColleges.Rows.Count) // Ensure a valid row is clicked
            {
                DataGridViewRow row = dgvColleges.Rows[e.RowIndex];

                txtCollegeID.Text = row.Cells["CollegeID"].Value?.ToString() ?? string.Empty;
                txtCollegeName.Text = row.Cells["CollegeName"].Value?.ToString() ?? string.Empty;
                txtCollegeCode.Text = row.Cells["CollegeCode"].Value?.ToString() ?? string.Empty;

               
                chkIsActive.Checked = row.Cells["IsActive"].Value != DBNull.Value && Convert.ToBoolean(row.Cells["IsActive"].Value);
            }
        }


        private void ClearFields()
        {
            txtCollegeID.Clear();
            txtCollegeName.Clear();
            txtCollegeCode.Clear();
            chkIsActive.Checked = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCollegeID.Text))
            {
                MessageBox.Show("Please select a college from the table first.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this college? This cannot be undone.",
                                                  "Confirm Deletion",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM College WHERE CollegeID = @ID";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtCollegeID.Text));
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("College deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadColleges(); 
                                ClearFields(); 
                            }
                            else
                            {
                                MessageBox.Show("No college was deleted. Please check if the record exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (SqlException ex) when (ex.Number == 547) 
                {
                    MessageBox.Show("This college is linked to other records and cannot be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCollegeID.Text))
            {
                MessageBox.Show("Please select a college from the table first.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE College SET CollegeName = @Name, CollegeCode = @Code, IsActive = @IsActive WHERE CollegeID = @ID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtCollegeID.Text));
                        cmd.Parameters.AddWithValue("@Name", txtCollegeName.Text);
                        cmd.Parameters.AddWithValue("@Code", txtCollegeCode.Text);
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("College updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadColleges();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating data: {ex.Message}", "There is a Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCollegeName.Text) || string.IsNullOrWhiteSpace(txtCollegeCode.Text))
            {
                MessageBox.Show("Please enter both College Name and College Code.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO College (CollegeName, CollegeCode, IsActive) VALUES (@Name, @Code, @IsActive)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtCollegeName.Text);
                        cmd.Parameters.AddWithValue("@Code", txtCollegeCode.Text);
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("College added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadColleges(); 
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "There is a Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chkIsActive_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void College_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtCollegeID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCollegeName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Hide();
        }
    }
}