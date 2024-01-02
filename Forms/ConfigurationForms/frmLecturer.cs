using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTableGenerator.Forms.ConfigurationForms
{
    public partial class frmLecturer : Form
    {
        public frmLecturer()
        {
            InitializeComponent();
        }
        public void EnableComponents()
        {
            dgvLecturer.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvLecturer.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        public void ClearForm()
        {
            txtFullName.Clear();
            txtContactNo.Clear();
            chkStatus.Checked = false;
        }

        public void FillGrid(string searchValue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchValue.Trim()))
                {
                    query = "select LecturerID [ID],FullName [Name] , ContactNo , isActive [Status] from LecturerTable";
                }
                else
                {
                    query = "select LecturerID [ID],FullName [Name] , ContactNo , isActive [Status] from LecturerTable where (FullName + ' ' + ContactNo ) like '%" + searchValue.Trim() + "%'";
                }
                DataTable labList = DatabaseLayer.Retrieve(query);
                dgvLecturer.DataSource = labList;
                if (dgvLecturer.Rows.Count > 0)
                {
                    dgvLecturer.Columns[0].Width = 80;
                    dgvLecturer.Columns[1].Width = 150;
                    dgvLecturer.Columns[2].Width = 100;
                    dgvLecturer.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }

        private void frmLecturer_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtFullName.Text.Length < 1)
            {
                ep.SetError(txtFullName, "Pleas Enter Correct Lecturer");
                txtFullName.Focus();
                txtFullName.SelectAll();
                return;
            }
            if (txtContactNo.Text.Length < 10)
            {
                ep.SetError(txtContactNo, "Pleas Enter Correct Lecturer");
                txtFullName.Focus();
                txtFullName.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from LecturerTable where FullName = '" + txtFullName.Text.Trim() + "' and ContactNo = '" + txtContactNo.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtFullName, "Already Exist!");
                    txtFullName.Focus();
                    txtFullName.SelectAll();
                    return;
                }
            }


            string insertquery = string.Format("insert into LecturerTable(FullName, ContactNo, isActive) values ( '{0}','{1}', '{2}')", txtFullName.Text.ToUpper().Trim(), txtContactNo.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Lecturer Details. Then Try Again!");

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if (dgvLecturer != null)
            {
                if (dgvLecturer.Rows.Count > 0)
                {
                    if (dgvLecturer.SelectedRows.Count == 1)
                    {
                        txtFullName.Text = Convert.ToString(dgvLecturer.CurrentRow.Cells[1].Value);
                        txtContactNo.Text = Convert.ToString(dgvLecturer.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvLecturer.CurrentRow.Cells[3].Value);
                        EnableComponents();

                    }
                    else
                    {
                        MessageBox.Show("Please select a record");
                    }
                }
                else
                {
                    MessageBox.Show("List is empty!!");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtFullName.Text.Length < 1)
            {
                ep.SetError(txtFullName, "Pleas Enter Correct Lecturer");
                txtFullName.Focus();
                txtFullName.SelectAll();
                return;
            }
            if (txtContactNo.Text.Length < 10)
            {
                ep.SetError(txtContactNo, "Pleas Enter Correct Lecturer");
                txtFullName.Focus();
                txtFullName.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from LecturerTable where FullName = '" + txtFullName.Text.Trim() + "' and ContactNo='" +txtContactNo.Text.Trim()+ "' and LecturerID ! ='" + Convert.ToString(dgvLecturer.CurrentRow.Cells[0].Value + "'"));
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtFullName, "Already Exist!");
                    txtFullName.Focus();
                    txtFullName.SelectAll();
                    return;
                }
            }


            string updatequery = string.Format("update LecturerTable  set FullName= '{0}', ContactNo = '{1}', isActive = '{2}' where LecturerID = '{3}'", txtFullName.Text.ToUpper().Trim(), txtContactNo.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvLecturer.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Lecturer Details. Then Try Again!");

            }
        }
    }
}
