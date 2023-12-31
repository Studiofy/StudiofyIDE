using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using WinUIEx;

namespace StudiofyIDE
{
    public sealed partial class SplashScreen : WinUIEx.SplashScreen
    {
        public SplashScreen(Type window) : base(window)
        {
            InitializeComponent();
            Title = string.Empty;
            this.SetIsShownInSwitchers(false);
            CheckWindowsVersion();
            VersionText.Text = "Canary Version: " + Package.Current.Id.Version.Major + "."
                                                  + Package.Current.Id.Version.Minor + "."
                                                  + Package.Current.Id.Version.Build + "."
                                                  + Package.Current.Id.Version.Revision;
        }

        private void CheckWindowsVersion()
        {
            OperatingSystem WinOS = Environment.OSVersion;
            if (WinOS.Version.Build < 22000)
            {
                Backdrop = new AcrylicSystemBackdrop();
            }
            else if (WinOS.Version.Build >= 22000)
            {
                Backdrop = new MicaSystemBackdrop();
            }
        }

        protected override async Task OnLoading()
        {
            for (int i = 0; i <= 100; i++)
            {
                LoadingBar.IsIndeterminate = true;
                
                if (Application.Current.Resources.Source != null)
                {
                    AssemblyName assemblyName = new AssemblyName(Application.Current.Resources.Source.AbsolutePath);
                    Assembly assembly = Assembly.Load(assemblyName);
                    var types = assembly.GetTypes().Where(t => typeof(UIElement).IsAssignableFrom(t));
                    foreach (var type in types)
                    {
                        ProgressText.Text = $"Loaded Component: {type.Name}";
                    }
                }
                else
                {
                    ProgressText.Text = "Loading Core Libraries";
                    for (int j = 0; j < i % 4; j++)
                    {
                        ProgressText.Text += ".";
                    }
                }
                await Task.Delay(250);
            }
            return;
        }
    }
}
