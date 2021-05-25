using System;
using System.Windows;
using System.Windows.Controls;

using ModuleA;

using Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Prism.Unity;

using SupportTools_Excel.Modules;
using SupportTools_Excel.Presentation.ViewModels;
using SupportTools_Excel.Presentation.Views;

using Unity;

using VNC;
using VNC.Core.Mvvm.Prism;
using VNC.Core.Services;

namespace SupportTools_Excel.Application
{
    public class Bootstrapper : PrismBootstrapperBase
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

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            moduleCatalog.AddModule(typeof(ModuleAModule));

            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule(typeof(SupportTools_ExcelModule));

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }


        //protected override void ConfigureModuleCatalog()
        //{
        //    long startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.PROJECT_NAME);

        //    var moduleCatalog = (ModuleCatalog)ModuleCatalog;
        //    moduleCatalog.AddModule(typeof(ModuleAModule));

        //    //moduleCatalog.AddModule(typeof(EditTextModule));

        //    //Type moduleAType = typeof(ModuleAModule);

        //    //moduleCatalog.AddModule(new ModuleInfo()
        //    //{
        //    //    ModuleName = moduleAType.Name,
        //    //    ModuleType = moduleAType.AssemblyQualifiedName,
        //    //    InitializationMode = InitializationMode.WhenAvailable
        //    //    // InitializationMode = InitializationMode.OnDemand
        //    //});

        //    Log.APPLICATION_INITIALIZE("Exit", Common.PROJECT_NAME, startTicks);
        //}

        // Step 2 - Configure the container

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            // Registers all types that are required by Prism to function with the container.

            base.RegisterRequiredTypes(containerRegistry);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            // Used to register types with the container that will be used by your application.
            containerRegistry.Register<IMessageDialogService, MessageDialogService>();

            containerRegistry.Register<CatViewModel>();

            containerRegistry.Register<Cat>();
            containerRegistry.Register<Cat3>();

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //protected override void ConfigureContainer()
        //{
        //    long startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.PROJECT_NAME);
        //    //Container.RegisterType<IEditTextViewModel, EditTextViewModel>();
        //    //Container.RegisterType<EditText>();

        //    Container.RegisterType<CatViewModel>();
        //    Container.RegisterType<Cat>();
        //    Container.RegisterType<Cat3>();

        //    base.ConfigureContainer();


        //    // Create a Singleton ShellService (DialogService)
        //    //Container.RegisterType<IShellService, ShellService>(new ContainerControlledLifetimeManager());

        //    Log.APPLICATION_INITIALIZE("Exit", Common.PROJECT_NAME, startTicks);
        //}

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

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        //{
        //    long startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.PROJECT_NAME);

        //    RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

        //    mappings.RegisterMapping(typeof(StackPanel), Container.TryResolve<StackPanelRegionAdapter>());

        //    Log.APPLICATION_INITIALIZE("Exit", Common.PROJECT_NAME, startTicks);

        //    return mappings;
        //}

        protected override IContainerExtension CreateContainerExtension()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);

            return new UnityContainerExtension();
        }

    }
}
