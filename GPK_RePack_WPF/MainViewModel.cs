using GPK_RePack.Core;
using GPK_RePack.Core.Editors;
using GPK_RePack.Core.IO;
using GPK_RePack.Core.Model;
using GPK_RePack.Core.Model.Composite;
using GPK_RePack.Core.Model.Interfaces;
using GPK_RePack.Core.Model.Payload;
using GPK_RePack.Core.Model.Prop;
using GPK_RePack.Core.Updater;
using GPK_RePack_WPF.Windows;
using NAudio.Wave;
using NLog;
using Nostrum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using UpkManager.Dds;
using Clipboard = System.Windows.Clipboard;
using DataFormats = System.Windows.Forms.DataFormats;
using Size = System.Windows.Size;

namespace GPK_RePack_WPF
{

    public class MainViewModel : TSPropertyChanged, IUpdaterCheckCallback, IDisposable
    {
        #region Properties
        // not actually a singleton, just a reference for contextmenu command binding
        public static MainViewModel Instance { get; private set; }

        // ReSharper disable once CollectionNeverQueried.Global
        public static readonly List<string> PropertyTypes = new List<string>
        {
            "ArrayProperty",
            "BoolProperty",
            "ByteProperty",
            "FloatProperty",
            "IntProperty",
            "NameProperty",
            "ObjectProperty",
            "StrProperty",
            "StructProperty"
        };

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly LogBuffer _logBuffer;
        private readonly DataFormats.Format _exportFormat = DataFormats.GetFormat(typeof(GpkExport).FullName);
        private readonly Logger _logger;
        private readonly GpkStore _gpkStore;
        private List<GpkExport>[] _changedExports;
        private GpkPackage _selectedPackage;
        private GpkExport _selectedExport;
        private GpkImport _selectedImport;
        private string _selectedClass = "";
        private PropertyViewModel _selectedProperty;

        private bool _isTextureTabVisible;
        private bool _isSoundTabVisible;
        private string _logText;
        private string _statusBarText;
        private string _infoText;
        private bool _generalButtonsEnabled;
        private bool _dataButtonsEnabled;
        private int _progressValue;
        private Tab _selectedTab = 0;

        private ImageSource _previewImage;
        private string _previewImageFormat;
        private string _previewImageName;

        private readonly List<GpkTreeNode> _searchResultNodes;
        private int _searchResultIndex;

