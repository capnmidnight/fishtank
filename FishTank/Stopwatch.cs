using System;
using System.Collections.Generic;
using System.Text;

namespace FishTank
{
    class Stopwatch
    {
        DateTime x;
        public void Start()
        {
            x = DateTime.Now;
        }

        public double ElapsedMilliseconds
        {
            get
            {
                DateTime y = DateTime.Now;
                TimeSpan t = y - x;
                return t.TotalMilliseconds;
            }
        }

        public void Reset()
        {
            x = DateTime.Now;
        }
    }
}
