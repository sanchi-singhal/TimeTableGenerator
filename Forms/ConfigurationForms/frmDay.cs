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
    public partial class frmDay : Form
    {
        public frmDay()
        {
            InitializeComponent();
        }
        public void EnableComponents()
        {
            dgvDay.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvDay.Enabled = true;
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
            txtDayName.Clear();
            chkStatus.Checked = false;
        }

        public void FillGrid(string searchValue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchValue.Trim()))
                {
                    query = "select DayID [ID], Name, isActive [Status] from DayTable";
                }
                else
                {
                    query = "select DayID [ID], Name, isActive [Status] from DayTable where Name like '%" + searchValue.Trim() + "%'";
                }
                DataTable dayList = DatabaseLayer.Retrieve(query);
                dgvDay.DataSource = dayList;
                if (dgvDay.Rows.Count > 0)
                {
                    dgvDay.Columns[0].Width = 80;
                    dgvDay.Columns[1].Width = 150;
                    dgvDay.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }

        private void frmDay_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if (dgvDay != null)
            {
                if (dgvDay.Rows.Count > 0)
                {
                    if (dgvDay.SelectedRows.Count == 1)
                    {
                        txtDayName.Text = Convert.ToString(dgvDay.CurrentRow.Cells[1].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvDay.CurrentRow.Cells[2].Value);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtDayName.Text.Length < 2)
            {
                ep.SetError(txtDayName, "Please Enter Correct Day");
                txtDayName.Focus();
                txtDayName.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from DayTable where Name = '" + txtDayName.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtDayName, "Already Exist!");
                    txtDayName.Focus();
                    txtDayName.SelectAll();
                    return;
                }
            }


            string insertquery = string.Format("insert into DayTable(Name, isActive) values ( '{0}','{1}')", txtDayName.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Day Details. Then Try Again!");

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtDayName.Text.Length < 2)
            {
                ep.SetError(txtDayName, "Pleas Enter Correct Day");
                txtDayName.Focus();
                txtDayName.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from DayTable where Name = '" + txtDayName.Text.Trim() + "' and DayID ! ='" + Convert.ToString(dgvDay.CurrentRow.Cells[0].Value + "'"));
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtDayName, "Already Exist!");
                    txtDayName.Focus();
                    txtDayName.SelectAll();
                    return;
                }
            }


            string updatequery = string.Format("update DayTable  set Name= '{0}', isActive = '{1}' where DayID = '{2}'", txtDayName.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvDay.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Day Details. Then Try Again!");

            }
        }
    }
}
