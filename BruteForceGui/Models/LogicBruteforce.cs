using BruteForceGui.Models.Args;
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
    public class LogicBruteforce
    {
        public LogicBruteforce()
        {
            _sw = new Stopwatch();
        }

        #region EventHandler
        public event EventHandler<BruteForceStatusArgs> PasswortStatus;
        public event EventHandler<PasswortFoundedArgs> Passwordfounded;
        public event EventHandler<ResetArgs> Reset;
        public event EventHandler<BruteForceStatusResetArgs> PasswortStatusReset;
        public event EventHandler<PasswortFoundedResetArgs> PasswordfoundedReset;
        public event EventHandler<TimerArgs> TimerUp;
        #endregion

        #region Variablen
        private string _eingegebenesPasswort;
        public string GeneriertesPasswort;
        public List<char> MeineZeichen;
        public List<int> ContinueSpeicher;
        public int Listenlänge;
        public bool IsWeitermachen = false;
        public int PasswortLänge;
        public long Zähler = 0;
        private Stopwatch _sw;
        private int _uiVerzögerer = 0;
        private long _alleVersuche;
        private int Aktualisierer;
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
            while (IsWeitermachen == false)
            {
                char[] zutestendesPasswort = new char[PasswortLänge];
                ZeichenGenerator(zutestendesPasswort, 0, maxLänge);
                PasswortLänge++;
            }
        }

        private void ZeichenGenerator(char[] zuTestendesPasswort, int Position, int maxLänge)
        {
            //Max angegebene Passwortlänge erreicht?
            if (PasswortLänge == maxLänge+1)
            {
                IsWeitermachen = true;
                GeneriertesPasswort = "Error! Password not in Range!!!";
                _alleVersuche = Zähler;
                _sw.Stop();
                OnPasswortFounded(GeneriertesPasswort, _sw.Elapsed, _alleVersuche);
            }
            else
            {
                //PW Array Schleife
                for (int i = 0; i < Listenlänge; i++)
                {
                    zuTestendesPasswort[Position] = MeineZeichen[i];
                    if (Position < PasswortLänge - 1)
                    {
                        ZeichenGenerator(zuTestendesPasswort,Position + 1, maxLänge);
                    }

                    //Passwort umwandeln aus Array in string
                    GeneriertesPasswort = new string(zuTestendesPasswort);

                    //Passwort überprüfen
                    if (Position == PasswortLänge - 1)
                    {
                        Zähler++;
                        if (_uiVerzögerer == 0)
                        {
                            //Status aktualisieren
                            OnPasswortStatus(GeneriertesPasswort, Zähler);
                            OnTimer(Convert.ToString(_sw.Elapsed));
                        }
                        _uiVerzögerer++;
                        if (_uiVerzögerer == Aktualisierer)
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
            if (GeneriertesPasswort == _eingegebenesPasswort)
            {
                _alleVersuche = Zähler;
                IsWeitermachen = true;
                _sw.Stop();
                OnPasswortFounded(GeneriertesPasswort, _sw.Elapsed, _alleVersuche);
            }
        }
        #endregion

        #region CharSelector
        public void CharSelector(bool lower, bool upper, bool numbers, bool special)
        {
            MeineZeichen = new List<char>();
            ContinueSpeicher = new List<int>();

            //kleinesAlphabet
            if (lower == true)
            {
                FuegeZeichenHinzu(MeineZeichen, 'a', 'z');
            }

            //großesAlphabet
            if (upper == true)
            {
                FuegeZeichenHinzu(MeineZeichen, 'A', 'Z');
            }

            //Zahlen
            if (numbers == true)
            {
                FuegeZeichenHinzu(MeineZeichen, '0', '9');
            }

            //Sonderzeichen
            if (special == true)
            {
                FuegeZeichenHinzu(MeineZeichen, '!', '/');
                FuegeZeichenHinzu(MeineZeichen, ':', '@');
                FuegeZeichenHinzu(MeineZeichen, '[', '`');
                FuegeZeichenHinzu(MeineZeichen, '{', '~');
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

        protected void OnPasswortStatusReset(string estimatedPasswort, long currentTry)
        {
            var args = new BruteForceStatusResetArgs(estimatedPasswort, currentTry);
            PasswortStatusReset(this, args);
        }

        //Event Passwort gefunden
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
            Zähler = 0;
            GeneriertesPasswort = "";
            _alleVersuche = 0;
            _uiVerzögerer = 0;

            //Zur Anzeige weiter geben
            OnPasswortStatusReset("nichts", Zähler);
            OnPasswortFoundedReset("Bitte starte die Suche!",_sw.Elapsed,_alleVersuche);
            OnTimer("00:00:00");
        }

        public void Configurate(string Passwort, int minLänge, int maxLänge, int AktRhythm)
        {
            //Min und Max auf Richtigkeit überprüfen
            if (minLänge > maxLänge)
            {
                OnReset(_überbrücker);
                MessageBox.Show("Error! Falsche Einstellung der Passwortlänge");
            }
            else
            {
                //Startwerte festlegen
                _eingegebenesPasswort = Passwort;
                PasswortLänge = minLänge;
                Aktualisierer = AktRhythm;
                Listenlänge = MeineZeichen.Count;

                //BruteForce ausführen
                BruteForceExecute(maxLänge);

                //ResetButton freigeben
                OnReset(_überbrücker);
            }
        }
    }
}
