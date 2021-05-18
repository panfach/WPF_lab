using System;
using System.Collections.Generic;
using ClassLibrary;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase , IEnumerable<V3Data>
    {
        V3MainCollection mainCollection = new V3MainCollection();


        public MainViewModel()
        {
            mainCollection.PropertyChanged += PropertyChangedHandler;
        }

        public V3MainCollection MainCollection
        {
            get => mainCollection;
        }

        public bool HasChanged
        {
            get => mainCollection.HasChanged;
        }

        public float MaxDist
        {
            get => mainCollection.MaxDist;
        }


        public void AddDefaults()
        {
            mainCollection.AddDefaults();
        }

        public void AddRandomDataCollection()
        {
            mainCollection.AddRandomDataCollection();
        }

        public void AddRandomDataOnGrid()
        {
            mainCollection.AddRandomDataOnGrid();
        }

        public void Remove(object item)
        {
            mainCollection.Remove((V3Data)item);
        }

        public bool Save(string filename)
        {
            return mainCollection.Save(filename);
        }

        public bool Load(string filename)
        {
            return mainCollection.Load(filename);
        }

        public void Clear()
        {
            mainCollection.Clear();
        }

        public bool TryAddDataCollection(string filename)
        {
            V3DataCollection dataCollection;

            if (!(dataCollection = new V3DataCollection(filename)).incorrectFileRead)
            {
                mainCollection.Add(dataCollection);
                return true;
            }

            return false;
        }

        public bool IsDataCollection(object item) => item is V3DataCollection;

        public bool IsDataOnGrid(object item) => item is V3DataOnGrid;


        public void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }


        public IEnumerator<V3Data> GetEnumerator()
        {
            return mainCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return mainCollection.GetEnumerator();
        }
    }
}
