using Newtonsoft.Json;
using System.Drawing;
using System.IO;

namespace GPK_RePack.Core
{
    public class CoreSettings
    {
        [JsonIgnore]
        public static CoreSettings Default { get; private set; }

        public string CopyMode { get; set; } = "dataprops";
        public string LogLevel { get; set; } = "info";
        public bool Debug { get; set; } = false;
        public string ViewMode { get; set; } = "class";
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
