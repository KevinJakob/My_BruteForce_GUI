using BruteForceGui.Models.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BruteForceGui.ViewModels;

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
        #endregion

        #region Variablen
        private string _eingegebenesPasswort;
        public string GeneriertesPasswort;
        public List<char> MeineZeichen;
        public int Listenlänge;
        public bool IsWeitermachen = false;
        public int PasswortLänge = 0;
        public long Zähler = 0;
        private Stopwatch _sw;
        private int _uiVerzögerer = 0;
        private long _alleVersuche;
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

        public void StarteBruteForce(string Passwort)
        {
            _eingegebenesPasswort = Passwort;
            _sw.Start();
                //Passwort test beginnen mit Sonderzeichen
                Listenlänge = MeineZeichen.Count;
                while (IsWeitermachen == false)
                {
                    PasswortLänge++;
                    char[] ztPasswort = new char[PasswortLänge];
                    ZeichenGenerator(ztPasswort, 0);
                }
        }

        private void ZeichenGenerator(char[] zuTestendesPasswort, int Position)
        {
            for (int i = 0; i < Listenlänge; i++)
            {
                zuTestendesPasswort[Position] = MeineZeichen[i];
                if (Position < PasswortLänge - 1)
                {
                    ZeichenGenerator(zuTestendesPasswort, Position + 1);
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
                    if (_uiVerzögerer == 100000)
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

        //Passwort auf übereinstimmung prüfen
        public void PasswortCheck()
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
        #endregion

        
    }
}
