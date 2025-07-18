using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.Models.Models
{
    public class UserJobModel
    {
            public string JobId { get; set; }
            public string JobName { get; set; }
            public string Cron { get; set; }
            public string RssUrl { get; set; }
    }
}
