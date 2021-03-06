﻿using BruteForceGui.Models.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;

namespace BruteForceGui.Models
{
    public class LogicBruteforceTask
    {
        public LogicBruteforceTask()
        {
            _sw = new Stopwatch();
        }

        #region EventHandler
        public event EventHandler<BruteForceStatusThreadArgs> PasswortStatusThread;
        public event EventHandler<PasswortFoundedThreadArgs> PasswordfoundedThread;
        public event EventHandler<ResetThreadArgs> ResetThread;
        public event EventHandler<BruteForceStatusResetThreadArgs> PasswortStatusResetThread;
        public event EventHandler<PasswortFoundedResetThreadArgs> PasswordfoundedResetThread;
        public event EventHandler<TimerThreadArgs> TimerUpThread;
        public event EventHandler<WrongRangeThreadArgs> WrongRangeThread;
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
            for (int i = 0; i < maxLänge; i++)
            {
                Task t = new Task(() =>
                {
                    char[] zutestendesPasswort = new char[_passwortLänge];
                    ZeichenGenerator(zutestendesPasswort, 0, maxLänge);
                });
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
            if (_generiertesPasswort == _eingegebenesPasswort)
            {
                _alleVersuche = _zähler;
                IsWeitermachen = true;
                _sw.Stop();
                OnPasswortFounded(_generiertesPasswort, _sw.Elapsed, _alleVersuche);
            }
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
            var args = new BruteForceStatusThreadArgs(estimatedPasswort, currentTry);
            PasswortStatusThread(this, args);
        }

        //Event Passwort gefunden
        protected void OnPasswortFounded(string passwort, TimeSpan time, long allTrys)
        {
            var args = new PasswortFoundedThreadArgs(passwort, time, allTrys);
            PasswordfoundedThread(this, args);
        }

        protected void OnWrongRange(string errorPasswort, string errorEstimatedPasswort)
        {
            var args = new WrongRangeThreadArgs(errorPasswort, errorEstimatedPasswort);
            WrongRangeThread(this, args);
        }

        protected void OnPasswortStatusReset(string estimatedPasswort, long currentTry)
        {
            var args = new BruteForceStatusResetThreadArgs(estimatedPasswort, currentTry);
            PasswortStatusResetThread(this, args);
        }

        protected void OnPasswortFoundedReset(string passwort, TimeSpan time, long allTrys)
        {
            var args = new PasswortFoundedResetThreadArgs(passwort, time, allTrys);
            PasswordfoundedResetThread(this, args);
        }

        protected void OnReset(bool Resetter)
        {
            var args = new ResetThreadArgs(Resetter);
            ResetThread(this, args);
        }

        protected void OnTimer(string Timer)
        {
            var args = new TimerThreadArgs(Timer);
            TimerUpThread(this, args);
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

        public void Configurate(string Passwort, int minLänge, int maxLänge, int AktRhythm)
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
                _eingegebenesPasswort = Passwort;
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
