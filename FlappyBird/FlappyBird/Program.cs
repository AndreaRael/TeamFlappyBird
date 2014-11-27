using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    class Program
    {
        static void Main(string[] args)
        {
            Game newGame = new Game();
            newGame.PlayGame();
        }
    }

    
    class Object
    {
        public ConsoleColor Color { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool isHole { get; set; }

        Random rng = new Random();

        public Object()
        {
            this.Color = ConsoleColor.Green;
            this.Symbol = "|";
            this.X = Console.WindowWidth - 2;
            this.Y = 0;
            this.isHole = false;
        }

        public Object(int y, bool isHole)
        {
            //If wall....
            this.Color = ConsoleColor.Green;
            this.Symbol = "|";
            this.X = Console.WindowWidth - 2;
            this.Y = y;
            this.isHole = false;
            
            //If hole...
        }

        public Object(ConsoleColor Color, int x, int y, string Symbol, bool WallStatus)
        {
            this.Color = Color;
            this.Symbol = Symbol;
            this.X = x;
            this.Y = y;
            this.isHole = WallStatus;

        }

        public void Draw()
        {
            //draws the unit based on x and y, sets the color to color default, and writes the symbol
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);

        }
    }

    class Game
    {
        public Object FlappyBird { get; set; }
        public List<Object> WallList { get; set; }
        public int Score { get; set; }
        public bool isDead { get; set; }
        public int Speed { get; set; }

        Random rng = new Random();

        public Game()
        {

            Console.WindowWidth = 60;
            Console.BufferWidth = 60;
            Console.WindowHeight = 30;
            Console.BufferHeight = 30;
            this.FlappyBird = new Object(ConsoleColor.Yellow, 5, Console.WindowHeight / 2, ">", false);
            this.WallList = new List<Object>();
            this.Score = 0;
            this.isDead = false;
            this.Speed = 0;
        }

        public void PlayGame()
        {
            while (!isDead)
            {
                //create counter to keep spaces
                //then only create wall after counter count
                CreateWall();
                MoveBird();
                MoveObstacles();
                DrawGame();
                System.Threading.Thread.Sleep(170);

            }
        }

        public void CreateWall()
        {
            //Genereate a number between 0 and 25
            for (int i = 0; i < 29; i++)
            {
                //If i is NOT between generated number and generated number + 3, then....
                WallList.Add(new Object(29 - i, true));
                //Otherwise...
                //Wallist.Add(new Object(29 - i, false)
              
            }

            //int holePlacement = rng.Next(Console.WindowHeight-1);

            //Object hole = new Object(ConsoleColor.Black, Console.WindowWidth - 2, holePlacement, " ", true);
            
            //if (!(hole.Y > Console.WindowHeight - 5))
            //{
            //    holePlacement = rng.Next(Console.WindowHeight - 1);

            //    hole = new Object(ConsoleColor.Black, Console.WindowWidth - 2, holePlacement, " ", true); 
            //}
            //else
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        WallList.Add(hole);
            //        hole.Y--;
                
            //    }
            //}


        }

        public void MoveObstacles()
        {

            

            foreach (Object wall in WallList)
            {
                wall.X--;

                if (wall.isHole
                    && wall.X == FlappyBird.X
                    && wall.Y == FlappyBird.Y)
                {
                    Score += 1;
                    isDead = false;
                }
                if (wall.X == FlappyBird.X
                    && wall.Y == FlappyBird.Y)
                {
                    isDead = true;
                }
            }

            WallList = WallList.Where(x => x.X > 0).ToList();
        }

        public void MoveBird()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey keyPressed = Console.ReadKey().Key;

                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);

                }
                if (keyPressed == ConsoleKey.UpArrow
                        && FlappyBird.Y < (Console.WindowHeight))
                {
                    FlappyBird.Y--;
                }
                else if (keyPressed == ConsoleKey.DownArrow
                    && FlappyBird.Y > 0)
                {
                    FlappyBird.Y++;
                }
                else if (keyPressed == ConsoleKey.RightArrow
                    && FlappyBird.X < Console.WindowWidth)
                {
                    FlappyBird.X++;
                }
                else if (keyPressed == ConsoleKey.LeftArrow
                    && FlappyBird.X > 0)
                {
                    FlappyBird.X--;
                }
                else
                {
                    Console.WriteLine("Invalid Move");
                }
            }
            else if (!(Console.KeyAvailable))
            {
                FlappyBird.Y++;
            }
        }

        public void DrawGame()
        {
            Console.Clear();
            FlappyBird.Draw();

            foreach (Object wall in WallList)
            {
                wall.Draw();
            }
            PrintAtPosition(20, 2, "Score: " + this.Score, ConsoleColor.Green);
            //PrintAtPosition(20, 3, "Speed:" + this.Speed, ConsoleColor.Green);

        }

        public void PrintAtPosition(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);

        }

    }
}
