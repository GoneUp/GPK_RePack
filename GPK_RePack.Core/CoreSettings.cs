using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using Size = System.Windows.Size;

namespace GPK_RePack.Core
{
    public enum ViewMode
    {
        [Description("Normal (imports/exports)")]
        Normal,
        [Description("Per class")]
        Class,
        [Description("Per package")]
        Package
    }

    public enum CopyMode
    {
        [Description("Data and properties")]
        DataProps,
        [Description("Data")]
        Data,
        [Description("Properties")]
        Props,
        [Description("Everything")]
        All
    }


    public class CoreSettings
    {
        public event Action RecentFilesChanged;

        [JsonIgnore]
        public static CoreSettings Default { get; private set; }

        public CopyMode CopyMode { get; set; } = CopyMode.All;
        public string LogLevel { get; set; } = "info";
        public bool Debug { get; set; } = false;
        public ViewMode ViewMode { get; set; } = ViewMode.Class;
        public string SaveDir { get; set; } = "";
        public string OpenDir { get; set; } = "";
        public bool ShowImports { get; set; } = false;
        public bool PatchMode { get; set; } = false;
        public bool UseUID { get; set; } = false;
        public bool JitData { get; set; } = false;
        public bool GenerateMipMaps { get; set; } = false;
        public float ScaleFactorHack { get; set; } = 1.0F;
        public bool EnableTexture2D { get; set; } = true;
        public string WorkingDir { get; set; } = "";
        public Color PreviewColor { get; set; } = Color.Transparent;
        public string SaveFileSuffix { get; set; } = "_rebuild";
        public string CookedPCPath { get; set; } = "";
        public bool EnableCompression { get; set; } = false;
        public bool EnableSortTreeNodes { get; set; } = false;
        public bool LoadMappingOnStart { get; set; } = true;
        public GridLength LogSize { get; set; } = new GridLength(1, GridUnitType.Star);
        public GridLength TopSize { get; set; } = new GridLength(2, GridUnitType.Star);
        public GridLength TreeViewSize { get; set; } = new GridLength(1, GridUnitType.Star);
        public GridLength PropViewSize { get; set; } = new GridLength(2, GridUnitType.Star);
        public Size WindowSize { get; set; } = new Size(1300, 800);
        public WindowState WindowState { get; set; } = WindowState.Normal;
        public bool LogToUI { get; set; } = true;
        public List<string> RecentFiles { get; set; } = new List<string>();

        public void AddRecentFile(string path)
        {
            if (RecentFiles.Count >= 10)
            {
                var last = RecentFiles.Last();
                RecentFiles.Remove(last);
            }

            if (RecentFiles.Any(s => s == path))
            {
                RecentFiles.Remove(path);
            }

            RecentFiles.Insert(0, path);

            RecentFilesChanged?.Invoke();
        }

        public static void Load()
        {
            Default = File.Exists("settings.json")
                ? JsonConvert.DeserializeObject<CoreSettings>(File.ReadAllText("settings.json"))
                : new CoreSettings();
        }
        public static void Save()
        {
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(Default, Formatting.Indented));
        }
    }
}
