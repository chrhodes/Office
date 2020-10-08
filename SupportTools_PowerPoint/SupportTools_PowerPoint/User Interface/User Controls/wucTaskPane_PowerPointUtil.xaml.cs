using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Microsoft.Office.Interop.PowerPoint;

using System.Text.RegularExpressions;

namespace SupportTools_PowerPoint.User_Interface.User_Controls
{
    /// <summary>
    /// Interaction logic for wucTaskPane_PowerPointUtil.xaml
    /// </summary>
    public partial class wucTaskPane_PowerPointUtil : UserControl
    {
        #region Fields and Properties


        #endregion

        #region Constructors and Load

        public wucTaskPane_PowerPointUtil()
        {
            InitializeComponent();
            LoadControlContents();
        }

        private void LoadControlContents()
        {
            try
            {
                //wucSQLInstance_Picker1.PopulateControlFromFile(Common.cCONFIG_FILE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //wucSQLInstance_Picker1.ControlChanged += WucSQLInstance_Picker1_ControlChanged;
            //wucTFSProvider_Picker.ControlChanged += tfsProvider_Picker1_ControlChanged;
        }

        private void WucSQLInstance_Picker1_ControlChanged()
        {
            VNC.AddinHelper.Common.WriteToDebugWindow("wucSQLInstance_Picker1.ControlChanged");
        }

        #endregion

        #region Event Handlers
        private void btnListSlideMasters_Click(object sender, RoutedEventArgs e)
        {
            ListSlideMasters();
        }
        private void btnLinkShape_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnListSlides_Click(object sender, RoutedEventArgs e)
        {
            ListSlides();
        }
        private void btnListShapes_Click(object sender, RoutedEventArgs e)
        {
            ListShapes();
        }
        private void btnRewriteShape_Click(object sender, RoutedEventArgs e)
        {

        }




        #endregion

        #region Main Function Routines
        private void ListSlideMasters()
        {
            Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
            DocumentWindow docWindow = Globals.ThisAddIn.Application.ActiveWindow;
            Slide activeSlide = docWindow.View.Slide;

            Pane pane = docWindow.ActivePane;

            foreach (Design design in presentation.Designs)
            {
                DisplayDesignInfo(design);
            }
        }

        private void ListSlides()
        {
            Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
            DocumentWindow docWindow = Globals.ThisAddIn.Application.ActiveWindow;
            Slide activeSlide = docWindow.View.Slide;

            Pane pane = docWindow.ActivePane;

            foreach (Slide slide in presentation.Slides)
            {
                DisplaySlideInfo(slide);
            }
        }

        private void ListShapes()
        {
            Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
            DocumentWindow docWindow = Globals.ThisAddIn.Application.ActiveWindow;
            Slide activeSlide = docWindow.View.Slide;

            Pane pane = docWindow.ActivePane;

            foreach (Shape shape in activeSlide.Shapes)
            {
                DisplayShapeInfo(shape);
            }
        }

        #endregion

        #region Utility Routines

        void DisplayDesignInfo(Design design)
        {
            Common.WriteToWatchWindow(string.Format("Name: {0,6}  SlideMaster.Name: {1, 20}  SlideMaster.Theme: {2, 20}",
                design.Name, design.SlideMaster.Name, design.SlideMaster.Theme.ToString()));

            if (design.HasTitleMaster == Microsoft.Office.Core.MsoTriState.msoTrue)
            {
                Common.WriteToWatchWindow(string.Format("TitleMaster Name: {0}  Theme: {1}",
                    design.TitleMaster.Name, design.TitleMaster.Theme.ToString()));
            }

            foreach (CustomLayout layout in design.SlideMaster.CustomLayouts)
            {
                Common.WriteToWatchWindow(string.Format("  Layout Name: {0}", 
                    layout.Name));
            }
        }

        private void DisplayShapeInfo(Shape shape)
        {
            string textFrame = "";
            string textFrame2 = "";
            string Id = "";
            string name = "";
            string dashStyle = "";
            string BackColor = "";
            string foreColor = "";

            try
            {
                Id = shape.Id.ToString();
            }
            catch (Exception ex)
            {
                Id = "<No Id>";
            }

            try
            {
                name = shape.Name;
            }
            catch (Exception ex)
            {
                name = "<No Name>";
            }

            try
            {
                dashStyle = shape.Line.DashStyle.ToString();
            }
            catch (Exception ex)
            {
                dashStyle = "<No DashStyle>";
            }

            try
            {
                BackColor = shape.Fill.BackColor.RGB.ToString();
            }
            catch (Exception ex)
            {
                BackColor = "<No BackColor>";
            }

            try
            {
                foreColor = shape.Fill.ForeColor.RGB.ToString();
            }
            catch (Exception ex)
            {
                foreColor = "<No ForeColor>";
            }

            string textFrameHyperlinkAddress = "";
            string textFrameHyperlinkSubAddress = "";

            try
            {
                if (shape.TextFrame.HasText == Microsoft.Office.Core.MsoTriState.msoTrue)
                {
                    //textFrame = shape.TextFrame.TextRange.Text;
                    TextRange txtRange = shape.TextFrame.TextRange;

                    textFrame = txtRange.Text;

                    Hyperlink txtFrameHyperlink = txtRange.ActionSettings[PpMouseActivation.ppMouseClick].Hyperlink;

                    textFrameHyperlinkAddress = txtFrameHyperlink.Address != null ? txtFrameHyperlink.Address.ToString() : "null";
                    textFrameHyperlinkSubAddress = txtFrameHyperlink.SubAddress != null ? txtFrameHyperlink.SubAddress.ToString() : "null";

                }
                else
                {
                    textFrame = "";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToDebugWindow("ex shape.TextFrame.HasText");
            }

            string textFrame2HyperlinkAddress;
            string textFrame2HyperlinkSubAddress;

            try
            {
                if (shape.TextFrame2.HasText == Microsoft.Office.Core.MsoTriState.msoTrue)
                {
                    Microsoft.Office.Core.TextRange2 txtRange2 = shape.TextFrame2.TextRange;

                    textFrame2 = txtRange2.Text;

                    //Hyperlink txtFrame2Hyperlink = txtRange2.ActionSettings[PpMouseActivation.ppMouseClick].Hyperlink;

                    //textFrame2HyperlinkAddress = txtFrame2Hyperlink.Address != null ? txtFrame2Hyperlink.Address.ToString() : "null";
                    //textFrame2HyperlinkSubAddress = txtFrame2Hyperlink.SubAddress != null ? txtFrame2Hyperlink.SubAddress.ToString() : "null";
                }
                else
                {
                    textFrame2 = "";
                }
            }
            catch (Exception ex)
            {
                Common.WriteToDebugWindow("ex shape.TextFrame2.HasText");
            }

            ActionSetting mouseClick = shape.ActionSettings[PpMouseActivation.ppMouseClick];
            ActionSetting mouseOver = shape.ActionSettings[PpMouseActivation.ppMouseOver];

            Hyperlink hyperlink = mouseClick.Hyperlink;


            string hyperLinkAddress = hyperlink.Address != null ? hyperlink.Address.ToString() : "null";
            string hyperLinkSubAddress = hyperlink.SubAddress != null ? hyperlink.SubAddress.ToString() : "null";

            string hyperLinkType = hyperlink.Type.ToString();

            Common.WriteToWatchWindow(string.Format("Id:{0,-3}  Name:{1,-20}  ForeColor:{2,-10}  BackColor:{3,-10}   DashStyle:{4,-10}   TextFrame:{5,-20}   TextFrame2:{6,-20}",
                Id, name, foreColor, BackColor, dashStyle, textFrame, textFrame2));

            Common.WriteToWatchWindow(string.Format("Id:{0,-3}  textFrameHyperlinkAddress:{1,-20}  textFrameHyperlinkSubAddress:{2,-10}",
                Id, textFrameHyperlinkAddress, textFrameHyperlinkSubAddress));

            Common.WriteToWatchWindow(string.Format("Id:{0,-3}  hyperLinkAddress:{1,-20}  hyperLinkSubAddress:{2,-10}  hyperLinkType:{3,-10}",
                Id, hyperLinkAddress, hyperLinkSubAddress, hyperLinkType));
        }
        void DisplaySlideInfo(Slide slide)
        {
            PpSlideLayout ppSlideLayout = slide.Layout;
            Master master = slide.Master;
            var customLayout = slide.CustomLayout;

            Common.WriteToWatchWindow(string.Format("ID: {0, -3}  Name: >{1, 20}<  Master Name: >{2, 20}<  ppSlideLayout: >{3}<  CustomLayout: >{4}<",
                slide.SlideID, slide.Name, master.Name, ppSlideLayout.ToString(), customLayout.Name));
        }

        #endregion

        #region Private Methods

        private bool ValidUISelections()
        {
            //if (cbeTeamProjectCollections.SelectedText.Length > 0)
            //{
            return true;
            //}
            //else
            //{
            //    MessageBox.Show("Must Select Team Project Collection first", "UI Selection Incomplete");
            //    return false;
            //}
        }

        #endregion
    }
}
