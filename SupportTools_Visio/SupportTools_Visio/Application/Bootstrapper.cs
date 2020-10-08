using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ModuleA;

using Prism.Modularity;
using Prism.Regions;

using Prism.Unity;
using SupportTools_Visio.Modules;
using SupportTools_Visio.Presentation.ViewModels;
using SupportTools_Visio.Presentation.Views;
using Unity;
using Unity.Lifetime;
using VNC;
using VNC.Core.Mvvm.Prism;

//using VNC.Core.Mvvm;
//using VNC.Core.Mvvm.Prism;

namespace SupportTools_Visio.Application
{
    class Bootstrapper : UnityBootstrapper
    {
        // Step 1a - Create the catalog of Modules

        protected override IModuleCatalog CreateModuleCatalog()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            Log.Trace("Exit", Common.PROJECT_NAME);
            return new ConfigurationModuleCatalog();
        }

        // Step 1b - Configure the catalog of modules
        // Modules are loaded at Startup and must be a project reference

        protected override void ConfigureModuleCatalog()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(ModuleAModule));

            moduleCatalog.AddModule(typeof(EditTextModule));

            //Type moduleAType = typeof(ModuleAModule);

            //moduleCatalog.AddModule(new ModuleInfo()
            //{
            //    ModuleName = moduleAType.Name,
            //    ModuleType = moduleAType.AssemblyQualifiedName,
            //    InitializationMode = InitializationMode.WhenAvailable
            //    // InitializationMode = InitializationMode.OnDemand
            //});
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        // Step 2 - Configure the container

        protected override void ConfigureContainer()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            Container.RegisterType<IEditTextViewModel, EditTextViewModel>();
            Container.RegisterType<EditText>();

            Container.RegisterType<EditParagraphViewModel>();
            Container.RegisterType<EditParagraph>();


            Container.RegisterType<EditControlRowsViewModel>();
            Container.RegisterType<EditControlRows>();

            base.ConfigureContainer();
            Log.Trace("Exit", Common.PROJECT_NAME);

            // Create a Singleton ShellService (DialogService)
            //Container.RegisterType<IShellService, ShellService>(new ContainerControlledLifetimeManager());
        }

        // Step 3 - Configure the RegionAdapters if any custom ones have been created

        // Step 4 - Create the Shell that will hold the modules in designated regions.

        protected override DependencyObject CreateShell()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            Log.Trace("Exit (null)", Common.PROJECT_NAME);
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
            Log.Trace("Enter", Common.PROJECT_NAME);
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

            mappings.RegisterMapping(typeof(StackPanel), Container.TryResolve<StackPanelRegionAdapter>());

            Log.Trace("Exit", Common.PROJECT_NAME);
            return mappings;
        }
    }
}
