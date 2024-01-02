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
    public partial class frmLab : Form
    {
        public frmLab()
        {
            InitializeComponent();
        }
        public void EnableComponents()
        {
            dgvLab.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvLab.Enabled = true;
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
            txtLabNo.Clear();
            txtCapacity.Clear();
            chkStatus.Checked = false;
        }

        public void FillGrid(string searchValue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchValue.Trim()))
                {
                    query = "select LabID [ID],LabNo [Lab] , Capacity , isActive [Status] from LabTable";
                }
                else
                {
                    query = "select LabID [ID],LabNo [Lab] , Capacity , isActive [Status] from LabTable where LabNo like '%" + searchValue.Trim() + "%'";
                }
                DataTable labList = DatabaseLayer.Retrieve(query);
                dgvLab.DataSource = labList;
                if (dgvLab.Rows.Count > 0)
                {
                    dgvLab.Columns[0].Width = 80;
                    dgvLab.Columns[1].Width = 150;
                    dgvLab.Columns[2].Width = 100;
                    dgvLab.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }


        private void frmLab_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }

        private void txtCapacity_KeyPress(object sender, KeyPressEventArgs e)
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
            if (txtLabNo.Text.Length < 1)
            {
                ep.SetError(txtLabNo, "Pleas Enter Correct Lab");
                txtLabNo.Focus();
                txtLabNo.SelectAll();
                return;
            }
            if (txtCapacity.Text.Length < 1)
            {
                ep.SetError(txtCapacity, "Pleas Enter Correct Lab");
                txtLabNo.Focus();
                txtLabNo.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from LabTable where LabNo = '" + txtLabNo.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtLabNo, "Already Exist!");
                    txtLabNo.Focus();
                    txtLabNo.SelectAll();
                    return;
                }
            }


            string insertquery = string.Format("insert into LabTable(LabNo, Capacity, isActive) values ( '{0}','{1}', '{2}')", txtLabNo.Text.ToUpper().Trim(), txtCapacity.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Lab Details. Then Try Again!");

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

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if (dgvLab != null)
            {
                if (dgvLab.Rows.Count > 0)
                {
                    if (dgvLab.SelectedRows.Count == 1)
                    {
                        txtLabNo.Text = Convert.ToString(dgvLab.CurrentRow.Cells[1].Value);
                        txtCapacity.Text = Convert.ToString(dgvLab.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvLab.CurrentRow.Cells[3].Value);
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
            if (txtLabNo.Text.Length < 1)
            {
                ep.SetError(txtLabNo, "Pleas Enter Correct Lab");
                txtLabNo.Focus();
                txtLabNo.SelectAll();
                return;
            }
            if (txtCapacity.Text.Length < 1)
            {
                ep.SetError(txtCapacity, "Pleas Enter Correct Lab");
                txtLabNo.Focus();
                txtLabNo.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from LabTable where LabNo = '" + txtLabNo.Text.Trim() + "' and LabID ! ='" + Convert.ToString(dgvLab.CurrentRow.Cells[0].Value + "'"));
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtLabNo, "Already Exist!");
                    txtLabNo.Focus();
                    txtLabNo.SelectAll();
                    return;
                }
            }


            string updatequery = string.Format("update LabTable  set LabNo= '{0}', Capacity = '{1}',isActive = '{2}' where LabID = '{3}'", txtLabNo.Text.ToUpper().Trim(), txtCapacity.Text.Trim(), chkStatus.Checked, Convert.ToString(dgvLab.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Lab Details. Then Try Again!");

            }
        }
    }
}
