using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using Microsoft.Win32;
using ClassLibrary;
using System.Windows.Input;
using System.Windows.Controls;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand AddCommand = new RoutedCommand("Add", typeof(MainWindow));

        V3MainCollection mainCollection = new V3MainCollection();
        DataItemCreator dataItemCreator;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = mainCollection;
        }

        public void New(object sender, RoutedEventArgs e)
        {
            New();
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

        public void OnClosing(object sender, CancelEventArgs e)
        {
            if (!CheckChangedDataConditions()) e.Cancel = true;
        }

        public void InitDataItemCreator(object sender, RoutedEventArgs e)
        {
            dataItemCreator = new DataItemCreator(listBox_Main.SelectedItem as V3DataCollection);
            grid_DataItemCreator.DataContext = dataItemCreator;
        }


        public void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mainCollection.HasChanged;
        }

        public void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        public void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (!CheckChangedDataConditions()) return;
            Load();
        }

        public void CanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (listBox_Main != null && listBox_Main.SelectedItem != null);
        }

        public void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            mainCollection.Remove((V3Data)listBox_Main.SelectedItem);
        }

        public void CanAddCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (listBox_Main != null && listBox_Main.SelectedItem != null && listBox_Main.SelectedItem is V3DataCollection)
            {
                e.CanExecute = !(Validation.GetHasError(textblock_DataItemXCoord) || Validation.GetHasError(textblock_DataItemYCoord) || Validation.GetHasError(textblock_DataItemValue));
            }
            else e.CanExecute = false;
        }

        public void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            dataItemCreator.AddDataItem();
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
