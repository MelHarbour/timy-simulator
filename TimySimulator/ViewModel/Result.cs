using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimySimulator.ViewModel
{
    public class Result
    {
        private string bibNumberText = String.Empty;

        public int ResultId { get; set; }
        public int BibNumber { get; set; }
        public string BibNumberText
        {
            get { return bibNumberText; }
            set
            {
                bibNumberText = value;
                BibNumber = int.Parse(bibNumberText, CultureInfo.InvariantCulture);
            }
        }
        public TimeSpan Time { get; set; }
        public int Channel { get; set; }
        public bool IsCleared { get; set; }
        public bool IsManuallyChangedTime { get; set; }
        public bool IsOverwrittenBibNumber { get; set; }
        public bool IsManualTime { get; set; }
        public bool IsSaved { get; set; }
    }
}
