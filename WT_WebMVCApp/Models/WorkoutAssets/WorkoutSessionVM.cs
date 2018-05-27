using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebMVCApp.Models
{
    public class WorkoutSessionVM
    {
        public int ID { get; set; }
        public DateTime? Date { get; set; }


        public int? WTUserID { get; set; }

        public ICollection<ConcreteExerciseVM> ConcreteExercises { get; set; }
    }
}
