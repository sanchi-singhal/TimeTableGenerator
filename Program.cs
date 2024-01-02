﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTableGenerator.Forms;
using TimeTableGenerator.Forms.ConfigurationForms;
using TimeTableGenerator.Forms.ConfigurationForms.TimeSlotForms;
using TimeTableGenerator.Forms.LectureSubjectForms;
using TimeTableGenerator.Forms.ProgramSemesterForms;

namespace TimeTableGenerator
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmDayTimeSlots());
        }
    }
}
