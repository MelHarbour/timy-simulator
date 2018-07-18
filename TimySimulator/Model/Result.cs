using System;

namespace TimySimulator.Model
{
    public class Result
    {
        public int ResultId { get; set; }
        public int BibNumber { get; set; }
        public TimeSpan Time { get; set; }
        public int Channel { get; set; }
        public bool IsCleared { get; set; }
        public bool IsManuallyChangedTime { get; set; }
        public bool IsOverwrittenBibNumber { get; set; }
        public bool IsManualTime { get; set; }
        public bool IsSaved { get; set; }
    }
}
