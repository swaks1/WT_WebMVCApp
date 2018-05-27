using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebMVCApp.Models
{
    public class BodyStatisticVM
    {
        public int ID { get; set; }
        public DateTime? DateCreated { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }

        public ICollection<BodyStatAttributeVM> BodyStatAttributes { get; set; }
        public ICollection<ProgressImageVM> ProgressImages { get; set; }

        public int? WTUserID { get; set; }
    }
}
