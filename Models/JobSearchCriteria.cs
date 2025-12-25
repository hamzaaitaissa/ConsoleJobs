using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Models
{
    public class JobSearchCriteria
    {
        public string Keywords { get; set; } = "Software Engineer";
        public string Country { get; set; } = "de";
        public string DatePosted { get; set; } = "today";
    }
}
