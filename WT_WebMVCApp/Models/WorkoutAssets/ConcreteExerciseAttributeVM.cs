using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebMVCApp.Models
{
    public class ConcreteExerciseAttributeVM
    {
        public int ID { get; set; }

        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public bool IsDeletable { get; set; }

        public int? ConcreteExerciseID { get; set; }
    }
}
