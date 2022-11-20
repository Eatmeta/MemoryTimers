using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Timers
{
    public class Timer
    {
        // Use this method in your solution to fit report formatting requirements from the tests
        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }
    }
}
