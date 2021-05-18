using System;
using Xunit;

namespace ViewModel.Tests
{
    public class MainViewModelTests
    {
        [Fact]
        public void IsDataCollection()
        {
            var instance = new MainViewModel();
            var a = new ClassLibrary.V3DataCollection("info1", DateTime.Now);
            var b = new object();
            Assert.True(instance.IsDataCollection(a));
            Assert.False(instance.IsDataCollection(b));
        }

        [Fact]
        public void IsDataOnGrid()
        {
            var instance = new MainViewModel();
            var a = new ClassLibrary.V3DataOnGrid("info1", DateTime.Now, new ClassLibrary.Grid1D(1,2), new ClassLibrary.Grid1D(1,4));
            var b = new object();
            Assert.True(instance.IsDataOnGrid(a));
            Assert.False(instance.IsDataOnGrid(b));
        }

        [Fact]
        public void TryAddDataCollectionFromIncorrectFile()
        {
            var instance = new MainViewModel();
            var filename1 = "iajefiuhau3hfbahjbrfh";
            Assert.False(instance.TryAddDataCollection(filename1));
        }
    }

    public class DataConvertionTests
    {
        [Fact]
        public void DataOnGrid()
        {
            var data1 = new ClassLibrary.V3DataOnGrid("info1", DateTime.Now, new ClassLibrary.Grid1D(1, 2), new ClassLibrary.Grid1D(1, 4));
            var data2 = new object();
            Assert.Equal("   SizeX: 2   SizeY: 4", DataConvertion.DataOnGrid(data1));
            Assert.Equal(" ", DataConvertion.DataOnGrid(data2));
        }
    }
}
