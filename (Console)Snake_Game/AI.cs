using System;
using System.Collections.Generic;
using System.Text;
namespace _Console_Snake_Game
{
    class AI
    {
        public static Game.SnakeHeading DecideNextMove(ScreenPoint[] Snake, ScreenPoint food,Game.SnakeHeading CurrentHeading)
        {
            ScreenPoint snakeHead = Snake[Snake.Length - 1];
            Game.SnakeHeading FinalDecision=CurrentHeading;
            int ScreenX, ScreenY;
            int HeadX,HeadY,FoodX,FoodY;
            HeadX = snakeHead.X;
            HeadY = snakeHead.Y;
            FoodX = food.X;
            FoodY = food.Y;
            ScreenX = Screen.ScreenSize.X;
            ScreenY = Screen.ScreenSize.Y;
            if(CurrentHeading == Game.SnakeHeading.RIGHT)
            {
                if (HeadX < FoodX && !IsDecisionFatal(Snake, CurrentHeading, CurrentHeading))
                    FinalDecision = Game.SnakeHeading.RIGHT;
                else if(HeadX == FoodX || HeadX > FoodX)
                {
                    if(HeadY < FoodY)
                    {
                        FinalDecision = Game.SnakeHeading.DOWN;
                    }
                    else if(HeadY > FoodY)
                    {
                        FinalDecision = Game.SnakeHeading.UP;
                    }
                    else if(HeadY == FoodY)
                    {
                        if (!IsDecisionFatal(Snake, CurrentHeading, Game.SnakeHeading.UP))
                            FinalDecision = Game.SnakeHeading.UP;
                        else
                            FinalDecision = Game.SnakeHeading.DOWN;
                    }
                }
            }

            if(CurrentHeading == Game.SnakeHeading.LEFT)
            {
                if (HeadX > FoodX && !IsDecisionFatal(Snake,CurrentHeading,CurrentHeading))
                    FinalDecision = Game.SnakeHeading.LEFT;
                else if(HeadX == FoodX || HeadX < FoodX)
                {
                    if (HeadY < FoodY)
                    {
                        FinalDecision = Game.SnakeHeading.DOWN;
                    }
                    else if (HeadY > FoodY)
                    {
                        FinalDecision = Game.SnakeHeading.UP;
                    }
                    else if (HeadY == FoodY)
                    {
                        if (!IsDecisionFatal(Snake, CurrentHeading, Game.SnakeHeading.UP))
                            FinalDecision = Game.SnakeHeading.UP;
                        else
                            FinalDecision = Game.SnakeHeading.DOWN;
                    }
                }
            }

            if(CurrentHeading == Game.SnakeHeading.DOWN)
            {
                if (HeadY < FoodY && !IsDecisionFatal(Snake, CurrentHeading, CurrentHeading))
                {
                    FinalDecision = CurrentHeading;
                }
                else if(HeadY == FoodY || HeadY > FoodY)
                {
                    if(HeadX < FoodX)
                    {
                        FinalDecision = Game.SnakeHeading.RIGHT;
                    }
                    else if(HeadX > FoodX)
                    {
                        FinalDecision = Game.SnakeHeading.LEFT;
                    }
                    else if(HeadX == FoodX)
                    {
                        if (!IsDecisionFatal(Snake, CurrentHeading, Game.SnakeHeading.RIGHT))
                            FinalDecision = Game.SnakeHeading.RIGHT;
                        else
                            FinalDecision = Game.SnakeHeading.LEFT;
                    }
                }
            }

            if (CurrentHeading == Game.SnakeHeading.UP)
            {
                if (HeadY > FoodY && !IsDecisionFatal(Snake, CurrentHeading, CurrentHeading))
                {
                    FinalDecision = CurrentHeading;
                }
                else if (HeadY == FoodY || HeadY < FoodY)
                {
                    if (HeadX < FoodX)
                    {
                        FinalDecision = Game.SnakeHeading.RIGHT;
                    }
                    else if (HeadX > FoodX)
                    {
                        FinalDecision = Game.SnakeHeading.LEFT;
                    }
                    else if (HeadX == FoodX)
                    {
                        if (!IsDecisionFatal(Snake, CurrentHeading, Game.SnakeHeading.RIGHT))
                            FinalDecision = Game.SnakeHeading.RIGHT;
                        else
                            FinalDecision = Game.SnakeHeading.LEFT;
                    }
                }
            }
            
            Random r = new Random();
            Game.SnakeHeading[] ops=EscapeOptions(Snake,food,4);
            FinalDecision = PickRightOption(Snake,ops,CurrentHeading,FinalDecision);
            FinalDecision = MakenonFatalDecision((ScreenPoint[])Snake.Clone(), CurrentHeading, FinalDecision);
            return FinalDecision;
        }

