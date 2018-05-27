using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WT_WebMVCApp.Models;

namespace WT_WebMVCApp.Models
{
    public class UserVM
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool NotificationActivated { get; set; }

        //Navigational Properties        
        public ICollection<ExerciseVM> Exercises { get; set; }
        public ICollection<WorkoutRoutineVM> WorkoutRoutines { get; set; }
        public ICollection<WorkoutProgramVM> WorkoutPrograms { get; set; }
        public ICollection<WorkoutSessionVM> WorkoutSession { get; set; }
        public ICollection<BodyStatisticVM> BodyStatistics { get; set; }
    }
}
