using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pentago.Pentago;
using System.Windows.Forms;

internal class CPU
{
    private static int[,] spielBrett = new int[6, 6];

    public static Button spieleZug(List<Button> buttons, int tiefe)
    {
        UpdateSpielBrett(buttons); //Spielbrett wird nach aktuellem Zustand aktualisiert

        var (reihe, spalte) = MinimaxAlgorithmus.FindeBestenZug(buttons, 2, tiefe); //Minmax wird aufgerufen

        if (reihe != -1 && spalte != -1) //Falls Zug gefunden, sonst random zug
        {
            return buttons[reihe * 6 + spalte];
        }
        return RandomZug(buttons);
    }

    // RandomZug methode
    private static Button RandomZug(List<Button> buttons)
    {
        Random random = new Random();
        Button selectedButton = null;

        do
        {
            int index = random.Next(buttons.Count);
            selectedButton = buttons[index];
        } while (selectedButton.Text != "?");

        return selectedButton;
    }

    //Methode die den aktuellen Stand des Spielbretts zurückgibt
    private static void UpdateSpielBrett(List<Button> buttons)
    {
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
    }
}

public static class MinimaxAlgorithmus
{
    public static (int, int) FindeBestenZug(List<Button> buttons, int spieler, int tiefe)
    {
        // Konvertiert Spielbrett in einen Array
        int[,] spielBrett = KonvertiereButtonsZuSpielBrett(buttons);
        List<(int, int)> verfügbareZüge = GetVerfügbareZüge(spielBrett);

        // Führt Minmax aus und gibt besten Zug zurück
        return Minimax(spielBrett, tiefe, int.MinValue, int.MaxValue, true, spieler, verfügbareZüge).Item2;
    }

    // Minimax-Algo Alpha-Beta-Pruning
    private static (int, (int, int)) Minimax(int[,] spielBrett, int tiefe, int alpha, int beta, bool maximieren, int spieler, List<(int, int)> verfügbareZüge)
    {
        int gegner = (spieler == 1) ? 2 : 1;
        int bewertung = GewinnUeberpruefung.Gewinner(spielBrett);

        // Falls ein Spieler gewonnen hat gib eine entsprechende Bewertung zurück
        if (bewertung == spieler)
            return (10, (-1, -1));
        if (bewertung == gegner)
            return (-10, (-1, -1));
        if (tiefe == 0 || verfügbareZüge.Count == 0)
            return (BewerteSpielBrett(spielBrett, spieler), (-1, -1));

        (int, int) besterZug = (-1, -1);
        int besteBewertung = maximieren ? int.MinValue : int.MaxValue;

        // Iteriere über alle verfügbaren Züge
        foreach (var zug in verfügbareZüge)
        {

            // Führe den Zug aus
            spielBrett[zug.Item1, zug.Item2] = maximieren ? spieler : gegner;
            List<(int, int)> neueZüge = new List<(int, int)>(verfügbareZüge);
            neueZüge.Remove(zug);

            // Aufruf von Minmax
            int bewertungZug = Minimax(spielBrett, tiefe - 1, alpha, beta, !maximieren, spieler, neueZüge).Item1;
            spielBrett[zug.Item1, zug.Item2] = 0;

            // Update der besten Bewertung und des besten Zuges
            if (maximieren)
            {
                if (bewertungZug > besteBewertung)
                {
                    besteBewertung = bewertungZug;
                    besterZug = zug;
                }
                alpha = Math.Max(alpha, besteBewertung);
            }
            else
            {
                if (bewertungZug < besteBewertung)
                {
                    besteBewertung = bewertungZug;
                    besterZug = zug;
                }
                beta = Math.Min(beta, besteBewertung);
            }
            // Alpha-Beta-Pruning
            if (beta <= alpha)
                break;
        }

        return (besteBewertung, besterZug);
    }

    // Methode zur Bewertung des Spielbretts
    private static int BewerteSpielBrett(int[,] spielBrett, int spieler)
    {
        int gegner = (spieler == 1) ? 2 : 1;
        int spielerBewertung = ZaehlReihen(spielBrett, spieler);
        int gegnerBewertung = ZaehlReihen(spielBrett, gegner);
        return spielerBewertung - gegnerBewertung;
    }

    // Methode, die die Anzahl der Reihen eines Spielers zählt
    private static int ZaehlReihen(int[,] spielBrett, int spieler)
    {
        int reihen = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (spielBrett[i, j] == spieler)
                {
                    reihen += ZaehleReihenAbPosition(spielBrett, spieler, i, j);
                }
            }
        }
        return reihen;
    }

    // Methode, die die Reihen ab einer Position zählt
    private static int ZaehleReihenAbPosition(int[,] spielBrett, int spieler, int reihe, int spalte)
    {
        int reihen = 0;
        reihen += GewinneRichtung(spielBrett, spieler, reihe, spalte, 1, 0); // Horizontal
        reihen += GewinneRichtung(spielBrett, spieler, reihe, spalte, 0, 1); // Vertikal
        reihen += GewinneRichtung(spielBrett, spieler, reihe, spalte, 1, 1); // Diagonal
        reihen += GewinneRichtung(spielBrett, spieler, reihe, spalte, 1, -1); // Diagonal
        return reihen;
    }

    // Methode, die überprüft, ob eine Richtung eine Gewinnreihe bildet
    private static int GewinneRichtung(int[,] spielBrett, int spieler, int reihe, int spalte, int deltaReihe, int deltaSpalte)
    {
        int zaehler = 0;
        for (int i = 0; i < 5; i++)
        {
            int r = reihe + i * deltaReihe;
            int s = spalte + i * deltaSpalte;

            if (r >= 0 && r < 6 && s >= 0 && s < 6 && spielBrett[r, s] == spieler)
            {
                zaehler++;
            }
            else
            {
                break;
            }
        }
        return zaehler == 5 ? 1 : 0;
    }

    // Methode, die die Liste der verfügbaren Züge zurückgibt
    private static List<(int, int)> GetVerfügbareZüge(int[,] spielBrett)
    {
        List<(int, int)> verfügbareZüge = new List<(int, int)>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (spielBrett[i, j] == 0)
                    verfügbareZüge.Add((i, j));
            }
        }
        return verfügbareZüge;
    }

    // Methode, die die Buttons in ein Spielbrett konvertiert
    private static int[,] KonvertiereButtonsZuSpielBrett(List<Button> buttons)
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

        return spielBrett;
    }
}

public static class GewinnUeberpruefung
{
    public static int Gewinner(int[,] spielBrett)
    {
        if (SpaltenUeberpruefen(spielBrett, 1))
            return 1;
        if (SpaltenUeberpruefen(spielBrett, 2))
            return 2;
        return 0;
    }

    private static bool SpaltenUeberpruefen(int[,] spielBrett, int spieler)
    {
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