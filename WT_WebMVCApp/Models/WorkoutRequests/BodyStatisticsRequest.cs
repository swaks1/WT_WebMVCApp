using System;
using System.Collections.Generic;
using System.Text;
using WT_WebMVCApp.Models;

namespace WT_WebMVCApp.Models
{
    public class BodyStatisticsRequest
    {
        public int ID { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }

        public ICollection<BodyAttributeTemplateVM> BodyAttributeTemplates { get; set; }
        public ICollection<ProgressImageVM> ProgressImages { get; set; }

    }
}
