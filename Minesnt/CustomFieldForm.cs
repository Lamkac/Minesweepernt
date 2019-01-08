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
    public partial class CustomFieldForm : Form {

        private int minSize = 9;
        private int maxSizeX = 30;
        private int maxSizeY = 24;

        private int minMines = 10;

        private int defaultSizeX;
        private int defaultSizeY;
        private int defaultMinesCount;

        private int sizeX;
        public int SizeX {
            get {
                return sizeX;
            }
        }
        private int sizeY;
        public int SizeY {
            get {
                return sizeY;
            }
        }
        private int mines;
        public int Mines {
            get {
                return mines;
            }
        }

        public CustomFieldForm(int sizeX, int sizeY, int minesCount) {
            InitializeComponent();
            defaultSizeX = sizeX;
            textBoxWidth.Text = sizeX.ToString();
            defaultSizeY = sizeY;
            textBoxHeight.Text = sizeY.ToString();
            defaultMinesCount = minesCount;
            textBoxMines.Text = minesCount.ToString();
        }

        private void buttonOk_Click(object sender, EventArgs e) {

            sizeX = CheckNumber(textBoxWidth.Text, defaultSizeX, minSize, maxSizeX);
            sizeY = CheckNumber(textBoxHeight.Text, defaultSizeY, minSize, maxSizeY);
            int max = (sizeX - 1) * (sizeY - 1);
            mines = CheckNumber(textBoxMines.Text, defaultMinesCount, minMines, max);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private int CheckNumber(String numberString,int numberDefault, int min, int max) {

            if (int.TryParse(numberString, out int number)) {
                return CheckLimits(number,min,max);
            } else {
                return CheckLimits(numberDefault, min, max);
            }
        }

        private int CheckLimits(int number, int min, int max) {
            if (number < min)
                return min;
            if (number > max)
                return max;
            return number;
        }
    }
}
