using GPK_RePack.Core.Model;
using GPK_RePack.Core.Model.Composite;
using Nostrum;
using Nostrum.Extensions;
using Nostrum.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using NLog;

namespace GPK_RePack_WPF.Windows
{
    public partial class MapperWindow : Window
    {
        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }

        public MapperWindow(GpkStore gpkStore)
        {
            InitializeComponent();

            var vm = new MapperViewModel(gpkStore);
            vm.CloseRequested += Close;
            DataContext = vm;
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
            ResizeMode = ResizeMode.NoResize;
            this.TryDragMove();
            ResizeMode = ResizeMode.CanResize;
        }

        private void OnSelectedNodeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is CompositeMapEntryViewModel vm)
            {
                ((MapperViewModel)DataContext).SelectEntry(vm);
            }
        }
    }

    public class MapperViewModel : TSPropertyChanged
    {
        private readonly GpkStore gpkStore;
        private readonly DispatcherTimer _searchTimer;
        public static string _searchFilter = "";
        private readonly Logger _logger;
        public static event Action<string> SearchUpdated;
        public event Action CloseRequested;

        public TSObservableCollection<CompositeMapEntryViewModel> GpkList { get; }
        public ICollectionView GpkListView { get; }
        public CompositeMapEntry SelectedEntry { get; set; }

        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                if (_searchFilter == value) return;
                _searchFilter = value;
                N();
                _searchTimer.Refresh();
            }
        }

        public ICommand ClearMappingCommand { get; }
        public ICommand DeleteEntryCommand { get; }

        public MapperViewModel(GpkStore store)
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            GpkList = new TSObservableCollection<CompositeMapEntryViewModel>();
            GpkListView = CollectionViewFactory.CreateCollectionView(GpkList);
            this.gpkStore = store;
            _searchTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Background, OnSearchTimerElapsed, Dispatcher);
            _logger = LogManager.GetCurrentClassLogger();

            ClearMappingCommand = new RelayCommand(ClearMapping);
            DeleteEntryCommand = new RelayCommand(DeleteEntry);

            Load();
        }

        private void ClearMapping()
        {
            gpkStore.clearCompositeMap();
            _logger.Info("Deleted Mapping Cache");
            CloseRequested?.Invoke();
        }

        public void SelectEntry(CompositeMapEntryViewModel entry)
        {
            SelectedEntry = entry.IsSubGpk ? entry.Source : null;
        }


        private void DeleteEntry()
        {
            //if (treeMapperView.SelectedNode == null || treeMapperView.SelectedNode.Tag == null)
            //    return;
            if (SelectedEntry == null) return;

            gpkStore.CompositeMap[SelectedEntry.SubGPKName].Remove(SelectedEntry);
            var targetVm = GpkList.FirstOrDefault(g => g.UID == SelectedEntry.SubGPKName);
            var targetEntry = targetVm.SubGpkList.FirstOrDefault(sg => sg.Source == SelectedEntry);
            targetVm?.SubGpkList.Remove(targetEntry);
            //treeMapperView.OnDrawNodes();

        }

        private void Load()
        {
            GpkList.Clear();
            Task.Run(() =>
            {

                foreach (var pair in gpkStore.CompositeMap)
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        var vm = new CompositeMapEntryViewModel(pair.Key, pair.Value);
                        vm.SubGpkList.Add(new CompositeMapEntryViewModel("", null));
                        GpkList.Add(vm);
                    }, DispatcherPriority.Background);
                }
            });
        }

        private void OnSearchTimerElapsed(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            GpkListView.Filter = o => ((CompositeMapEntryViewModel)o).UID.IndexOf(_searchFilter, StringComparison.InvariantCultureIgnoreCase) != -1
                                   || ((CompositeMapEntryViewModel)o).HasEntry(s => s.UID.IndexOf(_searchFilter, StringComparison.InvariantCultureIgnoreCase) != -1);
            GpkListView.Refresh();
            SearchUpdated?.Invoke(_searchFilter);
        }
    }


    public class GpkTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CompositeGpkDataTemplate { get; set; }
        public DataTemplate SubGpkDataTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is CompositeMapEntryViewModel vm)) return null;

            return vm.IsSubGpk ? SubGpkDataTemplate : CompositeGpkDataTemplate;
        }
    }
    public class CompositeMapEntryViewModel : TSPropertyChanged
    {
        private bool _isExpanded;
        private bool _isSubTreeLoaded;
        private readonly string _uid;
        private readonly List<CompositeMapEntry> _entries;
        private bool _isFound;

        public bool IsSubGpk { get; }

        public string UID => IsSubGpk ? Source?.UID : _uid;

        public string CompositeUID => IsSubGpk ? Source?.CompositeUID : "";
        public string ParentGPK => IsSubGpk ? Source?.SubGPKName : "";
        public string FileSize => IsSubGpk
            ? Source.FileLength >= 1024 * 1024
                ? $"{Source?.FileLength / (1024 * 1024)} MB"
                    : Source.FileLength >= 1024
                    ? $"{Source?.FileLength / 1024} KB"
                    : $"{Source?.FileLength} B"

                : "";

        public string ContentInfo => IsSubGpk
            ? ""
            : _isSubTreeLoaded
                ? $"({LoadedSubGpkCount}/{SubGpkCount})"
                : $"({SubGpkCount})";

        public int SubGpkCount => _entries?.Count ?? 0;
        public int LoadedSubGpkCount => SubGpkList.Count;
        public TSObservableCollection<CompositeMapEntryViewModel> SubGpkList { get; }
        public ICollectionViewLiveShaping SubGpkListView { get; }
        public bool IsSubTreeLoaded
        {
            get => _isSubTreeLoaded;
            set
            {
                if (_isSubTreeLoaded == value) return;
                _isSubTreeLoaded = value;
                N();
            }
        }
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                if (_isExpanded)
                {
                    if (!IsSubTreeLoaded)
                    {
                        LoadSubGpks();
                        IsSubTreeLoaded = true;
                    }
                }
                else
                {
                    IsSubTreeLoaded = false;
                    ResetSubGpkList();
                }

                N();
                ((ICollectionView)SubGpkListView).Refresh();
            }
        }

        private CompositeMapEntryViewModel()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            SubGpkList = new TSObservableCollection<CompositeMapEntryViewModel>();
            SubGpkListView = CollectionViewFactory.CreateLiveCollectionView(SubGpkList,
                predicate: o => o.IsVisible,
                filters: new[] { nameof(IsVisible) },
                sortFilters: new[] { new SortDescription(nameof(UID), ListSortDirection.Ascending) });
            MapperViewModel.SearchUpdated += OnSearchUpdated;

        }
        public CompositeMapEntryViewModel(string uid, List<CompositeMapEntry> entries) : this()
        {
            _uid = uid;
            _entries = entries;
            IsSubGpk = false;
            OnSearchUpdated(MapperViewModel._searchFilter);


        }
        public CompositeMapEntryViewModel(CompositeMapEntry cme) : this()
        {
            this.Source = cme;
            IsSubGpk = true;
            OnSearchUpdated(MapperViewModel._searchFilter);
        }

        private void OnSearchUpdated(string obj)
        {
            if (IsSubGpk)
            {
                IsFound = obj != "" && this.UID.IndexOf(obj, StringComparison.InvariantCultureIgnoreCase) != -1;
            }
        }


        public bool IsVisible
        {
            get
            {
                var ret = MapperViewModel._searchFilter == "" || IsFound || UID == "";
                return ret;
            }
        }

        public bool IsFound
        {
            get => _isFound;
            set
            {
                if (_isFound == value) return;
                _isFound = value;
                N();
                N(nameof(IsVisible));
            }
        }

        public CompositeMapEntry Source { get; }

        private void ResetSubGpkList()
        {
            SubGpkList.Clear();
            SubGpkList.Add(new CompositeMapEntryViewModel("", null));
            N(nameof(ContentInfo));
        }
        private void LoadSubGpks()
        {
            SubGpkList.Clear();
            Task.Run(() =>
            {
                Parallel.ForEach(_entries, entry =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        var vm = new CompositeMapEntryViewModel(entry);
                        SubGpkList.Add(vm);
                        N(nameof(ContentInfo));

                    }, DispatcherPriority.Input);
                });

            });
        }

        public bool HasEntry(Func<CompositeMapEntry, bool> func)
        {
            return _entries.Any(func);
        }
    }
}
