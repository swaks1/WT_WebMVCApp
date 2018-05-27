using System;
using System.Collections.Generic;
using System.Text;

namespace WT_WebMVCApp.Models
{
    public class ProgressImageVM
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public DateTime? DateCreated { get; set; }

        public int? BodyStatisticID { get; set; }        
    }
}
