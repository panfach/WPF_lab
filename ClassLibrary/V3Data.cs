using System;
using System.Numerics;

namespace Lab
{
    abstract class V3Data
    {
        public string Info { get; set; }
        public DateTime Time { get; set; }

        public V3Data() { }

        public V3Data(string info, DateTime time)
        {
            Info = info;
            Time = time;
        }

        public abstract Vector2[] Nearest(Vector2 v);
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
        //public abstract static explicit operator V3DataCollection(V3DataOnGrid inp);
        /*public static explicit operator V3DataCollection(V3Data inp)
        {
            if (inp.GetType() == typeof(V3DataOnGrid)) return (V3DataCollection)((V3DataOnGrid)inp);
            else return (V3DataCollection)inp;
        }*/

        public override string ToString()
        {
            return Time + ": " + Info;
        }
    }
}
