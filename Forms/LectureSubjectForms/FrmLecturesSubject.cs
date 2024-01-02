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

namespace TimeTableGenerator.Forms.LectureSubjectForms
{
    public partial class FrmLecturesSubject : Form
    {

        public void FillGrid(string searchValue)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(searchValue.Trim()))
                {
                    query = "select LecturerSubjectID [ID], SubjectTitle, LecturerID, FullName [Lecture], CourseID," +
"Title [Course], isActive [Status] from v_AllSubjectsTeachers";
                }
                else
                {
                    query = "select LecturerSubjectID [ID], SubjectTitle, LecturerID, FullName [Lecture], CourseID," +
"Title [Course], isActive [Status] from v_AllSubjectsTeachers" +
"where (SubjectTitle + ' ' + FullName + ' ' + Title) like '%"+ searchValue.Trim()+"%'";
                }
                DataTable semesterList = DatabaseLayer.Retrieve(query);
                dgvTeacherSubjects.DataSource = semesterList;
                if (dgvTeacherSubjects.Rows.Count > 0)
                {
                    dgvTeacherSubjects.Columns[0].Visible = false;  //LecturerSubjectID
                    dgvTeacherSubjects.Columns[1].Width = 250; //Subject Title
                    dgvTeacherSubjects.Columns[2].Visible = false; //LecturerID
                    dgvTeacherSubjects.Columns[3].Width = 150; //FullName
                    dgvTeacherSubjects.Columns[4].Visible = false; //CourseId
                    dgvTeacherSubjects.Columns[5].Width = 300;  //Title
                    dgvTeacherSubjects.Columns[6].Width = 100;  //isActive
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }

        public void ClearForm()
        {
            cmbTeacher.SelectedIndex = 0;
            cmbSubjects.SelectedIndex = 0;
            chkStatus.Checked = true;
        }

        public void EnableComponents()
        {
            dgvTeacherSubjects.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvTeacherSubjects.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        public FrmLecturesSubject()
        {
            InitializeComponent();
        }

        private void FrmLecturesSubject_Load(object sender, EventArgs e)
        {
            ComboHelper.AllSubjects(cmbSubjects);
            ComboHelper.AllTeachers(cmbTeacher);
            FillGrid(string.Empty);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ep.Clear();
                if (cmbTeacher.SelectedIndex == 0)
                {
                    ep.SetError(cmbTeacher, "Please select Teacher");
                    cmbTeacher.Focus();
                    return;
                }

                if (cmbSubjects.SelectedIndex == 0)
                {
                    ep.SetError(cmbSubjects, "Please select Subject");
                    cmbTeacher.Focus();
                    return;
                }
                DataTable dt = DatabaseLayer.Retrieve("select * from LecturerSubjectTable where LecturerID= '" + cmbTeacher.SelectedValue + "' and CourseID= '" + cmbSubjects.SelectedValue + "'");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ep.SetError(cmbSubjects, "Already Registered");
                        cmbTeacher.Focus();
                        return;

                    }
                }

                string insertquery = string.Format("insert into LecturerSubjectTable(SubjectTitle, LecturerID, CourseID, isActive) values('{0}','{1}','{2}','{3}')", cmbSubjects.Text + "(" + cmbTeacher.Text + ")", cmbTeacher.SelectedValue, cmbSubjects.SelectedValue, chkStatus.Checked);
                bool result = DatabaseLayer.Insert(insertquery);

                if (result == true)
                {
                    MessageBox.Show("Subjects Assigned Successfully");
                    DisableComponents();
                    return;
                }
                else
                {
                    MessageBox.Show("Please Provide Correct Details");
                }

            }
            catch
            {
                MessageBox.Show("Please try agian later.");
            }
        }

        private void cmsOption_Opening(object sender, CancelEventArgs e)
        {

        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgvTeacherSubjects != null)
                {
                    if(dgvTeacherSubjects.Rows.Count > 0)
                    {
                        if(dgvTeacherSubjects.SelectedRows.Count == 1)
                        {

                            string id = Convert.ToString(dgvTeacherSubjects.CurrentRow.Cells[0].Value);
                            bool status = (Convert.ToBoolean(dgvTeacherSubjects.CurrentRow.Cells[6].Value) == true ? false: true);
                            string updatequery = "update LecturerSubjectTable set isActive = '" + status + "' where LecturerSubjectID ='" + id + "'";
                            bool result = DatabaseLayer.Update(updatequery);

                            if (result == true)
                            {
                                MessageBox.Show("Status Changed Successfully!");
                                DisableComponents();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Please Provide Correct Details");
                            }


                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Try Again Later!!");
            }
        }
    }
}
