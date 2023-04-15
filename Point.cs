using System;
using System.Diagnostics.CodeAnalysis;

namespace SnakeGame
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }
    }
}

