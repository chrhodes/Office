using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    /// <summary>
    /// Interaction logic for SelectedShapesInfo.xaml
    /// </summary>
    public partial class ShapeSheetSection : UserControl
    {
        private readonly ShapeSheetSectionBase _viewModel;

        public ShapeSheetSection(ShapeSheetSectionBase viewModel, ContentControl ssUserControl)
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            ssSectionUserControl.Content = ssUserControl;
            Log.Trace("Exit", Common.PROJECT_NAME);
        }
    }
}
