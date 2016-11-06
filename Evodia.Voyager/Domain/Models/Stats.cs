using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evodia.Voyager.Domain.Models
{
    public class Stats
    {
        public int Created { get; set; }

        public int Updated { get; set; }

        public int Deleted { get; set; }

        public double ElapsedSeconds { get; set; }

        public Stats()
        {
        }
    }
}
