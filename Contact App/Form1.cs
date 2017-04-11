using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Contact_App
{
    public partial class Form1 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=G:\SCHOOL\Enterprise.App.Dev\DOT NET MOB\db\ContactDB.mdf;Integrated Security=True;Connect Timeout=30");
        public Form1()
        {
            InitializeComponent();
            FillDataGridViw();
        }


        void FillDataGridViw()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewOrSearch", sqlCon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.AddWithValue("@ContactName",txtSearch.Text.Trim());
            DataTable dataTable = new DataTable();
            sqlDa.Fill(dataTable);
            dgvContacts.DataSource = dataTable;
            dgvContacts.Columns[0].Visible = false;
            sqlCon.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SqlCommand sqlCmd = new SqlCommand("ContactsAddOrEdit", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@mode", "Add");
                sqlCmd.Parameters.AddWithValue("ContactId", 0);
                sqlCmd.Parameters.AddWithValue("Name", txtName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("MobileNumber", txtMobileNumber.Text.Trim());
                sqlCmd.Parameters.AddWithValue("Address", txtAddress.Text.Trim());
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Saved Success");
                FillDataGridViw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridViw();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e)
        {
            if (dgvContacts.CurrentRow.Index != -1)
            {
                txtName.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                txtMobileNumber.Text = dgvContacts.CurrentRow.Cells[2].Value.ToString();
                txtAddress.Text = dgvContacts.CurrentRow.Cells[3].Value.ToString();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FillDataGridViw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
        }
    }
}