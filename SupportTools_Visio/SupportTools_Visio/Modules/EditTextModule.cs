using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Unity;

using VNC;

using SupportTools_Visio.Infrastructure;
using SupportTools_Visio.Presentation.Views;

namespace SupportTools_Visio.Modules
{
    public class EditTextModule : IModule
    {
        // 01
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY, 0);


            Log.Trace("Exit", Common.LOG_CATEGORY, 0, startTicks);
        }

        // 02
        public void OnInitialized(IContainerProvider containerProvider)
        {
            long startTicks = Log.Trace("Enter", Common.LOG_CATEGORY, 0);

            var regionManager = containerProvider.Resolve<IRegionManager>();

            // Multiple ToolBar Regions

            //IRegion region = regionManager.Regions[RegionNames.ToolBarRegionA];

            //region.Add(containerProvider.Resolve<ToolBarView>());
            //region.Add(containerProvider.Resolve<ToolBarView>());
            //region.Add(containerProvider.Resolve<ToolBarView>());
            //region.Add(containerProvider.Resolve<ToolBarView>());
            //region.Add(containerProvider.Resolve<ToolBarView>());

            //regionManager.RegisterViewWithRegion(RegionNames.ToolBarRegionA, typeof(ToolBarView));

            // NOTE(crhodes)
            // Can't get this to work.  Hum.  May have to eschew multiple toolbars :)

            //IRegion region = regionManager.Regions[RegionNames.ToolBarRegionA];

            //region.Add(containerProvider.Resolve<ToolBarView>());
            //region.Add(containerProvider.Resolve<ToolBarView>());

            regionManager.RegisterViewWithRegion(RegionNames.EditTextRegion, typeof(EditText));

            regionManager.RegisterViewWithRegion(RegionNames.EditControlPointsRegion, typeof(EditControlPoints));

            regionManager.RegisterViewWithRegion(RegionNames.EditParagraphRegion, typeof(EditParagraph));

            Log.Trace("Exit", Common.LOG_CATEGORY, 0, startTicks);
        }
    }
}
