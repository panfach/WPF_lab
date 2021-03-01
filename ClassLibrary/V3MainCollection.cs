using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace Lab
{
    class V3MainCollection : IEnumerable<V3Data>
    {
        List<V3Data> data;

        public int Count
        {
            get => data.Count;
        }

        public int MinItems
        {
            get
            {
                return data.Min(
                    x => (x.GetType() == typeof(V3DataOnGrid)) ? ((V3DataCollection)(V3DataOnGrid)x).items.Count : ((V3DataCollection)x).items.Count
                );
            }
        }

        public float MaxDist
        {
            get
            {
                var dataContainers = from item in data
                                     let _item = (item.GetType() == typeof(V3DataOnGrid)) ? (V3DataCollection)(V3DataOnGrid)item : (V3DataCollection)item
                                     select _item;

                var squaredDistances = from item1 in dataContainers
                                       from item2 in dataContainers
                                       from first in item1.items
                                       from second in item2.items
                                       select Vector2.DistanceSquared(first.Coord, second.Coord);

                float maxDistance = (squaredDistances == null || !squaredDistances.Any()) ? 0f : squaredDistances.Max();

                return (float)Math.Sqrt(maxDistance);
            }
        }

        public IEnumerable<DataItem> GetRepetitiveItems
        {
            get
            {
                var dataContainers = from item in data
                                     let _item = (item.GetType() == typeof(V3DataOnGrid)) ? (V3DataCollection)(V3DataOnGrid)item : (V3DataCollection)item
                                     select _item;

                var repetitive = from item in dataContainers
                                 from dataItem in item
                                 group dataItem by dataItem.Coord into rep
                                 let c = rep.Count()
                                 where c > 1
                                 from _item in rep
                                 select _item;

                return repetitive;
            }
        }

        public V3MainCollection()
        {
            data = new List<V3Data>();
        }

        public void Add(V3Data item)
        {
            data.Add(item);
        }

        public bool Remove(string id, DateTime date)
        {
            bool presence = false;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Info == id && data[i].Time.Hour == date.Hour)
                {
                    data.RemoveAt(i);
                    presence = true;
                    i--;
                }
            }
            return presence;
        }

        public void AddDefaults()
        {
            AddRandomDataOnGrid("DEFAULTDATA" + data.Count, DateTime.Now, new Grid1D(1f, 2), new Grid1D(1f, 2), 0f, 10f);

            AddRandomDataOnGrid("DEFAULTDATA" + data.Count, DateTime.Now, new Grid1D(1f, 0), new Grid1D(1f, 0), 0f, 10f); // 0 точек

            AddRandomDataCollection("DEFAULTDATA" + data.Count, DateTime.Now, 2, 4f, 4f, 0f, 10f);

            AddRandomDataCollection("DEFAULTDATA" + data.Count, DateTime.Now, 0, 4f, 4f, 0f, 10f); // 0 точек
        }

        public void AddRandomDataOnGrid(string info, DateTime time, Grid1D xGrid, Grid1D yGrid, double minValue, double maxValue)
        {
            V3DataOnGrid item = new V3DataOnGrid
            (
                info,
                time,
                xGrid,
                yGrid
            );

            item.InitRandom(minValue, maxValue);
            data.Add(item);
        }

        public void AddRandomDataCollection(string info, DateTime time, int nItems, float maxXCoord, float maxYCoord, double minValue, double maxValue)
        {
            V3DataCollection item = new V3DataCollection
            (
                info,
                time
            );

            item.InitRandom(nItems, maxXCoord, maxYCoord, minValue, maxValue);
            data.Add(item);
        }

        public override string ToString()
        {
            return ToString("F2");
        }

        public string ToString(string format = "F2")
        {
            List<string> strings = new List<string>();
            foreach (V3Data _data in data)
            {
                strings.Add(_data.ToLongString(format));
            }
            return "################# V3MainCollection ###################################################\n\n" + 
                   string.Join('\n', strings) + 
                   "\n############## end of main collection ################################################\n";
        }

        public IEnumerator<V3Data> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}
