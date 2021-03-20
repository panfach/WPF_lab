using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Numerics;
using System.Linq;

namespace ClassLibrary
{
    public class V3MainCollection : IEnumerable<V3Data>, INotifyCollectionChanged
    {
        List<V3Data> data;

        public bool HasChanged { get; set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged;


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
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            HasChanged = false;
        }

        public V3MainCollection(List<V3Data> _data)
        {
            data = _data;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            HasChanged = true;
        }


        public void Add(V3Data item)
        {
            data.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            HasChanged = true;
        }

        public bool Remove(V3Data item)
        {
            if (data.Remove(item))
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                HasChanged = true;

                return true;
            }

            return false;
        }

        public bool Remove(string id, DateTime date)
        {
            bool presence = false;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Info == id && data[i].Time.Hour == date.Hour)
                {
                    data.RemoveAt(i);
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    HasChanged = true;
                    presence = true;
                    i--;
                }
            }
            return presence;
        }

        public void AddDefaults()
        {
            AddDataOnGrid("DEFAULTDATA" + data.Count, DateTime.Now, new Grid1D(1f, 2), new Grid1D(1f, 2), 0f, 10f);

            AddDataOnGrid("DEFAULTDATA" + data.Count, DateTime.Now, new Grid1D(1f, 0), new Grid1D(1f, 0), 0f, 10f); // 0 точек

            AddDataCollection("DEFAULTDATA" + data.Count, DateTime.Now, 2, 4f, 4f, 0f, 10f);

            AddDataCollection("DEFAULTDATA" + data.Count, DateTime.Now, 0, 4f, 4f, 0f, 10f); // 0 точек
        }

        public void AddRandomDataOnGrid()
        {
            AddDataOnGrid("DEFAULTDATA" + data.Count, DateTime.Now, new Grid1D(1f, 2), new Grid1D(1f, 2), 0f, 10f);
        }

        public void AddDataOnGrid(string info, DateTime time, Grid1D xGrid, Grid1D yGrid, double minValue, double maxValue)
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
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            HasChanged = true;
        }

        public void AddRandomDataCollection()
        {
            AddDataCollection("DEFAULTDATA" + data.Count, DateTime.Now, 2, 4f, 4f, 0f, 10f);
        }

        public void AddDataCollection(string info, DateTime time, int nItems, float maxXCoord, float maxYCoord, double minValue, double maxValue)
        {
            V3DataCollection item = new V3DataCollection
            (
                info,
                time
            );

            item.InitRandom(nItems, maxXCoord, maxYCoord, minValue, maxValue);
            data.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            HasChanged = true;
        }

        public bool Save(string filename)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.Open(filename, FileMode.OpenOrCreate);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, data);
                HasChanged = false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public bool Load(string filename)
        {
            FileStream fileStream = null;
            List<V3Data> result = null;

            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                result = binaryFormatter.Deserialize(fileStream) as List<V3Data>;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            data = result;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            HasChanged = false;

            return true;
        }

        public void Clear()
        {
            data.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            HasChanged = false;
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
