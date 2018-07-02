using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebMVCApp.Models
{
    public class WorkoutProgramVM
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActivated { get; set; }
        public IFormFile Image { get; set; }
        public byte[] ImageBytes { get; set; }
        public string RoutinesSerialized { get; set; }

        public int? WTUserID { get; set; }

        public ICollection<WorkoutRoutineVM> WorkoutRoutines { get; set; }

        public bool ToBeActivated { get; set; }
    }
}
