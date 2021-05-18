using System;
using System.Numerics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;

namespace ViewModel
{
    public class DataItemCreator : IDataErrorInfo, INotifyPropertyChanged
    {
        public V3DataCollection source;
        float xCoord, yCoord, value;

        public float X
        {
            get => xCoord;
            set
            {
                xCoord = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
            }
        }

        public float Y
        {
            get => yCoord;
            set
            {
                yCoord = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
            }
        }

        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public Vector2 Coord => new Vector2(xCoord, yCoord);

        public event PropertyChangedEventHandler PropertyChanged;

        public DataItemCreator(object _source)
        {
            source = _source as V3DataCollection;
        }


        public void AddDataItem()
        {
            source.Add(new DataItem(new Vector2(X, Y), Value));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
        }

        public string Error { get { return "Invalid input data"; } }

        public string this[string property]
        {
            get
            {
                string msg = null;
                Vector2[] nearest;
                switch (property)
                {
                    case "X":
                    case "Y":
                        // AddDefaults в MainCollection создает измерения с координатами с двумя знаками после запятой (В случае DataCollection). Поэтому этот случай можно легко проверить
                        if (source != null)
                        {
                            if ((nearest = source.Nearest(Coord)).Length > 0 && Equals(nearest[0], Coord)) msg = "Measuring point must not be repeated";
                        }
                        else msg = "source == null (Data on grid is selected)";
                        break;
                    case "Value":
                        if (Value <= 0f) msg = "Value is less than zero";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }
    }
}
