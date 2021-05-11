using System;
using System.Windows;
using System.Windows.Controls;

using ModuleA;

using Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

using SupportTools_Visio.Modules;
using SupportTools_Visio.Presentation.ViewModels;
using SupportTools_Visio.Presentation.Views;

using VNC;
using VNC.Core.Mvvm.Prism;

//using VNC.Core.Mvvm;
//using VNC.Core.Mvvm.Prism;

namespace SupportTools_Visio.Application
{
    public class Bootstrapper : PrismBootstrapperBase
    {
        // Step 1a - Create the catalog of Modules

        protected override IModuleCatalog CreateModuleCatalog()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
            return new ConfigurationModuleCatalog();
        }

        // Step 1b - Configure the catalog of modules
        // Modules are loaded at Startup and must be a project reference

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            moduleCatalog.AddModule(typeof(ModuleAModule));

            moduleCatalog.AddModule(typeof(EditTextModule));

            base.ConfigureModuleCatalog(moduleCatalog);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //protected override void ConfigureModuleCatalog()
        //{
        //    Log.Trace("Enter", Common.LOG_CATEGORY);
        //    var moduleCatalog = (ModuleCatalog)ModuleCatalog;
        //    moduleCatalog.AddModule(typeof(ModuleAModule));

        //    moduleCatalog.AddModule(typeof(EditTextModule));

        //    //Type moduleAType = typeof(ModuleAModule);

        //    //moduleCatalog.AddModule(new ModuleInfo()
        //    //{
        //    //    ModuleName = moduleAType.Name,
        //    //    ModuleType = moduleAType.AssemblyQualifiedName,
        //    //    InitializationMode = InitializationMode.WhenAvailable
        //    //    // InitializationMode = InitializationMode.OnDemand
        //    //});
        //    Log.Trace("Exit", Common.LOG_CATEGORY);
        //}

        // Step 2 - Configure the container

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            containerRegistry.Register<IEditTextViewModel, EditTextViewModel>();
            containerRegistry.Register<EditText>();

            containerRegistry.Register<EditParagraphViewModel>();
            containerRegistry.Register<EditParagraph>();

            containerRegistry.Register<EditControlRowsViewModel>();
            containerRegistry.Register<EditControlRows>();

            base.RegisterRequiredTypes(containerRegistry);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //protected override void ConfigureContainer()
        //{
        //    Log.Trace("Enter", Common.LOG_CATEGORY);
        //    Container.RegisterType<IEditTextViewModel, EditTextViewModel>();
        //    Container.RegisterType<EditText>();

        //    Container.RegisterType<EditParagraphViewModel>();
        //    Container.RegisterType<EditParagraph>();


        //    Container.RegisterType<EditControlRowsViewModel>();
        //    Container.RegisterType<EditControlRows>();

        //    base.ConfigureContainer();
        //    Log.Trace("Exit", Common.LOG_CATEGORY);

        //    // Create a Singleton ShellService (DialogService)
        //    //Container.RegisterType<IShellService, ShellService>(new ContainerControlledLifetimeManager());
        //}

        // Step 3 - Configure the RegionAdapters if any custom ones have been created

        // Step 4 - Create the Shell that will hold the modules in designated regions.

        protected override DependencyObject CreateShell()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            Log.APPLICATION_INITIALIZE("Exit (null)", Common.LOG_CATEGORY, startTicks);
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

            // TODO(crhodes)
            // Figure out this next line

            //regionAdapterMappings.RegisterMapping(typeof(StackPanel), (IRegionAdapter)typeof(StackPanelRegionAdapter));
            //regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.TryResolve<StackPanelRegionAdapter>());
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        //{
        //    Log.Trace("Enter", Common.LOG_CATEGORY);
        //    RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

        //    mappings.RegisterMapping(typeof(StackPanel), Container.TryResolve<StackPanelRegionAdapter>());

        //    Log.Trace("Exit", Common.LOG_CATEGORY);
        //    return mappings;
        //}

        //protected override IContainerExtension CreateContainerExtension()
        //{
        //    Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

        //    Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);

        //}

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            base.ConfigureDefaultRegionBehaviors(regionBehaviors);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            Int64 startTicks = Log.APPLICATION_INITIALIZE("Enter", Common.LOG_CATEGORY);

            Log.APPLICATION_INITIALIZE("Exit", Common.LOG_CATEGORY, startTicks);

            return new UnityContainerExtension();
        }
    }
}
