using BruteForceGui.Models.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using ICSharpCode.SharpZipLib.Zip;
using BruteForceGui.Abstraction;
using System.IO;

namespace BruteForceGui.Models
{
    public class LogicBruteforce
    {
        private IZipMaster _zipMaster;
        public LogicBruteforce(IZipMaster zipMaster)
        {
            _zipMaster = zipMaster;
            _sw = new Stopwatch();
        }

        

        #region EventHandler
        public event EventHandler<BruteForceStatusArgs> PasswortStatus;
        public event EventHandler<PasswortFoundedArgs> Passwordfounded;
        public event EventHandler<ResetArgs> Reset;
        public event EventHandler<BruteForceStatusResetArgs> PasswortStatusReset;
        public event EventHandler<PasswortFoundedResetArgs> PasswordfoundedReset;
        public event EventHandler<TimerArgs> TimerUp;
        public event EventHandler<WrongRangeArgs> WrongRange;
        #endregion

        #region Variablen
        private string _eingegebenesPasswort;
        private string _generiertesPasswort;
        private List<char> _meineZeichen;
        private int _listenlänge;
        public bool IsWeitermachen = false;
        private int _passwortLänge;
        private long _zähler = 0;
        private Stopwatch _sw;
        private int _uiVerzögerer = 0;
        private long _alleVersuche;
        private int _aktualisierer;
        private bool _überbrücker=true;
        private string _currentPath;
        private string _outPath;
        #endregion

        #region Brute Force execute
        //eingegebenes Passwort speichern
        public void SetPassword(string passwordSecure)
        {
            if (string.IsNullOrEmpty(passwordSecure))
            {
                throw new ArgumentNullException(nameof(passwordSecure));
            }
            _eingegebenesPasswort = passwordSecure;
        }

        public void BruteForceExecute(int maxLänge)
        {
            _sw.Start();
            //Starte Rekursive schleife
            while (IsWeitermachen == false)
            {
                char[] zutestendesPasswort = new char[_passwortLänge];
                ZeichenGenerator(zutestendesPasswort, 0, maxLänge);
                _passwortLänge++;
            }
        }

        private void ZeichenGenerator(char[] zuTestendesPasswort, int Position, int maxLänge)
        {
            //Max angegebene Passwortlänge erreicht?
            if (_passwortLänge == maxLänge+1)
            {
                IsWeitermachen = true;
                _generiertesPasswort = "Error! Password not in Range!!!";
                _alleVersuche = _zähler;
                _sw.Stop();
                OnPasswortFounded(_generiertesPasswort, _sw.Elapsed, _alleVersuche);
            }
            else
            {
                //PW Array Schleife
                for (int i = 0; i < _listenlänge; i++)
                {
                    zuTestendesPasswort[Position] = _meineZeichen[i];
                    if (Position < _passwortLänge - 1)
                    {
                        ZeichenGenerator(zuTestendesPasswort,Position + 1, maxLänge);
                    }

                    //Passwort umwandeln aus Array in string
                    _generiertesPasswort = new string(zuTestendesPasswort);

                    //Passwort überprüfen
                    if (Position == _passwortLänge - 1)
                    {
                        _zähler++;
                        if (_uiVerzögerer == 0)
                        {
                            //Status aktualisieren
                            OnPasswortStatus(_generiertesPasswort, _zähler);
                            OnTimer(Convert.ToString(_sw.Elapsed));
                        }
                        _uiVerzögerer++;
                        if (_uiVerzögerer == _aktualisierer)
                        {
                            _uiVerzögerer = 0;
                        }
                        PasswortCheck();
                    }

                    //Schleife Abbrechen
                    if (IsWeitermachen == true)
                    {
                        break;
                    }
                }
            }
        }

        //Passwort auf übereinstimmung prüfen
        private void PasswortCheck()
        {

            try
            {
                var result = _zipMaster.GetAllFilesUnorderd(_currentPath, _generiertesPasswort);
                if(result.Any())
                {
                    bool exists = System.IO.Directory.Exists(_outPath);

                    if (!exists)
                        System.IO.Directory.CreateDirectory(_outPath);

                    foreach (var item in result)
                    {
                        var path = Path.Combine(_outPath, item.Name);
                        var ms = item.Stream as MemoryStream;
                        if(ms == null)
                        {
                            ms = new MemoryStream();
                            item.Stream.CopyTo(ms);
                        }
                        File.WriteAllBytes(path, ms.ToArray());
                    }

                    
                    FinishBruteFoce();
                }
            }
            catch (Exception e)
            {
                
            }

            //if (_generiertesPasswort == _eingegebenesPasswort)
            //{
            //    FinishBruteFoce();
            //}
        }

