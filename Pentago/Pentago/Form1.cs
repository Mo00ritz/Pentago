using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

        public enum SpielStatus
        {
            SpielerZug,
            SpielerDrehen,
            CPUZug,
            CPUDrehen
        }
        public enum Spielmodus
        {
            CPU,
            Mehrspieler
        }

        private SpielStatus currentState;
        private Player currentPlayer;
        private Spielmodus modus;
        private List<Button> buttons;
        private Random random = new Random();

        public int Spielergewonnen = 0;
        public int CPUgewonnen = 0;

        private Label labelSpielerSiege;
        private Label labelCPUSiege;
        private CheckBox checkBoxSpielmodus;
        private Label labelCurrentPlayer;

        public Pentago()
        {
            InitializeComponent();
            InitializeLabels();
            InitializeCheckBox();
            InitializeCurrentPlayer();
            currentState = SpielStatus.SpielerZug;
            modus = Spielmodus.CPU;
            RestartGame();
            drehButtonsAus();
            difficultyComboBox.Items.Add(new ComboBoxItem("Leicht", 2));
            difficultyComboBox.Items.Add(new ComboBoxItem("Mittel", 4));
            difficultyComboBox.Items.Add(new ComboBoxItem("Schwer", 6));
            difficultyComboBox.SelectedIndex = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void SpielerZugButton(object sender, EventArgs e)
        {
            UpdateCurrentPlayerLabel();
            if (currentState != SpielStatus.SpielerZug)
                return;
            
            var button = (Button)sender;

            if (button.Text != "?")
                return;

            button.Text = currentPlayer.ToString();
            button.BackColor = currentPlayer == Player.X ? Color.Blue : Color.Red;

            checkGewonnen();

            currentState = SpielStatus.SpielerDrehen;
            SpielerDrehenButton(sender, e);
           
        }

        private void SpielerDrehenButton(object sender, EventArgs e)
        {
            if (currentState != SpielStatus.SpielerDrehen)
                return;
            UpdateCurrentPlayerLabel();
            drehButtonsAn();
            Button button = sender as Button;
            checkGewonnen();
            if (modus == Spielmodus.CPU)
            {
                currentState = SpielStatus.SpielerDrehen;
            }
            else
            {
                checkGewonnen();
                currentPlayer = currentPlayer == Player.X ? Player.O : Player.X;
                currentState = SpielStatus.SpielerZug;
                currentState = SpielStatus.SpielerDrehen;
            }
        }

        private void CPUZug(object sender, EventArgs e)
        {
            drehButtonsAus();
            
            if (modus != Spielmodus.CPU || currentState != SpielStatus.CPUZug)
                return;

            if (difficultyComboBox.SelectedItem is ComboBoxItem selectedDifficulty)
            {
                int tiefe = selectedDifficulty.Tiefe;
                Button cpuZug = CPU.spieleZug(buttons, tiefe);
                cpuZug.Text = "O";
                cpuZug.BackColor = Color.Red;
            }
            else
            {
                //Default-Tiefe, falls keine Auswahl getroffen wurde
                int tiefe = 3;
                Button cpuZug = CPU.spieleZug(buttons, tiefe);
                cpuZug.Text = "O";
                cpuZug.BackColor = Color.Red;
            }

            CPUTimer.Stop();
            currentPlayer = Player.X;
            UpdateCurrentPlayerLabel();
            checkGewonnen();
            CPUDrehen();
            currentState = SpielStatus.SpielerZug;
        }

        private void CPUDrehen()
        {
            int drehenRandom = random.Next(8);

            switch (drehenRandom)
            {
                case 0:
                    obenLinksDrehenUhr(null, null);
                    break;
                case 1:
                    obenLinksDrehenGegenUhr(null, null);
                    break;
                case 2:
                    obenRechtsDrehenUhr(null, null);
                    break;
                case 3:
                    obenRechtsDrehenGegenUhr(null, null);
                    break;
                case 4:
                    untenLinksDrehenUhr(null, null);
                    break;
                case 5:
                    untenLinksDrehenGegenUhr(null, null);
                    break;
                case 6:
                    untenRechtsDrehenUhr(null, null);
                    break;
                case 7:
                    untenRechtsDrehenGegenUhr(null, null);
                    break;
            }
            checkGewonnen();
            currentState = SpielStatus.SpielerZug;
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
            currentState = SpielStatus.SpielerZug;
        }

        private void InitializeLabels()
        {
            // Spieler Sieg Label
            labelSpielerSiege = new Label
            {
                Text = "Spieler Siege: 0",
                Font = new Font("Arial", 16, FontStyle.Bold), 
                ForeColor = Color.White, 
                BackColor = Color.Transparent, 
                AutoSize = true, 
                Location = new Point(10, 20)
            };
            this.Controls.Add(labelSpielerSiege);

            // CPU Sieg Label
            labelCPUSiege = new Label
            {
                Text = "CPU Siege: 0",
                Font = new Font("Arial", 16, FontStyle.Bold), 
                ForeColor = Color.White, 
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(10, 50)
            };
            this.Controls.Add(labelCPUSiege);
        }

        private void UpdateSiegeLabels()
        {
            labelSpielerSiege.Text = "Spieler1 Siege: " + Spielergewonnen;
            if (checkBoxSpielmodus.Checked)
            {
                labelCPUSiege.Text = "Spieler2 Siege: " + CPUgewonnen;
            }
            else
            {
                labelCPUSiege.Text = "CPU Siege: " + CPUgewonnen;
            }
        }

        private void InitializeCheckBox()
        {
            // Spielmodus CheckBox
            checkBoxSpielmodus = new CheckBox
            {
                Text = "Mehrspieler Modus",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent, 
                AutoSize = true, 
                Location = new Point(10, 650)
            };
            checkBoxSpielmodus.CheckedChanged += new EventHandler(MehrspielerModus);
            this.Controls.Add(checkBoxSpielmodus);
        }

        //Momentaner Spieler Anzeige
        private void InitializeCurrentPlayer()
        {
            labelCurrentPlayer = new Label
            {
                Text = $"{currentPlayer} ist am Zug",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Blue,
                BackColor = Color.Transparent, 
                AutoSize = true,
                Location = new Point(500, 20)
            };
            this.Controls.Add(labelCurrentPlayer);
        }
        //Methode zum Update
        private void UpdateCurrentPlayerLabel()
        {
            labelCurrentPlayer.Text = $"{currentPlayer} ist am Zug";
            if (currentPlayer == Player.X)
            {
                labelCurrentPlayer.ForeColor = Color.Blue;
            }
            else 
            {
                labelCurrentPlayer.ForeColor = Color.Red;
            }
        }


        // Methoden für Spielfeld-Drehung
        private void obenLinksDrehenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsObenLinks = new List<Button> { button1, button2, button3, button7, button8, button9, button13, button14, button15 };
            Spielfeld.DrehenImUhrzeigersinn(buttonsObenLinks);
            if (currentState == SpielStatus.SpielerDrehen)
            {
                if (modus == Spielmodus.CPU)
                {
                    currentState = SpielStatus.CPUZug;
                    CPUTimer.Start();
                    currentPlayer = Player.O;
                    UpdateCurrentPlayerLabel();
                }
                else
                {
                    currentState = SpielStatus.SpielerZug;
                    checkGewonnen();

                }
            drehButtonsAus();
            UpdateCurrentPlayerLabel();
            }
        }

        private void obenLinksDrehenGegenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsObenLinks = new List<Button> { button1, button2, button3, button7, button8, button9, button13, button14, button15 };
            Spielfeld.DrehenGegenUhrzeigersinn(buttonsObenLinks);
            if (currentState == SpielStatus.SpielerDrehen)
            {
                if (modus == Spielmodus.CPU)
                {
                    currentState = SpielStatus.CPUZug;
                    CPUTimer.Start();
                    currentPlayer = Player.O;
                    UpdateCurrentPlayerLabel();
                }
                else
                {
                    currentState = SpielStatus.SpielerZug;
                    checkGewonnen();
                }
            }
            drehButtonsAus();
            UpdateCurrentPlayerLabel();
        }

        private void obenRechtsDrehenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsobenRechts = new List<Button> { button4, button5, button6, button10, button11, button12, button16, button17, button18 };
            Spielfeld.DrehenImUhrzeigersinn(buttonsobenRechts);
            if (currentState == SpielStatus.SpielerDrehen)
            {
                if (modus == Spielmodus.CPU)
                {
                    currentState = SpielStatus.CPUZug;
                    CPUTimer.Start();
                    currentPlayer = Player.O;
                    UpdateCurrentPlayerLabel();
                }
                else
                {
                    currentState = SpielStatus.SpielerZug;
                    checkGewonnen();
                }
            }
            drehButtonsAus();
            UpdateCurrentPlayerLabel();
        }

        private void obenRechtsDrehenGegenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsobenRechts = new List<Button> { button4, button5, button6, button10, button11, button12, button16, button17, button18 };
            Spielfeld.DrehenGegenUhrzeigersinn(buttonsobenRechts);
            if (currentState == SpielStatus.SpielerDrehen)
            {
                if (modus == Spielmodus.CPU)
                {
                    currentState = SpielStatus.CPUZug;
                    CPUTimer.Start();
                    currentPlayer = Player.O;
                    UpdateCurrentPlayerLabel();
                }
                else
                {
                    currentState = SpielStatus.SpielerZug;
                    checkGewonnen();
                }
            }
            drehButtonsAus();
            UpdateCurrentPlayerLabel();
        }

        private void untenRechtsDrehenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsuntenRechts = new List<Button> { button22, button23, button24, button28, button29, button30, button34, button35, button36 };
            Spielfeld.DrehenImUhrzeigersinn(buttonsuntenRechts);
            if (currentState == SpielStatus.SpielerDrehen)
            {
                if (modus == Spielmodus.CPU)
                {
                    currentState = SpielStatus.CPUZug;
                    CPUTimer.Start();
                    currentPlayer = Player.O;
                    UpdateCurrentPlayerLabel();
                }
                else
                {
                    currentState = SpielStatus.SpielerZug;
                    checkGewonnen();
                }
            }
            drehButtonsAus();
            UpdateCurrentPlayerLabel();
        }

        private void untenRechtsDrehenGegenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsuntenRechts = new List<Button> { button22, button23, button24, button28, button29, button30, button34, button35, button36 };
            Spielfeld.DrehenGegenUhrzeigersinn(buttonsuntenRechts);
            if (currentState == SpielStatus.SpielerDrehen)
            {
                if (modus == Spielmodus.CPU)
                {
                    currentState = SpielStatus.CPUZug;
                    CPUTimer.Start();
                    currentPlayer = Player.O;
                    UpdateCurrentPlayerLabel();
                }
                else
                {
                    currentState = SpielStatus.SpielerZug;
                    checkGewonnen();
                }
            }
            drehButtonsAus();
            UpdateCurrentPlayerLabel();
        }

        private void untenLinksDrehenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsuntenLinks = new List<Button> { button19, button20, button21, button25, button26, button27, button31, button32, button33 };
            Spielfeld.DrehenImUhrzeigersinn(buttonsuntenLinks);
            if (currentState == SpielStatus.SpielerDrehen)
            {
                if (modus == Spielmodus.CPU)
                {
                    currentState = SpielStatus.CPUZug;
                    CPUTimer.Start();
                    currentPlayer = Player.O;
                    UpdateCurrentPlayerLabel();
                }
                else
                {
                    currentState = SpielStatus.SpielerZug;
                    checkGewonnen();
                }
            }
            drehButtonsAus();
            UpdateCurrentPlayerLabel();
        }

        private void untenLinksDrehenGegenUhr(object sender, EventArgs e)
        {
            List<Button> buttonsuntenLinks = new List<Button> { button19, button20, button21, button25, button26, button27, button31, button32, button33 };
            Spielfeld.DrehenGegenUhrzeigersinn(buttonsuntenLinks);
            if (currentState == SpielStatus.SpielerDrehen)
            {
                if (modus == Spielmodus.CPU)
                {
                    currentState = SpielStatus.CPUZug;
                    CPUTimer.Start();
                    currentPlayer = Player.O;
                    UpdateCurrentPlayerLabel();
                }
                else
                {
                    currentState = SpielStatus.SpielerZug;
                    checkGewonnen();
                }
            }
            drehButtonsAus();
            UpdateCurrentPlayerLabel();
        }

        private void ResetButton(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void MehrspielerModus(object sender, EventArgs e)
        {
            modus = checkBoxSpielmodus.Checked ? Spielmodus.Mehrspieler : Spielmodus.CPU;
            RestartGame();
            UpdateSiegeLabels();
        }

        //gets für auswahl von Schwierigkeit
        public class ComboBoxItem
        {
            public string Name { get; }
            public int Tiefe { get; }

            public ComboBoxItem(string name, int tiefe)
            {
                Name = name;
                Tiefe = tiefe;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        private void drehButtonsAus()
        {
            button37.Enabled = false;
            button38.Enabled = false;
            button39.Enabled = false;
            button40.Enabled = false;    
            button41.Enabled = false;
            button42.Enabled = false;
            button43.Enabled = false;
            button44.Enabled = false;
        }
        private void drehButtonsAn()
        {
            button37.Enabled = true;
            button38.Enabled = true;
            button39.Enabled = true;
            button40.Enabled = true;
            button41.Enabled = true;
            button42.Enabled = true;
            button43.Enabled = true;
            button44.Enabled = true;
        }

        public void checkGewonnen()
        {

            if (GewinnUeberpruefung.Gewinner(buttons) == 1)
            {
                MessageBox.Show("Spieler 1 hat gewonnen!");
                Spielergewonnen++;
                UpdateSiegeLabels();
                RestartGame();
                return;
            }
            else if (GewinnUeberpruefung.Gewinner(buttons) == 2)
            {
                if (modus == Spielmodus.CPU)
                {
                    MessageBox.Show("CPU hat gewonnen!");
                }
                else
                {
                    MessageBox.Show("Spieler2 hat gewonnen!");
                }
                CPUgewonnen++;
                UpdateSiegeLabels();
                RestartGame();
                return;
            }
        }
    }
}