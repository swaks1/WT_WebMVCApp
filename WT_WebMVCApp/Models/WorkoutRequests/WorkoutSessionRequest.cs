using System;
using System.Collections.Generic;
using System.Text;
using WT_WebMVCApp.Models;

namespace WT_WebMVCApp.Models
{
    public class WorkoutSessionRequest
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? CurrentDate { get; set; }

        public List<WorkoutRoutineVM> Routines { get; set; }

        public List<ExerciseVM> Exercises { get; set; }

        public List<ConcreteExerciseVM> ConcreteExercises { get; set; }

        public UserVM User { get; set; }

        public int ID { get; set; }

        public string SerializedExerciseIds { get; set; }

        public string SerializedRoutineIds { get; set; }
        
    }
}
