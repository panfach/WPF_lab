using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using Microsoft.Win32;
using ClassLibrary;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        V3MainCollection mainCollection = new V3MainCollection();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void New(object sender, RoutedEventArgs e)
        {
            New();
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        public void Load(object sender, RoutedEventArgs e)
        {
            Load();
        }

        public void AddDefaults(object sender, RoutedEventArgs e)
        {
            mainCollection.AddDefaults();
        }

        public void AddDefaultDataCollection(object sender, RoutedEventArgs e)
        {
            mainCollection.AddRandomDataCollection();
        }

        public void AddDefaultDataOnGrid(object sender, RoutedEventArgs e)
        {
            mainCollection.AddRandomDataOnGrid();
        }

        public void AddElementFromFile(object sender, RoutedEventArgs e)
        {
            AddElementFromFile();
        }

        public void Remove(object sender, RoutedEventArgs e)
        {
            if (!mainCollection.Remove((V3Data)listBox_Main.SelectedItem)) 
                MessageBox.Show("Выберите элемент в Main Collection, который хотите удалить.", "Remove", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            mainCollection.CollectionChanged += CollectionChangedHandler;
        }

        public void OnClosing(object sender, CancelEventArgs e)
        {
            if (!CheckChangedDataConditions()) e.Cancel = true;
        }


        void AddElementFromFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            V3DataCollection dataCollection;
            string dirPath = System.IO.Path.Combine("..\\..\\..\\SaveFiles");
            dlg.InitialDirectory = System.IO.Path.GetFullPath(dirPath);
            dlg.RestoreDirectory = true;
            dlg.Filter = "Text file (*.txt)|*.txt";

            if (dlg.ShowDialog() == false)
            {
                return;
            }
            else if (!(dataCollection = new V3DataCollection(dlg.FileName)).incorrectFileRead)
            {
                mainCollection.Add(dataCollection);
                return;
            }

            MessageBox.Show("Невозможно прочитать выбранный файл. Формат данных некорректен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void New()
        {
            if (!CheckChangedDataConditions()) return;

            mainCollection.Clear();
        }

        bool Save()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string dirPath = System.IO.Path.Combine("..\\..\\..\\SaveFiles");
            dlg.InitialDirectory = System.IO.Path.GetFullPath(dirPath);
            dlg.RestoreDirectory = true;
            dlg.Filter = "Data list save (*.datalist)|*.datalist|Text file(*.txt) | *.txt";

            if (dlg.ShowDialog() == false)
                return false;
            else if (mainCollection.Save(dlg.FileName))
                return true;

            MessageBox.Show("Что-то пошло не так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        bool Load()
        {
            if (!CheckChangedDataConditions()) return false;

            OpenFileDialog dlg = new OpenFileDialog();
            string dirPath = System.IO.Path.Combine("..\\..\\..\\SaveFiles");
            dlg.InitialDirectory = System.IO.Path.GetFullPath(dirPath);
            dlg.RestoreDirectory = true;
            dlg.Filter = "Data list save (*.datalist)|*.datalist|Text file (*.txt)|*.txt";

            if (dlg.ShowDialog() == false)
                return false;
            else if (mainCollection.Load(dlg.FileName))
                return true;

            MessageBox.Show("Что-то пошло не так", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            DataContext = null;
            DataContext = mainCollection;
        }

        void CollectionFilter(object source, FilterEventArgs args) 
        {
            V3Data data = args.Item as V3Data;

            if (data != null && data is V3DataCollection) args.Accepted = true;
            else args.Accepted = false;
        }

        void OnGridFilter(object source, FilterEventArgs args)
        {
            V3Data data = args.Item as V3Data;

            if (data != null && data is V3DataOnGrid) args.Accepted = true;
            else args.Accepted = false;
        }

        bool CheckChangedDataConditions()
        {
            if (mainCollection.HasChanged)
            {
                MessageBoxResult res = MessageBox.Show("Имеются несохраненные данные. Сохранить их?", "Несохраненные данные", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (res)
                {
                    case MessageBoxResult.Yes:
                        return Save();
                    case MessageBoxResult.No:
                        return true;
                    case MessageBoxResult.Cancel:
                        return false;
                }
            }

            return true;
        }
    }
}
