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
        public int x, y;

        public bool visited = false;
        
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Program
    {
        static Random random = new Random();

        static readonly string WALL = "██";
        static readonly string PATH = "  ";

        static int width = 15;
        static int height = 15;

        static Cell[,] cells = new Cell[width,height];

        static void InitiateGrid()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[x, y] = new Cell(x, y);
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

        static int[] GetNextCell(int curr_x, int curr_y)
        {
            int[] next = new int[2];
            try
            {
                if (((curr_x - 1 < 0 || cells[curr_x - 1, curr_y].visited) && (curr_x + 1 > width || cells[curr_x + 1, curr_y].visited)) && ((curr_y - 1 < 0 || cells[curr_x, curr_y - 1].visited) && (curr_y + 1 > height || cells[curr_x, curr_y + 1].visited)))
                {
                    next[0] = 0;
                    next[1] = 0;
                    return next;
                }
            }
            catch
            {
                next[0] = 0;
                next[1] = 0;
                return next;
            }
            int direction = random.Next(0,4);
            switch (direction)
            {
                case 0:
                    next[0] = 0;
                    next[1] = 1;
                    break;
                case 1:
                    next[0] = 1;
                    next[1] = 0;
                    break;
                case 2:
                    next[0] = 0;
                    next[1] = -1;
                    break;
                case 3:
                    next[0] = -1;
                    next[1] = 0;
                    break;
            }
            if (curr_x + next[0] < 0 || curr_x + next[0] > width-1 || curr_y - next[1] < 0 || curr_y - next[1] > height-1 || cells[curr_x + next[0], curr_y - next[1]].visited)
                return GetNextCell(curr_x, curr_y);
            else
                return next;
        }

        static void GeneratePath(int x = 0, int y = 0)
        {
            cells[x, y].visited = true;
            int[] nextCell = GetNextCell(x, y);
            if (nextCell[1] == 1)
            {
                cells[x, y].walls[2] = false;
                cells[x, y - nextCell[1]].walls[0] = false;
                GeneratePath(x, y - nextCell[1]);
            }
            nextCell = GetNextCell(x, y);
            if (nextCell[1] == -1)
            {
                cells[x,y].walls[2] = false;
                cells[x,y - nextCell[1]].walls[0] = false;
                GeneratePath(x, y - nextCell[1]);
            }
            nextCell = GetNextCell(x, y);
            if (nextCell[0] == 1)
            {
                cells[x, y].walls[1] = false;
                cells[x + nextCell[0], y].walls[3] = false;
                GeneratePath(x + nextCell[0], y);
            }
            nextCell = GetNextCell(x, y);
            if (nextCell[0] == 1)
            {
                cells[x,y].walls[0] = false;
                cells[x + nextCell[0], y].walls[1] = false;
                GeneratePath(x + nextCell[0], y);
            }
            if (nextCell[0] == 0 && nextCell[1] == 0)
                return;
             
            PrintGrid();
        }

        static void Main(string[] args)
        {
            InitiateGrid();
            GeneratePath();
        }
    }
}