        private static long CalculateDanger(ScreenPoint[] snake,Game.SnakeHeading heading,int Range=3)
        {
            long danger = 0;
            int HeadX, HeadY;
            HeadX = snake[snake.Length - 1].X;
            HeadY = snake[snake.Length - 1].Y;

            if(heading == Game.SnakeHeading.UP)
            {
                for(int i=1;i<Range;i++)
                {
                    bool t1=false, t2=false;
                    for(int j=0;j<snake.Length - 1;j++)
                    {
                        int tmp1, tmp2;
                        tmp1 = HeadX + 1;
                        tmp2 = HeadX - 1;
                        if (snake[j].X == tmp1 && snake[j].Y == HeadY - i)
                            t1 = true;
                        if (snake[j].X == tmp2 && snake[j].Y == HeadY - i)
                            t2 = true;
                    }

                    if (t1 == true && t2 == true)
                        danger++;
                }

            }
            else if(heading == Game.SnakeHeading.DOWN)
            {
                for (int i = 1; i < Range; i++)
                {
                    bool t1 = false, t2 = false;
                    for (int j = 0; j < snake.Length - 1; j++)
                    {
                        int tmp1, tmp2;
                        tmp1 = HeadX + 1;
                        tmp2 = HeadX - 1;
                        if (snake[j].X == tmp1 && snake[j].Y == HeadY + i)
                            t1 = true;
                        if (snake[j].X == tmp2 && snake[j].Y == HeadY + i)
                            t2 = true;
                    }

                    if (t1 == true && t2 == true)
                        danger++;
                }
            }
            else if(heading == Game.SnakeHeading.LEFT)
            {
                for (int i = 1; i < Range; i++)
                {
                    bool t1 = false, t2 = false;
                    for (int j = 0; j < snake.Length - 1; j++)
                    {
                        int tmp1, tmp2;
                        tmp1 = HeadY + 1;
                        tmp2 = HeadY - 1;
                        if (snake[j].Y == tmp1 && snake[j].X == HeadX - i)
                            t1 = true;
                        if (snake[j].Y == tmp2 && snake[j].X == HeadX - i)
                            t2 = true;
                    }

                    if (t1 == true && t2 == true)
                        danger++;
                }
            }
            else if(heading == Game.SnakeHeading.RIGHT)
            {
                for (int i = 1; i < Range; i++)
                {
                    bool t1 = false, t2 = false;
                    for (int j = 0; j < snake.Length - 1; j++)
                    {
                        int tmp1, tmp2;
                        tmp1 = HeadY + 1;
                        tmp2 = HeadY - 1;
                        if (snake[j].Y == tmp1 && snake[j].X == HeadX + i)
                            t1 = true;
                        if (snake[j].Y == tmp2 && snake[j].X == HeadX + i)
                            t2 = true;
                    }

                    if (t1 == true && t2 == true)
                        danger++;
                }
            }
            return danger;
        }
        private static Game.SnakeHeading LessDangerousHeading(ScreenPoint[] Snake,Game.SnakeHeading[] Options,Game.SnakeHeading DecidedHeading)
        {
            Game.SnakeHeading ret = DecidedHeading;
            if (Options.Length > 0)
            {
                long[] Dangers = new long[Options.Length];
                
                for (int i = 0; i < Options.Length; i++)
                {
                    Dangers[i] = CalculateDanger(Snake, DecidedHeading);
                }
                long minI, minV;
                minI = 0;
                minV = Dangers[0];
                for (int i = 1; i < Dangers.Length; i++)
                {
                    if (Dangers[i] < minV)
                    {
                        minI = i;
                        minV = Dangers[i];
                    }
                }
                bool AllZero = true;
                foreach (long t in Dangers)
                {
                    if (t != 0)
                        AllZero = false;
                }

                if (AllZero == false)
                    ret = Options[minI];
                else
                    ret = DecidedHeading;
            }
                
                return ret;
        }
        private static bool IsDecisionFatal(ScreenPoint[] snake, Game.SnakeHeading currentHeading, Game.SnakeHeading nextHeading)
        {
            bool res = false;
            ScreenPoint[] tmpSnake = new ScreenPoint[snake.Length];
            ScreenPoint[] tmp2;
            tmpSnake = ArrayCopy.CopyByValue(snake);
            tmp2 = Game.CalculateNextMove(tmpSnake, currentHeading, nextHeading);
            if (Game.isSnakeCollapsingWithItself(tmp2))
                res = true;
            else if (Game.IsSnakeCollapsedToEmptyWall(tmp2))
                res = true;
            return res;
        }

