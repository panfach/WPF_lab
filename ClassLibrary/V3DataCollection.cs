using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using System.Globalization;
//using System.Culture

namespace Lab
{
    class V3DataCollection : V3Data, IEnumerable<DataItem>
    {
        public List<DataItem> items { get; set; }
        CultureInfo cultInfo = new CultureInfo("ru-RU"); // 

        public V3DataCollection(string info, DateTime time) : base(info, time)
        {
            items = new List<DataItem>();
        }

        // Формат файла:
        // Первая строка: текст Info
        // Вторая строка: Элементы даты в следующем порядке: день, месяц, год, час, минута, секунда.
        // Между каждым элементом должен был ровно один разделитель (Пробел, двоеточие или точка).
        // Следующие строки хранят данные об измерениях.
        // На каждой строке располагается данные об одном измерении.
        // Порядок расположения элементов измерения: координата х, координата y, значение.                     Разделителем в числе с плавающей точкой является запятая.
        // Между элементами ровно один разделитель точка с запятой.
        // В строках с данными об измерениях могут находиться пробелы по желанию
        public V3DataCollection(string filename)
        {
            Vector2 coord;
            double value;
            string line;
            string[] lineValues;

            items = new List<DataItem>();

            string path = Path.Combine($"..\\..\\..\\{filename}");

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    if ((line = reader.ReadLine()) == null) Info = "DEFAULTNAME";
                    else Info = line.Trim();
     
                    if ((line = reader.ReadLine()) == null) Time = DateTime.Now;
                    else
                    {
                        lineValues = line.Split('.', ':', ' ');
                        Time = new DateTime(
                            int.Parse(lineValues[2]),
                            int.Parse(lineValues[1]),
                            int.Parse(lineValues[0]),
                            int.Parse(lineValues[3]),
                            int.Parse(lineValues[4]),
                            int.Parse(lineValues[5])
                        );
                    }

                    while (true)
                    {
                        if ((line = reader.ReadLine()) == null) break;

                        line = line.Trim();
                        lineValues = line.Split(';');
                   
                        coord = new Vector2(float.Parse(lineValues[0], cultInfo), float.Parse(lineValues[1], cultInfo));
                        value = double.Parse(lineValues[2], cultInfo);
                        items.Add(new DataItem(coord, value));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {
            Vector2 coord;
            double value;

            for (int i = 0; i < nItems; i++)
            {
                coord = new Vector2((float)Rand.Double(xmax), (float)Rand.Double(ymax));
                value = Rand.Double(minValue, maxValue);

                items.Add(new DataItem(coord, value));
            }
        }

        public override Vector2[] Nearest(Vector2 point)
        {
            float dist, minDist = float.MaxValue;
            List<Vector2> minPoints = new List<Vector2>();

            foreach (DataItem item in items)
            {
                if ((dist = Vector2.DistanceSquared(item.Coord, point)) == minDist)
                {
                    minPoints.Add(item.Coord);
                }
                else if (dist < minDist)
                {
                    minPoints.Clear();
                    minDist = dist;
                    minPoints.Add(item.Coord);
                }
            }

            return minPoints.ToArray();
        }

        public IEnumerator<DataItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return "V3DataCollection: " + base.ToString() + " (" + items.Count + " items)";
        }

        public override string ToLongString()
        {
            return ToLongString("F2");
        }

        public override string ToLongString(string format)
        {
            string gridValues = "";
            foreach (DataItem item in items)
            {
                gridValues += item.ToString(format) + "\n";
            }
            return "V3DataCollection: " + base.ToString() + " (" + items.Count + " items)\n" + gridValues;
        }
    }
}
