using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTableGenerator.AllModels;
using TimeTableGenerator.SourceCode;

namespace TimeTableGenerator.Forms.ConfigurationForms.TimeSlotForms
{
    public partial class frmDayTimeSlots : Form
    {
        public frmDayTimeSlots()
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
                    query = "select DayTimeSlotID,ROW_NUMBER() OVER( Order by DayTimeSlotID) AS [S No], DayID, Name [Day], SlotTitle, StartTime [Start Time], EndTime [End Time], isActive [Status] from v_AllTimeSlots where isActive =1";
                }
                else
                {
                    query = "select DayTimeSlotID,ROW_NUMBER() OVER( Order by DayTimeSlotID) AS [S No], DayID, Name [Day], SlotTitle, StartTime [Start Time], EndTime [End Time], isActive [Status] from v_AllTimeSlots" + " where isActive = '1' AND (Name + ' '+ SlotTitle) Like '%" + searchValue.Trim() + "%'";
                }
                    DataTable semesterList = DatabaseLayer.Retrieve(query);
                    dgvSlots.DataSource = semesterList;
                    if (dgvSlots.Rows.Count > 0)
                    {
                        dgvSlots.Columns[0].Visible = false;  //DayTimeSlotID
                        dgvSlots.Columns[1].Width = 80; // S No
                        dgvSlots.Columns[2].Visible = false; //DayID
                        dgvSlots.Columns[3].Width = 130; //Name
                        dgvSlots.Columns[4].Width = 150; //SlotTitle
                        dgvSlots.Columns[5].Width = 180;  //StartTime
                        dgvSlots.Columns[6].Width = 180;  //EndTime
                        dgvSlots.Columns[7].Width = 80;   //isActive
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Some unexpected issue occur plz try again !!");
            }
        }

        public void ClearForm()
        {
            cmbDays.SelectedIndex = 0;
            cmbNumberofTimeSlot.SelectedIndex = 0;
            chkStatus.Checked = true;
        }

        public void EnableComponents()
        {
            dgvSlots.Enabled = false;
            btnClear.Visible = false;
            btnSave.Visible = false;
            txtSearch.Enabled = false;
        }
        public void DisableComponents()
        {
            dgvSlots.Enabled = true;
            btnClear.Visible = true;
            btnSave.Visible = true;            
            txtSearch.Enabled = true;
            ClearForm();
            FillGrid(string.Empty);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void frmDayTimeSlots_Load(object sender, EventArgs e)
        {
            dtpStartTime.Value = new DateTime(2020, 12, 12, 8,0,0);
            dtpEndTime.Value = new DateTime(2020, 12, 12, 16, 0, 0);
            ComboHelper.AllDays(cmbDays);
            ComboHelper.TimeSlotNumbers(cmbNumberofTimeSlot);
            FillGrid(string.Empty);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ep.Clear();
                if (cmbDays.SelectedIndex == 0)
                {
                    ep.SetError(cmbDays, "Please Select Day!");
                    cmbDays.Focus();
                    return;
                }
                if (cmbNumberofTimeSlot.SelectedIndex == 0)
                {
                    ep.SetError(cmbNumberofTimeSlot, "Please Select Time Slots Per Day!");
                    cmbNumberofTimeSlot.Focus();
                    return;
                }

                string UpdateQuery = "update DayTimeSlotTable set isActive =0 where DayID = '" + cmbDays.SelectedValue + "'";
                bool updateResult = DatabaseLayer.Update(UpdateQuery);
               /* if (updateResult)
                {*/

                    List<TimeSlotsMV> timeSlots = new List<TimeSlotsMV>();
                    TimeSpan time = dtpEndTime.Value - dtpStartTime.Value;
                    int totalminutes = (int)time.TotalMinutes;
                    int numberiftimeslot = Convert.ToInt32(cmbNumberofTimeSlot.SelectedValue);
                    int slot = totalminutes / numberiftimeslot;
                    TimeSpan starttime = dtpStartTime.Value.TimeOfDay;
                    int i = 0;
                    do
                    {
                        var timeslot = new TimeSlotsMV();
                        var FromTime = (dtpStartTime.Value).AddMinutes(slot * i);
                        i++;
                        var ToTime = (dtpStartTime.Value).AddMinutes(slot * i);
                        string title = FromTime.ToString("hh:mm tt") + "-" + ToTime.ToString("hh:mm tt");
                        timeslot.FromTime = FromTime;
                        timeslot.ToTime = ToTime;
                        timeslot.SlotTitle = title;
                        timeSlots.Add(timeslot);
                    } while (i < numberiftimeslot);
                    bool insertstatus = true;
                    foreach (TimeSlotsMV slottime in timeSlots)
                    {
                        string insertquery = string.Format("insert into DayTimeSlotTable(DayID,SlotTitle,StartTime,EndTime,isActive) values('{0}','{1}','{2}','{3}','{4}')", cmbDays.SelectedValue, slottime.SlotTitle, slottime.FromTime, slottime.ToTime, chkStatus.Checked);
                        bool result = DatabaseLayer.Insert(insertquery);
                        if (result == false)
                        {
                            insertstatus = false;
                        }
                    }
                    if (insertstatus == true)
                    {
                        MessageBox.Show("Slots Created Successfulyy!");
                        DisableComponents();
                    }
                    else
                    {
                        MessageBox.Show("Please provide correct details");


                    }
                /*}
                else
                {
                    MessageBox.Show("Please provide correct details");
                }*/
            }

            catch
            {

                MessageBox.Show("Please Try Again. Exception occurs");
            }
        }

        private void cmsedit_Click(object sender, EventArgs e)
        {
            if(dgvSlots!= null)
            {
                if(dgvSlots.Rows.Count>0)
                {
                    if (dgvSlots.SelectedRows.Count == 1)
                    {
                        string slotid= Convert.ToString(dgvSlots.CurrentRow.Cells[0].Value);
                        string updatequery = "Update DayTimeSlotTable set isActive =0 where DayTimeSlotID = '" + Convert.ToString(dgvSlots.CurrentRow.Cells[0].Value) + "'";
                        bool result = DatabaseLayer.Update(updatequery);
                        if(result == true)
                        {
                            MessageBox.Show("Break Time is Marked! and Excluded from Time Table");
                            DisableComponents();
                        }
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DisableComponents();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid(txtSearch.Text.Trim());
        }
    }



           
    }
