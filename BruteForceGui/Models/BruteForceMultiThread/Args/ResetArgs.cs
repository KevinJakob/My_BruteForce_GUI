using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.Models.Args
{
    public class ResetThreadArgs : EventArgs
    {
        private bool _reset;
        public bool Reset
        {
            get
            {
                return _reset;
            }
        }

        public ResetThreadArgs(bool resetter)
        {
            _reset = resetter;
        }
    }
}
