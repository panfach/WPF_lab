using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace ClassLibrary
{
    [Serializable]
    public struct DataItem: ISerializable
    {
        public Vector2 Coord { get; set; }
        public double Value { get; set; }

        public DataItem(Vector2 coord, double value)
        {
            Coord = coord;
            Value = value;
        }

        public DataItem(SerializationInfo info, StreamingContext streamingContext)
        {
            float x = info.GetSingle("C_X");
            float y = info.GetSingle("C_Y");
            Coord = new Vector2(x, y);
            Value = info.GetDouble("V");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("C_X", Coord.X);
            info.AddValue("C_Y", Coord.Y);
            info.AddValue("V", Value);
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
