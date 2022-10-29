using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
namespace _Console_Snake_Game
{
    
    class ScreenPoint
    {
        private int _x;
        private int _y;
        private bool _isHead;
        private Game.SnakeHeading _sh;
        public int X
        {
            get 
            {
                return _x;
            }
            set
            {
                _x = value;
            }

        }

        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        public ScreenPoint(int x, int y,bool isHead=false)
        {
            this._x = x;
            this._y = y;
            this._isHead = isHead;
        }
    }
    class Game
    {
        public enum SnakeHeading
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        };

        public static bool areHeadingsOposite(SnakeHeading op1,SnakeHeading op2)
        {
            bool ret = false;
            switch(op1)
            {
                case SnakeHeading.DOWN:
                    if (op2 == SnakeHeading.UP)
                        ret = true;
                    break;
                case SnakeHeading.UP:
                    if (op2 == SnakeHeading.DOWN)
                        ret = true;
                    break;

                case SnakeHeading.LEFT:
                    if (op2 == SnakeHeading.RIGHT)
                        ret = true;
                    break;

                case SnakeHeading.RIGHT:
                    if (op2 == SnakeHeading.LEFT)
                        ret = true;
                    break;
                default:
                    ret = false;
                    break;

            }
            return ret;
        }
        public static ScreenPoint MakeFood(ScreenPoint[] snake)
        {
            ScreenPoint ret=null;
            ScreenPoint sSize = Screen.ScreenSize;
            int XLimit, YLimit;
            if (sSize.Y > 40)
            {
                YLimit = 40;
            }
            else
            {
                YLimit = sSize.Y - 1;
            }
            if (sSize.X > 50)
            {
                XLimit = 50;
            }
            else
            {
                XLimit = sSize.X - 1;
            }
            int foodX, foodY;
            Random r = new Random();
            foodX = r.Next(1, XLimit);
            foodY = r.Next(1, YLimit);
            ret = new ScreenPoint(foodX, foodY);
            while(IsPointCollapsingWithSnake(snake,ret))
            {
                foodX = r.Next(1, XLimit);
                foodY = r.Next(1, YLimit);
                ret = new ScreenPoint(foodX, foodY);
            }
            Screen.WriteScreen(ret, ".");
            return ret;
        }
        public static bool hasSnakeAteFood(ScreenPoint[] snake, ScreenPoint food)
        {
            ScreenPoint snakeHead = snake[FindHeadIndex(snake)];
            if (snakeHead.X == food.X && snakeHead.Y == food.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static ScreenPoint[] GrowSnake(ScreenPoint[] snake)
        {
            List<ScreenPoint> tmpList = new List<ScreenPoint>();
            ScreenPoint last = snake[0];
            if (last.X - 1 > 0)
            {
                tmpList.Add(new ScreenPoint(last.X - 1, last.Y));
            }
            else
            {
                tmpList.Add(new ScreenPoint(last.X + 1, last.Y));
            }
            for (int i = 0; i < snake.Length; i++)
            {
                tmpList.Add(snake[i]);
            }

            return tmpList.ToArray();
        }
        public static bool isSnakeCollapsingWithItself(ScreenPoint[] snake)
        {
            ScreenPoint head = snake[FindHeadIndex(snake)];
            bool ret = false;
            for (int i = 0; i < snake.Length - 2;i++ )
            {
                if (snake[i].X == head.X && snake[i].Y == head.Y)
                {
                    ret = true;
                }
            }
            return ret;
        }
        public static bool IsSnakeCollapsedToEmptyWall(ScreenPoint[] snake)
        {
            bool failed = false;
            ScreenPoint snakeHead = snake[FindHeadIndex(snake)];
            ScreenPoint sSize = Screen.ScreenSize;
            if (snakeHead.X >= sSize.X || snakeHead.Y >= sSize.Y)
            {
                failed = true;
            }
            else if (snakeHead.X < 0 || snakeHead.Y < 0)
            {
                failed = true;
            }
            return failed;

        }
        public static ScreenPoint[] CalculateNextMove(ScreenPoint[] snake,SnakeHeading CurrentHeading,SnakeHeading NextHeading)
        {
            bool invalidMove = false;
            bool criticalMove = false;
            
            if ((CurrentHeading == SnakeHeading.DOWN && NextHeading == SnakeHeading.UP) || (CurrentHeading == SnakeHeading.UP && NextHeading == SnakeHeading.DOWN))
            {
                invalidMove = true;
                criticalMove = true;
                
            }
            if ((CurrentHeading == SnakeHeading.LEFT && NextHeading == SnakeHeading.RIGHT) || (CurrentHeading == SnakeHeading.RIGHT && NextHeading == SnakeHeading.LEFT))
            {
                criticalMove = true;
                invalidMove = true;
                
            }
            
            /*
            if (CurrentHeading == SnakeHeading.UP || CurrentHeading == SnakeHeading.DOWN)
            {
                if (Screen.IsPointArrayLinearY(snake))
                {
                    invalidMove = true;
                }
            }
             */
            if (CurrentHeading == NextHeading)
            {
                invalidMove = true;
            }
            if (!criticalMove)
            {
                if (invalidMove)
                {
                    if (Screen.IsPointArrayLinearX(snake))
                    {
                        if (CurrentHeading == SnakeHeading.RIGHT)
                        {
                            snake = Screen.AddToPointArray(snake, 1, Screen.PointAdd.AddToX);
                        }
                        else if (CurrentHeading == SnakeHeading.LEFT)
                        {
                            snake = Screen.AddToPointArray(snake, -1, Screen.PointAdd.AddToX);
                        }
                    }
                    else if (Screen.IsPointArrayLinearY(snake))
                    {
                        if (CurrentHeading == SnakeHeading.UP)
                        {
                            snake = Screen.AddToPointArray(snake, -1, Screen.PointAdd.AddToY);
                        }
                        else if (CurrentHeading == SnakeHeading.DOWN)
                        {
                            snake = Screen.AddToPointArray(snake, 1, Screen.PointAdd.AddToY);
                        }
                    }
                    else
                    {
                        int HeadIndex = FindHeadIndex(snake);
                        snake = MoveSnake(snake, HeadIndex, NextHeading);
                    }

                }
                else
                {
                    int HeadIndex = FindHeadIndex(snake);
                    snake = MoveSnake(snake, HeadIndex, NextHeading);

                }
            }
            return snake;
        }

        private static ScreenPoint FindRightPoint(ScreenPoint CurrentPoint,ScreenPoint NextPoint,Game.SnakeHeading sh)
        {

            if (sh == SnakeHeading.RIGHT || sh == SnakeHeading.LEFT)
            {
                if (CurrentPoint.Y < NextPoint.Y)
                {
                    CurrentPoint.Y++;
                }
                else if (CurrentPoint.Y > NextPoint.Y)
                {
                    CurrentPoint.Y--;
                }
                else if (CurrentPoint.X < NextPoint.X)
                {
                    CurrentPoint.X++;
                }
                else if (CurrentPoint.X > NextPoint.X)
                {
                    CurrentPoint.X--;
                }

            }
            else
            {
                if (CurrentPoint.X < NextPoint.X)
                {
                    CurrentPoint.X++;
                }
                else if (CurrentPoint.X > NextPoint.X)
                {
                    CurrentPoint.X--;
                }
                else if (CurrentPoint.Y < NextPoint.Y)
                {
                    CurrentPoint.Y++;
                }
                else if (CurrentPoint.Y > NextPoint.Y)
                {
                    CurrentPoint.Y--;
                }
            }
            
            return CurrentPoint;
        }
        private static ScreenPoint[] MoveSnake(ScreenPoint[] snake,int HeadIndex,Game.SnakeHeading sh)
        {
            if (sh == SnakeHeading.RIGHT)
            {
               snake[HeadIndex].X++;
            }

            else if (sh == SnakeHeading.LEFT)
            {
                snake[HeadIndex].X--;
            }
            else if (sh == SnakeHeading.DOWN)
            {
                snake[HeadIndex].Y++;
            }
            else if (sh == SnakeHeading.UP)
            {
                snake[HeadIndex].Y--;
            }
            for (int i = 0; i < HeadIndex; i++)
            {
                snake[i] = FindRightPoint(snake[i], snake[i + 1], sh);
            }
            return snake;
        }
        private static int FindHeadIndex(ScreenPoint[] snake)
        {
            return snake.Length - 1;
        }
        
        public static bool IsPointCollapsingWithSnake(ScreenPoint[] snake, ScreenPoint point)
        {
            bool ret = false;
            foreach (ScreenPoint sp in snake)
            {
                if (point.X == sp.X && point.Y == sp.Y)
                {
                    ret = true;
                }
            }
            return ret;
        }

        
    }
    class Screen
    {
        public enum PointAdd
        {
            AddToX,
            AddToY
        }
        public static ScreenPoint[] AddToPointArray(ScreenPoint[] input,int value,PointAdd pa)
        {
            foreach (ScreenPoint ip in input)
            {
                if (pa == PointAdd.AddToX)
                    ip.X += value;
                if (pa == PointAdd.AddToY)
                    ip.Y += value;
            }

            return input;
            
        }
        public static bool IsPointArrayLinearX(ScreenPoint[] input)
        {
            bool ret = true; ;
            if (input.Length >= 2)
            {
                int Y;
                Y = input[0].Y;

                for (int i = 1; i < input.Length; i++)
                {
                    if (input[i].Y != Y)
                    {
                        ret = false;
                    }
                }
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        public static bool IsPointArrayLinearY(ScreenPoint[] input)
        {
            bool ret = true; ;
            if (input.Length >= 2)
            {
                int X;
                X = input[0].X;

                for (int i = 1; i < input.Length; i++)
                {
                    if (input[i].X != X)
                    {
                        ret = false;
                    }
                }
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        public static ScreenPoint ScreenSize
        {
            get
            {
                return new ScreenPoint(Console.BufferWidth, Console.BufferHeight);
            }
        }
        private static ScreenPoint[] SortPoints(ScreenPoint[] input)
        {
            ScreenPoint[] tmp = input;
            for (int i = 0; i < tmp.Length; i++)
            {
                for (int j = i+1; j < tmp.Length; j++)
                {
                    bool swap = false;
                    ScreenPoint sp1, sp2,tmpSp;
                    sp1 = tmp[i];
                    sp2 = tmp[j];
                    if (sp1.X == sp2.X)
                    {
                        if (sp2.Y < sp1.Y)
                        {
                            swap = true;
                        }
                    }
                    else if (sp2.X < sp1.X)
                    {
                        if (sp2.Y <= sp1.Y)
                        {
                            swap = true;
                        }
                    }
                    else 
                    {
                        if (sp2.Y < sp1.Y)
                        {
                            swap = true;
                        }
                    }

                    if (swap == true)
                    {
                        tmpSp = sp1;
                        sp1 = sp2;
                        sp2 = tmpSp;
                        tmp[i] = sp1;
                        tmp[j] = sp2;
                    }
                }

            }
            return tmp;
        }

        public static void WriteScreen(ScreenPoint point, string input)
        {
            Console.SetCursorPosition(point.X,point.Y);
            Console.Write(input);
            
        }
        public static void WriteScreen(ScreenPoint[] points, char Character)
        {
            foreach (ScreenPoint sp in points)
            {
                Console.SetCursorPosition(sp.X, sp.Y);
                Console.Write(Character.ToString());
            }
        }
    }
}
