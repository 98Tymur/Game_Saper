using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Saper
{
    public partial class Form1 : Form
    {
        int height = 20;
        int width = 20;
        int distanceBetweenButtons = 30;
        int percentBombs = 30;
        bool isFirstClick = true;
        int cellsOpened = 0;
        int bombs = 0;

        ButtonExtended[,] allButtons;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        void FieldClick(object sender, MouseEventArgs e)
        {
            ButtonExtended button = (ButtonExtended)sender;
            // LBC
            if(e.Button == MouseButtons.Left && button.isClick)
            {
                if (button.isBomb)
                {
                    if(isFirstClick)
                    {
                        button.isBomb = false;
                        isFirstClick = false;
                        OpenRegion(button.xX, button.jJ, button);
                    }
                    else
                    {
                        Explode();
                    }
                }
                else
                {
                    EmptyFieldClick(button);
                }

                isFirstClick = false; 
            }
            //RBC
            if(e.Button == MouseButtons.Right)
            {
                button.isClick = !button.isClick;

                if(!button.isClick)
                {
                    button.Text = "B";
                }
                else
                {
                    button.Text = "";
                }
            }

            CheckWin();
        }

        void Explode()
        {
            foreach(ButtonExtended button in allButtons)
            {
                if(button.isBomb)
                {
                    button.Text = "#";
                }
            }

            MessageBox.Show("You lost. The end!");
            //Application.Restart();
        }

        void EmptyFieldClick(ButtonExtended button)
        {
            for (int j = 0; j < height; j++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (allButtons[x, j] == button)
                    {
                        OpenRegion(x, j, button);
                    }
                }
            }

            CheckWin();
        }

        void OpenRegion(int xX, int jJ, ButtonExtended button)
        {
            Queue<ButtonExtended> queue = new Queue<ButtonExtended>();
            queue.Enqueue(button);

            while(queue.Count > 0)
            { 
                ButtonExtended currentCell = queue.Dequeue();

                OpenCell(currentCell.xX, currentCell.jJ, currentCell);

                cellsOpened++;

                if (CountBombsAround(currentCell.xX, currentCell.jJ) == 0)
                {
                    for (int j = currentCell.jJ - 1; j <= currentCell.jJ + 1; j++)
                    {
                        for (int x = currentCell.xX - 1; x <= currentCell.xX + 1; x++)
                        {
                            if (x >= 0 && x < width && j < height && j >= 0)
                            {
                                if (!allButtons[x, j].wasAdded)
                                {
                                    queue.Enqueue(allButtons[x, j]);
                                    allButtons[x, j].wasAdded = true;
                                }
                            } 
                        }
                    }
                }
            }
        }

        void OpenCell(int x, int j, ButtonExtended button)
        {
            int bombsAround = CountBombsAround(x, j);
            if (bombsAround == 0)
            {

            }
            else
            {
                button.Text = "" + bombsAround;
            }

            button.Enabled = false;
        }

        int CountBombsAround(int xX, int jJ)
        {
            int bombCount = 0;

            for (int x = xX - 1; x <= xX + 1; x++)
            {
                for (int j = jJ - 1; j <= jJ + 1; j++)
                {
                    if (x >= 0 && x < width && j >= 0 && j < height)
                    {
                        if (allButtons[x, j].isBomb)
                        {
                            bombCount++;
                        }
                    }
                }
            }

            return bombCount;
        }

        void GenerateField()
        {
            allButtons = new ButtonExtended[width, height];

            Random random = new Random();

            for (int x = 10; (x - 10) < width * distanceBetweenButtons; x += distanceBetweenButtons)
            {

                for (int j = 25; (j - 25) < height * distanceBetweenButtons; j += distanceBetweenButtons)
                {
                    ButtonExtended button = new ButtonExtended();

                    button.Location = new Point(x, j);
                    button.Size = new Size(30, 30);
                    button.isClick = true;
                    
                    if (random.Next(0, 100) < percentBombs)
                    {
                        button.isBomb = true;
                    }

                    button.xX = x;
                    button.jJ = j;
                    allButtons[(x - 10) / distanceBetweenButtons, (j - 25) / distanceBetweenButtons] = button;

                    Controls.Add(button);

                    button.MouseUp += new MouseEventHandler(FieldClick);
                }
            }
        }

        private void x20ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            width = 20;
            height = 20;
            GenerateField();
        }

        private void x10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            width = 10;
            height = 10;
            GenerateField();
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            width = 5;
            height = 5;
            GenerateField();
        }

        void CheckWin()
        {
            int cells = width * height;
            int emptyCells = cells - bombs;
            if(cellsOpened >= emptyCells)
            {
                MessageBox.Show("You win!" + emptyCells + " " + cellsOpened);
            }
        }
    }

    class ButtonExtended : Button
    {
        public bool isBomb;
        public bool isClick;
        public bool wasAdded;
        public int xX, jJ;
    }
}
