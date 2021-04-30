using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class PageShapeSheetSection : UserControl
    {
        private readonly PageShapeSheetSectionBase _viewModel;

        public PageShapeSheetSection(PageShapeSheetSectionBase viewModel, ContentControl ssUserControl)
        {
            Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            ssSectionUserControl.Content = ssUserControl;

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME);
        }
    }
}
