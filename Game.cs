using System;

namespace SnakeGame
{
    public class Game
    {
        public enum Direction
        {
            Up, Down, Left, Right
        }

        public bool exit = false;
        public int score = 0;

        public Queue<Tuple<int, int>> snake = new Queue<Tuple<int, int>>();
        public char[,] board = new char[25, 100];

        public Tuple<int, int> fruitCoords;
        public int x, y;

        public Direction direction = Direction.Up;

        public Game()
        {
            Random r = new Random();

            int fruitX = r.Next(1, this.board.GetLength(0) - 1), fruitY = r.Next(1, this.board.GetLength(1) - 1);
            fruitCoords = Tuple.Create(fruitX, fruitY);

            x = this.board.GetLength(0) / 2;
            y = this.board.GetLength(1) / 2;

            this.direction = Direction.Up;
            this.snake.Enqueue(Tuple.Create(x + 3, y));
            this.snake.Enqueue(Tuple.Create(x + 2, y));
            this.snake.Enqueue(Tuple.Create(x + 1, y));
            this.snake.Enqueue(Tuple.Create(x, y));
        }

        public void Play()
        {
            Tuple<int, int> tp = Tuple.Create(x, y);
            bool valid = !exit && x != 0 && y != 0 && x != this.board.GetLength(0) && y != this.board.GetLength(1) && CountItems(tp) < 2;

            while (valid)
            {
                tp = Tuple.Create(x, y);

                this.PrintScore();
                this.PrintBoard();
                this.GetDirection();
                this.MoveSnake();
                Console.WriteLine(direction);
                // this.PrintQueue();

                valid = !exit && x != 0 && y != 0 && x != this.board.GetLength(0) - 1 && y != this.board.GetLength(1) - 1 && CountItems(tp) < 2;

                Thread.Sleep(100);
                if (valid)
                    Console.Clear();
            }
            Console.WriteLine($"Game ended with a score of {this.score}");
        }

        public int CountItems(Tuple<int, int> tuple)
        {
            int counter = 0;

            foreach (Tuple<int, int> item in this.snake)
            {
                if (item.Item1 == tuple.Item1 && item.Item2 == tuple.Item2)
                {
                    counter++;
                }
            }

            return counter;
        }

        public void PrintScore()
        {
            Console.Write("Score: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{this.score}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void ExtendSnake()
        {
            this.snake.Enqueue(Tuple.Create(-1, -1));
        }

        public void MoveSnake()
        {
            switch (this.direction)
            {
                case Direction.Up:
                    x--;
                    break;
                case Direction.Down:
                    x++;
                    break;
                case Direction.Left:
                    y--;
                    break;
                case Direction.Right:
                    y++;
                    break;
            }

            this.snake.Dequeue();
            this.snake.Enqueue(Tuple.Create(x, y));

            if (x == fruitCoords.Item1 && y == fruitCoords.Item2)
            {
                this.score++;
                Random r = new Random();

                int fruitX = r.Next(1, this.board.GetLength(0) - 1), fruitY = r.Next(1, this.board.GetLength(1) - 1);
                this.ExtendSnake();
                this.fruitCoords = Tuple.Create(fruitX, fruitY);
            }
        }

        public void GetDirection()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow && direction != Direction.Down)
                    this.direction = Direction.Up;
                else if (key == ConsoleKey.LeftArrow && direction != Direction.Right)
                    this.direction = Direction.Left;
                else if (key == ConsoleKey.RightArrow && direction != Direction.Left)
                    this.direction = Direction.Right;
                else if (key == ConsoleKey.DownArrow && direction != Direction.Up)
                    this.direction = Direction.Down;
                else if (key == ConsoleKey.UpArrow && direction == Direction.Down || key == ConsoleKey.LeftArrow && direction == Direction.Right || key == ConsoleKey.RightArrow && direction == Direction.Left || key == ConsoleKey.DownArrow && direction == Direction.Up)
                { }
                else
                    this.exit = true;
            }
        }

        public void PrintQueue()
        {
            foreach (Tuple<int, int> tuple in this.snake)
            {
                Console.WriteLine(tuple);
            }
        }

        public void PrintBoard()
        {
            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    if (i == 0 || i == this.board.GetLength(0) - 1 || j == 0 || j == this.board.GetLength(1) - 1)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        if (SnakeInCell(Tuple.Create(i, j)))
                        {
                            if (i == x && j == y)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("*");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("*");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            if (i == this.fruitCoords.Item1 && j == this.fruitCoords.Item2)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("$");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.Write(" ");
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool SnakeInCell(Tuple<int, int> coords)
        {
            foreach (Tuple<int, int> snakeCoords in this.snake)
            {
                if (snakeCoords.Item1 == coords.Item1 && snakeCoords.Item2 == coords.Item2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}