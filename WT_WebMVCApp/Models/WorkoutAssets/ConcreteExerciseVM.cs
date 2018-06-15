using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WT_WebMVCApp.Models
{
    public class ConcreteExerciseVM
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public int? RoutineId { get; set; }
        public string RoutineName { get; set; }

        public int? ProgramId { get; set; }
        public string ProgramName { get; set; }

        //Relationships (Navigational properties)
        public int? WTUserID { get; set; }

        public int? WorkoutSessionID { get; set; }

        public ICollection<ConcreteExerciseAttributeVM> Attributes { get; set; }
        public string AttributesSerialized { get; set; }

    }
}
