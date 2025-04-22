using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsCatcher.NewsCollector
{
    public interface ICustomJob   
    {
        Task<bool> Execute(JobObject jobObject);
    }
}



