using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;

using Microsoft.Office.Tools.Ribbon;
using Visio = Microsoft.Office.Interop.Visio;

using VNC;
using VNC.WPF.Presentation.Dx.Views;
using VNC.WPF.Presentation.Views;
using SupportTools_Visio.Domain;

namespace SupportTools_Visio
{
    public partial class Ribbon
    {
        // NOTE(crhodes)
        // This was moved out of designer so we can call bootstrapper.

        public Ribbon()
           : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
            var bootstrapper = new Application.Bootstrapper();
            bootstrapper.Run();
        }

        public static VNC.Core.Xaml.Presentation.WindowHost windowHostVNC = null;

        #region Event Handlers

        private void btnAddInInfo_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayAddInInfo();
        }

        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        #region Visio_Application Events

        private void btnGetApplicationInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Application.DisplayInfo();
        }

        private void btnLoadLayers_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Application.LayerManager();
        }

        #endregion Visio_Application Events

        #region Visio_Stencil Events

        private void btnGetStencilInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Stencil.GatherInfo();
        }

        #endregion Visio_Stencil Events

        #region Visio_Document Events

        // These are on Document Group

        private void btnAddDefaultLayers_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AddDefaultLayers();
        }

        private void btnAddFooter_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AddFooter();
        }

        private void btnAddHeader_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AddHeader();
        }

        private void btnAddNavigationLinks_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AddNavigationLinks();
        }

        private void btnAddTableOfContents_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.CreateTableOfContents();
        }

        private void btnAllPageOff_Click(object sender, RibbonControlEventArgs e)
        {
            string layerName = cmbLayers.Text;

            if (layerName.Length > 0)
            {
                Actions.Visio_Document.DisplayLayer(layerName, false);
            }
        }

        private void btnAllPageOn_Click(object sender, RibbonControlEventArgs e)
        {
            string layerName = cmbLayers.Text;

            if (layerName.Length > 0)
            {
                Actions.Visio_Document.DisplayLayer(layerName, true);
            }
        }

        private void btnAutoSizePagesOff_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AutoSizePagesOff();
        }

        private void btnAutoSizePagesOn_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.AutoSizePagesOn();
        }

        private void btnDeletePages_Click_1(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.DeletePages();
        }

        private void btnDisplayPageNames_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.DisplayPageNames();
        }

        private void btnGetDocumentInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.DisplayInfo();
        }

        public static DxThemedWindowHost duplicatePage_Host = null;

        private void btnDuplicatePage_Click(object sender, RibbonControlEventArgs e)
        {

            DxThemedWindowHost.DisplayUserControlInHost(ref duplicatePage_Host,
            "Duplicate Page",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.DuplicatePage(new Presentation.ViewModels.DuplicatePageViewModel()));
        }

        private void btnMovePages_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmMovePages();
            frm.Show();
        }

        private void btnPrintPages_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.PrintPages();
        }

        private void btnRemoveLayers_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.RemoveLayers();
        }

        private void btnRenamePages_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRenamePages();
            frm.Show();
        }

        private void btnSavePages_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.SavePages();
        }

        private void btnSortAllPages_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.SortAllPages();
        }

        private void btnSyncPageNames_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.SyncPageNames();
        }

        private void btnUpdatePageNameShapes_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Document.UpdatePageNameShapes();
        }

        #endregion Visio_Document Events

        #region Visio_Page Events

        private void btnAddDefaultLayers_Page_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.AddDefaultLayers();
        }

        private void btnAddNavLinks_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.AddNavigationLinks(Globals.ThisAddIn.Application.ActivePage);
        }

        private void btnAutoSizePageOff_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.AutoSizePageOff();
        }

        private void btnAutoSizePageOn_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.AutoSizePageOn();
        }

        private void btnGetPageInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.GatherInfo(Globals.ThisAddIn.Application.ActivePage);
        }

        private void btnLockBackground_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.LockLayer("Background");
        }

        private void btnPageOff_Click(object sender, RibbonControlEventArgs e)
        {
            string layerName = cmbLayers.Text;

            if (layerName.Length > 0)
            {
                Actions.Visio_Page.DisplayLayer(Globals.ThisAddIn.Application.ActivePage, layerName, false);
            }
        }

        private void btnPageOn_Click(object sender, RibbonControlEventArgs e)
        {
            string layerName = cmbLayers.Text;

            if (layerName.Length > 0)
            {
                Actions.Visio_Page.DisplayLayer(Globals.ThisAddIn.Application.ActivePage, layerName, true);
            }
        }

        private void btnPrintPage_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.PrintPage();
        }

        private void btnRemoveLayers_Page_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.RemoveLayers();
        }

        private void btnSavePage_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.SavePage(Globals.ThisAddIn.Application.ActivePage);
        }

        private void btnSyncPageNamesPage_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.SyncPageNames();
        }

        private void btnUnlockBackground_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.UnlockLayer("Background");
        }

        private void btnUpdatePageNameShapesPage_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Page.UpdatePageNameShapes(Globals.ThisAddIn.Application.ActivePage);
        }

        #endregion Visio_Page Events

        #region Visio_Shape Events

        private void btn0PtMargins_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.SetMargins("0 pt");
        }

        private void btn1PtMargins_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.SetMargins("1 pt");
        }

        private void btn2PtMargins_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.SetMargins("2 pt");
        }

        private void btnAddColorSupport_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.AddColorSupportToSelection();
        }

        private void btnAddHyperLink_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.AddHyperlinkToPage_FromShapeText();
        }

        private void btnAddIDAndTextSupport_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.Add_IDandTextSupport_ToSelection();
        }

        private void btnAddIDSupport_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.Add_IDSupport_ToSelection();
        }

        private void btnAddIsPageName_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.Add_User_IsPageName();
        }

        private void btnAddTextControl_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.Add_TextControl_ToSelection();
        }

        private void btnGetShapeInfo_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.GatherInfo();
        }

        private void btnMakeLinkableMaster_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.MakeLinkableMaster();
        }

        private void btnMoveToBackgroundLayer_Click(object sender, RibbonControlEventArgs e)
        {
            Actions.Visio_Shape.MoveToBackgroundLayer();
        }

        #endregion Visio_Shape Events

        #region UI Launch Events

        enum ShowWindowMode
        {
            Modeless_Show,
            Modal_ShowDialog
        }

        DxLayoutControl _dxLayoutControl;

        public DxLayoutControl DxLayoutControl
        {
            get
            {
                if (_dxLayoutControl is null)
                {
                    _dxLayoutControl = new DxLayoutControl();
                }

                return _dxLayoutControl;
            }
            set
            {
                _dxLayoutControl = value;
            }
        }

        private DxThemedWindowHost dxLayoutControlHost = null;

        private void btnDxLayoutControl_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref dxLayoutControlHost,
                "DxLayoutControl Test", 
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                DxLayoutControl);
        }

        DxDockLayoutControl _dxDockLayoutControl;

        public DxDockLayoutControl DxDockLayoutControl
        {
            get
            {
                if (_dxDockLayoutControl is null)
                {
                    _dxDockLayoutControl = new DxDockLayoutControl();
                }

                return _dxDockLayoutControl;
            }
            set
            {
                _dxDockLayoutControl = value;
            }
        }

        private DxThemedWindowHost dxDockLayoutControlHost = null;

        private void btnDxDockLayoutControl_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref dxDockLayoutControlHost,
                "DxDockLayoutControl Test", 
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                DxDockLayoutControl);
       }

        DxDockLayoutManagerControl _dxDockLayoutControlManager;

        public DxDockLayoutManagerControl DxDockLayoutManagerControl
        {
            get
            {
                if (_dxDockLayoutControlManager is null)
                {
                    _dxDockLayoutControlManager = new DxDockLayoutManagerControl();
                }

                return _dxDockLayoutControlManager;
            }
            set
            {
                _dxDockLayoutControlManager = value;
            }
        }

        private DxThemedWindowHost dxDockLayoutManagerControlHost = null;

        private void btnDxDockLayoutManagerControl_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref dxDockLayoutManagerControlHost, 
                "DxDocLayoutManagerControl Test", 
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                DxDockLayoutManagerControl);
       }

        private DxThemedWindowHost themedWindowHost = null;

        private void btnThemedWindowHostModal_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref themedWindowHost,
                "ThemedWindowHost (Modal)",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modal);
        }

        private void btnThemedWindowHost_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref themedWindowHost,
                "ThemedWindowHost (ModeLess)",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless);
        }

        private DxDXWindowHost dxWindowHost = null;

        private void btnDxWindowHost_Click(object sender, RibbonControlEventArgs e)
        {
            DxDXWindowHost.DisplayUserControlInHost(ref dxWindowHost,
                "DxWindowHost Test",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxDXWindowHost.ShowWindowMode.Modeless_Show);
        }

        public static WindowHost windowHostLocal = null;

        private void btnWindowHostLocal_Click(object sender, RibbonControlEventArgs e)
        {
            WindowHost.DisplayUserControlInHost(ref windowHostLocal,
                "WindowHost (local) Test",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                WindowHost.ShowWindowMode.Modeless_Show);
        }

        private void btnWindowHostVNC_Click(object sender, RibbonControlEventArgs e)
        {
            ShowEmptyHost(windowHostVNC, "WindowHost (VNC)", ShowWindowMode.Modal_ShowDialog);
        }

        private static void ShowEmptyHost(Window host, string title, ShowWindowMode mode)
        {
            long startTicks = Log.Trace("Enter", Common.PROJECT_NAME);

            if (host is null)
            {
                host = new DxThemedWindowHost();
                host.Height = Common.DEFAULT_WINDOW_SMALL_HEIGHT;
                host.Width = Common.DEFAULT_WINDOW_SMALL_WIDTH;
                host.Title = title;
            }

            if (mode == ShowWindowMode.Modal_ShowDialog)
            {
                long endTicks2 = Log.Trace("Exit", Common.PROJECT_NAME, startTicks);

                host.Title = $"{host.GetType()} loadtime: {Log.GetDuration(startTicks, endTicks2)}";

                host.ShowDialog();
            }
            else
            {
                host.Show();
            }

            long endTicks = Log.Trace("Exit", Common.PROJECT_NAME, startTicks);

            host.Title = $"{host.GetType()} loadtime: {Log.GetDuration(startTicks, endTicks)}";
        }

        #endregion UI Launch Events

        #region WPF UI Events

        #region WPF UI Events Document Related

        public static DxThemedWindowHost documentProperties_ShapeSheetSectionHost = null;

        private void btnDocumentProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref documentProperties_ShapeSheetSectionHost,
                "Document Properties",
                600, 450,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.DocumentShapeSheetSection(
                    new Presentation.ViewModels.DocumentPropertiesViewModel(),
                    new Presentation.Views.DocumentProperties()));
        }



        public static DxThemedWindowHost documentScratch_ShapeSheetSectionHost = null;

        private void btnDocumentScratch_Click(object sender, RibbonControlEventArgs e)
        {
            //DxThemedWindowHost.DisplayUserControlInHost(ref documentScratch_ShapeSheetSectionHost,
            //    "Document Scratch",
            //    600, 450,
            //    //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            //    DxThemedWindowHost.ShowWindowMode.Modeless,
            //    new Presentation.Views.DocumentShapeSheetSection(
            //        new Presentation.ViewModels.DocumentScratchViewModel(),
            //        new Presentation.Views.DocumentScratchRows()));
        }

        public static DxThemedWindowHost documentShapeData_ShapeSheetSectionHost = null;

        private void btnDocumentShapeData_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref documentShapeData_ShapeSheetSectionHost,
                "Document ShapeData",
                600, 450,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.DocumentShapeSheetSection(
                    new Presentation.ViewModels.DocumentPropertiesViewModel(),
                    new Presentation.Views.DocumentProperties()));
        }

        public static DxThemedWindowHost documentUserDefinedCells_ShapeSheetSectionHost = null;

        private void btnDocumentUserDefinedCells_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref documentUserDefinedCells_ShapeSheetSectionHost,
                "Document UserDefinedCells",
                600, 450,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.DocumentShapeSheetSection(
                    new Presentation.ViewModels.DocumentPropertiesViewModel(),
                    new Presentation.Views.DocumentProperties()));
        }

        #endregion

        #region WPF UI Events Page Related






        private void btnPageLayout_Click(object sender, RibbonControlEventArgs e)
        {

        }

        public static DxThemedWindowHost pageProperties_ShapeSheetSectionHost = null;

        private void btnPageProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref pageProperties_ShapeSheetSectionHost,
                "Page Properties",
                600, 450,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.PageShapeSheetSection(
                    new Presentation.ViewModels.PagePropertiesViewModel(),
                    new Presentation.Views.PageProperties()));
        }

        private void btnPageShapeData_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void btnPageThemeProperties_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void btnPageUserDefinedCells_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void btnRulerAndGrid_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void btnPrintProperties_Click(object sender, RibbonControlEventArgs e)
        {

        }

        #endregion

        #region WPF UI Events Shape Related - Section Based

        public static DxThemedWindowHost ss1DEndpoints_ShapeSheetSectionHost = null;

        private void btn1DEndpoints_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ss1DEndpoints_ShapeSheetSectionHost,
            "1-D Endpoints",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.OneDEndPointsViewModel(), 
                new Presentation.Views.OneDEndPoints()));
        }

        public static DxThemedWindowHost ssTextBlockFormat_ShapeSheetSectionHost = null;

        private void btnTextBlockFormat_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssTextBlockFormat_ShapeSheetSectionHost,
            "ShapeSheetSection(TextBlock Format)",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.TextBlockFormatViewModel(), 
                new Presentation.Views.TextBlockFormat()));
        }

        public static DxThemedWindowHost ssTextTransform_ShapeSheetSectionHost = null;

        private void btnTextTransform_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssTextTransform_ShapeSheetSectionHost,
            "ShapeSheetSection(Text Transform)",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.TextTransformViewModel(), 
                new Presentation.Views.TextTransformContent()));
        }

        public static DxThemedWindowHost ssThemeProperties_ShapeSheetSectionHost = null;

        private void btnThemeProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssThemeProperties_ShapeSheetSectionHost,
            "Theme Properties",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.ThemePropertiesViewModel(), 
            new Presentation.Views.ThemeProperties()));
        }

        public static DxThemedWindowHost ssGroupProperties_ShapeSheetSectionHost = null;

        private void btnGroupProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssGroupProperties_ShapeSheetSectionHost,
            "Group Properties",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.GroupPropertiesViewModel(), 
            new Presentation.Views.GroupProperties()));
        }

        public static DxThemedWindowHost ssLayerMembership_ShapeSheetSectionHost = null;

        private void btnLayerMembership_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssLayerMembership_ShapeSheetSectionHost,
            "Layer Membership",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.LayerMembershipViewModel(), 
                new Presentation.Views.LayerMembership()));
        }

        public static DxThemedWindowHost ssUserDefinedCells_ShapeSheetSectionHost = null;

        private void btnUserDefinedCells_Click(object sender, RibbonControlEventArgs e)
        {
            //DxThemedWindowHost.DisplayUserControlInHost(ref ssUserDefinedCells_ShapeSheetSectionHost,
            //"User-Defined Cells",
            //600, 450,
            ////Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            //DxThemedWindowHost.ShowWindowMode.Modeless,
            //new Presentation.Views.UserDefinedCells(new Presentation.ViewModels.UserDefinedCellRowViewModel()));
        }

        public static DxThemedWindowHost ssChangeShapeBehavior_ShapeSheetSectionHost = null;

        private void btnChangeShapeBehavior_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssChangeShapeBehavior_ShapeSheetSectionHost,
            "Change Shape Behavior",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.ChangeShapeBehaviorViewModel(), 
                new Presentation.Views.ChangeShapeBehavior()));
        }

        public static DxThemedWindowHost ssQuickStyle_ShapeSheetSectionHost = null;

        private void btnQuickStyle_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssQuickStyle_ShapeSheetSectionHost,
            "Quick Style",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.QuickStyleViewModel(), 
                new Presentation.Views.QuickStyle()));
        }

        public static DxThemedWindowHost threeDRotationProperties_ShapeSheetSectionHost = null;

        private void btn3DRotationProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref threeDRotationProperties_ShapeSheetSectionHost,
            "3D Rotation Properties",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.ThreeDRotationPropertiesViewModel(), 
                new Presentation.Views.ThreeDRotationProperties()));
        }

        public static DxThemedWindowHost ssBevelProperties_ShapeSheetSectionHost = null;

        private void btnBevelProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssBevelProperties_ShapeSheetSectionHost,
            "Bevel Properties",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.BevelPropertiesViewModel(), 
                new Presentation.Views.BevelProperties()));
        }

        public static DxThemedWindowHost ssAdditionalEffectProperties_ShapeSheetSectionHost = null;

        private void btnAdditionalEffectProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(
                ref ssAdditionalEffectProperties_ShapeSheetSectionHost,
                "Additional Effect Properties",
                600, 450,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.AdditionalEffectPropertiesViewModel(), 
                    new Presentation.Views.AdditionalEffectProperties()));
        }

        public static DxThemedWindowHost ssGradientProperties_ShapeSheetSectionHost = null;

        private void btnGradientProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssGradientProperties_ShapeSheetSectionHost,
            "Gradient Properties",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.GradientPropertiesViewModel(), 
            new Presentation.Views.GradientProperties()));
        }

        public static DxThemedWindowHost ssShapeLayout_ShapeSheetSectionHost = null;

        private void btnShapeLayout_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssShapeLayout_ShapeSheetSectionHost,
            "Shape Layout",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.ShapeLayoutViewModel(), 
                new Presentation.Views.ShapeLayout()));
        }

        public static DxThemedWindowHost ssGlueInfo_ShapeSheetSectionHost = null;

        private void btnGlueInfo_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssGlueInfo_ShapeSheetSectionHost,
            "GLue Info",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.GlueInfoViewModel(), 
                new Presentation.Views.GlueInfo()));
        }

        public static DxThemedWindowHost ssImageProperties_ShapeSheetSectionHost = null;

        private void btnImageProperties_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssImageProperties_ShapeSheetSectionHost,
            "Image Properties",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(new Presentation.ViewModels.ImagePropertiesViewModel(), new Presentation.Views.ImageProperties()));
        }

        public static DxThemedWindowHost ssEvents_ShapeSheetSectionHost = null;

        private void btnEvents_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssEvents_ShapeSheetSectionHost,
            "Events",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.EventsViewModel(), 
                new Presentation.Views.Events()));
        }

        //public static DxThemedWindowHost textBlockFormatHost = null;
        //private void btnTextBlockFormat_Click(object sender, RibbonControlEventArgs e)
        //{
        //    DxThemedWindowHost.DisplayUserControlInDxThemedWindowHost(ref textBlockFormatHost,
        //    "Text Block Format",
        //    600, 450,
        //    //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
        //    DxThemedWindowHost.ShowWindowMode.Modeless_Show,
        //    new Presentation.Views.TextBlockFormat(new Presentation.ViewModels.TextBlockFormatViewModel()));
        //}

        public static DxThemedWindowHost ssFillFormat_ShapeSheetSectionHost = null;

        private void btnFillFormat_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssFillFormat_ShapeSheetSectionHost,
            "Fill Format",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.FillFormatViewModel(), 
                new Presentation.Views.FillFormat()));
        }

        public static DxThemedWindowHost ssLineFormat_ShapeSheetSectionHost = null;

        private void btnLineFormat_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssLineFormat_ShapeSheetSectionHost,
            "Line Format",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.LineFormatViewModel(), 
                new Presentation.Views.LineFormat()));
        }

        public static DxThemedWindowHost ssMiscellaneous_ShapeSheetSectionHost = null;

        private void btnMiscelleaneous_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssMiscellaneous_ShapeSheetSectionHost,
            "Miscellaneous",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.MiscellaneousViewModel(),
                new Presentation.Views.Miscellaneous()));
        }

        public static DxThemedWindowHost ssProtection_ShapeSheetSectionHost = null;

        private void btnProtection_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssProtection_ShapeSheetSectionHost,
            "Protection",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.ProtectionViewModel(), 
                new Presentation.Views.Protection()));
        }

        public static DxThemedWindowHost ssShapeTransform_ShapeSheetSectionHost = null;
        private void btnShapeTransform_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssShapeTransform_ShapeSheetSectionHost,
                "Shape Transform",
                250, 450,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.ShapeTransformViewModel(), 
                    new Presentation.Views.ShapeTransform()));
        }

        #endregion

        #region UI Events Shape - Row Based

        #region Actions

        public static DxThemedWindowHost _pageActionsHost = null;

        private void btnActionsPage_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _pageActionsHost,
                "Actions (Page)",
                600, 800,
                //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    // NOTE(crhodes)
                    // 
                    //new Presentation.ViewModels.ActionsViewModel(), 
                    new Presentation.ViewModels.RowsViewModel<Domain.ActionRow, Presentation.ModelWrappers.ActionRowWrapper>(
                        "Update Actions",
                        Actions.Visio_Shape.Get_ActionsRows,
                        ShapeType.Page),
                    new Presentation.Views.Actions()));
        }

        public static DxThemedWindowHost _shapeActionsHost = null;

        private void btnActionsShape_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _shapeActionsHost,
                "Actions (Shape)",
                600, 800,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.ActionRow, Presentation.ModelWrappers.ActionRowWrapper>(
                        "Update Actions",
                        Actions.Visio_Shape.Get_ActionsRows,
                        ShapeType.Shape),
                    new Presentation.Views.Actions()));
        }

        #endregion

        #region ActionTags

        public static DxThemedWindowHost _pageActionTagsHost = null;

        private void btnActionTagsPage_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _pageActionTagsHost,
                "ActionsTags (Page)",
                600, 750,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.ActionTagRow, Presentation.ModelWrappers.ActionTagRowWrapper>(
                        "Hello Natalie", 
                        Actions.Visio_Shape.Get_ActionTagRows,
                        ShapeType.Page),
                    new Presentation.Views.ActionTags()));
        }

        public static DxThemedWindowHost _shapeActionTagsHost = null;

        private void btnActionTagsShape_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _shapeActionTagsHost,
                "ActionsTags (Shape)",
                600, 750,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.ActionTagRow, Presentation.ModelWrappers.ActionTagRowWrapper>(
                        "Hello Natalie", 
                        Actions.Visio_Shape.Get_ActionTagRows,
                        ShapeType.Shape),
                    new Presentation.Views.ActionTags()));
        }

        #endregion

        #region Hyperlinks

        public static DxThemedWindowHost _documentHyperLinksHost = null;

        private void btnDocumentHyperlinks_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _documentHyperLinksHost,
                "Hyperlinks (Document)",
                800, 700,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.HyperlinkRow, Presentation.ModelWrappers.HyperlinkRowWrapper>(
                        "Update Hyperlinks",
                        Actions.Visio_Shape.Get_HyperlinksRows,
                        ShapeType.Document),
                    new Presentation.Views.Hyperlinks()));
        }

        public static DxThemedWindowHost _pageHyperLinksHost = null;

        private void btnPageHyperlinks_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _pageHyperLinksHost,
                "Hyperlinks (Page)",
                800, 700,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                    new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.HyperlinkRow, Presentation.ModelWrappers.HyperlinkRowWrapper>(
                        "Update Hyperlinks",
                        Actions.Visio_Shape.Get_HyperlinksRows,
                        ShapeType.Page),
                    new Presentation.Views.Hyperlinks()));
        }

        public static DxThemedWindowHost _shapeHyperlinksHost = null;

        private void btnShapeHyperlinks_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _shapeHyperlinksHost,
                "Hyperlinks (Shape)",
                800, 700,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                    new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.HyperlinkRow, Presentation.ModelWrappers.HyperlinkRowWrapper>(
                        "Update Hyperlinks",
                        Actions.Visio_Shape.Get_HyperlinksRows,
                        ShapeType.Shape),
                    new Presentation.Views.Hyperlinks()));
        }

        #endregion

        #region Layers

        public static DxThemedWindowHost _pageLayersHost = null;

        private void btnLayers_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _pageLayersHost,
                "Layers (Page)",
                800, 700,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.LayerRow, Presentation.ModelWrappers.LayerRowWrapper>(
                        "Update Layers",
                        Actions.Visio_Shape.Get_LayerRows,
                        ShapeType.Page),
                    new Presentation.Views.Layers()));
        }

        #endregion

        #region Hyperlinks

        public static DxThemedWindowHost _documentScratchHost = null;

        private void btnDocumentScratch_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _documentScratchHost,
                "Hyperlinks (Document)",
                800, 700,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.HyperlinkRow, Presentation.ModelWrappers.HyperlinkRowWrapper>(
                        "Update Hyperlinks",
                        Actions.Visio_Shape.Get_HyperlinksRows,
                        ShapeType.Document),
                    new Presentation.Views.Hyperlinks()));
        }

        public static DxThemedWindowHost _pageScratchHost = null;

        private void btnPageScratch_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _pageScratchHost,
                "Hyperlinks (Page)",
                800, 700,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                    new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.HyperlinkRow, Presentation.ModelWrappers.HyperlinkRowWrapper>(
                        "Update Hyperlinks",
                        Actions.Visio_Shape.Get_HyperlinksRows,
                        ShapeType.Page),
                    new Presentation.Views.Hyperlinks()));
        }

        public static DxThemedWindowHost _shapeScratchHost = null;

        private void btnShapeScratch_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref _shapeScratchHost,
                "Hyperlinks (Shape)",
                800, 700,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                    new Presentation.Views.ShapeSheetSection(
                    new Presentation.ViewModels.RowsViewModel<Domain.HyperlinkRow, Presentation.ModelWrappers.HyperlinkRowWrapper>(
                        "Update Hyperlinks",
                        Actions.Visio_Shape.Get_HyperlinksRows,
                        ShapeType.Shape),
                    new Presentation.Views.Hyperlinks()));
        }

        #endregion

        private void btnCharacter_Click(object sender, RibbonControlEventArgs e)
        {

        }

        public static DxThemedWindowHost ssConnectionPoints_ShapeSheetSectionHost = null;

        private void btnConnectionPoints_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssConnectionPoints_ShapeSheetSectionHost,
            "Connection Points",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.ConnectionPointsViewModel(), 
                new Presentation.Views.ConnectionPoints()));
        }

        public static DxThemedWindowHost ssControls_ShapeSheetSectionHost = null;

        private void btnControls_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssControls_ShapeSheetSectionHost,
            "Controls",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(
                new Presentation.ViewModels.ControlsViewModel(), 
                new Presentation.Views.Controls()));
        }

        private void btnGeometry_Click(object sender, RibbonControlEventArgs e)
        {

        }

        public static DxThemedWindowHost ssGradientStops_ShapeSheetSectionHost = null;

        private void btnGradientStops_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void btnParagraph_Click(object sender, RibbonControlEventArgs e)
        {

        }



        public static DxThemedWindowHost ssShapeData_ShapeSheetSectionHost = null;

        private void btnShapeData_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref ssShapeData_ShapeSheetSectionHost,
            "Shape Data",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.ShapeSheetSection(new Presentation.ViewModels.ShapeDataViewModel(), new Presentation.Views.ShapeData()));
        }

        private void btnTabs_Click(object sender, RibbonControlEventArgs e)
        {

        }

        public static DxThemedWindowHost ssShapeUserDefinedCells_ShapeSheetSectionHost = null;

        private void btnShapeUserDefinedCells_Click(object sender, RibbonControlEventArgs e)
        {

        }

        #endregion

        #region WPF Events - Custom

        public static DxThemedWindowHost editControlRowsHost = null;
        private void btnEditControlRows_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref editControlRowsHost,
                "Edit Control Rows",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.EditControlRows(new Presentation.ViewModels.EditControlRowsViewModel()));
        }

        public static DxThemedWindowHost editParagraphHost = null;

        //public static VNC.Core.Xaml.Presentation.WindowHost editControlPointsHost = null;
        private void btnEditParagraph_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref editParagraphHost,
                "Edit Paragraph",
                300, 600,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.EditParagraph(new Presentation.ViewModels.EditParagraphViewModel()));
        }

        private Presentation.Views.EditControlPoints editControlPointsUC = null;

        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost editTextHost = null;
        // static VNC.Core.Xaml.Presentation.WindowHost editTextHost = null;
        private Presentation.Views.EditText editTextUC = null;

        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost editControlPointsHost = null;

        private void btnEditControlPoints_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref editControlPointsHost,
                "Edit Shape Control Points Text",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.EditControlPoints());
        }

        private void btnEditText_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref editTextHost,
                "Edit Text",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                new Presentation.Views.EditText(new Presentation.ViewModels.EditTextViewModel()));
        }

        public static WindowHost cylonHost = null;
        public static VNC.Core.Xaml.Presentation.WindowHost cylonHost2 = null;

        private void btnLaunchCylon_Click(object sender, RibbonControlEventArgs e)
        {
            WindowHost.DisplayUserControlInHost(ref cylonHost,
                "I am a Cylon loaded by name",
                Common.DEFAULT_WINDOW_SMALL_WIDTH, Common.DEFAULT_WINDOW_SMALL_HEIGHT,
                WindowHost.ShowWindowMode.Modeless_Show,
                "VNC.WPF.Presentation.Views.CylonEyeBall, VNC.WPF.Presentation");
        }

        private void btnLaunchCylon2_Click(object sender, RibbonControlEventArgs e)
        {
            WindowHost.DisplayUserControlInHost(ref cylonHost,
                "I am a Cylon loaded by type",
                Common.DEFAULT_WINDOW_SMALL_WIDTH, Common.DEFAULT_WINDOW_SMALL_HEIGHT,
                WindowHost.ShowWindowMode.Modeless_Show,
                new CylonEyeBall());
        }

        Presentation.Views.PrismRegionTest _prismRegionTest;

        public Presentation.Views.PrismRegionTest PrismRegionTest
        {
            get
            {
                if (_prismRegionTest is null)
                {
                    _prismRegionTest = new Presentation.Views.PrismRegionTest();
                }

                return _prismRegionTest;
            }
            set
            {
                _prismRegionTest = value;
            }
        }

        private DxThemedWindowHost prismRegionTestHost = null;

        private void btnPrismRegionTest_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref prismRegionTestHost,
                "Prism Region Test 2", 600, 400,
                DxThemedWindowHost.ShowWindowMode.Modeless,
                PrismRegionTest);
        }

        #endregion

        #endregion WPF UI Events

        #region Debug Events

        private void btnDebugWindow_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayDebugWindow();
        }

        private void btnDeveloperMode_Click(object sender, RibbonControlEventArgs e)
        {
            ToggleDeveloperMode();
        }

        private void btnWatchWindow_Click(object sender, RibbonControlEventArgs e)
        {
            DisplayWatchWindow();
        }

        private void chkDisplayChattyEvents_Click(object sender, RibbonControlEventArgs e)
        {
            Common.DisplayChattyEvents = chkDisplayChattyEvents.Checked;
        }

        private void chkDisplayEvents_Click(object sender, RibbonControlEventArgs e)
        {
            Common.DisplayEvents = chkDisplayEvents.Checked;
        }

        private void chkEnableAppEvents_Click(object sender, RibbonControlEventArgs e)
        {
            Common.HasAppEvents = chkEnableAppEvents.Checked;

            if (Common.HasAppEvents)
            {
                if (Common.AppEvents == null)
                {
                    Common.AppEvents = new Events.VisioAppEvents();
                    Common.AppEvents.VisioApplication = Globals.ThisAddIn.Application;
                }
            }
            else
            {
                Common.AppEvents = null;
            }
        }

        #endregion Debug Events

        #region SMARTS Events

        private void btnHilight_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmEditVisio();
            frm.Show();
        }

        private void btnNavigateDown_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("TODO: Navigate Down");
        }

        private void btnNavigateUp_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("TODO: Navigate Up");
        }

        private void btnRelatedIntfrastructure_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRelateShape();
            frm.Show();
        }

        private void btnRelatedProcess_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRelateShape();
            frm.Show();
        }

        private void btnRelatedSystem_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRelateShape();
            frm.Show();
        }

        private void btnRetrive_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRetrieveShape();
            frm.Show();
        }

        private void btnValidate_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new User_Interface.Forms.frmRetrieveShape();
            frm.Show();
        }

        private void btnWebPage_Click(object sender, RibbonControlEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("TODO: Navigate to Web Page");
        }

        #endregion SMARTS Events

        #endregion Event Handlers

        #region Main Function Routines

        private void DisplayAddInInfo()
        {
            long startTicks = Log.Trace("Enter", Common.PROJECT_NAME);

            VNC.AddinHelper.AddInInfo.DisplayInfo();
            
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        private void DisplayDebugWindow()
        {
            long startTicks = Log.Trace("Enter", Common.PROJECT_NAME);

            VNC.AddinHelper.Common.DebugWindow.Visible = ! VNC.AddinHelper.Common.DebugWindow.Visible;

            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        private void DisplayWatchWindow()
        {
            long startTicks = Log.Trace("Enter", Common.PROJECT_NAME);

            VNC.AddinHelper.Common.WatchWindow.Visible = ! VNC.AddinHelper.Common.WatchWindow.Visible;

            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        private void ToggleDeveloperMode()
        {
            long startTicks = Log.Trace("Enter", Common.PROJECT_NAME);

            VNC.AddinHelper.Common.DeveloperMode = ! VNC.AddinHelper.Common.DeveloperMode;
            Globals.Ribbons.Ribbon.grpDebug.Visible = VNC.AddinHelper.Common.DeveloperMode;

            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        #endregion Main Function Routines

        #region MVVM Examples

        public static DxThemedWindowHost vncMVVM_V1_Host = null;

        private void btnVNC_MVVM_V1_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1_Host,
            "MVVM View First (View is passed new ViewModel)",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.Views.Cat(new Presentation.ViewModels.CatViewModel()));
        }

        public static DxThemedWindowHost vncMVVM_VM1_Host = null;

        private void btnVNC_MVVM_VM1_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_VM1_Host,
            "MVVM ViewModel First (ViewModel is passed new View)",
            600, 450,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            DxThemedWindowHost.ShowWindowMode.Modeless,
            new Presentation.ViewModels.CatViewModel(new Presentation.Views.Cat()));
        }


        #endregion

    }
}