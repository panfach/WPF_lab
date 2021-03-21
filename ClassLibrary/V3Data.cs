using System;
using System.Numerics;
using System.ComponentModel;

namespace ClassLibrary
{
    [Serializable]
    public abstract class V3Data: INotifyPropertyChanged
    {
        public string Info { get; set; }
        public DateTime Time { get; set; }

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;


        public V3Data() { }

        public V3Data(string info, DateTime time)
        {
            Info = info;
            Time = time;
        }

        public void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract Vector2[] Nearest(Vector2 v);
        public abstract string ToLongString();
        public abstract string ToLongString(string format);

        public override string ToString()
        {
            return Time + ": " + Info;
        }
    }
}
