using ClassLibrary;

namespace ViewModel
{
    public static class DataConvertion
    {
        public static string DataOnGrid(object value)
        {
            if (value is V3DataOnGrid)
                return $"   SizeX: {((V3DataOnGrid)value).XGrid.Size}   SizeY: {((V3DataOnGrid)value).YGrid.Size}";
            else
                return " ";
        }

        public static string CoordDataItem(object value)
        {
            if (value is DataItem)
                return $"X: {((DataItem)value).Coord.X.ToString("F2")}  Y: {((DataItem)value).Coord.Y.ToString("F2")}";
            else
                return " ";
        }

        public static string ValueDataItem(object value)
        {
            if (value is DataItem)
                return $"Value: {((DataItem)value).Value.ToString("F2")}";
            else
                return " ";
        }
    }
}
