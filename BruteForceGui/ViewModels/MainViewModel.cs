using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BruteForceGui.Models;
using BruteForceGui.Models.Args;
using BruteForceGui.Views;
using System.Windows;
using System.Threading;

namespace BruteForceGui.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _withLowerCase;

        public bool WithLowerCase
        {
            get { return _withLowerCase; }
            set
            {
                _withLowerCase = value;
                OnPropertyChanged(nameof(WithLowerCase));
            }
        }

        private bool _withUpperCase;

        public bool WithUpperCase
        {
            get { return _withUpperCase; }
            set
            {
                _withUpperCase = value;
                OnPropertyChanged(nameof(WithUpperCase));
            }
        }

        private bool _withNumbers;

        public bool WithNumbers
        {
            get { return _withNumbers; }
            set
            {
                _withNumbers = value;
                OnPropertyChanged(nameof(WithNumbers));
            }
        }


        private bool _withSpecialChars;


        public bool WithSpecialChars
        {
            get { return _withSpecialChars; }
            set
            {
                _withSpecialChars = value;
                OnPropertyChanged(nameof(WithSpecialChars));
            }
        }


        private bool _isEditable;
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                _isEditable = value;
                OnPropertyChanged(nameof(IsEditable));
            }
        }

        private string _ePasswort;
        public string EPasswort
        {
            get
            {
                return _ePasswort;
            }
            set
            {
                _ePasswort = value;
                OnPropertyChanged(nameof(EPasswort));
            }
        }

        private string _geradeTestet ;
        public string GeradeTestet
        {
            get
            {
                return _geradeTestet;
            }
            set
            {
                _geradeTestet = value;
                OnPropertyChanged(nameof(GeradeTestet));
            }
        }

        private string _versuche;
        public string Versuche
        {
            get
            {
                return _versuche;
            }
            set
            {
                _versuche = value;
                OnPropertyChanged(nameof(Versuche));
            }
        }

        private string _gefundenesPasswort;
        public string GefundenesPasswort
        {
            get
            {
                return _gefundenesPasswort;
            }
            set
            {
                _gefundenesPasswort = value;
                OnPropertyChanged(nameof(GefundenesPasswort));
            }
        }

        private string _zeit;
        public string Zeit
        {
            get
            {
                return _zeit;
            }
            set
            {
                _zeit = value;
                OnPropertyChanged(nameof(Zeit));
            }
        }

        private string _alleVersuche;
        public string AlleVersuche
        {
            get
            {
                return _alleVersuche;
            }
            set
            {
                _alleVersuche = value;
                OnPropertyChanged(nameof(AlleVersuche));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        public MainViewModel()
        {
            LogicBruteforce logic = new LogicBruteforce();
            IsEditable = true;
            WithLowerCase = false;
            WithUpperCase = false;
            WithNumbers = false;
            WithSpecialChars = false;

        }

        public Task StartBruteForceAsync()
        {
            return Task.Factory.StartNew(()=>
            {
                IsEditable = false;
                var LowerCase = WithLowerCase;
                var UpperCase = WithUpperCase;
                var Numbers = WithNumbers;
                var SpecialChars = WithSpecialChars;
                LogicBruteforce logic = new LogicBruteforce();
                logic.PasswortStatus += logic_PasswortStatus;
                logic.Passwordfounded += logic_Passwordfounded;
                logic.CharSelector(LowerCase, UpperCase, Numbers, SpecialChars);
                logic.StarteBruteForce(EPasswort);
            },TaskCreationOptions.LongRunning);   
        }
        
        
        private void logic_PasswortStatus(object sender, BruteForceStatusArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                GeradeTestet = e.EstimatedPasswort;
                Versuche = Convert.ToString(e.CurrentTry);
            });
        }

        private void logic_Passwordfounded(object sender, PasswortFoundedArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                GefundenesPasswort = e.Passwort;
                Zeit = Convert.ToString(e.Time);
                AlleVersuche = Convert.ToString(e.AllTrys);
            });
        }

    }
}
