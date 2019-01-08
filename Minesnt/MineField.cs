using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesnt {

    class MineField {

        MineTemplate[] mineTemplates;
        private int[,] mines;
        private int[,] fakeMines = new int[2,2];
        private int flagMines = 0;
        public int FlagMines {
            get {
                return flagMines;
            }
        }
        private int minesCount = 0;
        public int MinesCount {
            get {
                return minesCount;
            }
        }
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
        private Boolean playing;

        public MineField() {
            mineTemplates = MineTemplate.LoadTemplates();
        }

        public void CreateNew(int sizeX, int sizeY, int minesCount) {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.minesCount = minesCount;
            Restart();
        }

        public void Restart() {
            flagMines = 0;
            playing = false;
            mines = new int[sizeX, sizeY];
        }

        public Boolean IsPlaying() {
            return playing;
        }

        public void GenerateMines(int startX, int startY) {

            MineTemplate mineTemp = GetRandomMineTemplate(startX, startY);

            for (int i = 0; i < mineTemp.sizeX; i++) {
                for (int j = 0; j < mineTemp.sizeY; j++) {
                    mines[mineTemp.startX + i, mineTemp.startY + j] = mineTemp.mines[i, j];
                    if (mineTemp.mines[i, j] < -2) {
                        fakeMines[mineTemp.mines[i, j] + 4,0] = mineTemp.startX + i;
                        fakeMines[mineTemp.mines[i, j] + 4,1] = mineTemp.startY + j;
                    }
                }
            }

            mines[startX, startY] = -1;
            playing = true;
            Random rnd = new Random();
            int minesTemp = minesCount - mineTemp.minesCount;
            while (minesTemp > 0) {
                int x = rnd.Next(0, sizeX);
                int y = rnd.Next(0, sizeY);

                if (mines[x, y] == 0) {
                    mines[x, y] = -2;
                    minesTemp--;
                }
            }
            
            Reveal(startX, startY);
        }

        private MineTemplate GetRandomMineTemplate(int startX, int startY) {

            Random random = new Random();
            MineTemplate result;

            while (true) {
                result = mineTemplates[random.Next(mineTemplates.Length)];
                int x = (result.horizontalLock == 0) ? 0 : (result.horizontalLock == 2) ? sizeX - result.sizeX : random.Next(sizeX - result.sizeX);
                int y = (result.verticalLock == 0) ? 0 : (result.verticalLock == 2) ? sizeY - result.sizeY : random.Next(sizeY - result.sizeY);
                result.startX = x;
                result.startY = y;

                if (startX >= x && startX <= x + result.sizeX && startY >= y && startY <= y + result.sizeY)
                    continue;

                break;
            }


            return result;
        }

        public int GetMine(int i, int j) {
            return mines[i, j];
        }

        public Boolean Reveal(int x, int y) {

            if(mines[x, y] < -1) {

                int minaId = 0;
                if (mines[x, y] < -2)
                    minaId = (mines[x, y] == -4) ? 1 : 0;
                if (mines[fakeMines[minaId, 0], fakeMines[minaId, 1]] < -2)
                    mines[fakeMines[minaId, 0], fakeMines[minaId, 1]] = 0;
                else
                    mines[fakeMines[minaId, 0], fakeMines[minaId, 1]] = 13;

                return true;
            }

            if (mines[x, y] >= 9) {
                return false;
            }

            int minesCount = 0;
            int thisMine = mines[x, y];
            
            for (int i = x - 1; i <= x + 1; i++) {
                for (int j = y - 1; j <= y + 1; j++) {

                    if (i < 0 || j < 0 || i >= sizeX || j >= sizeY)
                        continue;

                    else if (mines[i, j] > -4 && mines[i, j] <= -2 || mines[i, j] >= 9 && mines[i, j] < 13)
                        minesCount++;

                }
            }

            mines[x, y] = (minesCount == 0) ? -1 : minesCount;

            if (minesCount == 0) {
                for (int i = x - 1; i <= x + 1; i++) {
                    for (int j = y - 1; j <= y + 1; j++) {

                        if (i < 0 || j < 0 || i >= sizeX || j >= sizeY)
                            continue;

                        if (mines[i, j] == 0)
                            Reveal(i, j);

                    }
                }
            }

            return false;
        }

        public void ToogleFlag(int x, int y) {
            if (x < 0 || y < 0 || x >= sizeX || y >= sizeY)
                return;

            if (mines[x, y] <= 0) {
                mines[x, y] += 13;
                flagMines++;
            } else if (mines[x, y] > 8) {
                mines[x, y] += -13;
                flagMines--;
            }
                
        }
    }
}
