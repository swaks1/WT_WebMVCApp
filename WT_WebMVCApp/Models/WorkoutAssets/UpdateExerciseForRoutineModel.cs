using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WT_WebMVCApp.Models.WorkoutAssets
{
    public class UpdateExerciseForRoutineModel
    {
        public int ID { get; set; }
        public List<int> ExerciseIds { get; set; }
        public string SerializedExerciseIds { get; set; }
    }
}
