using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.Models.Args
{
    public class TimerArgs : EventArgs
    {
        private string _timer;

        public string Timer
        {
            get
            {
                return _timer;
            }
        }


        public TimerArgs(string timer)
        {
            _timer = timer;
        }
    }
}
