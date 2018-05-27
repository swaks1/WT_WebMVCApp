﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebMVCApp.Models
{
    public class WorkoutRoutineVM
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string PlannedDates { get; set; }


        public int? WTUserID { get; set; }

        public ICollection<ExerciseVM> Exercises { get; set; }
        public ICollection<int> ProgramsIds { get; set; }
    }
    
}