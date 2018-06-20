using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WT_WebMVCApp.Models.WorkoutAssets
{
    public class UpdateRoutinesForProgramModel
    {
        public int ID { get; set; }
        public List<WorkoutRoutineVM> Routines { get; set; }
        public string SerializedRoutineIds { get; set; }
    }
}
