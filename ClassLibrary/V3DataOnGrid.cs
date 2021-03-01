using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Lab
{
    class V3DataOnGrid : V3Data, IEnumerable<DataItem>
    {
        public Grid1D XGrid { get; set; }
        public Grid1D YGrid { get; set; }
        public double[,] Value { get; set; }

        public V3DataOnGrid(string info, DateTime time, Grid1D xGrid, Grid1D yGrid) : base(info, time)
        {
            XGrid = xGrid;
            YGrid = yGrid;
        }

        public Vector2 GetCoord(int x, int y)
        {
            return new Vector2(x * XGrid.Step, y * YGrid.Step);
        }

        public void InitRandom(double minValue, double maxValue)
        {
            Value = new double[XGrid.Size, YGrid.Size];

            for (int i = 0; i < XGrid.Size; i++)
            {
                for (int j = 0; j < YGrid.Size; j++)
                {
                    Value[i, j] = Rand.Double(maxValue, minValue);
                }
            }
        }

        // Оператор преобразования
        public static explicit operator V3DataCollection(V3DataOnGrid inp)
        {
            V3DataCollection outp = new V3DataCollection(inp.Info, inp.Time);
            List<DataItem> items = new List<DataItem>();
            for (int i = 0; i < inp.XGrid.Size; i++)
            {
                for (int j = 0; j < inp.YGrid.Size; j++)
                {
                    items.Add(new DataItem(new Vector2(i * inp.XGrid.Step, j * inp.YGrid.Step), inp.Value[i, j]));
                }
            }
            outp.items = items;
            return outp;
        }

        public override Vector2[] Nearest(Vector2 point)
        {
            float x = point.X / XGrid.Step;
            float y = point.Y / YGrid.Step;

            List<float> xList = new List<float>();
            List<float> yList = new List<float>();

            // На случай, если point вне сетки
            x = Math.Clamp(x, 0, XGrid.Size - 1);
            y = Math.Clamp(y, 0, YGrid.Size - 1);

            // Если число оканчивается на 0.5, то по координате х есть две ближайшие точки
            if (x % 0.5f == 0 && x % 1f != 0)
            {
                // Сначала добавляется точка с наименьшим х (Для этого вычитается 0.5f, а потом округляется к целому)
                xList.Add((float)(Math.Round(x - 0.5f) * XGrid.Step));
                // А после вторая точка
                xList.Add(xList[0] + XGrid.Step);
            }
            else
            {
                xList.Add((float)(Math.Round(x) * XGrid.Step));
            }

            // Если число оканчивается на 0.5, то по координате y есть две ближайшие точки
            if (y % 0.5f == 0 && y % 1f != 0)
            {
                // Сначала добавляется точка с наименьшим y (Для этого вычитается 0.5f, а потом округляется к целому)
                yList.Add((float)(Math.Round(y - 0.5f) * YGrid.Step));
                // А после вторая точка
                yList.Add(yList[0] + YGrid.Step);
            }
            else
            {
                yList.Add((float)(Math.Round(y) * YGrid.Step));
            }

            List<Vector2> nearest = new List<Vector2>();
            for (int i = 0; i < xList.Count; i++)
            {
                for (int j = 0; j < yList.Count; j++)
                {
                    nearest.Add(new Vector2(xList[i], yList[j]));
                }
            }

            return nearest.ToArray();
        }

        public IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < XGrid.Size; i++)
            {
                for (int j = 0; j < YGrid.Size; j++)
                {
                    yield return new DataItem(new Vector2(i * XGrid.Step, j * YGrid.Step), Value[i, j]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return $"V3DataOnGrid: {base.ToString()} X{XGrid} Y{YGrid}";
        }

        public override string ToLongString()
        {
            return ToLongString("F2");
        }

        public override string ToLongString(string format)
        {
            string gridValues = "";
            for (int i = 0; i < XGrid.Size; i++)
            {
                for (int j = 0; j < YGrid.Size; j++)
                {
                    gridValues += $"[{i * XGrid.Step}, {j * YGrid.Step}] : {Value[i, j].ToString(format)}\n";
                }
            }
            return $"V3DataOnGrid: {base.ToString()} X{XGrid} Y{YGrid}\n{gridValues}";
        }
    }
}
