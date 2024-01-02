using System.Data;
using System.Windows.Forms;

namespace TimeTableGenerator.SourceCode
{
    public class ComboHelper
    {


        public static void Semesters(ComboBox cmb)
        {
            DataTable dtSemester = new DataTable();
            dtSemester.Columns.Add("SemesterID");
            dtSemester.Columns.Add("Semester Name");
            dtSemester.Rows.Add("---Select---", "0");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select SemesterID, SemesterName from SemesterTable where isActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow semester in dt.Rows)
                        {
                            dtSemester.Rows.Add(semester["SemesterID"], semester["SemesterName"]);
                        }
                    }
                }
                cmb.DataSource = dtSemester;
                cmb.ValueMember = "SemesterID";
                cmb.DisplayMember = "SemesterName";
            }
            catch
            {
                cmb.DataSource = dtSemester;
            }
        }

        public static void Programs(ComboBox cmb)
        {
            DataTable dtProgram = new DataTable();
            dtProgram.Columns.Add("ProgramID");
            dtProgram.Columns.Add("Name");
            dtProgram.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select ProgramID, Name from ProgramTable where isActive = 1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow program in dt.Rows)
                        {
                            dtProgram.Rows.Add(program["ProgramID"], program["Name"]);
                        }
                    }
                }
                cmb.DataSource = dtProgram;
                cmb.ValueMember = "ProgramID";
                cmb.DisplayMember = "Name";
            }
            catch
            {
                cmb.DataSource = dtProgram;
            }
        }

        public static void RoomTypes(ComboBox cmb)
        {
            DataTable dtTypes = new DataTable();
            dtTypes.Columns.Add("RoomTypeID");
            dtTypes.Columns.Add("TypeName");
            dtTypes.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select RoomTypeID, TypeName from RoomTypeTable");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow type in dt.Rows)
                        {
                            dtTypes.Rows.Add(type["RoomTypeID"], type["TypeName"]);
                        }
                    }
                }
                cmb.DataSource = dtTypes;
                cmb.ValueMember = "RoomTypeID";
                cmb.DisplayMember = "TypeName";
            }
            catch
            {
                cmb.DataSource = dtTypes;
            }
        }

        public static void AllDays(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("DayID");
            dtlist.Columns.Add("Name");
            dtlist.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select DayID, Name from DayTable where isActive=1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow day in dt.Rows)
                        {
                            dtlist.Rows.Add(day["DayID"], day["Name"]);
                        }
                    }
                }
                cmb.DataSource = dtlist;
                cmb.ValueMember = "DayID";
                cmb.DisplayMember = "Name";
            }
            catch
            {
                cmb.DataSource = dtlist;
            }
        }

        public static void TimeSlotNumbers(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("ID");
            dtlist.Columns.Add("Number");
            dtlist.Rows.Add("0", "---Select---");
            dtlist.Rows.Add("1", "1");
            dtlist.Rows.Add("2", "2");
            dtlist.Rows.Add("3", "3");
            dtlist.Rows.Add("4", "4");
            dtlist.Rows.Add("5", "5");
            dtlist.Rows.Add("6", "6");
            dtlist.Rows.Add("7", "7");
            dtlist.Rows.Add("8", "8");
            dtlist.Rows.Add("9", "9");
            cmb.DataSource = dtlist;
            cmb.ValueMember = "ID";
            cmb.DisplayMember = "Number";
        }

        public static void AllTeachers(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("LecturerID");
            dtlist.Columns.Add("FullName");
            dtlist.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select LecturerID, FullName from LecturerTable where isActive=1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow day in dt.Rows)
                        {
                            dtlist.Rows.Add(day["LecturerID"], day["FullName"]);
                        }
                    }
                }
                cmb.DataSource = dtlist;
                cmb.ValueMember = "LecturerID";
                cmb.DisplayMember = "FullName";
            }
            catch
            {
                cmb.DataSource = dtlist;
            }
        }

        public static void AllSubjects(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("CourseID");
            dtlist.Columns.Add("Title");
            dtlist.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select CourseID, Title from CourseTable where isActive=1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow day in dt.Rows)
                        {
                            dtlist.Rows.Add(day["CourseID"], day["Title"]);
                        }
                    }
                }
                cmb.DataSource = dtlist;
                cmb.ValueMember = "CourseID";
                cmb.DisplayMember = "Title";
            }
            catch
            {
                cmb.DataSource = dtlist;
            }
        }

        public static void AllProgramSemesters(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("ProgramSemesterID");
            dtlist.Columns.Add("Title");
            dtlist.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select ProgramSemesterID, Title from v_ProgramSemesterActiveList where ProgramSemesterIsActive=1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow day in dt.Rows)
                        {
                            dtlist.Rows.Add(day["ProgramSemesterID"], day["Title"]);
                        }
                    }
                }
                cmb.DataSource = dtlist;
                cmb.ValueMember = "ProgramSemesterID";
                cmb.DisplayMember = "Title";
            }
            catch
            {
                cmb.DataSource = dtlist;
            }
        }

        public static void AllTeachersSubjects(ComboBox cmb)
        {
            DataTable dtlist = new DataTable();
            dtlist.Columns.Add("LecturerSubjectID");
            dtlist.Columns.Add("SubjectTitle");
            dtlist.Rows.Add("0", "---Select---");
            try
            {
                DataTable dt = DatabaseLayer.Retrieve("select LecturerSubjectID, SubjectTitle from v_AllSubjectsTeachers where isActive=1");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow day in dt.Rows)
                        {
                            dtlist.Rows.Add(day["LecturerSubjectID"], day["SubjectTitle"]);
                        }
                    }
                }
                cmb.DataSource = dtlist;
                cmb.ValueMember = "LecturerSubjectID";
                cmb.DisplayMember = "SubjectTitle";
            }
            catch
            {
                cmb.DataSource = dtlist;
            }
        }
    }
}
