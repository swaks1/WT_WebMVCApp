using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WT_WebMVCApp.Models
{
    public class ExerciseVM
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public bool IsEditable { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }
        public byte[] ImageBytes { get; set; }

        public int? WTUserID { get; set; }

        public ICollection<ExerciseAttributeVM> Attributes { get; set; }
        public string AttributesSerialized { get; set; }

    }
}