        private static bool IsLineEmpty(ScreenPoint[] Snake,int range,Game.SnakeHeading heading)
        {
            int HeadX, HeadY;
            HeadX = Snake[Snake.Length - 1].X;

            HeadY = Snake[Snake.Length - 1].Y;
            bool ret = true;
            for (int i = 1; i <= range;i++ )
            {
                for (int j = 0; j < Snake.Length - 1; j++)
                {
                    ScreenPoint tmp = Snake[j];
                    int t=-1, t2 = -1;
                    if (heading == Game.SnakeHeading.DOWN)
                    {
                        t = HeadX;
                        t2 = HeadY + i;

                    }
                    else if (heading == Game.SnakeHeading.UP)
                    {
                        t = HeadX;
                        t2 = HeadY - i;
                    }
                    else if (heading == Game.SnakeHeading.RIGHT)
                    {
                        t = HeadX + i;
                        t2 = HeadY; 
                    }
                    else if (heading == Game.SnakeHeading.LEFT)
                    {
                        t = HeadX - i;
                        t2 = HeadY;
                    }

                    if(tmp.X == t && tmp.Y == t2)
                    {
                        ret = false;
                    }
                }
            }
            return true;
        }
        private static Game.SnakeHeading[] EscapeOptions(ScreenPoint[] snake,ScreenPoint Food,int Range=5)
        {
            int HeadX, HeadY;
            bool FoodWithinRange = false;
            List<Game.SnakeHeading> GoodOptions = new List<Game.SnakeHeading>();
            ScreenPoint snakeHead = snake[snake.Length - 1];
            HeadX = snakeHead.X;
            HeadY = snakeHead.Y;
            bool uA=true,dA=true,lA=true,rA=true;
            
            int i;
            for (i = 1; i <= Range;i++ )
            {
                bool OptionAcceptable1 = true;
                bool OptionAcceptable2 = true;
                int tmp = HeadX + i;
                int tmp2 = HeadX - i;
                for(int j=0;j<snake.Length - 1;j++)
                {
                    if(snake[j].X == tmp && snake[j].Y == HeadY)
                    {
                        rA = false;
                    }
                    
                    if (snake[j].X == tmp2 && snake[j].Y == HeadY)
                    {
                        lA = false;
                    }
                    if (Food.Y == HeadY )
                    {
                        if (Food.X < HeadX && IsLineEmpty(snake, Range, Game.SnakeHeading.LEFT))
                        {
                            lA = true;
                        }
                        else if (Food.X > HeadX && IsLineEmpty(snake, Range, Game.SnakeHeading.RIGHT))
                        {
                            rA = true;
                        }
                    }
                }


                tmp = HeadY + i;
                tmp2 = HeadY - i;
                OptionAcceptable1 = true;
                OptionAcceptable2 = true;

                for(int j=0;j<snake.Length - 1;j++)
                {
                    if (snake[j].Y == tmp && snake[j].X == HeadX)
                    {
                        dA = false;
                    }
                    if (snake[j].Y == tmp2 && snake[j].X == HeadX)
                    {
                        uA = false;
                    }

                    if(Food.X == HeadX)
                    {
                        if(Food.Y < HeadY && IsLineEmpty(snake,Range,Game.SnakeHeading.UP))
                        {
                            uA = true;
                        }
                        else if (Food.Y > HeadY && IsLineEmpty(snake, Range, Game.SnakeHeading.DOWN))
                        {
                            dA = true;
                        }
                    }
                }

               
            }
            if(snakeHead.X + 1 >= Screen.ScreenSize.X)
            {
                rA = false;
            }
            if(snakeHead.X - 1 < 0)
            {
                lA = false;
            }
            if(snakeHead.Y + 1 >= Screen.ScreenSize.Y)
            {
                dA = false;
            }
            if(snakeHead.Y - 1 < 0)
            {
                uA = false;
            }
            if (dA)
                GoodOptions.Add(Game.SnakeHeading.DOWN);
            if (uA)
                GoodOptions.Add(Game.SnakeHeading.UP);
            if (lA)
                GoodOptions.Add(Game.SnakeHeading.LEFT);
            if (rA)
                GoodOptions.Add(Game.SnakeHeading.RIGHT);

            if(GoodOptions.Count <= 0)
            {
                if(Range >= 2)
                {
                    Game.SnakeHeading[] tmpOptions;
                    tmpOptions = EscapeOptions(snake, Food, Range - 1);
                    for(i=0;i<tmpOptions.Length;i++)
                    {
                        GoodOptions.Add(tmpOptions[i]);
                    }
                }
            }
            
            
                return GoodOptions.ToArray();
        }


