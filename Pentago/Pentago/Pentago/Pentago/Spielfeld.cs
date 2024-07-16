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
            // Speicher von Position damit sich das Spielfeld nicht verschiebt
            var buttonPositionen = new Dictionary<Button, Point>();
            foreach (var button in buttons)
            {
                buttonPositionen[button] = new Point(button.Left, button.Top);
            }

            Button[,] tempArray = new Button[3, 3];

            // Kopieren in Hilfsarray
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    tempArray[row, col] = buttons[row * 3 + col];
                }
            }

            // Position drehen
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    tempArray[row, col] = buttons[col * 3 + (2 - row)];
                }
            }

            // Buttons an richtige position von GUI
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    buttons[row * 3 + col].Left = buttonPositionen[tempArray[row, col]].X;
                    buttons[row * 3 + col].Top = buttonPositionen[tempArray[row, col]].Y;
                }
            }
        }





        public static void DrehenGegenUhrzeigersinn(List<Button> buttons)
        {
            var buttonOffsets = new Dictionary<Button, Point>();
            foreach (var button in buttons)
            {
                buttonOffsets[button] = new Point(button.Left, button.Top);
            }

            for (int col = 0; col < 3; col++)
            {
                for (int row = 2; row >= 0; row--)
                {
                    
                    int originalIndex = col * 3 + (2 - row);
                    buttons[originalIndex].Left = buttonOffsets[buttons[row * 3 + col]].X;
                    buttons[originalIndex].Top = buttonOffsets[buttons[row * 3 + col]].Y;
                }
            }
        }
    }
}
