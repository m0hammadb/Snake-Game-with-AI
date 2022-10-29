using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
namespace _Console_Snake_Game
{
    class Program
    {
        private enum Difficulty
        {
            Easy,Medium,Hard,VeryHard,AI,Tmp
        }
        private class MenuArray : ArrayList
        {
            private int _selectedIndex = 0;
            public int SelectedIndex
            {
                get { return _selectedIndex; }
                set { _selectedIndex = value; }
            }
        }
        private class Menu
        {
            private MenuArray _items=null;
            public void Select(int id)
            {
                this._items.SelectedIndex = id;
            }
            public MenuArray Items
            {
                get
                {
                    return _items;
                }

                
            }
        }
        private static Difficulty SelectDifficulty()
        {
            Console.Clear();
            MenuArray x = new MenuArray();
            
            x.Add("Easy");
            x.Add("Medium");
            x.Add("Hard");
            x.Add("Very Hard");
            x.Add("Artificial Intelligence");
            x.SelectedIndex = 1;
            
            Console.CursorVisible = false;
            ConsoleKeyInfo ki = new ConsoleKeyInfo();
            while (ki.Key != ConsoleKey.Enter)
            {
                if (ki.Key == ConsoleKey.UpArrow)
                {
                    if (x.SelectedIndex > 0)
                    {
                        x.SelectedIndex--;
                    }
                }

                if (ki.Key == ConsoleKey.DownArrow)
                {
                    if (x.SelectedIndex + 1 < x.Count)
                    {
                        x.SelectedIndex++;
                    }
                }
                Console.Clear();
                Console.Write("\t\tPlease Select Difficulty (With Keyboard Arrows)\n\n\t\t\t\t And Press Enter\n\n");
                Console.Write("\n\n\n\n\t\t\t====================\n\n\n");
                for (int i=0;i<x.Count;i++)
                {

                    
                    if (x.SelectedIndex == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write("\t\t\t\t" + x[i] + "     \n\n\n");
                    Console.ResetColor();
                }
                Console.Write("\t\t\t====================\n");
                
                
                ki = Console.ReadKey();
            }
            Difficulty ret = new Difficulty();
            switch (x[x.SelectedIndex].ToString())
            {
                case "Easy":
                    ret = Difficulty.Easy;
                    break;
                case "Medium":
                    ret = Difficulty.Medium;
                    break;
                case "Hard":
                    ret = Difficulty.Hard;
                    break;
                case "Very Hard":
                    ret = Difficulty.VeryHard;
                    break;
                case "Artificial Intelligence":
                    ret = Difficulty.AI;
                    break;
                default:
                    ret = Difficulty.Medium;
                    break;
            }
            return ret;
        }
        private static void StartGame(Difficulty dif)
        {
            Console.CursorVisible = false;
            int FrameRate=200;
            switch (dif)
            {
                case Difficulty.Easy :
                    FrameRate = 500;
                    break;
                case Difficulty.Medium:
                    FrameRate = 250;
                    break;
                case Difficulty.Hard:
                    FrameRate = 100;
                    break;
                case Difficulty.VeryHard:
                    FrameRate = 50;
                    break;
                case Difficulty.AI:
                    FrameRate = 40;
                    break;
            }
            int i = 0;
            int j = 0;
            ScreenPoint[] s = new ScreenPoint[6];
            s[0] = new ScreenPoint(1, j);
            s[1] = new ScreenPoint(2, j);
            s[2] = new ScreenPoint(3, j);
            s[3] = new ScreenPoint(4, j);
            s[4] = new ScreenPoint(5, j);
            s[5] = new ScreenPoint(6, j,true);

            
            Game.SnakeHeading ch, nh;
            ch = Game.SnakeHeading.RIGHT;
            nh = Game.SnakeHeading.RIGHT;
            bool fail=false;
            bool isFoodAvailable = false;
            ScreenPoint food = null;
            while(!fail)
            {
                if (!isFoodAvailable)
                {
                   food = Game.MakeFood(s);
                    isFoodAvailable = true;
                }
                Console.Clear();
                if (!isFoodAvailable)
                {
                    food = Game.MakeFood(s);
                    isFoodAvailable = true;
                }
                else
                {
                    Screen.WriteScreen(food, "o");
                }
                bool simplePause = false;
                if (dif !=  Difficulty.Tmp) {
                    if (Console.KeyAvailable)
                    {

                        ConsoleKeyInfo key = Console.ReadKey(true);
                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                {
                                    if (ch != Game.SnakeHeading.DOWN)
                                    {
                                        nh = Game.SnakeHeading.UP;
                                    }
                                    break;
                                }
                            case ConsoleKey.DownArrow:
                                {
                                    if (ch != Game.SnakeHeading.UP)
                                    {
                                        nh = Game.SnakeHeading.DOWN;
                                    }
                                    break;
                                }
                            case ConsoleKey.RightArrow:
                                {
                                    if (ch != Game.SnakeHeading.LEFT)
                                    {
                                        nh = Game.SnakeHeading.RIGHT;
                                    }
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                {
                                    if (ch != Game.SnakeHeading.RIGHT)
                                    {
                                        nh = Game.SnakeHeading.LEFT;
                                    }
                                }
                                break;
                            case ConsoleKey.P:
                                simplePause = true;
                                break;
                            case ConsoleKey.I:
                                FrameRate = FrameRate - 10;
                                break;
                            case ConsoleKey.D:
                                FrameRate = FrameRate + 10;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if(dif == Difficulty.AI)
                {
                    nh = AI.DecideNextMove(s, food, ch);
                }
                AI.DecideNextMove(s, food, ch);
                s = Game.CalculateNextMove(s, ch, nh);
                if (Game.hasSnakeAteFood(s, food))
                {
                    s = Game.GrowSnake(s);
                    food = Game.MakeFood(s);
                }
                if (!Game.IsSnakeCollapsedToEmptyWall(s) && !Game.isSnakeCollapsingWithItself(s))
                {
                    Screen.WriteScreen(s, '#');
                    if (simplePause)
                        Console.ReadKey(true);
                }
                else
                {
                    fail = true;
                }
                
                ch = nh;

                if (FrameRate <= 0)
                    FrameRate = 1;
                Thread.Sleep(FrameRate);
            }
            ShowGameOver();
        }

        private static void ShowGameOver()
        {
            Console.Clear();
            ScreenPoint sSize = Screen.ScreenSize;
            string Message="\n\n\n\n\n\n\n\n\n";
            Message += "\t\t\t===============================\n";
            Message += "\t\t\t=                             =\n";
            Message += "\t\t\t=          GAME OVER          =\n";
            Message += "\t\t\t=                             =\n";
            Message += "\t\t\t===============================\n";
            Console.Write(Message);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("\n\n\n\n\t\t\t\tPress R To Restart...");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n\n\n\n\n\n\n\n\t\t\tDeveloped By Mohammad Bashirinia");
            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            bool startGame = true;
            bool endGame = false;
            while (!endGame)
            {
                if (startGame)
                {
                    Console.SetWindowSize(80, 40);
                    Console.SetBufferSize(80, 40);
                    Difficulty newDif;
                    newDif = SelectDifficulty();
                        StartGame(newDif);
                }
                startGame = false;
                ConsoleKeyInfo k = Console.ReadKey();
                if (k.Key == ConsoleKey.R)
                {
                    startGame = true;
                }
                if (k.Key == ConsoleKey.Escape)
                {
                    endGame = true;
                }
            }
        }
    }
}
