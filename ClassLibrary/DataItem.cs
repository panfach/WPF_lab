using System;
using System.Numerics;

namespace Lab
{
    struct DataItem
    {
        public Vector2 Coord { get; set; }
        public double Value { get; set; }

        public DataItem(Vector2 coord, double value)
        {
            Coord = coord;
            Value = value;
        }

        public override string ToString()
        {
            return "[" + Math.Round(Coord.X, 2) + ", " + Math.Round(Coord.Y, 2) + "] : " + Math.Round(Value, 5);
        }

        public string ToString(string format)
        {
            return $"[{Coord.X.ToString(format)}, {Coord.Y.ToString(format)}] : {Value.ToString(format)}";
        }
    }
}
