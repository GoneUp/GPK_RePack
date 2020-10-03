using GPK_RePack.Core;
using Nostrum;
using Nostrum.Extensions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace GPK_RePack_WPF.Windows
{
    public partial class SettingsWindow : Window
    {
        public ICommand CloseCommand { get; set; }

        public SettingsWindow()
        {
            InitializeComponent();

            DataContext = new SettingsViewModel();

            CloseCommand = new RelayCommand(_ =>
            {
                CoreSettings.Save();
                Close();
            });
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            this.TryDragMove();
        }
    }

    public class SettingsViewModel : TSPropertyChanged
    {
        public List<string> LogLevels { get; } = new List<string> { "Info", "Debug", "Trace" };
        public List<CopyMode> CopyModes { get; } = Nostrum.EnumUtils.ListFromEnum<CopyMode>();
        public List<ViewMode> ViewModes { get; } = Nostrum.EnumUtils.ListFromEnum<ViewMode>();

        public CopyMode CopyMode
        {
            get => CoreSettings.Default.CopyMode;
            set
            {
                if (CoreSettings.Default.CopyMode == value) return;
                CoreSettings.Default.CopyMode = value;
                N();
            }
        }
        public ViewMode ViewMode
        {
            get => CoreSettings.Default.ViewMode;
            set
            {
                if (CoreSettings.Default.ViewMode == value) return;
                CoreSettings.Default.ViewMode = value;
                N();
            }
        }
        public string LogLevel
        {
            get => CoreSettings.Default.LogLevel.ToCapital();
            set
            {
                if (CoreSettings.Default.LogLevel == value.ToLowerInvariant()) return;
                CoreSettings.Default.LogLevel = value.ToLowerInvariant();
                N();
            }
        }
        public bool ShowImports
        {
            get => CoreSettings.Default.ShowImports;
            set
            {
                if (CoreSettings.Default.ShowImports == value) return;
                CoreSettings.Default.ShowImports = value;
                N();
            }
        }
        public bool UseUID
        {
            get => CoreSettings.Default.UseUID;
            set
            {
                if (CoreSettings.Default.UseUID == value) return;
                CoreSettings.Default.UseUID = value;
                N();
            }
        }
        public bool PatchMode
        {
            get => CoreSettings.Default.PatchMode;
            set
            {
                if (CoreSettings.Default.PatchMode == value) return;
                CoreSettings.Default.PatchMode = value;
                N();
            }
        }
        public bool JitData
        {
            get => CoreSettings.Default.JitData;
            set
            {
                if (CoreSettings.Default.JitData == value) return;
                CoreSettings.Default.JitData = value;
                N();
            }
        }
        public bool DebugMode
        {
            get => CoreSettings.Default.Debug;
            set
            {
                if (CoreSettings.Default.Debug == value) return;
                CoreSettings.Default.Debug = value;
                N();
            }
        }
        public bool TextureSupport
        {
            get => CoreSettings.Default.Debug;
            set
            {
                if (CoreSettings.Default.Debug == value) return;
                CoreSettings.Default.Debug = value;
                N();
            }
        }
        public bool GenerateMipMaps
        {
            get => CoreSettings.Default.GenerateMipMaps;
            set
            {
                if (CoreSettings.Default.GenerateMipMaps == value) return;
                CoreSettings.Default.GenerateMipMaps = value;
                N();
            }
        }
        public bool LoadMappingOnStart
        {
            get => CoreSettings.Default.LoadMappingOnStart;
            set
            {
                if (CoreSettings.Default.LoadMappingOnStart == value) return;
                CoreSettings.Default.LoadMappingOnStart = value;
                N();
            }
        }
        public bool EnableCompression
        {
            get => CoreSettings.Default.EnableCompression;
            set
            {
                if (CoreSettings.Default.EnableCompression == value) return;
                CoreSettings.Default.EnableCompression = value;
                N();
            }
        }
        public string SaveFileSuffix
        {
            get => CoreSettings.Default.SaveFileSuffix;
            set
            {
                if (CoreSettings.Default.SaveFileSuffix == value) return;
                CoreSettings.Default.SaveFileSuffix = value;
                N();
            }
        }

    }
}
