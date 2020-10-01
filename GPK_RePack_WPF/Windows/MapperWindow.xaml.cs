using GPK_RePack.Core.Model;
using GPK_RePack.Core.Model.Composite;
using Nostrum;
using Nostrum.Extensions;
using Nostrum.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GPK_RePack_WPF.Windows
{
    public partial class MapperWindow : Window
    {
        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }

        public MapperWindow(GpkStore gpkStore)
        {
            InitializeComponent();

            DataContext = new MapperViewModel(gpkStore);
            MinimizeCommand = new RelayCommand(_ =>
            {
                WindowState = WindowState.Minimized;
            });

            CloseCommand = new RelayCommand(_ =>
            {
                Close();
            });

        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            this.TryDragMove();
        }
    }

    public class MapperViewModel : TSPropertyChanged
    {
        private readonly GpkStore gpkStore;
        private readonly DispatcherTimer _searchTimer;
        private string _searchFilter = "";

        public TSObservableCollection<CompositeMapEntryViewModel> GpkList { get; }
        public ICollectionView GpkListView { get; }

        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                if (_searchFilter == value) return;
                _searchFilter = value;
                _searchTimer.Stop();
                _searchTimer.Start();
                N();
            }
        }

        public ICommand DeleteMappingCommand { get; }
        public ICommand DeleteEntryCommand { get; }

        public MapperViewModel(GpkStore store)
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            GpkList = new TSObservableCollection<CompositeMapEntryViewModel>();
            GpkListView = CollectionViewFactory.CreateCollectionView(GpkList);
            this.gpkStore = store;
            _searchTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Background, OnSearchTimerElapsed, Dispatcher);

            foreach (var pair in gpkStore.CompositeMap)
            {
                var vm = new CompositeMapEntryViewModel(pair.Key, pair.Value);
                GpkList.Add(vm);
            }
        }

        private void OnSearchTimerElapsed(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            GpkListView.Filter = o => ((CompositeMapEntryViewModel)o).UID.IndexOf(_searchFilter, StringComparison.InvariantCultureIgnoreCase) != -1 
                                   || ((CompositeMapEntryViewModel)o).SubGpkList.Any(s => s.UID.IndexOf(_searchFilter, StringComparison.InvariantCultureIgnoreCase) != -1);
            GpkListView.Refresh();
        }
    }



    public class CompositeMapEntryViewModel : TSPropertyChanged
    {
        public string UID  => IsSubGpk ? _entry.UID : _uid;

        public bool IsSubGpk { get; }

        private readonly CompositeMapEntry _entry;
        private readonly string _uid;
        public List<CompositeMapEntryViewModel> SubGpkList { get; }

        private CompositeMapEntryViewModel()
        {
            SubGpkList = new List<CompositeMapEntryViewModel>();
            
        }
        public CompositeMapEntryViewModel(string uid, List<CompositeMapEntry> entries) : this()
        {
            _uid = uid;
            SubGpkList.AddRange(entries.Select(sg => new CompositeMapEntryViewModel(sg)));
            SubGpkList.Sort(new CompositeMapEntryViewModelComparer());
            IsSubGpk = false;
        }

        public CompositeMapEntryViewModel(CompositeMapEntry cme) : this()
        {
            this._entry = cme;
            IsSubGpk = true;
        }
    }

    public class CompositeMapEntryViewModelComparer : IComparer<CompositeMapEntryViewModel>
    {
        public int Compare(CompositeMapEntryViewModel x, CompositeMapEntryViewModel y)
        {
            return x.UID.CompareTo(y.UID);
        }
    }

    //public class CompositeMapEntryViewModel : TSPropertyChanged
    //{


    //    public CompositeMapEntryViewModel(CompositeMapEntry cme)
    //    {
    //        this._entry = cme;
    //    }
    //}
}