        public SoundPreviewManager SoundPreviewManager { get; }
        public bool LogToUI
        {
            get => CoreSettings.Default.LogToUI;
            set
            {
                if (CoreSettings.Default.LogToUI == value) return;
                CoreSettings.Default.LogToUI = value;
                N();
                if (value)
                {
                    NLogConfig.EnableFormLogging();
                }
                else
                {
                    NLogConfig.DisableFormLogging();
                }
            }
        }
        public bool GeneralButtonsEnabled
        {
            get => _generalButtonsEnabled;
            set
            {
                if (_generalButtonsEnabled == value) return;
                _generalButtonsEnabled = value;
                N();
            }
        }
        public bool DataButtonsEnabled
        {
            get => _dataButtonsEnabled;
            set
            {
                if (_dataButtonsEnabled == value) return;
                _dataButtonsEnabled = value;
                N();
            }
        }
        public bool PropertyButtonsEnabled => _selectedExport != null && _selectedTab == Tab.Properties;
        public bool ImageButtonsEnabled => _selectedExport != null && _selectedTab == Tab.Texture && IsTextureTabVisible;
        public bool SoundButtonsEnabled => _selectedExport != null && _selectedTab == Tab.Sound && IsSoundTabVisible;
        public bool IsTextureTabVisible
        {
            get => _isTextureTabVisible;
            set
            {
                if (_isTextureTabVisible == value) return;
                _isTextureTabVisible = value;
                N();
                if (value == false && _selectedTab == Tab.Texture) SelectedTabIndex = (int)Tab.Info;
            }
        }
        public bool IsSoundTabVisible
        {
            get => _isSoundTabVisible;
            set
            {
                if (_isSoundTabVisible == value) return;
                _isSoundTabVisible = value;
                N();
                if (value == false && _selectedTab == Tab.Sound) SelectedTabIndex = (int)Tab.Info;
            }
        }
        public string LogText
        {
            get => _logText;
            set
            {
                if (_logText == value) return;
                _logText = value;
                N();
            }
        }
        public string StatusBarText
        {
            get => _statusBarText;
            set
            {
                if (_statusBarText == value) return;
                _statusBarText = value;
                N();
            }
        }
        public string InfoText
        {
            get => _infoText;
            set
            {
                if (_infoText == value) return;
                _infoText = value;
                N();
            }
        }
        public int SelectedTabIndex
        {
            get => (int)_selectedTab;
            set
            {
                if (_selectedTab == (Tab)value) return;
                _selectedTab = (Tab)value;
                N();
                N(nameof(PropertyButtonsEnabled));
                N(nameof(ImageButtonsEnabled));
                N(nameof(SoundButtonsEnabled));

            }
        }
        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                if (_progressValue == value) return;
                _progressValue = value;
                N();
            }
        }
        public GpkTreeNode TreeMain { get; }
        public PropertyViewModel SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                if (_selectedProperty == value) return;
                _selectedProperty = value;
                N();
            }
        }
        public TSObservableCollection<PropertyViewModel> Properties { get; }
        public TSObservableCollection<RecentFileViewModel> RecentFiles { get; }
        public GpkTreeNode SelectedNode { get; set; }
        public ImageSource PreviewImage
        {
            get => _previewImage;
            set
            {
                if (_previewImage == value) return;
                _previewImage = value;
                N();
            }
        }
        public string PreviewImageFormat
        {
            get => _previewImageFormat;
            set
            {
                if (_previewImageFormat == value) return;
                _previewImageFormat = value;
                N();
            }
        }
        public string PreviewImageName
        {
            get => _previewImageName;
            set
            {
                if (_previewImageName == value) return;
                _previewImageName = value;
                N();
            }
        }
        #region WindowParameters

        public GridLength LogSize
        {
            get => CoreSettings.Default.LogSize;
            set => CoreSettings.Default.LogSize = value;
        }
        public GridLength TreeViewSize
        {
            get => CoreSettings.Default.TreeViewSize;
            set => CoreSettings.Default.TreeViewSize = value;
        }
        public GridLength PropViewSize
        {
            get => CoreSettings.Default.PropViewSize;
            set => CoreSettings.Default.PropViewSize = value;
        }
        public GridLength TopSize
        {
            get => CoreSettings.Default.TopSize;
            set => CoreSettings.Default.TopSize = value;
        }
        public double WindowHeight
        {
            get => CoreSettings.Default.WindowSize.Height;
            set
            {
                if (CoreSettings.Default.WindowSize.Height == value) return;
                var oldSize = CoreSettings.Default.WindowSize;
                var newSize = new Size(oldSize.Width, value);
                CoreSettings.Default.WindowSize = newSize;
                N();
            }
        }
        public double WindowWidth
        {
            get => CoreSettings.Default.WindowSize.Width;
            set
            {
                if (CoreSettings.Default.WindowSize.Width == value) return;
                var oldSize = CoreSettings.Default.WindowSize;
                var newSize = new Size(value, oldSize.Height);
                CoreSettings.Default.WindowSize = newSize;
                N();
            }
        }
        public WindowState WindowState
        {
            get => CoreSettings.Default.WindowState;
            set
            {
                if (CoreSettings.Default.WindowState == value) return;
                CoreSettings.Default.WindowState = value;
                N();
            }
        }

        #endregion
        #endregion

        #region Commands
        public ICommand OpenCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand RefreshCommand { get; }

        public ICommand SwitchTabCommand { get; }

        public ICommand SavePropertiesCommand { get; }
        public ICommand ClearPropertiesCommand { get; }
        public ICommand ExportPropertiesToCsvCommand { get; }

        public ICommand ExportRawDataCommand { get; }
        public ICommand ImportRawDataCommand { get; }
        public ICommand RemoveObjectCommand { get; }
        public ICommand CopyObjectCommand { get; }
        public ICommand PasteObjectCommand { get; }
        public ICommand InsertObjectCommand { get; }
        public ICommand DeleteDataCommand { get; }
        public ICommand SaveSingleGpkCommand { get; }
        public ICommand PatchObjectMapperForSelectedPackageCommand { get; }

        public ICommand ExportDDSCommand { get; }
        public ICommand ImportDDSCommand { get; }
        public ICommand ExportOGGCommand { get; }
        public ICommand ImportOGGCommand { get; }
        //todo
        public ICommand AddEmptyOGGCommand { get; }

        public ICommand DecryptDatCommand { get; }
        public ICommand EncryptDatCommand { get; }
        public ICommand LoadMappingCommand { get; }
        public ICommand WriteMappingCommand { get; }
        public ICommand DumpAllTexturesCommand { get; }
        public ICommand DumpIconsCommand { get; }
        public ICommand DumpGPKObjectsCommand { get; }
        public ICommand MinimizeGpkCommand { get; }

        public ICommand SetPropsCustomCommand { get; }
        public ICommand SetFileSizeCommand { get; }
        public ICommand SetVolumeMultipliersCommand { get; }
        public ICommand AddNameCommand { get; }
        public ICommand RenameObjectCommand { get; }
        public ICommand BigBytePropImportCommand { get; }
        public ICommand BigBytePropExportCommand { get; }

        public ICommand SearchCommand { get; }
        public ICommand SearchNextCommand { get; }

        public ICommand PlayStopSoundCommand { get; }

        public ICommand TryToLoadAllExportDataFromCompositeCommand { get; }
        public ICommand LoadCompositeDataForSelectedExportCommand { get; }

        public ICommand ShowHelpCommand { get; }

        #endregion

        public MainViewModel()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
            Instance = this;

            //nlog
            NLogConfig.SetDefaultConfig(NLogConfig.NlogFormConfig.WPF);
            _logBuffer = new LogBuffer();
            _logBuffer.LinesFlushed += LogMessages;
            CustomEventTarget.LogMessageWritten += _logBuffer.AppendLine;
            _logger = LogManager.GetLogger("GUI");
            _logger.Info("Startup");
            ShowHelp();
            //
            UpdateCheck.checkForUpdate(this);
            _gpkStore = new GpkStore();
            _gpkStore.PackagesChanged += OnPackagesChanged;
            TreeMain = new GpkTreeNode("");
            Properties = new TSObservableCollection<PropertyViewModel>();
            RecentFiles = new TSObservableCollection<RecentFileViewModel>();
            OnRecentFilesChanged();
            CoreSettings.Default.RecentFilesChanged += OnRecentFilesChanged;

            _searchResultNodes = new List<GpkTreeNode>();
            SoundPreviewManager = new SoundPreviewManager();
            if (CoreSettings.Default.SaveDir == "")
                CoreSettings.Default.SaveDir = Directory.GetCurrentDirectory();

            if (CoreSettings.Default.OpenDir == "")
                CoreSettings.Default.OpenDir = Directory.GetCurrentDirectory();

            if (CoreSettings.Default.WorkingDir == "")
                CoreSettings.Default.WorkingDir = Directory.GetCurrentDirectory();

            //mappings
            if (CoreSettings.Default.LoadMappingOnStart && CoreSettings.Default.CookedPCPath != "")
            {
                new Task(() =>
                {
                    LoadAndParseMapping(CoreSettings.Default.CookedPCPath);
                }).Start();
            }

            //commands
            OpenCommand = new RelayCommand<string[]>(Open);
            SaveCommand = new RelayCommand<SaveMode>(Save);

            PatchObjectMapperForSelectedPackageCommand = new RelayCommand(PatchObjectMapperForSelectedPackage);

            RefreshCommand = new RelayCommand(Refresh);
            ClearCommand = new RelayCommand(Clear);

            SavePropertiesCommand = new RelayCommand(SaveProperties);
            ClearPropertiesCommand = new RelayCommand(ClearProperties);
            ExportPropertiesToCsvCommand = new RelayCommand(ExportPropertiesToCsv);

            ExportRawDataCommand = new RelayCommand(ExportRawData);
            ImportRawDataCommand = new RelayCommand(ImportRawData);
            RemoveObjectCommand = new RelayCommand(RemoveObject);
            CopyObjectCommand = new RelayCommand(CopyObject);
            PasteObjectCommand = new RelayCommand(PasteObject);
            InsertObjectCommand = new RelayCommand(InsertObject);
            DeleteDataCommand = new RelayCommand(DeleteData);

            ImportDDSCommand = new RelayCommand(ImportDDS);
            ExportDDSCommand = new RelayCommand(ExportDDS);
            ImportOGGCommand = new RelayCommand(ImportOGG);
            ExportOGGCommand = new RelayCommand(ExportOGG);
            AddEmptyOGGCommand = new RelayCommand(AddEmptyOGG);

            SaveSingleGpkCommand = new RelayCommand(SaveSingleGpk);

            SwitchTabCommand = new RelayCommand<Tab>(GotoTab);

            DecryptDatCommand = new RelayCommand(DecryptDat);
            EncryptDatCommand = new RelayCommand(EncryptDat);
            LoadMappingCommand = new RelayCommand(LoadMapping);
            WriteMappingCommand = new RelayCommand(WriteMapping);
            DumpAllTexturesCommand = new RelayCommand(DumpAllTextures);
            DumpIconsCommand = new RelayCommand(DumpIcons);
            MinimizeGpkCommand = new RelayCommand(MinimizeGPK);
            DumpGPKObjectsCommand = new RelayCommand(DumpGPKObjects);

            SetPropsCustomCommand = new RelayCommand(SetPropsCustom);
            SetFileSizeCommand = new RelayCommand(SetFileSize);
            SetVolumeMultipliersCommand = new RelayCommand(SetVolumeMultipliers);
            AddNameCommand = new RelayCommand(AddName);
            RenameObjectCommand = new RelayCommand(RenameObject);

            BigBytePropImportCommand = new RelayCommand(BigBytePropImport);
            BigBytePropExportCommand = new RelayCommand(BigBytePropExport);

            SearchCommand = new RelayCommand(Search);
            SearchNextCommand = new RelayCommand(prev => SelectNextSearchResult());

            PlayStopSoundCommand = new RelayCommand(PlayStopSound);

            TryToLoadAllExportDataFromCompositeCommand = new RelayCommand(TryToLoadAllExportDataFromComposite);
            LoadCompositeDataForSelectedExportCommand = new RelayCommand(LoadCompositeDataForSelectedExport);

            ShowHelpCommand = new RelayCommand(ShowHelp);
        }

        private void OnRecentFilesChanged()
        {
            RecentFiles.Clear();
            CoreSettings.Default.RecentFiles.ForEach(s => RecentFiles.Add(new RecentFileViewModel(s)));
        }

        private void OnPackagesChanged()
        {
            Reset();
            DrawPackages();
        }

        private void ShowHelp()
        {
            //todo remake and format this
            InfoText =
                " Quick Guide:\r\n1.Open a *.gpk file via File -> Open\r\n2.Select a Object you want to edit on the treeview\r\n3 - Editing\r\n3a) Use the buttons Export/ Replace raw data for editing any object data\r\n3b) Use the buttons Import/ Export / Empty ogg for editing audio (SoundNodeWave) files\r\n3c) Use the buttons Import/ Export DDS for editing images (Texture2D)\r\n3c) Use the buttons Copy/ Paste(or Control - C, Control - V) on any object.You can choose in the settings what the parts of the object the program should copy,\r\n4 - Saving\r\n4a) The normal way to save is Main -> Save.The program will rebuild the GPK file from scratch. \r\n4b) If the normal way is failing switch select Patch Mode in settings and try to import the data again(only over raw import / export).Save afterwards via Main - > Save patched.\r\n\r\nFor x64 modding see https://github.com/GoneUp/GPK_RePack/wiki/64-bit-Modding\r\n\r\nAbout Padding: \r\nTera accepts only files with the exact same file size as the orginal file in some cases, for example at soundfiles.You need to test it for the file you want to modify or use a patched datacenter.\r\nThe save menu offers you the option to save as-is and a option with added padding.Terahelper will fill the file up if it's too small and warn you if it is too big. \r\nIf you want to change the maximum size manually select a the package and use the Misc - > Set Filesize function.\r\n\r\nHotkeys;\r\nCTRL + F, F3, Shift + F3: Search, next result, previous result\r\nCTRL + O: Open package\r\nCTRL + S: Rebuild Save\r\nCTRL + P: Rebuild Save, with padding\r\nCTRL + SHIFT + P: Patch save\r\nCTRL + 1,CTRL + 2,CTRL + 3: Select tabs\r\nCRTL + M: Load / Show Mapping\r\n\r\nhfgl\r\n-- GoneUp\r\n\r\nSource Code is available on Github: https://github.com/GoneUp/GPK_RePack/\r\n";
        }

        private void GotoTab(Tab tab)
        {
            switch (tab)
            {
                case Tab.Texture:
                case Tab.Sound:
                    if (IsSoundTabVisible)
                    {
                        SelectedTabIndex = (int)Tab.Sound;
                    }
                    else if (IsTextureTabVisible)
                    {
                        SelectedTabIndex = (int)Tab.Texture;
                    }
                    break;
                default:
                    SelectedTabIndex = (int)tab;
                    break;
            }
        }

        private void Refresh()
        {
            Reset();
            DrawPackages();
        }

        private void Save(SaveMode mode)
        {
            var usePadding = false;
            var patchComposite = false;
            var addComposite = false;
            switch (mode)
            {
                case SaveMode.Rebuild: break;
                case SaveMode.RebuildPadding:
                    usePadding = true;
                    break;
                case SaveMode.Patched:
                    SavePatched();
                    return;
                case SaveMode.PatchedComposite:
                    usePadding = true;
                    patchComposite = true;
                    break;
                case SaveMode.AddedComposite:
                    addComposite = true;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            var start = DateTime.Now;
            var runningSavers = new List<IProgress>();
            var runningTasks = new List<Task>();


            if (_gpkStore.LoadedGpkPackages.Count == 0)
                return;

            //do it
            try
            {
                this._gpkStore.SaveGpkListToFiles(_gpkStore.LoadedGpkPackages, usePadding, patchComposite, addComposite, runningSavers, runningTasks);
                //display info while loading
                while (!Task.WaitAll(runningTasks.ToArray(), 50))
                {
                    DisplayStatus(runningSavers, "Saving", start);
                }

                //Diplay end info
                DisplayStatus(runningSavers, "Saving", start);

                _logger.Info("Saving done!");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Save failure!");
            }

            void SavePatched()
            {
                var save = false;
                if (_changedExports != null)
                {
                    for (var i = 0; i < _changedExports.Length; i++)
                    {
                        var list = _changedExports[i];
                        if (list.Count <= 0) continue;
                        try
                        {
                            var tmpS = new Writer();
                            var package = _gpkStore.LoadedGpkPackages[i];
                            var savepath = package.Path + "_patched";
                            tmpS.SaveReplacedExport(package, savepath, list);
                            _logger.Info($"Saved the changed data of package '{package.Filename} to {savepath}'!");
                            save = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.Fatal(ex, "Save failure! ");
                        }
                    }
                }

                if (!save)
                {
                    _logger.Info("Nothing to save in PatchMode!");
                }
            }
        }
        private void Clear()
        {
            Reset();
            _changedExports = null;
            SoundPreviewManager.ResetOggPreview();
            _gpkStore.clearGpkList();
        }
        private void Open(string[] providedFiles = null)
        {
            var files = providedFiles ?? MiscFuncs.GenerateOpenDialog(true, false);

            if (files.Length == 0) return;

            var start = DateTime.Now;
            var runningReaders = new List<IProgress>();
            var runningTasks = new List<Task>();


            foreach (var path in files)
            {
                if (!File.Exists(path)) continue;
                CoreSettings.Default.AddRecentFile(path);
                var newTask = new Task(() =>
                {
                    var reader = new Reader();
                    runningReaders.Add(reader);
                    _gpkStore.loadGpk(path, reader, false);
                });
                runningTasks.Add(newTask);
                newTask.Start();
            }
            CoreSettings.Save();

            Task.Run(() =>
            {
                //display info while loading
                while (!Task.WaitAll(runningTasks.ToArray(), 10))
                {
                    DisplayStatus(runningReaders, "Loading", start);
                }


            }).ContinueWith(t =>
            {
                //for patchmode
                Array.Resize(ref _changedExports, _gpkStore.LoadedGpkPackages.Count);
                for (var i = 0; i < _changedExports.Length; i++)
                {
                    _changedExports[i] = new List<GpkExport>();
                }
                //Diplay end info
                DisplayStatus(runningReaders, "Loading", start);

                DrawPackages();
            });
        }
        private void DisplayStatus(List<IProgress> list, string tag, DateTime start)
        {
            if (list.Count == 0)
            {
                Debug.WriteLine(DateTime.Now + " DisplayStatus list is empty");
                return;
            }

            long actual = 0, total = 0, finished = 0;
            foreach (var p in list)
            {
                if (p == null) continue;
                var stat = p.GetStatus();


                if (stat.subGpkCount > 1)
                {
                    //dont show actual objects, just the sub-file count
                    actual += stat.subGpkDone;
                    total += stat.subGpkCount;
                    if (actual == total) finished++;
                }
                else
                {
                    //normal gpk 
                    actual += stat.progress;
                    total += stat.totalobjects;
                    if (stat.finished) finished++;
                }
            }

            if (finished < list.Count)
            {
                if (total > 0) ProgressValue = (int)((actual / (double)total) * 100);
                StatusBarText = $"[{tag}] Finished {finished}/{list.Count}";
            }
            else
            {
                total = 0;
                var builder = new StringBuilder();
                builder.AppendLine($"[{tag} Task Info]");
                foreach (var p in list)
                {
                    var stat = p.GetStatus();
                    total += stat.time;
                    builder.AppendLine($"Task {stat.name}: {stat.time}ms");
                }
                builder.AppendLine($"Avg Worktime: {total / list.Count}ms");
                builder.AppendLine($"Total elapsed Time: {(int)DateTime.Now.Subtract(start).TotalMilliseconds}ms");

                InfoText = builder.ToString();
                ProgressValue = 0;
                StatusBarText = "Ready";
            }
        }
        public void PostUpdateResult(bool updateAvailable)
        {
            if (updateAvailable)
            {
                _logger.Info("A newer version is available. Download it at https://github.com/GoneUp/GPK_RePack/releases");
            }
        }
        private void LogMessages(string msg)
        {
            LogText += msg;
        }
        private void Reset()
        {
            _selectedImport = null;
            _selectedExport = null;
            _selectedPackage = null;
            SelectedNode = null;
            _selectedClass = "";
            InfoText = "";
            StatusBarText = "Ready";
            GeneralButtonsEnabled = false;
            DataButtonsEnabled = false;
            IsTextureTabVisible = false;
            IsSoundTabVisible = false;
            N(nameof(PropertyButtonsEnabled));
            N(nameof(ImageButtonsEnabled));
            N(nameof(SoundButtonsEnabled));
            ProgressValue = 0;
            PreviewImage = null;
            TreeMain.Children.Clear();
            Properties.Clear();
        }
        public void Dispose()
        {
            SoundPreviewManager.Dispose();

        }

        private void DrawPackages()
        {
            //we may get calls out of gpkStore
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DrawPackages);
                return;
            }

            TreeMain.Children.Clear();

            if (CoreSettings.Default.EnableSortTreeNodes)
            {
                TreeMain.TreeViewNodeSorter = new MiscFuncs.NodeSorter();
            }

            //var total = gpkStore.LoadedGpkPackages.Count;
            //var done = 0;
            ProgressValue = 0;
            foreach (var package in _gpkStore.LoadedGpkPackages)
            {
                //Task.Run(() =>
                //{
                //    Dispatcher.InvokeAsync(() =>
                //    {

                var packageNode = new GpkTreeNode(package.Filename, package)
                {
                    IsPackage = true,
                    Index = _gpkStore.LoadedGpkPackages.IndexOf(package)
                };

                var classNodes = new Dictionary<string, GpkTreeNode>();
                GpkTreeNode importsNode = null;
                GpkTreeNode exportsNode = null;

                if (CoreSettings.Default.ShowImports)
                {
                    foreach (var tmp in package.ImportList.OrderByDescending(pair => pair.Value.ObjectName)
                        .Reverse())
                    {
                        var key = tmp.Value.UID;
                        var value = tmp.Value.ObjectName;
                        if (CoreSettings.Default.UseUID) value = key;

                        switch (CoreSettings.Default.ViewMode)
                        {
                            case ViewMode.Normal:
                                if (importsNode == null)
                                {
                                    importsNode = new GpkTreeNode("Imports");
                                    packageNode.AddNode(importsNode);
                                }

                                importsNode.AddNode(new GpkTreeNode(value, key) { IsImport = true });
                                break;
                            case ViewMode.Class:
                                GpkTreeNode.CheckClassNode(tmp.Value.ClassName, classNodes, packageNode);
                                var n = new GpkTreeNode( /*key,*/ value, key) { Class = tmp.Value.ClassName };
                                classNodes[tmp.Value.ClassName].AddNode(n);
                                break;
                        }

                    }
                }

                //Exports
                foreach (var pair in package.ExportList.OrderByDescending(pair => pair.Value.ObjectName).Reverse())
                {
                    var export = pair.Value;
                    var key = export.UID;
                    var value = export.ObjectName;
                    if (CoreSettings.Default.UseUID) value = key;

                    switch (CoreSettings.Default.ViewMode)
                    {
                        case ViewMode.Normal:
                            if (exportsNode == null)
                            {
                                exportsNode = new GpkTreeNode("Exports");
                                packageNode.AddNode(exportsNode);
                            }

                            exportsNode.AddNode(new GpkTreeNode( /*key,*/ value, key));
                            break;
                        case ViewMode.Class:
                            GpkTreeNode.CheckClassNode(export.ClassName, classNodes, packageNode);
                            var cn = new GpkTreeNode( /*key,*/ value, key) { Class = export.ClassName };
                            classNodes[export.ClassName].AddNode(cn);
                            break;

                        case ViewMode.Package:
                            GpkTreeNode.CheckClassNode(pair.Value.PackageName, classNodes, packageNode);
                            var pn = new GpkTreeNode( /*key,*/ value, key) { Class = export.ClassName };
                            classNodes[export.PackageName].AddNode(pn);
                            break;

                    }

                }

                TreeMain.AddNode(packageNode);
                //        Interlocked.Increment(ref done);
                //        ProgressValue = done *100 / total;
                //        if (ProgressValue == 100) ProgressValue = 0;
                //    }, DispatcherPriority.Background);
                //});
            }
        }
        public void SelectNode(GpkTreeNode node)
        {
            if (node.IsPackage) //level = 1 (rootlevel = 0)
            {
                GeneralButtonsEnabled = true;
                DataButtonsEnabled = true;
                _selectedPackage = _gpkStore.LoadedGpkPackages[Convert.ToInt32(node.Index)];
                try
                {
                    InfoText = _selectedPackage.ToString();
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, "Failed to show package info.");
                    InfoText = $"Failed to show package info: \n{ex}";
                }
            }
            else if (node.Level == 2 && CoreSettings.Default.ViewMode == ViewMode.Class)
            {
                _selectedPackage = _gpkStore.LoadedGpkPackages[Convert.ToInt32(node.FindPackageNode().Index)];
                _selectedClass = node.Key;

                DataButtonsEnabled = true;
            }
            else if (node.Level == 3 && CoreSettings.Default.ViewMode == ViewMode.Normal || node.IsLeaf)
            {
                var package = _gpkStore.LoadedGpkPackages[Convert.ToInt32(node.FindPackageNode().Index)];
                var selected = package.GetObjectByUID(node.Key);

                if (selected is GpkImport imp)
                {
                    InfoText = imp.ToString();
                    _selectedImport = imp;
                    _selectedPackage = package;
                }
                else if (selected is GpkExport exp)
                {
                    //buttons
                    GeneralButtonsEnabled = true;
                    DataButtonsEnabled = true;
                    var exportChanged = _selectedExport == exp;
                    _selectedExport = exp;
                    _selectedPackage = package;

                    N(nameof(PropertyButtonsEnabled));
                    RefreshExportInfo(exportChanged);
                }
            }
            SelectedNode = node;
        }
        private void RefreshExportInfo(bool exportChanged)
        {
            //tabs
            InfoText = _selectedExport.ToString();
            DrawGrid(_selectedPackage, _selectedExport);

            PreviewImage = null;
            switch (_selectedExport.Payload)
            {
                case Texture2D texture:
                {
                    IsTextureTabVisible = true;
                    IsSoundTabVisible = false;
                    var image = texture;
                    var ddsFile = new DdsFile();
                    var imageStream = image.GetObjectStream();
                    if (imageStream != null)
                    {
                        ddsFile.Load(image.GetObjectStream());

                        PreviewImage = ddsFile.BitmapSource;
                        PreviewImageFormat = image.parsedImageFormat.ToString();
                        PreviewImageName = _selectedExport.ObjectName;
                    }

                    break;
                }
                case Soundwave sw:
                    SoundPreviewManager.Setup(sw.oggdata);
                    IsSoundTabVisible = true;
                    IsTextureTabVisible = false;
                    break;
                default:
                    IsTextureTabVisible = false;
                    IsSoundTabVisible = false;

                    break;
            }

            if (exportChanged)
            {
                SoundPreviewManager.ResetOggPreview();
            }
        }


        private void SaveSingleGpk()
        {
            if (_selectedPackage == null || SelectedNode == null || !SelectedNode.IsPackage) return;
            var packages = new List<GpkPackage> { _selectedPackage };
            var writerList = new List<IProgress>();
            var taskList = new List<Task>();

            this._gpkStore.SaveGpkListToFiles(packages, false, false, false, writerList, taskList);

            //wait
            while (!Task.WaitAll(taskList.ToArray(), 50))
            {

            }
            _logger.Info("Single export done!");
        }
        private void DrawGrid(GpkPackage package, GpkExport export)
        {
            Properties.Clear();


            foreach (var iProp in export.Properties)
            {
                var prop = (GpkBaseProperty)iProp;
                var row = new PropertyViewModel
                {
                    Name = prop.name,
                    PropertyType = prop.type,
                    Size = iProp.RecalculateSize(),
                    ArrayIndex = prop.arrayIndex,
                    InnerTypes = package.InnerTypes,
                    InnerType = "None"
                };

                // could be converted to two switch expression if moving to netcore
                switch (prop)
                {
                    case GpkArrayProperty tmpArray:
                        row.Value = tmpArray.GetValueHex();
                        //todo: context menu
                        #region ContextMenu
                        //row.ContextMenuStrip = new ContextMenuStrip();
                        //row.ContextMenuStrip.Items.Add(
                        //    new ToolStripButton("Export", null,
                        //        (sender, args) =>
                        //        {
                        //            BigBytePropExport(tmpArray);
                        //        }));
                        //row.ContextMenuStrip.Items.Add(
                        //    new ToolStripButton("Import", null,
                        //       (sender, args) =>
                        //       {
                        //           BigBytePropImport(tmpArray);
                        //       })
                        //    ); 
                        #endregion
                        break;
                    case GpkStructProperty tmpStruct:
                        row.InnerType = tmpStruct.innerType;
                        row.Value = tmpStruct.GetValueHex();
                        break;
                    case GpkNameProperty tmpName:
                        row.Value = tmpName.value;
                        row.EditAsEnum = true;
                        break;
                    case GpkObjectProperty tmpObj:
                        row.Value = tmpObj.objectName;
                        row.EditAsEnum = true;
                        break;
                    case GpkByteProperty tmpByte when tmpByte.size == 8 || tmpByte.size == 16:
                        row.InnerType = tmpByte.enumType;
                        row.Value = tmpByte.nameValue;
                        row.EditAsEnum = true;
                        break;
                    case GpkByteProperty tmpByte:
                        row.InnerType = tmpByte.enumType;
                        row.Value = tmpByte.byteValue;
                        break;
                    case GpkFloatProperty tmpFloat:
                        row.Value = tmpFloat.value;
                        break;
                    case GpkIntProperty tmpInt:
                        row.Value = tmpInt.value;
                        break;
                    case GpkStringProperty tmpString:
                        row.Value = tmpString.value;
                        break;
                    case GpkBoolProperty tmpBool:
                        row.Value = tmpBool.value;
                        break;
                    default:
                        _logger.Info("Unk Prop?!?");
                        break;
                }

                //todo: button with expanded view
                //const int maxInputLength = 32;//32767; //from winforms
                //if (row.Value != null && row.Value.ToString().Length > maxInputLength)
                //{
                //    row.Value = "[##TOO_LONG##]";
                //}

                Properties.Add(row);
            }
        }
        private void SaveProperties()
        {
            //1. compare and alter
            //or 2. read and rebuild  -- this. we skip to the next in case of user input error.

            if (_selectedExport == null || _selectedPackage == null)
            {
                _logger.Info("save failed");
                return;
            }

            var list = new List<IProperty>();
            foreach (var prop in Properties)
            {
                try
                {
                    list.Add(prop.GetIProperty(_selectedPackage));
                }
                catch (Exception ex)
                {

                    _logger.Info("Failed to save row {0}, {1}!", Properties.IndexOf(prop), ex);
                }

            }

            _selectedExport.Properties = list;
            _logger.Info("Saved properties of export {0}.", _selectedExport.UID);
        }
        private void ClearProperties()
        {
            if (_selectedExport == null || _selectedPackage == null)
            {
                _logger.Info("save failed");
                return;
            }

            _selectedExport.Properties.Clear();
            DrawGrid(_selectedPackage, _selectedExport);
            _logger.Info("Cleared!");
        }
        private void ExportPropertiesToCsv()
        {
            //JSON?
            //CSV?
            //XML?
            //Name;Type;Size;ArrayIndex;InnerType;Value

            var builder = new StringBuilder();
            builder.AppendLine("Name;Type;Size;ArrayIndex;InnerType;Value");

            foreach (var row in Properties)
            {
                if (/*row.IsNewRow ||*/ row.Name == null)
                    continue;

                var csvRow =
                    $"{row.Name};{row.PropertyType};{row.Size};{row.ArrayIndex};{row.InnerType};{row.Value}";
                builder.AppendLine(csvRow);
            }


            var path = MiscFuncs.GenerateSaveDialog(_selectedExport.ObjectName, ".csv");
            if (path == "") return;

            Task.Factory.StartNew(() => File.WriteAllText(path, builder.ToString(), Encoding.UTF8));
        }

        private void ExportRawData()
        {
            if (_selectedExport != null)
            {
                if (_selectedExport.Data == null)
                {
                    _logger.Info("Length is zero. Nothing to export");
                    return;
                }

                var path = MiscFuncs.GenerateSaveDialog(_selectedExport.ObjectName, "");
                if (path == "") return;
                DataTools.WriteExportDataFile(path, _selectedExport);
            }
            else if (_selectedPackage != null && _selectedClass != "")
            {
                var exports = _selectedPackage.GetExportsByClass(_selectedClass);

                if (exports.Count == 0)
                {
                    _logger.Info("No exports found for class {0}.", _selectedClass);
                    return;
                }


                var dialog = new FolderBrowserDialog { SelectedPath = CoreSettings.Default.SaveDir };
                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    CoreSettings.Default.SaveDir = dialog.SelectedPath;

                    foreach (var exp in exports)
                    {
                        if (exp.Data == null) continue;
                        DataTools.WriteExportDataFile($"{dialog.SelectedPath}\\{exp.ObjectName}", exp);
                        _logger.Trace("save for " + exp.UID);
                    }
                }
            }
            else if (_selectedPackage != null)
            {
                var dialog = new FolderBrowserDialog { SelectedPath = CoreSettings.Default.SaveDir };
                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    CoreSettings.Default.SaveDir = dialog.SelectedPath;

                    foreach (var exp in _selectedPackage.ExportList.Values)
                    {
                        if (exp.Data == null) continue;
                        DataTools.WriteExportDataFile($"{dialog.SelectedPath}\\{exp.ClassName}\\{exp.ObjectName}", exp);
                        _logger.Trace("save for " + exp.UID);
                    }
                }
            }

            _logger.Info("Data was saved!");
        }
        private void ImportRawData()
        {
            if (_selectedExport == null)
            {
                _logger.Trace("no selected export");
                return;
            }
            if (_selectedExport.Data == null)
            {
                _logger.Trace("no export data");
                return;
            }

            var files = MiscFuncs.GenerateOpenDialog(false);
            if (files.Length == 0) return;
            var path = files[0];

            if (!File.Exists(path)) return;

            var buffer = File.ReadAllBytes(path);

            if (CoreSettings.Default.PatchMode)
            {
                //if (treeMain.SelectedNode.Parent.Parent == null) return;
                //var packageIndex = Convert.ToInt32(treeMain.SelectedNode.Parent.Parent.Name);
                var package = SelectedNode.FindPackageNode();
                var packageIndex = package.Index;
                if (buffer.Length > _selectedExport.Data.Length)
                {
                    //Too long, not possible without rebuiling the gpk
                    _logger.Info("File size too big for PatchMode. Size: " + buffer.Length + " Maximum Size: " +
                                _selectedExport.Data.Length);
                    return;
                }

                //selectedExport.data = buffer;
                Array.Copy(buffer, _selectedExport.Data, buffer.Length);

                _changedExports[packageIndex].Add(_selectedExport);

            }
            else
            {
                //Rebuild Mode
                //We force the rebuilder to recalculate the size. (atm we dont know how big the propertys are)
                _logger.Trace($"rebuild mode old size {_selectedExport.Data.Length} new size {buffer.Length}");

                _selectedExport.Data = buffer;
                _selectedExport.GetDataSize();
                _selectedPackage.Changes = true;
            }


            _logger.Info($"Replaced the data of {_selectedExport.ObjectName} successfully! Dont forget to save.");



        }
        private void RemoveObject()
        {
            if (_selectedPackage != null && _selectedExport == null)
            {
                _gpkStore.DeleteGpk(_selectedPackage);

                _logger.Info("Removed package {0}...", _selectedPackage.Filename);

                TreeMain.Children.Remove(SelectedNode);
                _selectedPackage = null;
                GeneralButtonsEnabled = false;
                RefreshIndexes();
                //GC.Collect(); //memory 
            }
            else if (_selectedPackage != null && _selectedExport != null)
            {
                _selectedPackage.ExportList.Remove(_selectedPackage.GetObjectKeyByUID(_selectedExport.UID));

                _logger.Info("Removed object {0}...", _selectedExport.UID);

                _selectedExport = null;

                SelectedNode.Remove();
                SelectedNode = null;
            }
        }

        private void RefreshIndexes()
        {
            Task.Run(() =>
            {
                foreach (var pkgNode in TreeMain.Children)
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        pkgNode.Index = _gpkStore.LoadedGpkPackages.IndexOf(pkgNode.SourcePackage);
                    }, DispatcherPriority.Background);
                }
            });
        }

        private void CopyObject()
        {
            if (_selectedExport == null)
            {
                _logger.Trace("no selected export");
                return;
            }

            try
            {
                var memorystream = new MemoryStream();
                var bf = new BinaryFormatter();
                bf.Serialize(memorystream, _selectedExport);
            }
            catch (Exception ex)
            {
                _logger.Debug(ex);
                _logger.Info("Serialize failed, check debug log");
                return;
            }

            Clipboard.SetData(_exportFormat.Name, _selectedExport);
            _logger.Info("Made a copy of {0}...", _selectedExport.UID);
        }
        private void PasteObject()
        {
            if (_selectedExport == null)
            {
                _logger.Trace("no selected export");
                return;
            }
            var copyExport = (GpkExport)Clipboard.GetData(_exportFormat.Name);

            if (copyExport == null)
            {
                _logger.Info("copy paste fail");
                return;
            }

            _logger.Trace(CoreSettings.Default.CopyMode);
            var option = "";

            switch (CoreSettings.Default.CopyMode)
            {
                case CopyMode.All:
                    DataTools.ReplaceAll(copyExport, _selectedExport);
                    option = "everything";
                    break;
                case CopyMode.DataProps:
                    DataTools.ReplaceProperties(copyExport, _selectedExport);
                    DataTools.ReplaceData(copyExport, _selectedExport);
                    option = "data and properties";
                    break;
                case CopyMode.Data:
                    DataTools.ReplaceData(copyExport, _selectedExport);
                    option = "data";
                    break;
                case CopyMode.Props:
                    DataTools.ReplaceProperties(copyExport, _selectedExport);
                    option = "properties";
                    break;
                default:
                    _logger.Info("Your setting file is broken. Go to settings windows and select a copymode.");
                    break;

            }

            _selectedExport.GetDataSize();
            _selectedExport.motherPackage.CheckAllNamesInObjects();

            SelectNode(SelectedNode);
            _logger.Info("Pasted the {0} of {1} to {2}", option, copyExport.UID, _selectedExport.UID);
        }
        private void InsertObject()
        {
            if (_selectedPackage == null)
            {
                _logger.Trace("no selected package to insert");
                return;
            }
            var copyExport = (GpkExport)Clipboard.GetData(_exportFormat.Name);

            if (copyExport == null)
            {
                _logger.Info("copy paste fail");
                return;
            }

            _selectedPackage.CopyObjectFromPackage(copyExport.UID, copyExport.motherPackage, true);
            _selectedPackage.CheckAllNamesInObjects();

            RedrawPackage(TreeMain.Children.FirstOrDefault(p => p.SourcePackage == _selectedPackage));
            _logger.Info("Insert done");
        }

        private void RedrawPackage(GpkTreeNode packageNode)
        {
            var package = packageNode.SourcePackage;
            packageNode.Children.Clear();
            packageNode.Index = _gpkStore.LoadedGpkPackages.IndexOf(package);
            //var packageNode = new GpkTreeNode(package.Filename, package)
            //{
            //    IsPackage = true,
            //    Index = gpkStore.LoadedGpkPackages.IndexOf(package)
            //};



            var classNodes = new Dictionary<string, GpkTreeNode>();
            GpkTreeNode importsNode = null;
            GpkTreeNode exportsNode = null;

            if (CoreSettings.Default.ShowImports)
            {
                foreach (var tmp in package.ImportList.OrderByDescending(pair => pair.Value.ObjectName)
                    .Reverse())
                {
                    var key = tmp.Value.UID;
                    var value = tmp.Value.ObjectName;
                    if (CoreSettings.Default.UseUID) value = key;

                    switch (CoreSettings.Default.ViewMode)
                    {
                        case ViewMode.Normal:
                            if (importsNode == null)
                            {
                                importsNode = new GpkTreeNode("Imports");
                                packageNode.AddNode(importsNode);
                            }

                            importsNode.AddNode(new GpkTreeNode(value, key) { IsImport = true });
                            break;
                        case ViewMode.Class:
                            GpkTreeNode.CheckClassNode(tmp.Value.ClassName, classNodes, packageNode);
                            var n = new GpkTreeNode( /*key,*/ value, key) { Class = tmp.Value.ClassName };
                            classNodes[tmp.Value.ClassName].AddNode(n);
                            break;
                    }

                }
            }

            //Exports
            foreach (var pair in package.ExportList.OrderByDescending(pair => pair.Value.ObjectName).Reverse())
            {
                var export = pair.Value;
                var key = export.UID;
                var value = export.ObjectName;
                if (CoreSettings.Default.UseUID) value = key;

                switch (CoreSettings.Default.ViewMode)
                {
                    case ViewMode.Normal:
                        if (exportsNode == null)
                        {
                            exportsNode = new GpkTreeNode("Exports");
                            packageNode.AddNode(exportsNode);
                        }

                        exportsNode.AddNode(new GpkTreeNode( /*key,*/ value, key));
                        break;
                    case ViewMode.Class:
                        GpkTreeNode.CheckClassNode(export.ClassName, classNodes, packageNode);
                        var cn = new GpkTreeNode( /*key,*/ value, key) { Class = export.ClassName };
                        classNodes[export.ClassName].AddNode(cn);
                        break;

                    case ViewMode.Package:
                        GpkTreeNode.CheckClassNode(pair.Value.PackageName, classNodes, packageNode);
                        var pn = new GpkTreeNode( /*key,*/ value, key) { Class = export.ClassName };
                        classNodes[export.PackageName].AddNode(pn);
                        break;

                }

            }
        }

        private void DeleteData()
        {
            if (_selectedExport == null)
            {
                _logger.Trace("no selected export");
                return;
            }

            if (_selectedExport.Data == null)
            {
                _logger.Trace("no export data");
                return;
            }

            _selectedExport.Loader = null;
            _selectedExport.Data = null;
            _selectedExport.DataPadding = null;
            _selectedExport.Payload = null;
            _selectedExport.GetDataSize();

            SelectNode(SelectedNode);
        }
        private void RenameObject()
        {
            //todo: rename directly in tree too?
            if (_selectedExport == null && _selectedImport == null)
            {
                _logger.Info("Select an import or export to rename");
                return;
            }

            var input = new InputBoxWindow("New name?").ShowDialog();
            if (input != "")
            {
                if (_selectedExport != null)
                {
                    _selectedExport.ObjectName = input;

                }
                else if (_selectedImport != null)
                {
                    _selectedImport.ObjectName = input;
                }

                _selectedPackage?.CheckAllNamesInObjects();

                //uid is not renamed to not break internal references. will be regenerated on a new load.
                _logger.Info($"Renamed object to the new name {input}. Experimental, stuff may break.");
            }

            DrawPackages();
        }

        private void ImportDDS()
        {
            if (_selectedExport == null)
            {
                _logger.Trace("no selected export");
                return;
            }

            if (!(_selectedExport.Payload is Texture2D))
            {
                _logger.Info("Not a Texture2D object");
                return;
            }

            var files = MiscFuncs.GenerateOpenDialog(false);
            if (files.Length == 0) return;

            if (files[0] != "" && File.Exists(files[0]))
            {
                TextureTools.importTexture(_selectedExport, files[0]);
                RefreshExportInfo(false);
            }
        }
        private void ExportDDS()
        {
            if (_selectedExport == null)
            {
                _logger.Trace("no selected export");
                return;
            }

            if (!(_selectedExport.Payload is Texture2D))
            {
                _logger.Info("Not a Texture2D object");
                return;
            }

            var path = MiscFuncs.GenerateSaveDialog(_selectedExport.ObjectName, ".dds");
            if (path != "")
            {
                new Task(() => TextureTools.exportTexture(_selectedExport, path)).Start();
            }


        }

        private void ImportOGG()
        {
            try
            {
                if (_selectedExport != null)
                {
                    var files = MiscFuncs.GenerateOpenDialog(false);
                    if (files.Length == 0) return;

                    if (File.Exists(files[0]))
                    {
                        SoundwaveTools.ImportOgg(_selectedExport, files[0]);
                        SelectNode(SelectedNode);
                        _logger.Info("Import successful.");
                    }
                    else
                    {
                        _logger.Info("File not found.");
                    }

                }
                else if (_selectedPackage != null && _selectedClass == "Core.SoundNodeWave")
                {
                    var exports = _selectedPackage.GetExportsByClass(_selectedClass);

                    var dialog = new FolderBrowserDialog
                    {
                        SelectedPath = Path.GetDirectoryName(CoreSettings.Default.SaveDir)
                    };
                    var result = dialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        CoreSettings.Default.SaveDir = dialog.SelectedPath;

                        var files = Directory.GetFiles(dialog.SelectedPath);

                        foreach (var file in files)
                        {
                            var filename = Path.GetFileName(file); //AttackL_02.ogg
                            var oggname = filename.Remove(filename.Length - 4);

                            if (oggname == "") continue;

                            foreach (var exp in exports)
                            {
                                if (exp.ObjectName == oggname)
                                {
                                    SoundwaveTools.ImportOgg(exp, file);
                                    _logger.Trace("Matched file {0} to export {1}!", filename, exp.ObjectName);
                                    break;
                                }
                            }


                        }


                        _logger.Info("Mass import to {0} was successful.", dialog.SelectedPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Import failure!");
            }
        }
        private void ExportOGG()
        {

            if (_selectedExport != null && _selectedExport.ClassName == "Core.SoundNodeWave")
            {
                var path = MiscFuncs.GenerateSaveDialog(_selectedExport.ObjectName, ".ogg");
                if (path != "")
                    SoundwaveTools.ExportOgg(_selectedExport, path);
            }
            else if (_selectedPackage != null && _selectedClass == "Core.SoundNodeWave")
            {
                var exports = _selectedPackage.GetExportsByClass(_selectedClass);

                if (exports.Count == 0)
                {
                    _logger.Info("No oggs found for class {0}.", _selectedClass);
                    return;
                }


                var dialog = new FolderBrowserDialog
                {
                    SelectedPath = Path.GetDirectoryName(CoreSettings.Default.SaveDir)
                };
                var result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    CoreSettings.Default.SaveDir = dialog.SelectedPath;

                    foreach (var exp in exports)
                    {
                        SoundwaveTools.ExportOgg(exp, $"{dialog.SelectedPath}\\{exp.ObjectName}.ogg");
                        _logger.Trace("ogg save for " + exp.UID);
                    }

                    _logger.Info("Mass export to {0} was successful.", dialog.SelectedPath);
                }
            }
        }
        private void AddEmptyOGG()
        {
            if (_selectedExport != null)
            {
                SoundwaveTools.ImportOgg(_selectedExport, "fake");
                SelectNode(SelectedNode);
            }
        }

        private void PatchObjectMapperForSelectedPackage()
        {
            if (!IsPackageSelected()) return;

            _gpkStore.MultiPatchObjectMapper(_selectedPackage, CoreSettings.Default.CookedPCPath);
        }
        private void PlayStopSound()
        {
            try
            {
                switch (SoundPreviewManager.PlaybackState)
                {
                    case PlaybackState.Playing:
                        SoundPreviewManager.PauseSound();
                        break;
                    default:
                        SoundPreviewManager.PlaySound();
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Playback Error");
            }
        }

        private void DecryptDat()
        {
            var files = MiscFuncs.GenerateOpenDialog(false);
            if (files.Length == 0) return;

            var outfile = MiscFuncs.GenerateSaveDialog("decrypt", ".txt");

            new Task(() =>
            {
                _logger.Info("Decryption is running in the background");

                MapperTools.DecryptAndWriteFile(files[0], outfile);

                _logger.Info("Decryption done");

            }).Start();
        }
        private void EncryptDat()
        {
            var files = MiscFuncs.GenerateOpenDialog(false);
            if (files.Length == 0) return;

            var outfile = MiscFuncs.GenerateSaveDialog("encrypt", ".txt");

            new Task(() =>
            {
                _logger.Info("Encryption is running in the background");

                MapperTools.EncryptAndWriteFile(files[0], outfile);

                _logger.Info("Encryption done");

            }).Start();
        }
        private void LoadMapping()
        {
            if (_gpkStore.CompositeMap.Count > 0)
            {
                new MapperWindow(_gpkStore).Show();
                return;
            }

            var dialog = new FolderBrowserDialog();
            if (CoreSettings.Default.CookedPCPath != "")
                dialog.SelectedPath = CoreSettings.Default.CookedPCPath;
            dialog.Description = "Select a folder with PkgMapper.dat and CompositePackageMapper.dat in it. Normally your CookedPC folder.";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            var path = dialog.SelectedPath + "\\";
            CoreSettings.Default.CookedPCPath = path;

            LoadAndParseMapping(path);

            new MapperWindow(_gpkStore).Show();
        }
        private void WriteMapping()
        {
            var dialog = new FolderBrowserDialog();
            if (CoreSettings.Default.WorkingDir != "")
                dialog.SelectedPath = CoreSettings.Default.WorkingDir;
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            var path = dialog.SelectedPath + "\\";

            MapperTools.WriteMappings(path, _gpkStore, true, true);
        }
        private void DumpAllTextures()
        {
            //cookedpc path, outdir path
            var dialog = new FolderBrowserDialog();
            if (CoreSettings.Default.CookedPCPath != "")
                dialog.SelectedPath = CoreSettings.Default.CookedPCPath + "\\";
            dialog.Description = "Select a folder with PkgMapper.dat and CompositePackageMapper.dat in it. Normally your CookedPC folder.";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;


            var path = dialog.SelectedPath + "\\";
            _gpkStore.BaseSearchPath = path;

            CoreSettings.Default.CookedPCPath = path;
            MapperTools.ParseMappings(path, _gpkStore);

            var subCount = _gpkStore.CompositeMap.Sum(entry => entry.Value.Count);
            _logger.Info("Parsed mappings, we have {0} composite GPKs and {1} sub-gpks!", _gpkStore.CompositeMap.Count, subCount);

            //selection
            var text = "";//todo Microsoft.VisualBasic.Interaction.InputBox("Select range of composite gpks to load. Format: n-n [e.g. 1-5] or empty for all.", "Selection", "");
            var filterList = FilterCompositeList(text);

            //save dir
            dialog = new FolderBrowserDialog
            {
                SelectedPath = CoreSettings.Default.WorkingDir,
                Description = "Select your output dir"
            };
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            _logger.Warn("Warning: This function can be ultra long running (hours) and unstable. Monitor logfile and output folder for progress.");
            _logger.Warn("Disabling logging, dump is running in the background. Consider setting file logging to only info.");

            NLogConfig.DisableFormLogging();
            var outDir = dialog.SelectedPath;
            new Task(() => MassDumper.DumpMassTextures(_gpkStore, outDir, filterList)).Start();
        }
        private void DumpIcons()
        {
            //cookedpc path, outdir path
            var dialog = new FolderBrowserDialog();
            if (CoreSettings.Default.CookedPCPath != "")
                dialog.SelectedPath = CoreSettings.Default.CookedPCPath;
            dialog.Description = "Select a folder with PkgMapper.dat and CompositePackageMapper.dat in it. Normally your CookedPC folder.";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            var path = dialog.SelectedPath;
            _gpkStore.BaseSearchPath = path;
            CoreSettings.Default.CookedPCPath = path;
            MapperTools.ParseMappings(path, _gpkStore);

            var subCount = _gpkStore.CompositeMap.Sum(entry => entry.Value.Count);
            _logger.Info("Parsed mappings, we have {0} composite GPKs and {1} sub-gpks!", _gpkStore.CompositeMap.Count, subCount);
            var list = FilterCompositeList("");
            //save dir
            dialog = new FolderBrowserDialog
            {
                SelectedPath = CoreSettings.Default.WorkingDir,
                Description = "Select your output dir"
            };
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            _logger.Warn("Warning: This function can be ultra long running (hours) and unstable. Monitor logfile and output folder for progress.");
            _logger.Warn("Disabling logging, dump is running in the background. Consider setting file logging to only info.");

            NLogConfig.DisableFormLogging();
            var outDir = dialog.SelectedPath;
            new Task(() => MassDumper.DumpMassIcons(_gpkStore, outDir, list)).Start();

        }
        private Dictionary<string, List<CompositeMapEntry>> FilterCompositeList(string text)
        {
            try
            {
                if (text != "" && text.Split('-').Length > 0)
                {
                    var start = Convert.ToInt32(text.Split('-')[0]) - 1;
                    var end = Convert.ToInt32(text.Split('-')[1]) - 1;
                    var filterCompositeList = _gpkStore.CompositeMap.Skip(start).Take(end - start + 1).ToDictionary(k => k.Key, v => v.Value);
                    _logger.Info("Filterd down to {0} GPKs.", end - start + 1);
                    return filterCompositeList;
                }
                else
                {
                    return _gpkStore.CompositeMap;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "filter fail");
                return _gpkStore.CompositeMap;
            }
        }
        private void MinimizeGPK()
        {
            if (!IsPackageSelected()) return;

            DataTools.RemoveObjectRedirects(_selectedPackage);
            if (SelectedNode?.SourcePackage == _selectedPackage)
                RedrawPackage(SelectedNode);
            else
                DrawPackages(); //shouldn't happen, just in case
        }
        private void LoadAndParseMapping(string path)
        {
            _gpkStore.BaseSearchPath = path;
            MapperTools.ParseMappings(path, _gpkStore);

            var subCount = _gpkStore.CompositeMap.Sum(entry => entry.Value.Count);
            _logger.Info("Parsed mappings, we have {0} composite GPKs and {1} sub-gpks!", _gpkStore.CompositeMap.Count, subCount);
        }

        private void SetFileSize()
        {
            if (!IsPackageSelected()) return;

            var input = new InputBoxWindow($"New filesize for {_selectedPackage.Filename}? Old: {_selectedPackage.OrginalSize}").ShowDialog();

            if (input == "" || !int.TryParse(input, out var num))
            {
                _logger.Info("No/Invalid input");
            }
            else
            {
                _logger.Trace(num);
                _selectedPackage.OrginalSize = num;
                _logger.Info("Set filesize for {0} to {1}", _selectedPackage.Filename, _selectedPackage.OrginalSize);
            }

        }
        private void SetPropsCustom()
        {
            if (!IsPackageSelected()) return;

            try
            {
                var className = new InputBoxWindow("Classname UID?\nWrite #all to select every object.\nSupported types: Int, Float (x,xx), Bool, String").ShowDialog();
                var propName = new InputBoxWindow("Proprty Name to edit?").ShowDialog();
                var propValue = new InputBoxWindow("Proprty Value:").ShowDialog();

                var exports = _selectedPackage.GetExportsByClass(className);

                SoundwaveTools.SetPropertyDetails(exports, propName, propValue);

                _logger.Info("Custom set success for {0} Objects.", exports.Count);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Custom update fail. Ex " + ex);
            }


        }
        private void SetVolumeMultipliers()
        {
            if (!IsPackageSelected()) return;

            var input = new InputBoxWindow($"New VolumeMultiplier for all SoundCues in {_selectedPackage.Filename}: \nFormat: x,xx").ShowDialog();

            if (input == "" || !float.TryParse(input, out var num))
            {
                _logger.Info("No/Invalid input");
            }
            else
            {
                _logger.Trace(num);
                SoundwaveTools.SetAllVolumes(_selectedPackage, num);
                _logger.Info("Set Volumes for {0} to {1}.", _selectedPackage.Filename, num);
            }
        }
        private void AddName()
        {
            if (!IsPackageSelected()) return;

            var input = new InputBoxWindow("Add a new name to the package:").ShowDialog();
            if (input == "") return;
            _selectedPackage.AddString(input);
            if (_selectedExport != null)
                DrawGrid(_selectedPackage, _selectedExport);
        }
        private bool IsPackageSelected()
        {
            if (_selectedPackage != null) return true;
            _logger.Info("Select a package!");
            return false;

        }
        private bool IsExportSelected()
        {
            if (_selectedExport != null) return true;
            _logger.Info("Select a export!");
            return false;
        }
        private void DumpGPKObjects()
        {
            NLogConfig.DisableFormLogging();

            var files = MiscFuncs.GenerateOpenDialog(true, true, "GPK (*.gpk;*.upk;*.gpk_rebuild)|*.gpk;*.upk;*.gpk_rebuild|All files (*.*)|*.*");
            if (files.Length == 0) return;

            var outfile = MiscFuncs.GenerateSaveDialog("dump", ".txt");

            new Task(() =>
            {
                _logger.Info("Dump is running in the background");
                MassDumper.DumpMassHeaders(outfile, files);
                _logger.Info("Dump done");

                NLogConfig.EnableFormLogging();
            }).Start();
        }
        private void BigBytePropExport()
        {
            var arrayProp = CheckArrayRow();

            if (arrayProp?.value == null) return;
            var data = new byte[arrayProp.value.Length - 4];
            Array.Copy(arrayProp.value, 4, data, 0, arrayProp.value.Length - 4); //remove count bytes

            var path = MiscFuncs.GenerateSaveDialog(arrayProp.name, "");
            if (path == "") return;

            DataTools.WriteExportDataFile(path, data);
        }
        private void BigBytePropImport()
        {
            var arrayProp = CheckArrayRow();

            if (arrayProp == null) return;

            var files = MiscFuncs.GenerateOpenDialog(false);
            if (files.Length == 0) return;
            var path = files[0];
            if (!File.Exists(path)) return;

            var data = File.ReadAllBytes(path);
            //readd count bytes 
            arrayProp.value = new byte[data.Length + 4];
            Array.Copy(BitConverter.GetBytes(data.Length), arrayProp.value, 4);
            Array.Copy(data, 0, arrayProp.value, 4, data.Length);

            DrawGrid(_selectedPackage, _selectedExport);
        }
        private GpkArrayProperty CheckArrayRow()
        {
            if (_selectedExport == null) return null;
            if (SelectedProperty == null)//(gridProps.SelectedRows.Count != 1)
            {
                _logger.Info("select a complete row (click the arrow in front of it)");
                return null;
            }

            //var row = gridProps.SelectedRows[0];
            if (/*row.Cells["type"].Value.ToString() */ SelectedProperty.PropertyType != "ArrayProperty")
            {
                _logger.Info("select a arrayproperty row");
                return null;
            }

            return (GpkArrayProperty)SelectedProperty.GetIProperty(_selectedPackage);
        }

        private void Search()
        {
            var input = new InputBoxWindow("String to search?").ShowDialog();

            if (string.IsNullOrEmpty(input))
                return;

            _searchResultNodes.Clear();
            _searchResultIndex = 0;

            foreach (var node in TreeMain.Collect())
            {
                if (node.Key.ToLowerInvariant().Contains(input.ToLowerInvariant().Trim()))
                {
                    _searchResultNodes.Add(node);
                }
            }

            if (_searchResultNodes.Count > 0)
            {
                SelectNextSearchResult();
            }
            else
            {
                _logger.Info($"Nothing found for '{input}'!");
            }


        }
        private void SelectNextSearchResult(bool previous = false)
        {
            if (!CheckSearch()) return;

            SelectNode(_searchResultNodes[_searchResultIndex]);
            _searchResultNodes[_searchResultIndex].ParentsToPackage.ForEach(p => p.IsExpanded = true);
            _searchResultNodes[_searchResultIndex].IsSelected = true;
            if (!previous)
            {

                StatusBarText = $"Result {_searchResultIndex + 1}/{_searchResultNodes.Count}";
                _searchResultIndex++;
            }
            else
            {
                StatusBarText = $"Result {_searchResultIndex - 1}/{_searchResultNodes.Count}";
                _searchResultIndex--;
            }


            bool CheckSearch()
            {
                var found = true;
                var msg = "";
                if (_searchResultNodes.Count == 0)
                {
                    _searchResultIndex = 0;
                    msg = "No items found.";
                    found = false;
                }

                if (!previous && _searchResultIndex >= _searchResultNodes.Count)
                {
                    _searchResultIndex = 0;
                    msg = "End reached, searching from start.";
                    found = false;
                }

                if (previous && _searchResultIndex == 0)
                {
                    _searchResultIndex = _searchResultNodes.Count - 1;
                    msg = "Start reached, searching from end.";
                    found = false;
                }

                if (found) return true;

                SystemSounds.Asterisk.Play();
                StatusBarText = msg;

                return false;
            }

        }

        private void TryToLoadAllExportDataFromComposite()
        {
            if (!IsPackageSelected())
            {
                return;
            }

            foreach (var export in _selectedPackage.ExportList.Values)
            {
                LoadCompositeDataForExport(export);
            }
        }

        private void LoadCompositeDataForExport(GpkExport export)
        {

            //strat. find new name in pkgmapper, find comp entry in compmapper, load composite
            //hook adding of composite, replace all data and popoup a message

            var redirectUID = _gpkStore.FindObjectMapperEntryForObjectname(export.GetNormalizedUID());
            var compEntry = _gpkStore.FindCompositeMapEntriesForCompID(redirectUID);
            if (compEntry == null)
                return;

            _logger.Info($"Trying to load GPK {compEntry.SubGPKName} for Object {compEntry.CompositeUID}");

            var path = $"{_gpkStore.BaseSearchPath}{compEntry.SubGPKName}.gpk";

            if (!File.Exists(path))
            {
                _logger.Info("GPK to load not found");
                return;
            }

            new Task(() =>
            {
                var gpk = _gpkStore.loadSubGpk(path, compEntry);

                var obj = gpk.GetObjectByUID(compEntry.GetObjectName());
                if (!(obj is GpkExport))
                {
                    _logger.Error("Somehow found obj is not a export");
                    return;
                }
                var exportObj = (GpkExport)obj;

                _logger.Info($"Found something! Data to import is in {exportObj.UID}");

                DataTools.ReplaceAll(exportObj, export);

                export.GetDataSize();
                export.motherPackage.CheckAllNamesInObjects();

                _logger.Info("Done, succesfully imported composite data!");
            }).Start();
        }
        private void LoadCompositeDataForSelectedExport()
        {
            if (!IsExportSelected())
            {
                return;
            }

            LoadCompositeDataForExport(_selectedExport);
        }

    }

    public class RecentFileViewModel
    {
        public string FullPath { get; }
        public string FileName { get; }
        public ICommand OpenCommand { get; }

        public RecentFileViewModel(string path)
        {
            FullPath = path;
            FileName = Path.GetFileName(path);
            OpenCommand = new RelayCommand(Open);
        }

        private void Open()
        {
            MainViewModel.Instance.OpenCommand.Execute(new[] { FullPath });
        }
    }
}