﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceGui.Models.Args
{
    public class BruteForceStatusThreadArgs : EventArgs
    {
        private string _estimatedPasswort;
        private long _currentTry;

        public long CurrentTry
        {
            get
            {
                return _currentTry;
            }
        }
        public string EstimatedPasswort
        {
            get
            {
                return _estimatedPasswort;
            }
        }


        public BruteForceStatusThreadArgs(string estimatedPasswort, long currentTry)
        {
            _estimatedPasswort = estimatedPasswort;
            _currentTry = currentTry;
           
        }
    }
}
