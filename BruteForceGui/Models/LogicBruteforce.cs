using BruteForceGui.Models.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BruteForceGui.ViewModels;
using System.Windows;

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
        #endregion

        #region Variablen
        private string _eingegebenesPasswort;
        public string GeneriertesPasswort;
        public List<char> MeineZeichen;
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

        public void StarteBruteForce(string Passwort, int minLänge, int maxLänge, int AktRhythm)
        {
            if (minLänge > maxLänge)
            {
                OnReset(_überbrücker);
                MessageBox.Show("Error! Falsche Einstellung der Passwortlänge");
            }
            else
            {
                _eingegebenesPasswort = Passwort;
                _sw.Start();
                PasswortLänge = minLänge;
                Aktualisierer = AktRhythm;
                Listenlänge = MeineZeichen.Count;
                while (IsWeitermachen == false)
                {
                    char[] ztPasswort = new char[PasswortLänge];
                    ZeichenGenerator(ztPasswort, 0, maxLänge);
                    PasswortLänge++;
                }

                OnReset(_überbrücker);
            }
        }

        private void ZeichenGenerator(char[] zuTestendesPasswort, int Position, int maxLänge)
        {
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
                for (int i = 0; i < Listenlänge; i++)
                {
                    zuTestendesPasswort[Position] = MeineZeichen[i];
                    if (Position < PasswortLänge - 1)
                    {
                        ZeichenGenerator(zuTestendesPasswort, Position + 1, maxLänge);
                    }

                    //Passwort umwandeln aus Array in string
                    GeneriertesPasswort = new string(zuTestendesPasswort);

                    //Passwort überprüfen
                    if (Position == PasswortLänge - 1)
                    {
                        Zähler++;
                        if (_uiVerzögerer == 0)
                        {
                            OnPasswortStatus(GeneriertesPasswort, Zähler);
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

        protected void OnReset(bool Resetter)
        {
            var args = new ResetArgs(Resetter);
            Reset(this, args);
        }
        #endregion
        
        public void ResetData()
        {
            _sw.Reset();
            Zähler = 0;
            GeneriertesPasswort = "";
            _alleVersuche = 0;

            OnPasswortStatus("nichts", Zähler);
            OnPasswortFounded("Bitte starte die Suche!",_sw.Elapsed,_alleVersuche);

        }
    }
}
