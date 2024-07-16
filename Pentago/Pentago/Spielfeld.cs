using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pentago
{
    internal class Spielfeld
    {
        public static void DrehenImUhrzeigersinn(List<Button> buttons)
        {
            // aktuelle Position der Steine werden gespeichert
            string[] texte = new string[9];
            Color[] farben = new Color[9];
            for (int i = 0; i < 9; i++)
            {
                texte[i] = buttons[i].Text;
                farben[i] = buttons[i].BackColor;
            }

            // neue zuweisung
            buttons[0].Text = texte[6];
            buttons[0].BackColor = farben[6];
            buttons[1].Text = texte[3];
            buttons[1].BackColor = farben[3];
            buttons[2].Text = texte[0];
            buttons[2].BackColor = farben[0];
            buttons[3].Text = texte[7];
            buttons[3].BackColor = farben[7];
            buttons[4].Text = texte[4];
            buttons[4].BackColor = farben[4];
            buttons[5].Text = texte[1];
            buttons[5].BackColor = farben[1];
            buttons[6].Text = texte[8];
            buttons[6].BackColor = farben[8];
            buttons[7].Text = texte[5];
            buttons[7].BackColor = farben[5];
            buttons[8].Text = texte[2];
            buttons[8].BackColor = farben[2];
        }

        public static void DrehenGegenUhrzeigersinn(List<Button> buttons)
        {
            
            string[] texte = new string[9];
            Color[] farben = new Color[9];
            for (int i = 0; i < 9; i++)
            {
                texte[i] = buttons[i].Text;
                farben[i] = buttons[i].BackColor;
            }

            buttons[0].Text = texte[2];
            buttons[0].BackColor = farben[2];
            buttons[1].Text = texte[5];
            buttons[1].BackColor = farben[5];
            buttons[2].Text = texte[8];
            buttons[2].BackColor = farben[8];
            buttons[3].Text = texte[1];
            buttons[3].BackColor = farben[1];
            buttons[4].Text = texte[4];
            buttons[4].BackColor = farben[4];
            buttons[5].Text = texte[7];
            buttons[5].BackColor = farben[7];
            buttons[6].Text = texte[0];
            buttons[6].BackColor = farben[0];
            buttons[7].Text = texte[3];
            buttons[7].BackColor = farben[3];
            buttons[8].Text = texte[6];
            buttons[8].BackColor = farben[6];
        }
    }
}