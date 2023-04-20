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
            fruitCoords = this.generateFruit();

            this.fillSnake();

            timer = new Timer(TimerHandler, null, 0, 1000);
            startTime = DateTime.Now;
        }

        private void fillSnake()
        {
            // x is represented alongside the j axis on the board
            // y is represented alongside the i axis on the board

            int x = this.board.GetLength(1) / 2;
            int y = this.board.GetLength(0) / 2;

            // Append the end of the snake first so it gets dequeued first
            this.snake.Enqueue(new Point(x, y - 3));
            this.snake.Enqueue(new Point(x, y - 2));
            this.snake.Enqueue(new Point(x, y - 1));
            this.snake.Enqueue(new Point(x, y));

            head = new Point(x, y); // Keep track of where the head of the snake is
        }

        private Point generateFruit()
        {
            Random r = new Random();

            int fruitX = r.Next(1, this.board.GetLength(1) - 1), fruitY = r.Next(1, this.board.GetLength(0) - 1);

            return new Point(fruitX, fruitY);
        }

        private void TimerHandler(object? o)
        {
            this.time = DateTime.Now - this.startTime;
        }

        private bool gameState()
        {
            bool xConditions = head.X != 0 && head.X != this.board.GetLength(1) - 1;
            bool yConditions = head.Y != 0 && head.Y != this.board.GetLength(0) - 1;
            bool noOverlaps = CountItems(new Point(head.X, head.Y)) < 2;

            return !exit && xConditions && yConditions && noOverlaps;
        }

        public void Play()
        {
            do
            {
                Point point = new Point(head.X, head.Y);

                this.PrintScore();
                this.PrintBoard();
                this.GetDirection();
                this.MoveSnake();
                Console.WriteLine(direction);

                Thread.Sleep(100);
                Console.Clear();
            } while (gameState());

            this.PrintScore();
            this.PrintBoard();
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
                    head.Y++;
                    break;
                case Direction.Down:
                    head.Y--;
                    break;
                case Direction.Left:
                    head.X--;
                    break;
                case Direction.Right:
                    head.X++;
                    break;
            }

            this.snake.Dequeue();
            this.snake.Enqueue(head);

            if (head.X == fruitCoords.X && head.Y == fruitCoords.Y)
            {
                this.score++;
                this.ExtendSnake();
                this.fruitCoords = this.generateFruit();
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

        private static void CustomPrint(char character, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"{character}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void PrintBoard()
        {
            // Print the rows upside down so we represent correctly the (x, y) coordinates
            for (int i = this.board.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    if (i == 0 || i == this.board.GetLength(0) - 1 || j == 0 || j == this.board.GetLength(1) - 1)
                        Console.Write("#");
                    else
                    {
                        if (SnakeInCell(new Point(j, i)))
                        {
                            if (i == head.Y && j == head.X)
                                CustomPrint('*', ConsoleColor.Red);
                            else
                                CustomPrint('*', ConsoleColor.Green);
                        }
                        else
                        {
                            if (i == this.fruitCoords.Y && j == this.fruitCoords.X)
                                CustomPrint('$', ConsoleColor.Yellow);
                            else
                                Console.Write(" ");
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}