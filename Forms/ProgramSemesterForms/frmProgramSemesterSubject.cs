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
    public partial class frmProgramSemesterSubject : Form
    {
        public frmProgramSemesterSubject()
        {
            InitializeComponent();
        }

        public void FillGrid(string searchValue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchValue.Trim()))
                {
                    query = "select [ProgramSemesterSubjectID] [ID], [ProgramID], [Program], ProgramSemesterID, Title [Semester], LecturerSubjectID, SSTitle[Subject], Capacity, isSubjectActive [Status] from v_AllSemesterSubjects where [ProgramSemesterIsActive]=1 and [ProgramIsActive] =1 and [SemesterIsActive]=1 and [SubjectIsActive]=1 order by ProgramSemesterID";
                }
                else
                {
                    query = "select [ProgramSemesterSubjectID] [ID], [ProgramID], [Program], ProgramSemesterID, Title [Semester], LecturerSubjectID, SSTitle[Subject], Capacity, isSubjectActive [Status] from v_AllSemesterSubjects where [ProgramSemesterIsActive]=1 and [ProgramIsActive] =1 and [SemesterIsActive]=1 and [SubjectIsActive]=1 and (Program + ' ' + Title + ' ' + Status) like '%" +searchValue + "%'  order by ProgramSemesterID ";
                }
                DataTable programSemesterList = DatabaseLayer.Retrieve(query);
                dgvTeacherSubjects.DataSource = programSemesterList;
                if (dgvTeacherSubjects.Rows.Count > 0)
                {
                    dgvTeacherSubjects.Columns[0].Visible = false;
                    dgvTeacherSubjects.Columns[1].Visible = false;
                    dgvTeacherSubjects.Columns[2].Width = 120;
                    dgvTeacherSubjects.Columns[3].Visible = false;
                    dgvTeacherSubjects.Columns[4].Width = 150;
                    dgvTeacherSubjects.Columns[5].Visible = false;
                    dgvTeacherSubjects.Columns[6].Width = 300;
                    dgvTeacherSubjects.Columns[7].Width = 80;
                    dgvTeacherSubjects.Columns[8].Width = 80;
                    dgvTeacherSubjects.ClearSelection();
                }
            }
            catch 
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }


        private void frmProgramSemesterSubject_Load(object sender, EventArgs e)
        {
            ComboHelper.AllProgramSemesters(cmbSemesters);
            ComboHelper.AllTeachersSubjects(cmbSubjects);
            FillGrid(string.Empty);
        }

        private void cmbSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTitle.Text = cmbSubjects.SelectedIndex ==0 ? string.Empty : cmbSubjects.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ep.Clear();
            if( txtTitle.Text.Trim().Length == 0)
            {
                ep.SetError(txtTitle, "Please Enter Semester Subject Title");
                txtTitle.Focus();
                txtTitle.SelectAll();
                return;
            }
            if (cmbSemesters.SelectedIndex == 0)
            {
                ep.SetError(cmbSemesters, "Please Select Semester");
                cmbSemesters.Focus();
                return;
            }
            if (cmbSubjects.SelectedIndex == 0)
            {
                ep.SetError(cmbSubjects, "Please Select Subject");
                cmbSubjects.Focus();
                return;
            }

            string checkquery = "select * from ProgramSemesterSubjectTable where ProgramSemesterID ='" + cmbSemesters.SelectedValue +"' and LecturerSubjectID = '" + cmbSubjects.SelectedValue + "'";
            DataTable dt = DatabaseLayer.Retrieve(checkquery);
            if (dt != null)
            {
                if(dt.Rows.Count>0)
                {
                    ep.SetError(cmbSubjects, "Alredy exists");
                    cmbSubjects.Focus();
                    return;
                }
            }

            string insertquery = string.Format("insert into ProgramSemesterSubjectTable (SSTitle, ProgramSemesterID, LecturerSubjectID) values('{0}','{1}','{2}')", txtTitle.Text.Trim(), cmbSemesters.SelectedValue, cmbSubjects.SelectedValue);
            bool result = DatabaseLayer.Insert(insertquery);
            if(result== true)
            {
                MessageBox.Show("Subject Assign Successfully");
                FillGrid(string.Empty);
                FormClear();
            }
            else
            {
                MessageBox.Show("Please Provide Correct Details");
            }

        }

        private void FormClear()
        {
            txtTitle.Clear();
            cmbSubjects.SelectedIndex = 0;
            cmbSemesters.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            FormClear();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if(dgvTeacherSubjects != null)
            {
                if(dgvTeacherSubjects.Rows.Count>0)
                {
                    if(dgvTeacherSubjects.SelectedRows.Count == 1)
                    {
                        if(MessageBox.Show("Are you sure you want to change status?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
                        {
                            bool existstatus = Convert.ToBoolean(dgvTeacherSubjects.CurrentRow.Cells[0].Value);
                            int semestersubjectid = Convert.ToInt32(dgvTeacherSubjects.CurrentRow.Cells[0].Value);
                            bool status = false;
                            if( existstatus == true)
                            {
                                status = false;
                            }
                            else
                            {
                                status = true;
                            }
                            string updatequery = string.Format("update ProgramSemesterSubjectTable set isSubjectActive = '{0}' where ProgramSemesterSubjectID = '{1}'", status, semestersubjectid);
                            bool result = DatabaseLayer.Update(updatequery);
                            if(result == true)
                            {
                                MessageBox.Show("Changed");
                                FillGrid(string.Empty);
                            }
                            else
                            {
                                MessageBox.Show("Error");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select one record");
                    }
                }
                else
                {
                    MessageBox.Show("Empty list");
                }
            }
            else
            {
                MessageBox.Show("Empty list");
            }
        }
    }
}
