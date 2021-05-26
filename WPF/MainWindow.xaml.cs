using System.Windows;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Controls;
using ViewModel;

namespace WPF
{
    public partial class MainWindow : Window
    {
        public static RoutedCommand AddCommand = new RoutedCommand("Add", typeof(MainWindow));

        MainViewModel viewModel = new MainViewModel(new WPFUIServices());                                
        DataItemCreator dataItemCreator;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !viewModel.CheckChangedDataConditions();
        }

        public void InitDataItemCreator(object sender, RoutedEventArgs e)
        {
            dataItemCreator = new DataItemCreator(listBox_Main.SelectedItem);
            grid_DataItemCreator.DataContext = dataItemCreator;
        }


        public void CanAddCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (listBox_Main != null && listBox_Main.SelectedItem != null && viewModel.IsDataCollection(listBox_Main.SelectedItem))
            {
                e.CanExecute = !(Validation.GetHasError(textblock_DataItemXCoord) || Validation.GetHasError(textblock_DataItemYCoord) || Validation.GetHasError(textblock_DataItemValue));
            }
            else e.CanExecute = false;
        }

        public void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            dataItemCreator.AddDataItem();
        }
    }

    public class WPFUIServices : IUIServices
    {
        public bool ChooseElementFromFile(ref string filename, string dirPath)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.GetFullPath(dirPath);
            dlg.RestoreDirectory = true;
            dlg.Filter = "Text file (*.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                filename = dlg.FileName;
                return true;
            }
            return false;
        }

        public bool ChooseLoadFile(ref string filename, string dirPath)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.GetFullPath(dirPath);
            dlg.RestoreDirectory = true;
            dlg.Filter = "Data list save (*.datalist)|*.datalist";
            if (dlg.ShowDialog() == true)
            {
                filename = dlg.FileName;
                return true;
            }
            return false;
        }

        public bool ChooseSaveFile(ref string filename, string dirPath)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = System.IO.Path.GetFullPath(dirPath);
            dlg.RestoreDirectory = true;
            dlg.Filter = "Data list save (*.datalist)|*.datalist";
            if (dlg.ShowDialog() == true)
            {
                filename = dlg.FileName;
                return true;
            }
            return false;
        }

        public void ConfirmError(string text, string title)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool? ConfirmWarning(string text, string title)
        {
            switch (MessageBox.Show(text, title, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
            {
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return null;
            }
        }
    }
}
