using System.Reflection;
using System.Windows;
using GPK_RePack.Core;

namespace GPK_RePack_WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetAlignment();
            CoreSettings.Load();
        }

        private static void SetAlignment()
        {
            var ifLeft = SystemParameters.MenuDropAlignment;
            if (!ifLeft) return;
            var t = typeof(SystemParameters);
            var field = t.GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            if (field != null) field.SetValue(null, false);
        }

    }


}
