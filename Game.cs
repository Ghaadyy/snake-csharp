using System;

namespace SnakeGame
{
    public class Game
    {
        private bool exit = false;
        private int score = 0;

        private Timer timer;
        private DateTime startTime;
        private TimeSpan time;

        private Queue<Point> snake = new Queue<Point>();
        private char[,] board = new char[25, 100];

        private Point fruitCoords;
        private Point head;

        private Direction direction = Direction.Up;

        public Game()
        {
            Random r = new Random();

            int fruitX = r.Next(1, this.board.GetLength(0) - 1), fruitY = r.Next(1, this.board.GetLength(1) - 1);
            fruitCoords = new Point(fruitX, fruitY);

            int x = this.board.GetLength(0) / 2;
            int y = this.board.GetLength(1) / 2;

            this.direction = Direction.Up;
            this.snake.Enqueue(new Point(x - 3, y));
            this.snake.Enqueue(new Point(x - 2, y));
            this.snake.Enqueue(new Point(x - 1, y));
            this.snake.Enqueue(new Point(x, y));

            head = new Point(x, y);
            timer = new Timer(TimerHandler, null, 0, 1000);
            startTime = DateTime.Now;
        }

        private void TimerHandler(object? o)
        {
            this.time = DateTime.Now - this.startTime;
        }

        public void Play()
        {
            bool valid = !exit && head.X != 0 && head.Y != 0 && head.X != this.board.GetLength(0) && head.Y != this.board.GetLength(1) && CountItems(new Point(head.X, head.Y)) < 2;

            while (valid)
            {
                Point point = new Point(head.X, head.Y);

                this.PrintScore();
                this.PrintBoard();
                this.GetDirection();
                this.MoveSnake();
                Console.WriteLine(direction);

                valid = !exit && head.X != 0 && head.Y != 0 && head.X != this.board.GetLength(0) - 1 && head.Y != this.board.GetLength(1) - 1 && CountItems(new Point(head.X, head.Y)) < 2;
                if (valid)
                {
                    Thread.Sleep(100);
                    Console.Clear();
                }
            }
            Console.WriteLine($"Game ended with a score of {this.score}");
        }

        private int CountItems(Point point)
        {
            int counter = 0;

            foreach (Point item in this.snake)
            {
                if (item.X == point.X && item.Y == point.Y)
                {
                    counter++;
                }
            }

            return counter;
        }

        private void PrintScore()
        {
            Console.Write("Score: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{this.score}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\t{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}\n");
        }

        private void ExtendSnake()
        {
            this.snake.Enqueue(new Point(-1, -1));
        }

        private void MoveSnake()
        {
            switch (this.direction)
            {
                case Direction.Up:
                    head.X++;
                    break;
                case Direction.Down:
                    head.X--;
                    break;
                case Direction.Left:
                    head.Y--;
                    break;
                case Direction.Right:
                    head.Y++;
                    break;
            }

            this.snake.Dequeue();
            this.snake.Enqueue(head);

            if (head.X == fruitCoords.X && head.Y == fruitCoords.Y)
            {
                this.score++;
                Random r = new Random();

                int fruitX = r.Next(1, this.board.GetLength(0) - 1), fruitY = r.Next(1, this.board.GetLength(1) - 1);
                this.ExtendSnake();
                this.fruitCoords.X = fruitX;
                this.fruitCoords.Y = fruitY;
            }
        }

        private void GetDirection()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey().Key;

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

        private void PrintQueue()
        {
            foreach (Point point in this.snake)
            {
                Console.WriteLine(point);
            }
        }

        private void PrintBoard()
        {
            for (int i = this.board.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    if (i == 0 || i == this.board.GetLength(0) - 1 || j == 0 || j == this.board.GetLength(1) - 1)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        if (SnakeInCell(new Point(i, j)))
                        {
                            if (i == head.X && j == head.Y)
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
                            if (i == this.fruitCoords.X && j == this.fruitCoords.Y)
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

        private bool SnakeInCell(Point coords)
        {
            foreach (Point snakeCoords in this.snake)
            {
                if (snakeCoords.X == coords.X && snakeCoords.Y == coords.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}