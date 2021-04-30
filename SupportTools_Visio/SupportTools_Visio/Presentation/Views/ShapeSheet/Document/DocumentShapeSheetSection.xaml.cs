using System.Windows.Controls;
using SupportTools_Visio.Presentation.ViewModels;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class DocumentShapeSheetSection : UserControl
    {
        private readonly DocumentShapeSheetSectionBase _viewModel;

        public DocumentShapeSheetSection(DocumentShapeSheetSectionBase viewModel, ContentControl ssUserControl)
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
