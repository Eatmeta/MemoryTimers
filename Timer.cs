using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        private static StringWriter Writer { get; set; }
        private List<Timer> Children { get;  } = new List<Timer>();
        private string Name { get; }
        private DateTime StartTime { get; }
        private DateTime EndTime { get; set; }
        private int Duration { get; set; }
        private long Rest { get; set; }
        private int Level { get; set; }

        private Timer(string name, DateTime start)
        {
            Name = name;
            StartTime = start;
        }
        
        public static Timer Start(StringWriter writer, string name = "*")
        {
            Writer = writer;
            var timer = new Timer(name, DateTime.Now);
            return timer;
        }
        
        public Timer StartChildTimer(string name)
        {
            var timer = new Timer(name, DateTime.Now);
            timer.Level = Level + 1;
            Children.Add(timer);
            return timer;
        }

        public void Dispose()
        {
            EndTime = DateTime.Now;
            Duration = (EndTime - StartTime).Milliseconds;
            if (Children.Count != 0)
                Rest = Duration - Children.Sum(c => c.Duration);
            if (Level == 0)
                PrepareReport(this);
        }

        private static void PrepareReport(Timer timer)
        {
            Writer.Write(FormatReportLine(timer.Name, timer.Level, timer.Duration));
            if (timer.Children.Count == 0) return;
            foreach (var timerChild in timer.Children)
                PrepareReport(timerChild);
            Writer.Write(FormatReportLine("Rest", timer.Level + 1, timer.Rest));
        }   
        
        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }
    }
}