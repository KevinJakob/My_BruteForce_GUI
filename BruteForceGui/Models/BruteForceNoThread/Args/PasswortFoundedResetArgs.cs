using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.Models.Args
{
    public class PasswortFoundedResetArgs : EventArgs
    {
        private long _allTrys;
        private string _passwort;
        private TimeSpan _time;

        public TimeSpan Time
        {
            get
            {
                return _time;
            }
        }

        public string Passwort
        {
            get
            {
                return _passwort;
            }
        }

        public PasswortFoundedResetArgs(string passwort, TimeSpan time, long allTrys)
        {
            _passwort = passwort;
            _time = time;
            _allTrys = allTrys;
        }

        public long AllTrys
        {
            get
            {
                return _allTrys;
            }
        }

    }
}
