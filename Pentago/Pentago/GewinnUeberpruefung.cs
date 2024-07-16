using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pentago.Pentago;
using System.Windows.Forms;

namespace Pentago
{
    public static class GewinnUeberpruefung
    {
        // Methode zur Gewinnüberprüfung
        public static int Gewinner(List<Button> buttons)
        {
            int[,] spielBrett = new int[6, 6];

            for (int i = 0; i < buttons.Count; i++)
            {
                int reihe = i / 6;
                int spalte = i % 6;

                if (buttons[i].Text == "X")
                {
                    spielBrett[reihe, spalte] = 1;
                }
                else if (buttons[i].Text == "O")
                {
                    spielBrett[reihe, spalte] = 2;
                }
                else
                {
                    spielBrett[reihe, spalte] = 0;
                }
            }

            if (SpaltenUeberpruefen(spielBrett, 1))
            {
                return 1; // Spieler X hat gewonnen
            }
            else if (SpaltenUeberpruefen(spielBrett, 2))
            {
                return 2; // Spieler O hat gewonnen
            }

            return 0; // Kein Gewinner
        }

        private static bool SpaltenUeberpruefen(int[,] spielBrett, int spieler)
        {
            // Überprüfe Reihen und Spalten
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (GewinnBedingung(spielBrett, spieler, i, j, 0, 1) || // Horizontal
                        GewinnBedingung(spielBrett, spieler, i, j, 1, 0) || // Vertikal
                        GewinnBedingung(spielBrett, spieler, i, j, 1, 1) || // Diagonal
                        GewinnBedingung(spielBrett, spieler, i, j, 1, -1))  // Diagonal 
                        return true;
                }
            }
            return false;
        }

        private static bool GewinnBedingung(int[,] spielBrett, int spieler, int startReihe, int startSpalte, int deltaReihe, int deltaSpalte)
        {
            int zaehler = 0;

            for (int i = 0; i < 5; i++)
            {
                int reihe = startReihe + i * deltaReihe;
                int spalte = startSpalte + i * deltaSpalte;

                if (reihe < 0 || reihe >= 6 || spalte < 0 || spalte >= 6)
                    break;

                if (spielBrett[reihe, spalte] == spieler)
                    zaehler++;
                else
                    break;
            }

            return zaehler == 5;
        }
    }
}