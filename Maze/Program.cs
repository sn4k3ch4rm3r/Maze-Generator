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
        //█
        static readonly string wall = "█";
        static readonly string path = " ";
        static readonly string solution = "x";

        static int width = 5;//78;
        static int height = 5;//19;

        static int[] start = { 0, 0 };
        static int[] finish = { width-1, height-1 };

        static Cell[,] cells = new Cell[width,height];

        static bool[,] maze = new bool[width*2+1, height*2+1];

        static void InitiateGrid()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[x, y] = new Cell();
                }
            }
            CursorVisible = false;
            //PrintGrid();
        }

        static void PrintGrid()
        {
            int w = wall.Length;
            int cursor_x = w;
            int cursor_y = 1;
            for (int y = 0; y < height; y++)
            {
                SetCursorPosition(cursor_x, cursor_y);
                for (int x = 0; x < width; x++)
                {
                    SetCursorPosition(cursor_x - w, cursor_y - 1);
                    Write(wall);
                    SetCursorPosition(cursor_x, cursor_y - 1);
                    if (cells[x, y].walls[0])
                        Write(wall);
                    else
                        Write(path);
                    SetCursorPosition(cursor_x - w, cursor_y);
                    if (cells[x, y].walls[3])
                        Write(wall);
                    else
                        Write(path);
                    Write(path);
                    cursor_x += 2*w;
                }
                SetCursorPosition(cursor_x - w, cursor_y-1);
                Write(wall);
                SetCursorPosition(cursor_x-w, cursor_y);
                Write(wall);
                cursor_y+=2;
                cursor_x = w;
            }
            SetCursorPosition(start[0] * wall.Length*2+wall.Length, start[1]*2+1);
            Write("S");
            SetCursorPosition(finish[0] * wall.Length*2+wall.Length, finish[1]*2+1);
            Write("F");
            SetCursorPosition(0, cursor_y-1);
            for (int i = 0; i < width*2+1; i++)
            {
                Write(wall);
            }
            WriteLine();
        }

        static void ConvertCellsToMaze()
        {
            for (int x = 0; x < width*2; x++)
            {
                for (int y = 1; y < height*2; y+=2)
                {
                    maze[y-1, x] = cells[(y-1)/2, x / 2].walls[0];
                    maze[y,x] = cells[y / 2 , x / 2].walls[1];
                }
            }
            WriteLine();
            for (int i = 0; i < height*2+1; i++)
            {
                for (int j = 0; j < width*2+1; j++)
                {
                    Write(maze[i,j] ? path : wall);
                }
                WriteLine();
            }
            return;
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
            //PrintGrid();
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
            ConvertCellsToMaze();
        }
    }
}
