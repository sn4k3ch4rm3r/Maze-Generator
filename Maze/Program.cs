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
        static readonly string solution = "█";

        static int width = 90;
        static int height = 20;

        static int[] start = { 0, 0 };
        static int[] finish = { width-1, height-1 };

        static Cell[,] cells = new Cell[width,height];

        static bool[,] maze = new bool[width * 2 + 1, height * 2 + 1];
        static int[,] distance = new int[width * 2 + 1, height * 2 + 1];

        static void InitiateGrid()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[x, y] = new Cell();
                }
            }
        }

        static void PrintGrid()
        {
            ConvertCellsToMaze();
            SetCursorPosition(0, 0);
            for (int i = 0; i < height * 2 + 1; i++)
            {
                for (int j = 0; j < width * 2 + 1; j++)
                {
                    Write(maze[j, i] ? path : wall);
                }
                WriteLine();
            }
        }

        static void ConvertCellsToMaze()
        {
            for (int x = 1; x < width*2; x+=2)
            {
                for (int y = 1; y < height*2; y+=2)
                {
                    maze[x+1, y] = !cells[x / 2, y / 2].walls[1];
                    maze[x, y-1] = !cells[x / 2, y / 2].walls[0];
                    maze[x, y] = true;
                }
            }
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

        static void Flood(bool[,] m, int x, int y, int dist)
        {
            if (x < 0 || x > width*2-1 || y < 0 || y > height*2-1 || !m[x,y])
            {
                if (!maze[x, y])
                {
                    distance[x, y] = -1;
                }
                return;
            }
            m[x, y] = false;
            distance[x, y] = dist;
            Flood(m, x + 1, y, dist + 1);
            Flood(m, x - 1, y, dist + 1);
            Flood(m, x, y + 1, dist + 1);
            Flood(m, x, y - 1, dist + 1);
        }

        static void SolveFlood(int x, int y)
        {
            SetCursorPosition(x * wall.Length, y);
            Write(solution);
            if (distance[x, y] == 0)
                return;

            if (distance[x-1, y] < distance[x, y] && distance[x-1, y] != -1)
                SolveFlood(x - 1, y);
            else if (distance[x + 1, y] < distance[x, y] && distance[x+1, y] != -1)
                SolveFlood(x + 1, y);
            else if (distance[x, y-1] < distance[x, y] && distance[x, y-1] != -1)
                SolveFlood(x, y-1);
            else if (distance[x, y+1] < distance[x, y] && distance[x, y+1] != -1)
                SolveFlood(x, y+1);
        }

        static void Main(string[] args)
        {
            WindowWidth = width * 2 * wall.Length+2;
            WindowHeight = height * 2 +2;
            CursorVisible = false;
            InitiateGrid();
            GeneratePath();
            PrintGrid();
            ConvertCellsToMaze();
            bool[,] cp = new bool[width*2+1, height*2+1];
            for (int i = 0; i < width*2+1; i++)
            {
                for (int j = 0; j < height*2+1; j++)
                {
                    cp[i, j] = maze[i,j];
                }
            }
            Flood(cp, finish[0]* 2+1, finish[1] * 2+1, 0);
            ForegroundColor = ConsoleColor.DarkRed;
            SolveFlood(start[0]+1, start[1]+1);
            ResetColor();
            BackgroundColor = ConsoleColor.DarkRed;
            ForegroundColor = ConsoleColor.White;
            SetCursorPosition(start[0] * wall.Length * 2 + wall.Length, start[1] * 2 + 1);
            Write("S");
            SetCursorPosition(finish[0] * wall.Length * 2 + wall.Length, finish[1] * 2 + 1);
            Write("F");
            SetCursorPosition(0, height * 2 + 1);
            ResetColor();
            WriteLine(distance[start[0]+1, start[1]+1] + " steps");
            /*
            for (int i = 0; i < width*2+1; i++)
            {
                Write("[ ");
                for (int j = 0; j < height*2+1; j++)
                {
                    Write(Convert.ToString(distance[j, i]).Length < 2? " " + distance[j, i] + " " : distance[j, i] + " ");
                }
                Write("]\n");
            }*/
        }
    }
}
