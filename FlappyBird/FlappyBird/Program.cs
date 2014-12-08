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
            Console.Title = "BouncyBird";
            //calls our new game, and play game function
            Game newGame = new Game();
            newGame.PlayGame();
        }
    }

    //creates our object class
    class Object
    {
        //color property
        public ConsoleColor Color { get; set; }
        //symbol property
        public string Symbol { get; set; }
        //x and y coordinates
        public int X { get; set; }
        public int Y { get; set; }
        //determines if it is a hole
        public bool isHole { get; set; }
        public bool isPowerUp { get; set; }
        public bool isGhostPowerUp { get; set; }
        

        //new rng var
        Random rng = new Random();
        //constructor that does not require overloads
        //not used but could be used in the future
        public Object()
        {
            //sets color to green by default
            this.Color = ConsoleColor.Green;
            //symbol is a wall
            this.Symbol = "|";
            //starts two away from window width
            this.X = Console.WindowWidth - 2;
            //begins at top
            this.Y = 0;
            //it is not a hole
            this.isHole = false;
            this.isPowerUp = false;
        }

        //another constructor that takes y coordinate
        //and bool of wether object is a hole
        public Object(int y, bool isHole)
        {
            //if it is a wall
            if (!isHole)
            {
                //color is green, symbol is |, is hole is false
                this.Color = ConsoleColor.Green;
                this.Symbol = "|";
                this.X = Console.WindowWidth - 2;
                this.Y = y;
                this.isHole = false;
            }
            
            //If hole...
            if (isHole)
            {
                //color is black, symbol is space
                this.Color = ConsoleColor.Black;
                this.Symbol = " ";
                this.X = Console.WindowWidth - 2;
                this.Y = y;
                //is hole set to true
                this.isHole = true;
            }


        }
        public Object(int y, ConsoleColor color, string symbol, bool isPowerUp, bool isGhostPowerUp)
        {
            this.Symbol = symbol;
            this.Color = color;
            this.X = Console.WindowWidth - 2;
            this.Y = y;
            this.isHole = false;
            this.isPowerUp = isPowerUp;
            this.isGhostPowerUp = isGhostPowerUp;
        }

        //constructor that takes color, x and y values, symbol, and wallstatus
        public Object(ConsoleColor Color, int x, int y, string Symbol, bool WallStatus)
        {
            //sets those values accordingly
            this.Color = Color;
            this.Symbol = Symbol;
            this.X = x;
            this.Y = y;
            this.isHole = WallStatus;

        }
        //draws our objects
        public void Draw()
        {
            //draws the unit based on x and y, sets the color to color default, and writes the symbol
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = Color;
            Console.Write(Symbol);

        }
    }


    //class for our game
    class Game
    {
        //create our bird of type object
        public Object FlappyBird { get; set; }
        //list to hold our objects
        public List<Object> WallList { get; set; }
        //value for our score
        public int Score { get; set; }
        //value for whether or not bird is dead
        public bool isDead { get; set; }
        //not used, but can adjust speed if necessary
        public int Speed { get; set; }
        public int ghostLives { get; set; }

        //new rng generator
        Random rng = new Random();

        //game constructor
        public Game()
        {
            //window height and width, along with buffer
            Console.WindowWidth = 60;
            Console.BufferWidth = 60;
            Console.WindowHeight = 30;
            Console.BufferHeight = 30;
            // creates a new flappy bird, color yellow, x cordinate, y coordinate, symbol, and if it is a hole
            this.FlappyBird = new Object(ConsoleColor.Yellow, 5, Console.WindowHeight / 2, ">", false);
            this.WallList = new List<Object>();
            this.Score = 0;
            this.isDead = false;
            this.Speed = 0;
            this.ghostLives = 0;
        }

        public void PlayGame()
        {
            //create var to alter wall intervals
            var createWallsAtInterval = 20;
            var interval = 19;
            //greets players
            Greeting();

            //while we arent dead
            while (!isDead)
            {
                //while the wall counter is greater than the interval
                if (createWallsAtInterval > interval)
                {
                    //create wall
                    CreateWall();
                    //if score is greater than 15
                    if (Score > 15)
                    {
                        //reduce the wall intervall
                        interval = 14;
                    }
                    //greater than 25
                    if (Score > 25)
                    {
                        //reduce wall interval again
                        interval = 9;
                    }
                    //greater than 35
                    if (Score > 35)
                    {
                        //reduce wall interval a final time
                        interval = 7;
                    }
                    //resets wall counter to 0
                    createWallsAtInterval = 0;

                }
                //moves bird
                MoveBird();
                //moves wall
                MoveObstacles();
                //draws game
                DrawGame();
                //adds to wall counter
                createWallsAtInterval++;
                //sleeps so as to control wall speed
                System.Threading.Thread.Sleep(170);

            }
            //tells player they died and score
            ExitGreeting();
        }
        //creates wall
        public void CreateWall()
        {
            //rng for our hole in wall
            var holeInWall = rng.Next(26);
            
            //loop through filling wall with objects
            for (int i = 0; i < 29; i++)
            {
                //If i is between generated number and generated number + 7, then....
                if (i > holeInWall
                    && i < holeInWall + 7)
                {
                    //add hole in wall, 8 spaces high
                    WallList.Add(new Object(29 - i, true));
                }
           
                else
                {
                    //add wall object
                    WallList.Add(new Object(29 - i, false));

                    
                }
                
              
            }
            var chanceOfPowerUp = rng.Next(100);
            if (Score > 15)
            {


                if (chanceOfPowerUp > 50)
                {
                    var powerUpPlacement = rng.Next(27);
                    if (WallList[powerUpPlacement].isPowerUp
                        && WallList[powerUpPlacement + 1].isPowerUp
                        && WallList[powerUpPlacement + 2].isPowerUp
                        && WallList[powerUpPlacement].isHole
                        && WallList[powerUpPlacement + 1].isHole
                        && WallList[powerUpPlacement + 2].isHole)
                    {
                        powerUpPlacement = rng.Next(27);
                    }
                    else
                    {

                        WallList.Add(new Object(powerUpPlacement, ConsoleColor.Red, "@", true, false));
                        WallList.Add(new Object(powerUpPlacement + 1, ConsoleColor.Red, "@", true, false));
                        WallList.Add(new Object(powerUpPlacement + 2, ConsoleColor.Red, "@", true, false));
                    }
                }
            }
            if (Score > 25)
            {


                if (chanceOfPowerUp > 55)
                {
                    var powerUpPlacement = rng.Next(27);
                    if (WallList[powerUpPlacement].isPowerUp
                        && WallList[powerUpPlacement + 1].isPowerUp
                        && WallList[powerUpPlacement + 2].isPowerUp
                        && WallList[powerUpPlacement].isHole
                        && WallList[powerUpPlacement + 1].isHole
                        && WallList[powerUpPlacement + 2].isHole
                        )
                    {
                        powerUpPlacement = rng.Next(27);
                    }
                    else
                    {
                        WallList.Add(new Object(powerUpPlacement, ConsoleColor.White, "]", false, true));
                        WallList.Add(new Object(powerUpPlacement + 1, ConsoleColor.White, "]", false, true));
                        WallList.Add(new Object(powerUpPlacement + 2, ConsoleColor.White, "]", false, true));
                    }
                }
            }

        }
        //moves obstacles
        public void MoveObstacles()
        {
            //for each item in walllist
            foreach (Object wall in WallList)
            {
                //decrease x position 
                wall.X--;
                //if bird goes through hole
                if (wall.isHole
                    && wall.X == FlappyBird.X
                    && wall.Y == FlappyBird.Y)
                {
                    //if score is less than 15
                    if (Score < 15)
                    {
                        Score += 1;
                    }
                        //if less than 25 but greater than 15
                    else if (Score < 25
                        && Score >= 15)
                    {
                        Score += 2;
                    }
                        //otherwise
                    else
                    {
                        Score += 3;
                    }
                    //is not dead
                    isDead = false;
                }
                else if (wall.isPowerUp
                    && wall.X == FlappyBird.X
                    && wall.Y == FlappyBird.Y)
                {
                    isDead = false;
                    Score += 10;

                }
                else if(wall.isGhostPowerUp
                    && wall.X == FlappyBird.X
                    && wall.Y == FlappyBird.Y)
                {
                    ghostLives++;
                    isDead = false;

                }
                    //if it is not a whole
                else if (!wall.isHole
                    && wall.X == FlappyBird.X
                    && wall.Y == FlappyBird.Y)
                {
                    if (ghostLives == 0)
                    {
                        //bird is dead
                        isDead = true;
                    }
                    else
                    {
                        ghostLives--;
                        isDead = false;
                    }
                }

                
            }
            //orders list where the X cord is greater than 0
            WallList = WallList.Where(x => x.X > 0).ToList();
        }

        public void MoveBird()
        {
            //moves bird as long as it is in params
            if (FlappyBird.X > 1
                && FlappyBird.X < Console.WindowWidth -1
                && FlappyBird.Y > 1
                && FlappyBird.Y < Console.WindowHeight -1)
            {
                //if a key is pressed
                if (Console.KeyAvailable)
                {
                    //read it and assign it to var
                    ConsoleKey keyPressed = Console.ReadKey().Key;
                    //while it is available
                    while (Console.KeyAvailable)
                    {
                        //readkey with true so it doesnt echo input
                        Console.ReadKey(true);

                    }
                    //if up is pressed
                    if (keyPressed == ConsoleKey.UpArrow
                            && FlappyBird.Y < (Console.WindowHeight))
                    {
                            //decrease y by 2
                            FlappyBird.Y -= 2;
                            this.FlappyBird.Symbol = "^";
                        
                    }
                        //if down
                    else if (keyPressed == ConsoleKey.DownArrow
                        && FlappyBird.Y > 0)
                    {
                       
                            //increase y by 2
                            FlappyBird.Y += 2;
                            this.FlappyBird.Symbol = "▼";
                        
                    }
                        //if right
                    else if (keyPressed == ConsoleKey.RightArrow
                        && FlappyBird.X < Console.WindowWidth)
                    {
                        //increase x by 1
                        FlappyBird.X += 1;
                        FlappyBird.Symbol = ">";
                    }
                        //if left
                    else if (keyPressed == ConsoleKey.LeftArrow
                        && FlappyBird.X > 0)
                    {
                        //decrease by 2
                        FlappyBird.X -= 2;
                        FlappyBird.Symbol = "<";
                    }
                    else
                    {
                        //otherwise display invalid move
                        Console.WriteLine("Invalid Move");
                    }
                }
                    //if a key is not available
                else if (!(Console.KeyAvailable))
                {
                    //fall down
                    FlappyBird.Y++;
                    FlappyBird.Symbol = "▼";
                }
            }
                //if flappy bird is at the edge of boundaries
            else
            {
                //if it is at the top
                if (FlappyBird.Y <= 1)
                {
                    //make it bounce down
                    FlappyBird.Y++;
                }
                    //if it is at the bottom
                else if (FlappyBird.Y >= Console.WindowHeight - 1)
                {
                    //make it bounce up
                    FlappyBird.Y--;

                }
                    //if it is against the wall
                else if (FlappyBird.X <= 1)
                {
                    //make it bounce forward
                    FlappyBird.X++;
                }
            }
        }
        //draws game
        public void DrawGame()
        {
            //clears console
            Console.Clear();
            var ghostNoticeTimer = 5;
            if (ghostLives > 0)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                if (ghostNoticeTimer > 0)
                {
                    PrintAtPosition(20, 1, "Welcome to Ghost World!", ConsoleColor.Red);
                    
                }
                ghostNoticeTimer--;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            //draws flappybird object
            FlappyBird.Draw();
            //for every wall in list
            foreach (Object wall in WallList)
            {
                //draws wall
                wall.Draw();
            }
            //prints score in same place
            PrintAtPosition(20, 2, "Score: " + this.Score, ConsoleColor.Red);
            PrintAtPosition(20, 3, "Ghost Lives: " + this.ghostLives, ConsoleColor.Red);


        }
        //prints score
        public void PrintAtPosition(int x, int y, string text, ConsoleColor color)
        {
            //sets the cursor and text
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);

        }

        public void Greeting()
        {
            Console.WriteLine(@"         ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░
         ░░░░░░░░░░░░░████████████░░░░░░░░░░░░
         ░░░░░░░░░████░░░░░░██░░░░██░░░░░░░░░░
         ░░░░░░░██░░░░░░░░██░░░░░░░░██░░░░░░░░
         ░░░████████░░░░░░██░░░░░░██░░██░░░░░░
         ░░█░░░░░░░░██░░░░██░░░░░░██░░██░░░░░░
         ░░█░░░░░░░░░░██░░░░██░░░░░░░░██░░░░░░
         ░░█▒▒░░░░░░▒▒██░░░░░░████████████░░░░
         ░░░██▒▒▒▒▒▒██░░░░░░██▓▓▓▓▓▓▓▓▓▓▓▓██░░
         ░░░░░██████▒▒▒▒▒▒██▓▓████████████░░░░
         ░░░░░██▒▒▒▒▒▒▒▒▒▒▒▒██▓▓▓▓▓▓▓▓▓▓██░░░░
         ░░░░░░░████▒▒▒▒▒▒▒▒▒▒███████████░░░░░
         ░░░░░░░░░░░██████████░░░░░░░░░░░░░░░░
         ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░");
            LoopWords("\n\nWelcome to BouncyBird!\n\nCertainly not to be confused with FlappyBird.\nThe object of the game is simple:" +
            "\nFly your little bird through the gaps in the wall.\nUse the up, down, left, and right arrows to flap your wings.\nIf you don't you'll fall like a stone.\nYou will bounce off of boundary walls,\nand get " +
            "a point for each hole you fly through.\nBut be warned!\nThe higher your score, the closer the walls will get!\n\n\nPress enter to play!");
            Console.ReadLine();
            

        }

        public void ExitGreeting()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine(@"            ┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼
            ███▀▀▀██┼███▀▀▀███┼███▀█▄█▀███┼██▀▀▀
            ██┼┼┼┼██┼██┼┼┼┼┼██┼██┼┼┼█┼┼┼██┼██┼┼┼
            ██┼┼┼▄▄▄┼██▄▄▄▄▄██┼██┼┼┼▀┼┼┼██┼██▀▀▀
            ██┼┼┼┼██┼██┼┼┼┼┼██┼██┼┼┼┼┼┼┼██┼██┼┼┼
            ███▄▄▄██┼██┼┼┼┼┼██┼██┼┼┼┼┼┼┼██┼██▄▄▄
            ┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼
            ███▀▀▀███┼▀███┼┼██▀┼██▀▀▀┼██▀▀▀▀██▄┼
            ██┼┼┼┼┼██┼┼┼██┼┼██┼┼██┼┼┼┼██┼┼┼┼┼██┼
            ██┼┼┼┼┼██┼┼┼██┼┼██┼┼██▀▀▀┼██▄▄▄▄▄▀▀┼
            ██┼┼┼┼┼██┼┼┼██┼┼█▀┼┼██┼┼┼┼██┼┼┼┼┼██┼
            ███▄▄▄███┼┼┼─▀█▀┼┼─┼██▄▄▄┼██┼┼┼┼┼██▄
            ┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼██┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼██┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼████▄┼┼┼▄▄▄▄▄▄▄┼┼┼▄████┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼┼▀▀█▄█████████▄█▀▀┼┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼┼┼┼█████████████┼┼┼┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼┼┼┼██▀▀▀███▀▀▀██┼┼┼┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼┼┼┼██┼┼┼███┼┼┼██┼┼┼┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼┼┼┼█████▀▄▀█████┼┼┼┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼┼┼┼┼███████████┼┼┼┼┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼▄▄▄██┼┼█▀█▀█┼┼██▄▄▄┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼▀▀██┼┼┼┼┼┼┼┼┼┼┼██▀▀┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼┼┼▀▀┼┼┼┼┼┼┼┼┼┼┼▀▀┼┼┼┼┼┼┼┼┼┼┼
            ┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼");

            LoopWords("\nYou have crashed into a wall. Poor little fella.");
            Console.WriteLine("\nYour final score was: {0}\n\nToo bad you weren't a phoenix.", Score);
            Console.ReadLine();

        }

        public void LoopWords(string stringToLoop)
        {
            foreach (Char letter in stringToLoop)
            {
                Console.Write(letter);
                System.Threading.Thread.Sleep(30);
            }
        }

    }
}
