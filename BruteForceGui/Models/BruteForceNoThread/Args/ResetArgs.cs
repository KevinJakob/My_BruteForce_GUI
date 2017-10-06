using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.Models.Args
{
    public class ResetArgs : EventArgs
    {
        private bool _reset;
        public bool Reset
        {
            get
            {
                return _reset;
            }
        }

        public ResetArgs(bool resetter)
        {
            _reset = resetter;
        }
    }
}
