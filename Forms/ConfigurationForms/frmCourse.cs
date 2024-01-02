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

namespace TimeTableGenerator.Forms.ConfigurationForms
{
    public partial class frmCourse : Form
    {
        public frmCourse()
        {
            InitializeComponent();
        }
        public void ClearForm()
        {
            txtTitle.Clear();
            cmbSelectType.SelectedIndex = 0;
            cmbCrHrs.SelectedIndex = 0;
            chkStatus.Checked = false;
        }

        public void SaveClearForm()
        {
            txtTitle.Clear();
            chkStatus.Checked = true;
            FillGrid(string.Empty);
        }
        public void EnableComponents()
        {
            dgvCourse.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = true;
            btnUpdate.Visible = true;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvCourse.Enabled = true;
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
                    query = "select CourseID [ID], Title [Course], CrHrs, RoomTypeId, TypeName [Type], isActive from v_AllSubjects";
                }
                else
                {
                    query = "select CourseID [ID], Title [Course], CrHrs, RoomTypeId, TypeName [Type], isActive from v_AllSubjects  where (Title + ' ' + TypeName)  like '%" + searchValue.Trim() + "%'";
                }
                DataTable programSemesterList = DatabaseLayer.Retrieve(query);
                dgvCourse.DataSource = programSemesterList;
                if (dgvCourse.Rows.Count > 0)
                {
                    dgvCourse.Columns[0].Width = 80;  //CourseID
                    dgvCourse.Columns[1].Width = 250; //Title
                    dgvCourse.Columns[2].Width = 60;  //CrHrs
                    dgvCourse.Columns[3].Visible = false;  //RoomTypeID
                    dgvCourse.Columns[4].Width = 250;   //TypeName
                    dgvCourse.Columns[5].Width = 80;  //isActive
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }

        private void frmCourse_Load(object sender, EventArgs e)
        {
            cmbCrHrs.SelectedIndex = 0;
            ComboHelper.RoomTypes(cmbSelectType);
            FillGrid(string.Empty);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                ep.SetError(txtTitle, "Please Enter correct Course!!");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            if (cmbSelectType.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectType, "Please Select Type");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from CourseTable where Title = '" + txtTitle.Text.Trim() + "'");
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


            string insertquery = string.Format("insert into CourseTable(Title, CrHrs, RoomTypeID , isActive) values ( '{0}','{1}', '{2}', '{3}')", txtTitle.Text.Trim(), cmbCrHrs.Text, cmbSelectType.SelectedValue, chkStatus.Checked);
            bool result = DatabaseLayer.Insert(insertquery);
            if (result == true)
            {
                MessageBox.Show("Save Successfully");
                SaveClearForm();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Details. Then Try Again!");

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
            if (dgvCourse != null)
            {
                if (dgvCourse.Rows.Count > 0)
                {
                    if (dgvCourse.SelectedRows.Count == 1)
                    {
                        txtTitle.Text = Convert.ToString(dgvCourse.CurrentRow.Cells[1].Value);
                        cmbSelectType.SelectedValue = Convert.ToString(dgvCourse.CurrentRow.Cells[3].Value);
                        cmbCrHrs.Text = Convert.ToString(dgvCourse.CurrentRow.Cells[2].Value);
                        chkStatus.Checked = Convert.ToBoolean(dgvCourse.CurrentRow.Cells[5].Value);
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
                ep.SetError(txtTitle, "Please Enter correct Course!!");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            if (cmbSelectType.SelectedIndex == 0)
            {
                ep.SetError(cmbSelectType, "Please Select Type");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }

            DataTable checktitle = DatabaseLayer.Retrieve("select * from CourseTable where Title = '" + txtTitle.Text.Trim() + "' and CourseID != '" + Convert.ToString(dgvCourse.CurrentRow.Cells[0].Value) +"'");
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


            string updatequery = string.Format("update CourseTable  set Title = '{0}',CrHrs= '{1}', RoomTypeID = '{2}', isActive = '{3}' where CourseID= '{4}' ", txtTitle.Text.Trim(), cmbCrHrs.Text, cmbSelectType.SelectedValue, chkStatus.Checked, Convert.ToString(dgvCourse.CurrentRow.Cells[0].Value));
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
