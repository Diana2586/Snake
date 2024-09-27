namespace Tema2
{
    internal class SnakeGame
    {
        public Queue<Coordinate> Snake { get; set; }
        public char[,] Grid {get; set;}
        public int GridSize { get; set;}
        public Coordinate Apple { get; set; }
        private bool IsFinished;
        private string LastMove;
        private bool IsAppleGone;

        public SnakeGame()
        {
            InitializeGrid();
        }

        public void StartGame()
        {
            var command = "";
            ShowGrid();

            while (!IsFinished)
            {
                Console.WriteLine("Introdu o comanda: ");
                command = Console.ReadLine();

                if(command != null)
                {
                    command = command.ToUpper();
                    Move(command);
                    ShowGrid();
                }
            } 

            Console.WriteLine("Game Over");
        }

        private void Move(string move)
        {
            var isFirst = true;
            var placeholder = new Coordinate(0, 0);
            var count = 0; 

            foreach(var coordinate in Snake)
            {
                //Muta capul sarpelui in functie de comanda primita
                if (isFirst)
                {
                    placeholder = new Coordinate(coordinate.X, coordinate.Y);

                    switch (move)
                    {
                        case "W":
                            if(CheckMove(new Coordinate(coordinate.X - 1, coordinate.Y), "W"))
                            {
                                coordinate.X -= 1;
                            }
                            else
                            {
                                return;
                            }
                            break;

                        case "A":
                            if (CheckMove(new Coordinate(coordinate.X, coordinate.Y - 1), "A"))
                            {
                                coordinate.Y -= 1;
                            }
                            else
                            {
                                return;
                            }
                            break;

                        case "S":
                            if (CheckMove(new Coordinate(coordinate.X + 1, coordinate.Y), "S"))
                            {
                                coordinate.X += 1;
                            }
                            else
                            {
                                return;
                            }
                            break;

                        case "D":
                            if (CheckMove(new Coordinate(coordinate.X, coordinate.Y + 1), "D"))
                            {
                                coordinate.Y += 1;
                            }
                            else
                            {
                                return;
                            }
                            break;

                        default:
                            Console.WriteLine("Invalid Comand!");
                            return;
                    }

                    LastMove = move;
                    Grid[coordinate.X, coordinate.Y] = 'H';
                    isFirst = false;
                    count++;
                }
                // Se muta restul coordonatelor dupa cap
                else
                {
                    var aux = new Coordinate(coordinate.X, coordinate.Y);
                    coordinate.X = placeholder.X;
                    coordinate.Y = placeholder.Y;
                    placeholder.X = aux.X;
                    placeholder.Y = aux.Y;
                    Grid[coordinate.X, coordinate.Y] = 'X';
                    count++;

                    if(count == Snake.Count)
                    {
                        Grid[placeholder.X, placeholder.Y] = ' ';
                    }
                }
            }

            //Daca A fost mancat marul, se adauga un X la finalul cozii
            if (IsAppleGone)
            {
                Snake.Enqueue(new Coordinate(placeholder.X, placeholder.Y));
                Grid[placeholder.X, placeholder.Y] = 'X';
                SetApple();
                IsAppleGone = false;
            }
        }

        private bool CheckMove(Coordinate coordinate, string currentMove)
        {
            if((currentMove == "W" && LastMove == "S")
                || (currentMove == "S" && LastMove == "W")
                || (currentMove == "A" && LastMove == "D")
                || (currentMove == "D" && LastMove == "A"))
            {
                Console.WriteLine("Invalid Move");
                return false;
            }
            else if (Grid[coordinate.X, coordinate.Y] == 'A')
            {
                IsAppleGone = true;
                return true;
            }
            else if(Grid[coordinate.X, coordinate.Y] != ' ')
            {
                IsFinished = true;
                return false;
            }

            return true;
        }

        private void InitializeGrid()
        {
            IsFinished = false;
            GridSize = 9;
            Snake = new Queue<Coordinate>();
            Snake.Enqueue(new Coordinate(2, 5));
            Snake.Enqueue(new Coordinate(1, 5));
            Grid = new char[GridSize, GridSize];
            LastMove = "S";
            IsAppleGone = false;

            for(var i = 0; i < GridSize; i++)
            {
                for(var j = 0; j < GridSize; j++)
                {
                    if((i == 0 || i == GridSize - 1) && (j > 0 && j < GridSize - 1))
                    {
                        Grid[i, j] = '-';
                    }
                    else if((j == 0 || j == GridSize - 1) && (i > 0 && i < GridSize - 1))
                    {
                        Grid[i, j] = '|';
                    }
                    else
                    {
                        Grid[i, j] = ' ';
                    }
                }
            }

            var isHead = 0;

            foreach (var coordinate in Snake)
            {
                if (isHead == 0)
                    Grid[coordinate.X, coordinate.Y] = 'H';
                else
                    Grid[coordinate.X, coordinate.Y] = 'X';
                isHead++;
            }

            SetApple();
            Grid[Apple.X, Apple.Y] = 'A';
        }

        private void ShowGrid()
        {
            for (var i = 0; i < GridSize; i++)
            {
                for(var j = 0; j < GridSize; j++)
                {
                    Console.Write(Grid[i, j]);
                }
                Console.WriteLine();
            }
        }

        private void SetApple()
        {
            Random random = new Random();
            var x = random.Next(1, 8);
            var y = random.Next(1, 8);

            while (Grid[x, y] != ' ')
            {
                x = random.Next(1, 8);
                y = random.Next(1, 8);
            }

            Apple = new Coordinate(x, y);
            Grid[Apple.X, Apple.Y] = 'A';
        }
    }
}
