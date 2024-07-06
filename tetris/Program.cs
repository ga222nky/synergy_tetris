using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventProject
{
    delegate void Up();
    delegate void Down();
    delegate void Left();
    delegate void Right();

    class EventUp
    {
        public event Up UpEvent;

        public void UpUserEvent()
        {
            UpEvent();
        }
    }


    class EventDown
    {

        public event Down DownEvent;


        public void DownUserEvent()
        {
            DownEvent();
        }
    }

    class EventLeft
    {

        public event Left LeftEvent;


        public void LeftUserEvent()
        {
            LeftEvent();
        }
    }

    class EventRight
    {

        public event Right RightEvent;

        public void RightUserEvent()
        {
            RightEvent();
        }
    }

    public enum FigType { line, square, rightL, leftL, pyramide, leftZ, rightZ };


    class Figura
    {
        public bool[,] matrix = new bool[4, 4];
        public FigType type;
        public int position;



        public void Clear(bool[,] m)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    m[i, j] = false;
        }


        public void Create(FigType figtype)
        {
            Clear(matrix);
            this.type = figtype;
            this.position = 1;
            switch (figtype)
            {
                case FigType.line:
                    {
                        for (int i = 0; i < 4; i++)
                            matrix[0, i] = true;
                        break;
                    }

                case FigType.square:
                    {
                        for (int i = 0; i < 2; i++)
                            for (int j = 0; j < 2; j++)
                                matrix[i, j] = true;
                        break;
                    }

                case FigType.leftL:
                    {
                        for (int i = 0; i < 3; i++)
                            matrix[0, i] = true;
                        matrix[1, 2] = true;
                        break;
                    }

                case FigType.rightL:
                    {
                        for (int i = 0; i < 3; i++)
                            matrix[0, i] = true;
                        matrix[1, 0] = true;
                        break;
                    }

                case FigType.pyramide:
                    {
                        for (int i = 0; i < 3; i++)
                            matrix[1, i] = true;
                        matrix[0, 1] = true;
                        break;
                    }

                case FigType.leftZ:
                    {
                        matrix[0, 0] = true; matrix[1, 0] = true;
                        matrix[1, 1] = true; matrix[2, 1] = true;
                        break;
                    }

                case FigType.rightZ:
                    {
                        matrix[0, 1] = true; matrix[1, 0] = true;
                        matrix[1, 1] = true; matrix[2, 0] = true;
                        break;
                    }
            }

        }


        public void Rotate()
        {
            if (this.position == 4) this.position = 1;
            this.position++;

            switch (type)
            {
                case FigType.line:
                    {
                        int k;
                        if (matrix[0, 0] == true)
                        {
                            Clear(matrix);
                            for (k = 0; k < 4; k++)
                                matrix[k, 1] = true;
                        }
                        else
                        {
                            Clear(matrix);
                            for (k = 0; k < 4; k++)
                                matrix[0, k] = true;
                        }
                        break;
                    }

                case FigType.square:
                    {
                        return;
                    }

                default:
                    {
                        bool[,] tempFig = new bool[4, 4];
                        Clear(tempFig);

                        for (int j = 3 - 1, c = 0; j >= 0; j--, c++)
                            for (int i = 0; i < 3; i++)
                                tempFig[c, i] = matrix[i, j];

                        Clear(matrix);

                        for (int f = 0; f < 3; f++)
                            for (int d = 0; d < 3; d++)
                                matrix[f, d] = tempFig[f, d];
                        break;
                    }
            }

        }


    }


    class Field
    {
        public Figura fig = new Figura();
        int width; 
        int height;
        static bool[,] tetrisField;
        int curY;
        int curX;
        public int scores;
        public int level;


        public Field(int w, int h)
        {
            this.width = w;
            this.height = h;
            tetrisField = new bool[height, width];
            level = 0;
            scores = 0;
        }

        public void DrawField()
        {

            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    Console.CursorLeft = j;
                    Console.CursorTop = i;
                    if (tetrisField[i, j] == false) Console.WriteLine(" ");
                    else Console.WriteLine("#");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n   Уровень " + this.level);
            Console.WriteLine("\n  Очки " + this.scores);
        }

        public void Copy()
        {
            int x = curX;
            int y = curY;

            for (int i = 0; i < 4; i++)
            {
                x = curX;

                for (int j = 0; j < 4; j++)
                {
                    if (fig.matrix[i, j] == true) tetrisField[y, x] = true;
                    x++;
                }
                y++;
            }
        }

        public void NewFig()
        {
            Random r = new Random();
            curY = 0;
            curX = 5;

            FigType t = (FigType)r.Next(0, 7);
            fig.Create(t);

            this.Copy();         

        }

        public void Move()
        {
            this.ClearPrevious();
            curY++;
            this.Copy();
            this.DrawField();

        }

        public void ClearPrevious()
        {
            int m = 0;
            int n = 0;

            for (int i = curY; i < curY + 4; i++)
            {
                for (int j = curX; j < curX + 4; j++)
                {
                    if (fig.matrix[m, n] == true) tetrisField[i, j] = false;
                    n++;
                }
                m++;
                n = 0;
            }

        }

        public bool CheckRotation()
        {

            return false;
        }

        public bool CheckLeft()
        {
            switch (fig.type)
            {
                case FigType.line:
                    {
                        if (fig.position == 1 || fig.position == 3)
                        {
                            if (tetrisField[curY, curX - 1] == true || curX == 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2 || fig.position == 4)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (tetrisField[curY + i, curX] || curX == 0) return false;
                                }
                                return true;
                            }
                        }
                        break;
                    }

                case FigType.square:
                    {
                        if (tetrisField[curY, curX - 1] == true || tetrisField[curY + 1, curX - 1] == true || curX == 1) return false;
                        else return true;
                    }

                case FigType.rightL:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX - 1] == true || tetrisField[curY + 1, curX - 1] == true || curX == 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (tetrisField[curY + i, curX - 1] == true || curX == 1) return false;
                                }
                                return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 2, curX - 1] == true || tetrisField[curY + 1, curX + 1] == true || curX == 1) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY, curX] == true || tetrisField[curY + 1, curX + 1] == true || tetrisField[curY + 2, curX + 1] || curX == 0) return false;
                                else return true;
                            }

                        }
                        break;
                    }

                case FigType.leftL:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX - 1] == true || tetrisField[curY + 1, curX + 1] == true || curX == 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (tetrisField[curY + i, curX - 1] == true || curX == 1) return false;
                                }
                                return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 1, curX - 1] == true || tetrisField[curY + 2, curX - 1] == true || curX == 1) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY + 2, curX] == true || tetrisField[curY + 1, curX + 1] == true || tetrisField[curY, curX + 1] == true || curX == 0) return false;
                                else return true;
                            }

                        }
                        break;
                    }

                case FigType.pyramide:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX] == true || tetrisField[curY + 1, curX - 1] == true || curX == 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY, curX] == true || tetrisField[curY + 1, curX - 1] == true || tetrisField[curY + 2, curX] == true || curX == 1) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 1, curX - 1] == true || tetrisField[curY + 2, curX] == true || curX == 1) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (tetrisField[curY + i, curX] == true || curX == 0) return false;
                                }
                                return true;
                            }


                        }

                        break;
                    }

                case FigType.leftZ:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX - 1] == true || tetrisField[curY + 1, curX - 1] == true || tetrisField[curY + 2, curX] == true || curX == 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 1, curX] == true || tetrisField[curY + 2, curX - 1] == true || curX == 1) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY, curX] == true || tetrisField[curY + 1, curX] == true || tetrisField[curY + 2, curX + 1] == true || curX == 0) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY, curX] == true || tetrisField[curY + 1, curX - 1] == true || curX == 1) return false;
                                else return true;
                            }

                        }


                        break;
                    }

                case FigType.rightZ:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX] == true || tetrisField[curY + 1, curX - 1] == true || tetrisField[curY + 2, curX - 1] == true || curX == 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 1, curX - 1] == true || tetrisField[curY + 2, curX] == true || curX == 1) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY, curX + 1] == true || tetrisField[curY + 1, curX] == true || tetrisField[curY + 2, curX] == true || curX == 0) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY, curX - 1] == true || tetrisField[curY + 1, curX] == true || curX == 1) return false;
                                else return true;
                            }

                        }
                        break;
                    }
            }

            return false;

        }

        public bool CheckRight()
        {
            switch (fig.type)
            {
                case FigType.line:
                    {
                        if (fig.position == 1 || fig.position == 3)
                        {
                            if (tetrisField[curY, curX + 4] == true || curX == this.width - 5) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2 || fig.position == 4)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (tetrisField[curY + i, curX + 2] || curX == this.width - 3) return false;
                                }
                                return true;
                            }
                        }
                        break;
                    }

                case FigType.square:
                    {
                        if (tetrisField[curY, curX + 2] == true || tetrisField[curY + 1, curX + 2] == true || curX == this.width - 3) return false;
                        else return true;
                    }

                case FigType.rightL:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX + 3] == true || tetrisField[curY + 1, curX + 1] == true || curX == this.width - 4) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY, curX + 1] == true || tetrisField[curY + 1, curX + 1] == true || tetrisField[curY + 2, curX + 2] || curX == this.width - 3) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 1, curX + 3] == true || tetrisField[curY + 2, curX + 3] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY, curX + 3] == true || tetrisField[curY + 1, curX + 3] == true || tetrisField[curY + 2, curX + 3] || curX == this.width - 4) return false;
                                else return true;
                            }

                        }
                        break;
                    }

                case FigType.leftL:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX + 3] == true || tetrisField[curY + 1, curX + 3] == true || curX == this.width - 4) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY, curX + 2] == true || tetrisField[curY + 1, curX + 1] == true || tetrisField[curY + 2, curX + 1] == true || curX == this.width - 3) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 1, curX + 1] == true || tetrisField[curY + 2, curX + 3] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY, curX + 3] == true || tetrisField[curY + 1, curX + 3] == true || tetrisField[curY + 2, curX + 3] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                        }
                        break;
                    }

                case FigType.pyramide:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX + 2] == true || tetrisField[curY + 1, curX + 3] == true || curX == this.width - 4) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY, curX + 2] == true || tetrisField[curY + 1, curX + 2] == true || tetrisField[curY + 2, curX + 2] == true || curX == this.width - 3) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 1, curX + 3] == true || tetrisField[curY + 2, curX + 2] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY, curX + 2] == true || tetrisField[curY + 1, curX + 3] == true || tetrisField[curY + 2, curX + 2] == true || curX == this.width - 4) return false;
                                else return true;
                            }


                        }

                        break;
                    }

                case FigType.leftZ:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX + 1] == true || tetrisField[curY + 1, curX + 2] == true || tetrisField[curY + 2, curX + 2] == true || curX == this.width - 3) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 1, curX + 3] == true || tetrisField[curY + 2, curX + 2] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY, curX + 2] == true || tetrisField[curY + 1, curX + 3] == true || tetrisField[curY + 2, curX + 3] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY, curX + 3] == true || tetrisField[curY + 1, curX + 2] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                        }


                        break;
                    }

                case FigType.rightZ:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY, curX + 2] == true || tetrisField[curY + 1, curX + 2] == true || tetrisField[curY + 2, curX + 1] == true || curX == this.width - 3) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 1, curX + 2] == true || tetrisField[curY + 2, curX + 3] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY, curX + 3] == true || tetrisField[curY + 1, curX + 3] == true || tetrisField[curY + 2, curX + 2] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY, curX + 2] == true || tetrisField[curY + 1, curX + 3] == true || curX == this.width - 4) return false;
                                else return true;
                            }

                        }
                        break;
                    }
            }

            return false;



        }

        public bool CheckDown()
        {
            switch (fig.type)
            {
                case FigType.line:
                    {
                        if (fig.position == 1 || fig.position == 3)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (tetrisField[curY + 1, curX + i] == true || (curY + 1) == this.height - 1) return false;
                            }
                            return true;

                        }

                        else
                        {
                            if (fig.position == 2 || fig.position == 4)
                            {
                                if (tetrisField[curY + 4, curX + 1] == true || (curY + 4) == this.height - 1) return false;
                            }
                            return true;
                        }
                    }

                case FigType.square:
                    {
                        if (tetrisField[curY + 2, curX] == true || tetrisField[curY + 2, curX + 1] == true || (curY + 2) == this.height - 1) return false;
                        return true;
                    }

                case FigType.rightL:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY + 2, curX] == true || tetrisField[curY + 1, curX + 1] == true || tetrisField[curY + 1, curX + 2] || (curY + 2) == this.height - 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 3, curX] == true || tetrisField[curY + 3, curX + 1] == true || (curY + 3) == this.height - 1) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (tetrisField[curY + 3, curX + i] || (curY + 3) == this.height - 1) return false;
                                }
                                return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY + 1, curX + 1] == true || tetrisField[curY + 3, curX + 2] || (curY + 3) == this.height - 1) return false;
                                else return true;
                            }
                        }

                        break;
                    }

                case FigType.leftL:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY + 1, curX] == true || tetrisField[curY + 1, curX + 1] == true || tetrisField[curY + 2, curX + 2] || (curY + 2) == this.height - 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 3, curX] == true || tetrisField[curY + 1, curX + 1] || curY + 3 == this.height - 1) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (tetrisField[curY + 3, curX + i] == true || (curY + 3) == this.height - 1) return false;
                                }
                                return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY + 3, curX + 1] == true || tetrisField[curY + 3, curX + 2] == true || (curY + 3) == this.height - 1) return false;
                                else return true;
                            }

                        }
                        break;
                    }

                case FigType.pyramide:
                    {
                        if (fig.position == 1)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (tetrisField[curY + 2, curX + i] == true || (curY + 2) == this.height - 1) return false;
                            }
                            return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 2, curX] == true || tetrisField[curY + 3, curX + 1] == true || (curY + 3) == this.height - 1) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 2, curX] == true || tetrisField[curY + 3, curX + 1] == true || tetrisField[curY + 2, curX + 2] == true || (curY + 3) == this.height - 1) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY + 3, curX + 1] == true || tetrisField[curY + 2, curX + 2] == true || (curY + 3) == this.height - 1) return false;
                                else return true;
                            }

                        }

                        break;
                    }

                case FigType.leftZ:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY + 2, curX] == true || tetrisField[curY + 3, curX + 1] == true || (curY + 3) == this.height - 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 3, curX] == true || tetrisField[curY + 3, curX + 1] == true || tetrisField[curY + 2, curX + 2] == true || (curY + 3) == this.height - 1) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 2, curX + 1] == true || tetrisField[curY + 3, curX + 2] == true || curY + 3 == this.height - 1) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY + 2, curX] == true || tetrisField[curY + 2, curX + 1] == true || tetrisField[curY + 1, curX + 2] == true || curY + 2 == this.height - 1) return false;
                                else return true;
                            }

                        }
                        break;
                    }

                case FigType.rightZ:
                    {
                        if (fig.position == 1)
                        {
                            if (tetrisField[curY + 3, curX] == true || tetrisField[curY + 2, curX + 1] == true || curY + 3 == this.height - 1) return false;
                            else return true;
                        }

                        else
                        {
                            if (fig.position == 2)
                            {
                                if (tetrisField[curY + 2, curX] == true || tetrisField[curY + 3, curX + 1] == true || tetrisField[curY + 3, curX + 2] == true || curY + 3 == this.height - 1) return false;
                                else return true;
                            }

                            if (fig.position == 3)
                            {
                                if (tetrisField[curY + 3, curX + 1] == true || tetrisField[curY + 2, curX + 2] == true || curY + 3 == this.height - 1) return false;
                                else return true;
                            }

                            if (fig.position == 4)
                            {
                                if (tetrisField[curY + 1, curX] == true || tetrisField[curY + 2, curX + 1] == true || tetrisField[curY + 2, curX + 2] == true || curY + 2 == this.height - 1) return false;
                                else return true;
                            }

                        }

                        break;
                    }

                default:
                    {
                        return false;
                    }

            }
            return false;


        }
        public bool IsAtBottom()
        {
            switch (fig.type)
            {
                case FigType.line:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (tetrisField[1, curX + i] == true) return true;
                        }

                        break;
                    }

                case FigType.square:
                    {
                        if (tetrisField[2, curX] == true || tetrisField[2, curX + 1] == true) return true;
                        break;
                    }

                case FigType.rightL:
                    {
                        if (tetrisField[2, curX] == true || tetrisField[1, curX + 1] == true || tetrisField[1, curX + 2] == true) return true;
                        break;
                    }

                case FigType.leftL:
                    {
                        if (tetrisField[1, curX] == true || tetrisField[1, curX + 1] == true || tetrisField[2, curX + 2]) return true;
                        break;
                    }

                case FigType.pyramide:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (tetrisField[2, curX + i] == true) return true;
                        }
                        break;
                    }

                case FigType.leftZ:
                    {
                        if (tetrisField[2, curX] == true || tetrisField[3, curX + 1] == true) return true;
                        break;
                    }

                case FigType.rightZ:
                    {
                        if (tetrisField[3, curX] == true || tetrisField[2, curX + 1] == true) return true;
                        break;
                    }
            }

            return false;
        }

        public bool CheckLine()
        {
            int counter = 0;
            int k = 0;

            for (int i = 0; i < height; i++)
            {
                counter = 0;
                for (int j = 0; j < width; j++)
                {
                    if (tetrisField[i, j] == true) counter++;
                    if (counter == 10)
                    {
                        k = i;
                        break;
                    }
                }
            }

            if (k == 0) return false;

            else
            {
                for (int i = 0; i < width; i++)
                {
                    tetrisField[k, i] = false;
                }

                for (int i = k; i > 0; i--)
                {
                    for (int j = 0; j < width; j++)
                    {
                        tetrisField[i, j] = tetrisField[i - 1, j];
                    }
                }
                this.scores += 100;
                if (scores == 1000)
                {
                    level++;
                    scores = 0;
                }
                return true;
            }

        }

        public void UpFig()
        {
            this.ClearPrevious();
            fig.Rotate();
            this.Copy();
        }

        public void DownFig()
        {
            while (this.CheckDown() == true) this.Move();
        }

        public void LeftFig()
        {
            if (CheckLeft() == true)
            {
                this.ClearPrevious();
                curX--;
                this.Copy();
            }
            else return;
        }

        public void RightFig()
        {
            if (CheckRight() == true)
            {
                this.ClearPrevious();
                curX++;
                this.Copy();
            }
            else return;
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < 20; i++)
            {

                Console.CursorLeft = 0;
                Console.CursorTop = i;
                Console.WriteLine("#");
            }

            for (int i = 1; i < 12; i++)
            {
                Console.CursorLeft = i;
                Console.CursorTop = 19;
                Console.WriteLine("#");
            }

            for (int i = 0; i < 20; i++)
            {
                Console.CursorLeft = 11;
                Console.CursorTop = i;
                Console.WriteLine("#");
            }

            Console.BackgroundColor = ConsoleColor.Blue;

            Field f = new Field(12, 20);

            f.NewFig();

            f.DrawField();


            EventUp up = new EventUp();
            EventDown down = new EventDown();
            EventLeft left = new EventLeft();
            EventRight right = new EventRight();

            up.UpEvent += f.UpFig;
            down.DownEvent += f.DownFig;
            left.LeftEvent += f.LeftFig;
            right.RightEvent += f.RightFig;

            ConsoleKeyInfo cki;

            while (true)
            {
                if (f.CheckDown() == true) f.Move();
                else
                {
                    while (true)
                    {
                        bool flag = f.CheckLine();
                        if (flag == false) break;
                    }
                    f.NewFig();
                    if (f.IsAtBottom() == true) break;
                }


                for (int i = 0; i < 10 - f.level; i++)
                {
                    System.Threading.Thread.Sleep(50);
                    if (Console.KeyAvailable)
                    {
                        cki = Console.ReadKey();


                        switch (cki.Key)
                        {
                            case ConsoleKey.UpArrow:
                                {
                                    up.UpUserEvent();
                                    f.DrawField();
                                    break;
                                }

                            case ConsoleKey.DownArrow:
                                {
                                    down.DownUserEvent();
                                    break;
                                }

                            case ConsoleKey.LeftArrow:
                                {
                                    left.LeftUserEvent();
                                    f.DrawField();
                                    break;
                                }

                            case ConsoleKey.RightArrow:
                                {
                                    right.RightUserEvent();
                                    f.DrawField();
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }

                    }
                }


            }

            Console.Clear();

            Console.WriteLine("\n\n\n      ИГРА ОКОНЧЕНА!");
            Console.WriteLine("\n   Счёт " + (f.level * 1000 + f.scores) + "\n\n\n\n\n\n\n\n\n");

        }
    }
}