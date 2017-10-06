using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.Models.Args
{
    public class WrongRangeThreadArgs : EventArgs
    {
        private string _passwort;
        public string Passwort
        {
            get
            {
                return _passwort;
            }
        }

        public WrongRangeThreadArgs(string passwort, string estimatedPasswort)
        {
            _passwort = passwort;
            _estimatedPasswort = estimatedPasswort;
        }

        private string _estimatedPasswort;
        public string EstimatedPasswort
        {
            get
            {
                return _estimatedPasswort;
            }
        }

    }
}
