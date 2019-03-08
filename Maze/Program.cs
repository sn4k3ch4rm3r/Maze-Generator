using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;

namespace Maze
{

    class Cell
    {
        public bool[] walls = { true, true, true, true};
        public bool visited = false;
    }

    class Program
    {
        static Random random = new Random();

        static readonly string WALL = "██";
        static readonly string PATH = "  ";

        static int width = 25;
        static int height = 15;

        static Cell[,] cells = new Cell[width,height];

        static void InitiateGrid()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[x, y] = new Cell();
                }
            }
            PrintGrid();
        }

        static void PrintGrid()
        {
            int cursor_x = 2;
            int cursor_y = 1;
            for (int y = 0; y < height; y++)
            {
                SetCursorPosition(cursor_x, cursor_y);
                for (int x = 0; x < width; x++)
                {
                    SetCursorPosition(cursor_x - 2, cursor_y - 1);
                    Write(WALL);
                    SetCursorPosition(cursor_x, cursor_y - 1);
                    if (cells[x, y].walls[0])
                        Write(WALL);
                    else
                        Write(PATH);
                    SetCursorPosition(cursor_x - 2, cursor_y);
                    if (cells[x, y].walls[3])
                        Write(WALL);
                    else
                        Write(PATH);
                    cursor_x += 4;
                }
                SetCursorPosition(cursor_x - 2, cursor_y-1);
                Write(WALL);
                SetCursorPosition(cursor_x-2, cursor_y);
                Write(WALL);
                cursor_y+=2;
                cursor_x = 2;
            }
            SetCursorPosition(0, cursor_y-1);
            for (int i = 0; i < width*2+1; i++)
            {
                Write(WALL);
            }
            WriteLine();
        }

        static int[] GetNextCells()
        {
            int[] order = { 0, 1, 2, 3 };
            int n = 3;
            while (n > 0)
            {
                int k = random.Next(0,n);
                int temp = order[n];
                order[n] = order[k];
                order[k] = temp;
                n--;
            }
            return order;   
        }

        static void GeneratePath(int x = 0, int y = 0)
        {
            if (x < 0 || x > width || y < 0 || y > height || cells[x, y].visited)
                return;
            cells[x, y].visited = true;
            PrintGrid();
            WriteLine(x + " " + y);
            int[] order = GetNextCells();
            foreach (var next in order)
            {
                switch(next) {
                    case 0:
                        if (y - 1 >= 0 && !cells[x, y-1].visited)
                        {
                            cells[x, y].walls[0] = false;
                            cells[x, y - 1].walls[2] = false;
                            GeneratePath(x, y - 1);
                        }
                        break;
                    case 1:
                        if (x + 1 < width && !cells[x + 1, y].visited)
                        {
                            cells[x, y].walls[1] = false;
                            cells[x + 1, y].walls[3] = false;
                            GeneratePath(x + 1, y);
                        }
                        break;
                    case 2:
                        if (y + 1 < height && !cells[x, y+1].visited)
                        {
                            cells[x, y].walls[2] = false;
                            cells[x, y + 1].walls[0] = false;
                            GeneratePath(x, y + 1);
                        }
                        break;
                    case 3:
                        if (x - 1 >= 0 && !cells[x-1, y].visited)
                        {
                            cells[x, y].walls[3] = false;
                            cells[x - 1, y].walls[1] = false;
                            GeneratePath(x - 1, y);
                        }
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            InitiateGrid();
            GeneratePath();
            PrintGrid();
        }
    }
}