        private static Game.SnakeHeading PickRightOption(ScreenPoint[] Snake,Game.SnakeHeading[] Options,Game.SnakeHeading currentHeading,Game.SnakeHeading nextHeading)
        {
            Game.SnakeHeading ret = nextHeading;
            bool opAvaialble = false;
            for (int i = 0; i < Options.Length;i++)
            {
                if (Options[i] == nextHeading)
                    opAvaialble = true;
            }

            if(opAvaialble == false)
            {
                for(int i=0;i < Options.Length; i++)
                {
                    if(!Game.areHeadingsOposite(currentHeading,Options[i]))
                    {
                        if(!AI.IsDecisionFatal(Snake,currentHeading,Options[i]))
                        {
                            ret = Options[i];
                        }
                    }
                }
            }
            //ret = LessDangerousHeading(Snake, Options, ret);
                return ret;
        }
        private static Game.SnakeHeading MakenonFatalDecision(ScreenPoint[] snake,Game.SnakeHeading currentHeading,Game.SnakeHeading nextHeading,Game.SnakeHeading? ExceptionH = null)

        {
            Game.SnakeHeading final = nextHeading;
            if(IsDecisionFatal(snake,currentHeading,nextHeading) == true)
            {
               switch(nextHeading)
               {
                   case Game.SnakeHeading.DOWN:
                       if (currentHeading != nextHeading)
                           final = Game.SnakeHeading.UP;
                       else
                       {
                           if (ExceptionH != Game.SnakeHeading.RIGHT)
                               final = Game.SnakeHeading.RIGHT;
                           else
                               final = Game.SnakeHeading.LEFT;
                       }
                       break;
                   case Game.SnakeHeading.UP:
                       if(currentHeading != nextHeading)
                        final = Game.SnakeHeading.DOWN;
                       else
                       {
                           if (ExceptionH != Game.SnakeHeading.RIGHT)
                               final = Game.SnakeHeading.RIGHT;
                           else
                               final = Game.SnakeHeading.LEFT;
                       }
                       break;
                   case Game.SnakeHeading.LEFT:
                       if (currentHeading != nextHeading)
                           final = Game.SnakeHeading.RIGHT;
                       else
                       {
                           if (ExceptionH != Game.SnakeHeading.UP)
                               final = Game.SnakeHeading.UP;
                           else
                               final = Game.SnakeHeading.DOWN;
                       }
                       break;
                   case Game.SnakeHeading.RIGHT:
                       if (currentHeading != nextHeading)
                           final = Game.SnakeHeading.LEFT;
                       else
                       {
                           if (ExceptionH != Game.SnakeHeading.UP)
                               final = Game.SnakeHeading.UP;
                           else
                               final = Game.SnakeHeading.DOWN;
                       }
                       break;
               }
            }
            if(IsDecisionFatal(snake,currentHeading,final))
            {
                if(ExceptionH == null)
                    final = MakenonFatalDecision(snake, currentHeading, nextHeading, final);
            }
            return final;
        }
    }
}
