using System;
using System.Collections.Generic;
using ClassLibrary;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;

namespace ViewModel
{
    public interface IUIServices
    {
        bool ChooseElementFromFile(ref string filename, string dirPath);
        bool ChooseSaveFile(ref string filename, string dirPath);
        bool ChooseLoadFile(ref string filename, string dirPath);
        void ConfirmError(string errorText, string errorTitle);
        bool? ConfirmWarning(string text, string title);
    }

    public class MainViewModel : ViewModelBase
    {
        V3MainCollection mainCollection = new V3MainCollection();
        IUIServices svc;

        public ICommand AddElementFromFileCommand { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand AddDefaultsCommand { get; private set; }
        public ICommand AddDefaultDataCollectionCommand { get; private set; }
        public ICommand AddDefaultDataOnGridCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public V3Data MainSelectedItem { get; set; }
        public V3MainCollection MainCollection
        {
            get => mainCollection;
        }
        public IEnumerable DataCollectionView
        {
            get => from item in mainCollection where item is V3DataCollection select item;
        }
        public IEnumerable DataOnGridView
        {
            get => from item in mainCollection where item is V3DataOnGrid select item;
        }
        public bool HasChanged
        {
            get => mainCollection.HasChanged;
        }
        public float MaxDist
        {
            get => mainCollection.MaxDist;
        }

        public MainViewModel(IUIServices _svc)
        {
            mainCollection.PropertyChanged += PropertyChangedHandler;
            mainCollection.CollectionChanged += CollectionChangedHandler;
            svc = _svc;

            AddElementFromFileCommand = new RelayCommand(
                _ =>
                {
                    string filename = string.Empty;
                    string dirPath = System.IO.Path.Combine("..\\..\\..\\..\\SaveFiles");
                    if (svc.ChooseElementFromFile(ref filename, dirPath))
                    {
                        if (!TryAddDataCollection(filename))
                            svc.ConfirmError("Невозможно прочитать выбранный файл. Формат данных некорректен.", "Ошибка");
                    }
                }
                );

            NewCommand = new RelayCommand(
                _ =>
                {
                    if (CheckChangedDataConditions())
                        mainCollection.Clear();
                }
                );

            AddDefaultsCommand = new RelayCommand(_ => mainCollection.AddDefaults());

            AddDefaultDataCollectionCommand = new RelayCommand(_ => mainCollection.AddRandomDataCollection());

            AddDefaultDataOnGridCommand = new RelayCommand(_ => mainCollection.AddRandomDataOnGrid());

            RemoveCommand = new RelayCommand(
                _ => mainCollection.Remove(MainSelectedItem),
                _ => (MainCollection != null && MainSelectedItem != null)
                );

            SaveCommand = new RelayCommand(
                _ =>
                {
                    string filename = string.Empty;
                    string dirPath = System.IO.Path.Combine("..\\..\\..\\..\\SaveFiles");
                    if (svc.ChooseSaveFile(ref filename, dirPath))
                    {
                        if (!mainCollection.Save(filename))
                            svc.ConfirmError("Что-то пошло не так", "Ошибка");
                    }
                },
                _ => HasChanged
                );

            LoadCommand = new RelayCommand(
                _ =>
                {
                    if (!CheckChangedDataConditions()) return;

                    string filename = string.Empty;
                    string dirPath = System.IO.Path.Combine("..\\..\\..\\..\\SaveFiles");
                    if (svc.ChooseLoadFile(ref filename, dirPath))
                    {
                        if (!mainCollection.Load(filename))
                            svc.ConfirmError("Что-то пошло не так", "Ошибка");
                    }
                }
                );
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

        public bool CheckChangedDataConditions()
        {
            if (HasChanged)
            {
                switch (svc.ConfirmWarning("Имеются несохраненные данные. Сохранить их?", "Несохраненные данные"))
                {
                    case true:
                        SaveCommand.Execute(this);
                        return !HasChanged;
                    case false:
                        return true;
                    case null:
                        return false;
                }
            }

            return true;
        }

        public bool IsDataCollection(object item) => item is V3DataCollection;

        public bool IsDataOnGrid(object item) => item is V3DataOnGrid;


        public void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        public void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("DataCollectionView");
            RaisePropertyChanged("DataOnGridView");
        }
    }
}
