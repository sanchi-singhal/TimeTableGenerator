using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTableGenerator.SourceCode;

namespace TimeTableGenerator.Forms.ProgramSemesterForms
{
    public partial class frmProgramSemester : Form
    {
        public frmProgramSemester()
        {
            InitializeComponent();
        }
        public void ClearForm()
        {
            txtTitle.Clear();
            cmbSelectSemester.SelectedIndex = 0;
            cmbSelectProgram.SelectedIndex = 0;
            chkStatus.Checked = false;
        }

        public void SaveClearForm()
        {
            txtTitle.Clear();
            cmbSelectSemester.SelectedIndex = 0;
            chkStatus.Checked = true;
            FillGrid(string.Empty);
        }
        public void EnableComponents()
        {
            dgvProgramSemester.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvProgramSemester.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = false;
            btnUpdate.Visible = false;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        public void FillGrid(string searchValue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchValue.Trim()))
                {
                    query = "select ProgramSemesterID [ID], Title, Capacity, ProgramSemesterIsActive [Status], ProgramID, SemesterID from v_ProgramSemesterActiveList";
                }
                else
                {
                    query = "select ProgramSemesterID [ID], Title, Capacity, ProgramSemesterIsActive [Status], ProgramID, SemesterID from v_ProgramSemesterActiveList  where Title like '%" + searchValue.Trim() + "%'";
                }
                DataTable programSemesterList = DatabaseLayer.Retrieve(query);
                dgvProgramSemester.DataSource = programSemesterList;
                if (dgvProgramSemester.Rows.Count > 0)
                {
                    dgvProgramSemester.Columns[0].Width = 80;
                    dgvProgramSemester.Columns[1].Width = 250;
                    dgvProgramSemester.Columns[2].Width = 100;
                    dgvProgramSemester.Columns[3].Width = 100;
                    dgvProgramSemester.Columns[4].Visible = false ;
                    dgvProgramSemester.Columns[5].Visible = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }

        private void chkStatus_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmProgramSemester_Load(object sender, EventArgs e)
        {
            ComboHelper.Programs(cmbSelectProgram);
            ComboHelper.Semesters(cmbSelectSemester);
            FillGrid(string.Empty);


        }

        private void cmbSelectProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!cmbSelectProgram.Text.Contains("Select"))
            {
                if (cmbSelectSemester.SelectedIndex > 0)
                {
                    txtTitle.Text = cmbSelectProgram.Text + " " + cmbSelectSemester.Text;

                }
               
            }
            else
            {
                txtTitle.Text = "";
            }

        }

        private void cmbSelectSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbSelectSemester.Text.Contains("Select"))
            {
                if (cmbSelectProgram.SelectedIndex > 0)
                {
                    txtTitle.Text = cmbSelectProgram.Text + " " + cmbSelectSemester.Text;
                }
                
            }
            else
            {
                txtTitle.Text = " ";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if (txtTitle.Text.Length < 1)
            {
                ep.SetError(txtTitle, "Please Select Again. Title is Empty!!");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            if (cmbSelectProgram.SelectedIndex ==0)
            {
                ep.SetError(cmbSelectProgram, "Please Select Program");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            if (cmbSelectSemester.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectSemester, "Please Select Semester");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }

            if(txtCapacity.Text.Trim().Length == 0)
            {
                ep.SetError(txtCapacity, "Please Enter Semester Capacity");
                txtCapacity.Focus();
                return;
            }
            DataTable checktitle = DatabaseLayer.Retrieve("select * from ProgramSemesterTable where ProgramID = '"+ cmbSelectProgram.SelectedValue +"' and SemesterID = '" + cmbSelectSemester.SelectedValue + "'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtTitle, "Already Exist!");
                    txtTitle.Focus();
                    txtTitle.SelectAll();
                    return;
                }
            }


            string insertquery = string.Format("insert into ProgramSemesterTable(Title, ProgramID, SemesterID , isActive, Capacity) values ( '{0}','{1}', '{2}', '{3}', '{4}')", txtTitle.Text.ToUpper().Trim(), cmbSelectProgram.SelectedValue, cmbSelectSemester.SelectedValue, chkStatus.Checked, txtCapacity.Text.Trim());
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Successfully");
                SaveClearForm();
            }
            else
            {
                MessageBox.Show("Please Provide Correct  Details. Then Try Again!");

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
            if (dgvProgramSemester != null)
            {
                if (dgvProgramSemester.Rows.Count > 0)
                {
                    if (dgvProgramSemester.SelectedRows.Count == 1)
                    {
                        txtTitle.Text = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[1].Value);
                        txtCapacity.Text = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[2].Value);
                        cmbSelectProgram.SelectedValue = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[4].Value);
                        cmbSelectSemester.SelectedValue = Convert.ToString(dgvProgramSemester.CurrentRow.Cells[5].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvProgramSemester.CurrentRow.Cells[3].Value);
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
            if (txtTitle.Text.Length < 1)
            {
                ep.SetError(txtTitle, "Please Select Again. Title is Empty!!");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            if (cmbSelectProgram.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectProgram, "Please Select Program");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            if (cmbSelectSemester.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectSemester, "Please Select Semester");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            DataTable checktitle = DatabaseLayer.Retrieve("select * from ProgramSemesterTable where ProgramID = '" + cmbSelectProgram.SelectedValue + "' and SemesterID = '" + cmbSelectSemester.SelectedValue + "' and ProgramSemesterID!= '" + Convert.ToString(dgvProgramSemester.CurrentRow.Cells[0])+"'");
            if (checktitle != null)
            {
                if (checktitle.Rows.Count > 0)
                {
                    ep.SetError(txtTitle, "Already Exist!");
                    txtTitle.Focus();
                    txtTitle.SelectAll();
                    return;
                }
            }


            string updatequery = string.Format("update ProgramSemesterTable  set Title= '{0}', ProgramID = '{1}', SemesterID = '{2}', isActive = '{3}', Capacity = '{4}' where ProgramSemesterID= '{5}' ", txtTitle.Text.ToUpper().Trim(),cmbSelectProgram.SelectedValue, cmbSelectSemester.SelectedValue, chkStatus.Checked,txtCapacity.Text.Trim() ,Convert.ToString(dgvProgramSemester.CurrentRow.Cells[0].Value));
            bool result = DatabaseLayer.Update(updatequery);
            if (result == true)
            {
                MessageBox.Show("Updated Successfully");
                DisableComponents();
            }
            else
            {
                MessageBox.Show("Please Provide Correct  Details. Then Try Again!");

            }
        }
    }
    }
