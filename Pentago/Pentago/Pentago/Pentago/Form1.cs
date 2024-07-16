using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace Pentago
{
    public partial class Pentago : Form
    {
        public enum Player
        {
            X, O
        }

        Player currentPlayer;
        int runden = 0;
        int gewonnenSpieler = 0;
        int gewonnenCPU = 0;
        List<Button> buttons;

        public Pentago()
        {
            InitializeComponent();
            RestartGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void CPUZug(object sender, EventArgs e)
        {
            // CPU-Zug-Logik hier
        }

        private void SpielerZugButton(object sender, EventArgs e)
        {
            var button = (Button)sender;

            if (button.Text != "?")
                return;

            currentPlayer = Player.X;
            button.Text = currentPlayer.ToString();
            button.BackColor = Color.Blue;

            if (GewinnUeberpruefung.Gewonnen(buttons))
            {
                MessageBox.Show("Spieler hat gewonnen!");
                RestartGame();
            }
            else
            {
                currentPlayer = Player.O;
                CPUTimer.Start();
            }
        }

        private void RestartGame()
        {
            buttons = new List<Button>
            {
                button1, button2, button3, button4, button5, button6,
                button7, button8, button9, button10, button11, button12,
                button13, button14, button15, button16, button17, button18,
                button19, button20, button21, button22, button23, button24,
                button25, button26, button27, button28, button29, button30,
                button31, button32, button33, button34, button35, button36
            };

            foreach (Button button in buttons)
            {
                button.Enabled = true;
                button.Text = "?";
                button.BackColor = DefaultBackColor;
            }

            currentPlayer = Player.X;
        }

        // Methoden für Spielfeld-Drehung

        private void obenLinksDrehenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsObenLinks = new List<Button> { button1, button2, button3, button7, button8, button9, button13, button14, button15 };
            Spielfeld.DrehenImUhrzeigersinn(buttonsObenLinks);
        }

        private void obenLinksDrehenGegenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsObenLinks = new List<Button> { button1, button2, button3, button7, button8, button9, button13, button14, button15 };
            Spielfeld.DrehenGegenUhrzeigersinn(buttonsObenLinks);
        }

        private void obenRechtsDrehenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsobenRechts = new List<Button> { button4, button5, button6, button10, button11, button12, button16, button17, button18 };
            Spielfeld.DrehenImUhrzeigersinn(buttonsobenRechts);
        }

        private void obenRechtsDrehenGegenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsobenRechts = new List<Button> { button4, button5, button6, button10, button11, button12, button16, button17, button18 };
            Spielfeld.DrehenGegenUhrzeigersinn(buttonsobenRechts);
        }

        private void untenRechtsDrehenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsuntenRechts = new List<Button> { button22, button23, button24, button28, button29, button30, button34, button35, button36 };
            Spielfeld.DrehenImUhrzeigersinn(buttonsuntenRechts);
        }

        private void untenRechtsDrehenGegenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsuntenRechts = new List<Button> { button22, button23, button24, button28, button29, button30, button34, button35, button36 };
            Spielfeld.DrehenGegenUhrzeigersinn(buttonsuntenRechts);
        }

        private void untenLinksDrehenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsuntenLinks = new List<Button> { button19, button20, button21, button25, button26, button27, button31, button32, button33 };
            Spielfeld.DrehenImUhrzeigersinn(buttonsuntenLinks);
        }

        private void untenLinksDrehenGegenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsuntenLinks = new List<Button> { button19, button20, button21, button25, button26, button27, button31, button32, button33 };
            Spielfeld.DrehenGegenUhrzeigersinn(buttonsuntenLinks);
        }
    }
}


