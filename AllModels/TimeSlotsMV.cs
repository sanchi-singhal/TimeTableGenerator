﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableGenerator.AllModels
{
    internal class TimeSlotsMV
    {
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string SlotTitle { get; set; }

    }
}
