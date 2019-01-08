using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesnt {
    class MineTemplate {
        public int horizontalLock;
        public int verticalLock;

        public int[,] mines;
        public int minesCount;

        public int sizeX;
        public int sizeY;
        
        public int startX;
        public int startY;

        // TODO load from file mines.txt lol??? Im too tired and drunk so idc
        // also this program is just for reddit so who care
        public static MineTemplate[] LoadTemplates() {
            MineTemplate[] mines = new MineTemplate[4];

            mines[0] = new MineTemplate {
                horizontalLock = 2,
                verticalLock = 2,
                mines = new int[,] { { -2, 0, 0 }, { -2, -3, -4 } },
                minesCount = 3,
                sizeX = 2,
                sizeY = 3
            };

            mines[1] = new MineTemplate {
                horizontalLock = 0,
                verticalLock = 0,
                mines = new int[,] { { -4, -3, -2 }, { 0, 0, -2 } },
                minesCount = 3,
                sizeX = 2,
                sizeY = 3
            };

            mines[2] = new MineTemplate {
                horizontalLock = 0,
                verticalLock = 2,
                mines = new int[,] { { 0, -4 }, { 0, -3 }, { -2, -2 } },
                minesCount = 3,
                sizeX = 3,
                sizeY = 2
            };

            mines[3] = new MineTemplate {
                horizontalLock = 2,
                verticalLock = 0,
                mines = new int[,] { { -2, -2 }, { -3, 0 }, { -4, 0 } },
                minesCount = 3,
                sizeX = 3,
                sizeY = 2
            };

            return mines;

        }
    }
}
