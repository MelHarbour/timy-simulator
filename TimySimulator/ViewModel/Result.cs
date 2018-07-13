using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimySimulator.ViewModel
{
    public class Result
    {
        public int BibNumber { get; set; }
        public TimeSpan Time { get; set; }
        public int Channel { get; set; }
        public bool IsCleared { get; set; }
        public bool IsManuallyChangedTime { get; set; }
        public bool IsOverwrittenBibNumber { get; set; }
        public bool IsManualTime { get; set; }
    }
}
