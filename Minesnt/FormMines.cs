using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesnt {
    public partial class FormMines : Form {

        MineField mineField = new MineField();

        int cellX = -1;
        int cellY = -1;

        Boolean mousePressed = false;
        Boolean buttonPressed = false;
        Boolean gameEnd = false;

        int time = 0;

        Image[] numberImages = new Image[] {
            Minesnt.Properties.Resources.number0,
            Minesnt.Properties.Resources.number1,
            Minesnt.Properties.Resources.number2,
            Minesnt.Properties.Resources.number3,
            Minesnt.Properties.Resources.number4,
            Minesnt.Properties.Resources.number5,
            Minesnt.Properties.Resources.number6,
            Minesnt.Properties.Resources.number7,
            Minesnt.Properties.Resources.number8,
            Minesnt.Properties.Resources.number9,
            Minesnt.Properties.Resources.numberminus
        };

        Image[] mineNumerImages = new Image[] {
            Minesnt.Properties.Resources.mine1,
            Minesnt.Properties.Resources.mine2,
            Minesnt.Properties.Resources.mine3,
            Minesnt.Properties.Resources.mine4,
            Minesnt.Properties.Resources.mine5,
            Minesnt.Properties.Resources.mine6,
            Minesnt.Properties.Resources.mine7,
            Minesnt.Properties.Resources.mine8,
            Minesnt.Properties.Resources.flag
        };

        public FormMines() {
            InitializeComponent();
            timer1.Stop();
            timer1.Tick += new System.EventHandler(Timer);
        }

        private void Form1_Load(object sender, EventArgs e) {
            int startSize = 16;
            int starMinesCount = 40;
            Restart(startSize, startSize, starMinesCount);;
        }

        private void panel1_Paint(object sender, PaintEventArgs e) {
            Graphics graphics = e.Graphics;

            // border
            graphics.DrawImage(Minesnt.Properties.Resources.topleft, 0, 0);
            graphics.DrawImage(Minesnt.Properties.Resources.top, 10, 0, panel1.Width, 10);
            graphics.DrawImage(Minesnt.Properties.Resources.topright, panel1.Width - 7, 0);
            graphics.DrawImage(Minesnt.Properties.Resources.left1,0, 10);
            graphics.DrawImage(Minesnt.Properties.Resources.right1, panel1.Width - 7, 10);

            graphics.DrawImage(Minesnt.Properties.Resources.centerleft, 0, 43);
            graphics.DrawImage(Minesnt.Properties.Resources.center, 10, 43, panel1.Width, 11);
            graphics.DrawImage(Minesnt.Properties.Resources.centerright, panel1.Width - 8, 43);

            graphics.DrawImage(Minesnt.Properties.Resources.left2, 0, 54, 11, panel1.Height - 43);
            graphics.DrawImage(Minesnt.Properties.Resources.right2, panel1.Width - 8, 54, 8, panel1.Height - 43);
            graphics.DrawImage(Minesnt.Properties.Resources.leftbottom, 0 , panel1.Height - 8);
            graphics.DrawImage(Minesnt.Properties.Resources.bottom, 11, panel1.Height - 8, panel1.Width, 8);
            graphics.DrawImage(Minesnt.Properties.Resources.rightbottom, panel1.Width - 8, panel1.Height - 8);

            // empty counter
            graphics.DrawImage(Minesnt.Properties.Resources.emptycounter, 15, 14);
            graphics.DrawImage(Minesnt.Properties.Resources.emptycounter, panel1.Width - 55, 14);

            // button
            Image buttonImage = Minesnt.Properties.Resources.button;
            if (mousePressed && cellX >= 0)
                buttonImage = Minesnt.Properties.Resources.buttonmine;
            if (gameEnd && cellX >= 0)
                buttonImage = Minesnt.Properties.Resources.losegame;
            if (buttonPressed)
                buttonImage = Minesnt.Properties.Resources.buttonpressed;
            graphics.DrawImage(buttonImage, (panel1.Width / 2) - 14, 14);

            // mines count
            DrawNumber(17, 16, graphics, mineField.MinesCount - mineField.FlagMines);

            // timer
            DrawNumber(panel1.Width - 53, 16, graphics, time);

            // mines
            int startX = 10;
            int startY = 54;

            for (int i = 0; i < mineField.SizeX; i++) {
                for (int j = 0; j < mineField.SizeY; j++) {

                    int mine = mineField.GetMine(i, j);
                    
                    Image mineImage = Minesnt.Properties.Resources.hiddenmine;
                    if (mine > 0)
                        mineImage = mineNumerImages[(mine>8)?8:mine - 1];

                    if ((mine <= -2 || mine >= 9) && gameEnd)
                        mineImage = Minesnt.Properties.Resources.showmine;
                    if (mine == 13 && gameEnd)
                        mineImage = Minesnt.Properties.Resources.showwrongmine;
                    if (mine == -1)
                        mineImage = Minesnt.Properties.Resources.emptymine;

                    if (cellX == i && cellY == j) {
                        if (mine <= 0)
                            mineImage = Minesnt.Properties.Resources.emptymine;
                        if (gameEnd)
                            mineImage = Minesnt.Properties.Resources.showredmine;
                    }
                    
                    graphics.DrawImage(mineImage, startX+(i*16), startY + (j * 16));

                }
            }

        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            panel1.Invalidate();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left)
                CheckCell(e.X, e.Y, true);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                CheckCell(e.X, e.Y, false);
            else
                if(!gameEnd)
                    ToogleFlag(e.X, e.Y);
        }

        private void DrawNumber(int startX, int startY, Graphics graphics, int number) {

            if (number < -99)
                number = -99;
            if (number > 999)
                number = 999;

            string numberString = number.ToString();
            while (numberString.Length < 3) {
                numberString = "0" + numberString;
            }

            for (int i = 0; i < 3; i++) {

                Image numberImage = null;

                if (i == 0 && (number < 0)) {
                    numberImage = numberImages[10];
                } else {
                    if (int.TryParse(numberString[i].ToString(), out int tempNumber)) {
                        numberImage = numberImages[tempNumber];
                    } else {
                        numberImage = numberImages[0];
                    }
                }

                graphics.DrawImage(numberImage, startX + (13*i), startY);
            }
        }

        private void ToogleFlag(int x, int y) {

            int cx = (x - 10) / 16;
            int cy = (y - 54) / 16;
            
            mineField.ToogleFlag(cx, cy);

            panel1.Invalidate();
        }

        private void CheckCell(int x, int y, Boolean mouseDown) {
            mousePressed = mouseDown;
            if (x >= (panel1.Width / 2) - 14 && x <= (panel1.Width / 2) + 14 && y >= 14 && y <= 42) {
                buttonPressed = mouseDown;
                if (mouseDown) {
                    panel1.Invalidate();
                } else {
                    Restart();
                }
                panel1.Invalidate();
                return;
            } else {
                buttonPressed = false;
            }
            if (gameEnd)
                return;
            if (mouseDown) {
                cellX = (x - 10) / 16;
                cellY = (y - 54) / 16;
                if ((x - 10) < 0 || cellX >= mineField.SizeX || (y - 54) < 0 || cellY >= mineField.SizeY) {
                    cellX = -1;
                    cellY = -1;
                }

            } else {
                if (cellX > -1 && cellY > -1) {
                    if (!mineField.IsPlaying()) {
                        mineField.GenerateMines(cellX, cellY);
                        timer1.Start();
                    } else {
                        if (mineField.Reveal(cellX, cellY)) {
                            gameEnd = true;
                            timer1.Stop();
                        }
                    }
                }
                if (!gameEnd) {
                    cellX = -1;
                    cellY = -1;
                }
            }
            panel1.Invalidate();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                if (cellX > -1) {
                    CheckCell(e.X, e.Y, true);
                }
        }

        private void Timer(object sender, EventArgs e) {
            time++;
            panel1.Invalidate();
        }

        public void Restart(int sizeX, int sizeY, int minesCount) {
            mineField.CreateNew(sizeX, sizeY, minesCount);
            this.Width = 16 + 18 + (sizeX * 16);
            this.Height = 66 + 59 + (sizeY * 16);
            Restart();
        }

        public void Restart() {
            mineField.Restart();
            time = 0;
            timer1.Stop();
            cellX = -1;
            cellY = -1;
            gameEnd = false;
            panel1.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            Restart();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void UncheckAllModes() {
            beginnerToolStripMenuItem.Checked = false;
            intermediateToolStripMenuItem.Checked = false;
            expertToolStripMenuItem.Checked = false;
            customToolStripMenuItem.Checked = false;
        }

        private void beginnerToolStripMenuItem_Click(object sender, EventArgs e) {
            UncheckAllModes();
            beginnerToolStripMenuItem.Checked = true;
            Restart(9,9,10);
        }

        private void intermediateToolStripMenuItem_Click(object sender, EventArgs e) {
            UncheckAllModes();
            intermediateToolStripMenuItem.Checked = true;
            Restart(16,16,40);
        }

        private void expertToolStripMenuItem_Click(object sender, EventArgs e) {
            UncheckAllModes();
            expertToolStripMenuItem.Checked = true;
            Restart(30, 16, 99);
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e) {
            UncheckAllModes();
            customToolStripMenuItem.Checked = true;
            using (CustomFieldForm form = new CustomFieldForm(mineField.SizeX, mineField.SizeY, mineField.MinesCount)) {
                if (form.ShowDialog() == DialogResult.OK) {
                    Restart(form.SizeX, form.SizeY, form.Mines);
                }
            }

        }

        private void aboutMinesweeperToolStripMenuItem_Click(object sender, EventArgs e) {
            new About().ShowDialog();
        }
    }
}