        private void FinishBruteFoce()
        {
            _alleVersuche = _zähler;
            IsWeitermachen = true;
            _sw.Stop();
            OnPasswortFounded(_generiertesPasswort, _sw.Elapsed, _alleVersuche);
        }
        #endregion

        #region CharSelector
        public void CharSelector(bool lower, bool upper, bool numbers, bool special)
        {
            _meineZeichen = new List<char>();

            //kleinesAlphabet
            if (lower == true)
            {
                FuegeZeichenHinzu(_meineZeichen, 'a', 'z');
            }

            //großesAlphabet
            if (upper == true)
            {
                FuegeZeichenHinzu(_meineZeichen, 'A', 'Z');
            }

            //Zahlen
            if (numbers == true)
            {
                FuegeZeichenHinzu(_meineZeichen, '0', '9');
            }

            //Sonderzeichen
            if (special == true)
            {
                FuegeZeichenHinzu(_meineZeichen, '!', '/');
                FuegeZeichenHinzu(_meineZeichen, ':', '@');
                FuegeZeichenHinzu(_meineZeichen, '[', '`');
                FuegeZeichenHinzu(_meineZeichen, '{', '~');
            }
        }

        private void FuegeZeichenHinzu(IList<char> list, char start, char ende)
        {
            //Methode um Zeichen zur Liste zum generieren des Passwortes hinzufügen
            for (int i = start; i < ende + 1; i++)
            {
                list.Add((char)i);
            }
        }
        #endregion

        #region Events
        //Event Passwort Status
        protected void OnPasswortStatus(string estimatedPasswort,long currentTry)
        {
            var args = new BruteForceStatusArgs(estimatedPasswort, currentTry);
            PasswortStatus(this, args);
        }

        //Event Passwort gefunden
        protected void OnPasswortFounded(string passwort, TimeSpan time, long allTrys)
        {
            var args = new PasswortFoundedArgs(passwort, time, allTrys);
            Passwordfounded(this, args);
        }

        protected void OnWrongRange(string errorPasswort, string errorEstimatedPasswort)
        {
            var args = new WrongRangeArgs(errorPasswort, errorEstimatedPasswort);
            WrongRange(this, args);
        }

        protected void OnPasswortStatusReset(string estimatedPasswort, long currentTry)
        {
            var args = new BruteForceStatusResetArgs(estimatedPasswort, currentTry);
            PasswortStatusReset(this, args);
        }

        protected void OnPasswortFoundedReset(string passwort, TimeSpan time, long allTrys)
        {
            var args = new PasswortFoundedResetArgs(passwort, time, allTrys);
            PasswordfoundedReset(this, args);
        }

        protected void OnReset(bool Resetter)
        {
            var args = new ResetArgs(Resetter);
            Reset(this, args);
        }

        protected void OnTimer(string Timer)
        {
            var args = new TimerArgs(Timer);
            TimerUp(this, args);
        }
        #endregion
        
        public void ResetData()
        {
            //Werte zurücksetzen
            _sw.Reset();
            _zähler = 0;
            _generiertesPasswort = "";
            _alleVersuche = 0;
            _uiVerzögerer = 0;

            //Zur Anzeige weiter geben
            OnPasswortStatusReset("nichts", _zähler);
            OnPasswortFoundedReset("Bitte starte die Suche!",_sw.Elapsed,_alleVersuche);
            OnTimer("00:00:00");
        }

        public void Configurate(string pathIn, string pathOut, int minLänge, int maxLänge, int AktRhythm)
        {
            
            //Min und Max auf Richtigkeit überprüfen
            if (minLänge > maxLänge)
            {
                OnReset(_überbrücker);
                OnWrongRange("Error! Falsche Range-Einstellung!!!", "Error! Falsche Range-Einstellung!!!");
            }
            else
            {
                //Startwerte festlegen
                _currentPath = pathIn;
                _outPath = pathOut;
                _passwortLänge = minLänge;
                _aktualisierer = AktRhythm;
                _listenlänge = _meineZeichen.Count;

                //BruteForce ausführen
                BruteForceExecute(maxLänge);

                //ResetButton freigeben
                OnReset(_überbrücker);
            }
        }
    }
}
