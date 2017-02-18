using System;
using System.IO;
using Catel.Data;
using Catel.Runtime.Serialization;
using Catel.Runtime.Serialization.Xml;
using VKbox.Models;

namespace VKbox
{
    using System.Windows;

    using Catel.ApiCop;
    using Catel.ApiCop.Listeners;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private Settings set;
        private SerializationConfiguration _sc = new SerializationConfiguration();

        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener();
#endif

            Log.Info("Starting application");

            // To force the loading of all assemblies at startup, uncomment the lines below:

            //Log.Info("Preloading assemblies");
            //AppDomain.CurrentDomain.PreloadAssemblies();


            // Want to improve performance? Uncomment the lines below. Note though that this will disable
            // some features. 
            //
            // For more information, see https://catelproject.atlassian.net/wiki/display/CTL/Performance+considerations

            // Log.Info("Improving performance");
            // Catel.Data.ModelBase.DefaultSuspendValidationValue = true;
            // Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            // Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;


            // TODO: Register custom types in the ServiceLocator
            //Log.Info("Registering custom types");
            
            if (File.Exists("Config.txt"))
            {
                using (var reader = new StreamReader("Config.txt"))
                {
                   set = ModelBase.Load<Settings>(reader.BaseStream, SerializationMode.Xml, _sc);        
                }
            }
            else
            {
                set = new Settings();
            }
            var serviceLocator = ServiceLocator.Default;
                serviceLocator.RegisterInstance(typeof (Settings), set);
           
            StyleHelper.CreateStyleForwardersForDefaultStyles();

            Log.Info("Calling base.OnStartup");

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Get advisory report in console
            ApiCopManager.AddListener(new ConsoleApiCopListener());
            ApiCopManager.WriteResults();
            using (
                var writer =
                    new StreamWriter("Config.txt"))
                set.Save(writer.BaseStream, SerializationMode.Xml, _sc);

            base.OnExit(e);
        }
    }
}