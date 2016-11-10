using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evodia.Voyager.Domain.Models
{
    public class SyncFile
    {
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public DateTime FileUpDateTime { get; set; }
        public string JobReferenceNumber { get; set; }
    }
}
