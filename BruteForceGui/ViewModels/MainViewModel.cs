using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BruteForceGui.Models;
using BruteForceGui.Models.Args;
using System.Windows;
using System.Threading;

namespace BruteForceGui.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        LogicBruteforce logic = new LogicBruteforce();
        //Verknüpfte Variablen
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

        private bool _isEditableStartButton;
        public bool IsEditableStartButton
        {
            get
            {
                return _isEditableStartButton;
            }
            set
            {
                _isEditableStartButton = value;
                OnPropertyChanged(nameof(IsEditableStartButton));
            }
        }

        private bool _isEditableResetButton;
        public bool IsEditableResetButton
        {
            get
            {
                return _isEditableResetButton;
            }
            set
            {
                _isEditableResetButton = value;
                OnPropertyChanged(nameof(IsEditableResetButton));
            }
        }

        private bool _isEditableStopButton;
        public bool IsEditableStopButton
        {
            get
            {
                return _isEditableStopButton;
            }
            set
            {
                _isEditableStopButton = value;
                OnPropertyChanged(nameof(IsEditableStopButton));
            }
        }

        private bool _isEditableContinueButton;
        public bool IsEditableContinueButton
        {
            get
            {
                return _isEditableContinueButton;
            }
            set
            {
                _isEditableContinueButton = value;
                OnPropertyChanged(nameof(IsEditableContinueButton));
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

        private int _minZeichenAnzahl;
        public int MinZeichenAnzahl
        {
            get
            {
                return _minZeichenAnzahl;
            }
            set
            {
                _minZeichenAnzahl = value;
                OnPropertyChanged(nameof(MinZeichenAnzahl));
            }
        }

        private int _maxZeichenAnzahl;
        public int MaxZeichenAnzahl
        {
            get
            {
                return _maxZeichenAnzahl;
            }
            set
            {
                _maxZeichenAnzahl = value;
                OnPropertyChanged(nameof(MaxZeichenAnzahl));
            }
        }

        private int _aktRhythm;
        public int AktRhythm
        {
            get
            {
                return _aktRhythm;
            }
            set
            {
                _aktRhythm = value;
                OnPropertyChanged(nameof(AktRhythm));
            }
        }

        private string _verstricheneZeit;
        public string VerstricheneZeit
        {
            get
            {
                return _verstricheneZeit;
            }
            set
            {
                _verstricheneZeit = value;
                OnPropertyChanged(nameof(VerstricheneZeit));
            }
        }


        //Event
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        //Ersteinstellung
        public MainViewModel()
        {
            IsEditableStartButton = true;
            IsEditableResetButton = false;
            IsEditableStopButton = false;
            IsEditableContinueButton = false;
            MinZeichenAnzahl = 1;
            MaxZeichenAnzahl = 64;
            AktRhythm = 100000;
            WithLowerCase = false;
            WithUpperCase = false;
            WithNumbers = false;
            WithSpecialChars = false;
            Zeit = "00:00:00";
            VerstricheneZeit = "00:00:00";
            AlleVersuche = "0";
            GefundenesPasswort = "Bitte suche starten";
            Versuche = "0";
            GeradeTestet = "nichts";

        }

        //Bruteforce Task als neuen Thread
        public Task StartBruteForceAsync()
        {
            return Task.Factory.StartNew(()=>
            {
                //Einstellungen abspeichern
                IsEditableStartButton = false;
                IsEditableStopButton = true;
                IsEditableResetButton = false;
                IsEditableContinueButton = false;
                var LowerCase = WithLowerCase;
                var UpperCase = WithUpperCase;
                var Numbers = WithNumbers;
                var SpecialChars = WithSpecialChars;
                var MinZeichen = MinZeichenAnzahl;
                var MaxZeichen = MaxZeichenAnzahl;
                var Aktualisierung = AktRhythm;
                GefundenesPasswort = "Passwort wird gesucht!";


                //Einstellungen übergeben
                logic.PasswortStatus += logic_PasswortStatus;
                logic.Passwordfounded += logic_Passwordfounded;
                logic.TimerUp += logic_TimerUp;
                logic.Reset += logic_Reset;
                logic.CharSelector(LowerCase, UpperCase, Numbers, SpecialChars);
                logic.Configurate(EPasswort, MinZeichen, MaxZeichen, AktRhythm);


            },TaskCreationOptions.LongRunning);   
        }

        public void ProgressStoped()
        {
            IsEditableResetButton = true;
            IsEditableContinueButton = true;
            IsEditableStopButton = false;
            IsEditableStartButton = false;
            logic.IsWeitermachen = true;
        }

        public void ProgressContinue()
        {
            IsEditableStartButton = false;
            IsEditableResetButton = false;
            IsEditableStopButton = true;
            IsEditableContinueButton = false;
            logic.IsWeitermachen = false;
            logic.BruteForceExecute(MaxZeichenAnzahl);

        }


        //Reset der Anzeigen
        public void Reset()
        {
            logic.PasswortStatusReset += logic_PasswortStatusReset;
            logic.PasswordfoundedReset += logic_PasswordfoundedReset;
            logic.ResetData();
            logic.IsWeitermachen = false;


            IsEditableStartButton = true;
            IsEditableResetButton = false;
            IsEditableStopButton = false;
            IsEditableContinueButton = false;
        }


        //ResetButtonSichtbar
        private void logic_Reset(object sender, ResetArgs e)
        {
            IsEditableResetButton = e.Reset;
        }
        
        
        //Abbonierte EventMethoden
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
                IsEditableStopButton = false;
            });
        }

        private void logic_TimerUp(object sender, TimerArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                VerstricheneZeit = e.Timer;
            });
        }

        private void logic_PasswortStatusReset(object sender, BruteForceStatusResetArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                GeradeTestet = e.EstimatedPasswort;
                Versuche = Convert.ToString(e.CurrentTry);
            });
        }

        private void logic_PasswordfoundedReset(object sender, PasswortFoundedResetArgs e)
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
