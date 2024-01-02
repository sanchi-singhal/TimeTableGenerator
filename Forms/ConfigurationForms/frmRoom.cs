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
    public partial class frmRoom : Form
    {
        public frmRoom()
        {
            InitializeComponent();
        }
        public void EnableComponents()
        {
            dgvRoom.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvRoom.Enabled = true;
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
            txtRoomNo.Clear();
            txtCapacity.Clear();
            chkStatus.Checked = false;
        }

        private void chkStatus_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        public void FillGrid(string searchValue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchValue.Trim()))
                {
                    query = "select RoomID [ID],RoomNo [Room] , Capacity , isActive [Status] from RoomTable";
                }
                else
                {
                    query = "select RoomID [ID],RoomNo [Room] , Capacity , isActive [Status] from RoomTable where RoomNo like '%" + searchValue.Trim() + "%'";
                }
                DataTable roomList = DatabaseLayer.Retrieve(query);
                dgvRoom.DataSource = roomList;
                if (dgvRoom.Rows.Count > 0)
                {
                    dgvRoom.Columns[0].Width = 80;
                    dgvRoom.Columns[1].Width = 150;
                    dgvRoom.Columns[2].Width = 100;
                    dgvRoom.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtRoomNo.Text.Length < 1)
            {
                ep.SetError(txtRoomNo, "Pleas Enter Correct Room");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;
            }
            if (txtCapacity.Text.Length < 1)
            {
                ep.SetError(txtCapacity, "Pleas Enter Correct Room");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from RoomTable where RoomNo = '" + txtRoomNo.Text.Trim() + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtRoomNo, "Already Exist!");
                    txtRoomNo.Focus();
                    txtRoomNo.SelectAll();
                    return;
                }
            }


            string insertquery = string.Format("insert into RoomTable(RoomNo, Capacity, isActive) values ( '{0}','{1}', '{2}')", txtRoomNo.Text.ToUpper().Trim(), txtCapacity.Text.Trim(), chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Room Details. Then Try Again!");

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
            if (dgvRoom != null)
            {
                if (dgvRoom.Rows.Count > 0)
                {
                    if (dgvRoom.SelectedRows.Count == 1)
                    {
                        txtRoomNo.Text = Convert.ToString(dgvRoom.CurrentRow.Cells[1].Value);
                        txtCapacity.Text = Convert.ToString(dgvRoom.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvRoom.CurrentRow.Cells[3].Value);
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
            if (txtRoomNo.Text.Length < 1)
            {
                ep.SetError(txtRoomNo, "Pleas Enter Correct Room");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;
            }
            if (txtCapacity.Text.Length < 1)
            {
                ep.SetError(txtCapacity, "Pleas Enter Correct Room");
                txtRoomNo.Focus();
                txtRoomNo.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from RoomTable where RoomNo = '" + txtRoomNo.Text.Trim() + "' and RoomID ! ='" + Convert.ToString(dgvRoom.CurrentRow.Cells[0].Value + "'"));
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtRoomNo, "Already Exist!");
                    txtRoomNo.Focus();
                    txtRoomNo.SelectAll();
                    return;
                }
            }


            string updatequery = string.Format("update RoomTable  set RoomNo= '{0}', Capacity = '{1}',isActive = '{2}' where RoomID = '{3}'", txtRoomNo.Text.ToUpper().Trim(), txtCapacity.Text.Trim(),chkStatus.Checked, Convert.ToString(dgvRoom.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Room Details. Then Try Again!");

            }
        }

        private void dgvDay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmRoom_Load(object sender, EventArgs e)
        {
            FillGrid(string.Empty);
        }
    }
    }
