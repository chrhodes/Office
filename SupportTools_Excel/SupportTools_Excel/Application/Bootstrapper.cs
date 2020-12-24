using System.Windows;
using System.Windows.Controls;

using ModuleA;

using Prism.Modularity;
using Prism.Regions;

using Prism.Unity;
using SupportTools_Excel.Presentation.ViewModels;
using SupportTools_Excel.Presentation.Views;

using Unity;

using VNC;
using VNC.Core.Mvvm.Prism;

namespace SupportTools_Excel.Application
{
    public class Bootstrapper : UnityBootstrapper
    {
        // Step 1a - Create the catalog of Modules

        protected override IModuleCatalog CreateModuleCatalog()
        {
            long startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.PROJECT_NAME);
            Log.APPLICATION_INITIALIZE("Exit", Common.PROJECT_NAME, startTicks);

            return new ConfigurationModuleCatalog();
        }

        // Step 1b - Configure the catalog of modules
        // Modules are loaded at Startup and must be a project reference

        protected override void ConfigureModuleCatalog()
        {
            long startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.PROJECT_NAME);

            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(ModuleAModule));

            //moduleCatalog.AddModule(typeof(EditTextModule));

            //Type moduleAType = typeof(ModuleAModule);

            //moduleCatalog.AddModule(new ModuleInfo()
            //{
            //    ModuleName = moduleAType.Name,
            //    ModuleType = moduleAType.AssemblyQualifiedName,
            //    InitializationMode = InitializationMode.WhenAvailable
            //    // InitializationMode = InitializationMode.OnDemand
            //});

            Log.APPLICATION_INITIALIZE("Exit", Common.PROJECT_NAME, startTicks);
        }

        // Step 2 - Configure the container

        protected override void ConfigureContainer()
        {
            long startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.PROJECT_NAME);
            //Container.RegisterType<IEditTextViewModel, EditTextViewModel>();
            //Container.RegisterType<EditText>();

            Container.RegisterType<CatViewModel>();
            Container.RegisterType<Cat>();
            Container.RegisterType<Cat3>();

            base.ConfigureContainer();


            // Create a Singleton ShellService (DialogService)
            //Container.RegisterType<IShellService, ShellService>(new ContainerControlledLifetimeManager());

            Log.APPLICATION_INITIALIZE("Exit", Common.PROJECT_NAME, startTicks);
        }

        // Step 3 - Configure the RegionAdapters if any custom ones have been created

        // Step 4 - Create the Shell that will hold the modules in designated regions.

        protected override DependencyObject CreateShell()
        {
            long startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.PROJECT_NAME);
            Log.APPLICATION_INITIALIZE($"Exit (null)", Common.PROJECT_NAME, startTicks);

            return null;
            //return Container.Resolve<Views.MainWindow>();
            //return Container.TryResolve<Views.MainWindow>();
        }

        // Step 5 - Show the MainWindow

        //protected override void InitializeShell()
        //{
        //    var regionManager = RegionManager.GetRegionManager(Shell);
        //    RegionManagerAware.SetRegionManagerAware(Shell, regionManager);

        //    Application.Current.MainWindow.Show();

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            long startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.PROJECT_NAME);

            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(StackPanel), Container.TryResolve<StackPanelRegionAdapter>());

            Log.APPLICATION_INITIALIZE("Exit", Common.PROJECT_NAME, startTicks);

            return mappings;
        }
    }
}
