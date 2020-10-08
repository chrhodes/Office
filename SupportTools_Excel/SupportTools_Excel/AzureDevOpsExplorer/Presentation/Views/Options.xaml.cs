using System.Windows.Controls;

using SupportTools_Excel.Domain;
using SupportTools_Excel.Infrastructure.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;
using SupportTools_Excel.AzureDevOpsExplorer.Domain;

namespace SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views
{
    public partial class Options : UserControl, IView
    {
        #region Constructors and Load

        // ViewModel First.  ViewModel creates View 
        // and sets DataContext by setting ViewModel property

        public Options()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();

            // If View First with ViewModel in Xaml
            // Expose ViewModel
            // ViewModel = (ICatViewModel)DataContext;

            // Can create directly
            ViewModel = (IAZDOOptionsViewModel)DataContext;
            // If ViewModel needs access to view (or view's ViewModel)
            // wire them up.
            ViewModel.View = this;

            InitializeView();

            Log.Trace($"Exit", Common.LOG_APPNAME, startTicks);
        }

        // View First.  View is passed ViewModel through Injection
        // or can declare ViewModel in Xaml

        public Options(IAZDOOptionsViewModel viewModel)
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeComponent();

            ViewModel = viewModel;
            ViewModel.View = this;

            InitializeView();

            Log.Trace($"Exit", Common.LOG_APPNAME, startTicks);
        }

        private void InitializeView()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);
            // TODO(crhodes)
            // Perform any initialization or configuration of View

            //lgMain.IsCollapsed = true;

            debugOptions.IsCollapsed = true;
            dateRange.IsCollapsed = true;
            workItemOptions.IsCollapsed = true;
            loopingDelays.IsCollapsed = true;
            miscOptions.IsCollapsed = true;
            excelOutputOptions.IsCollapsed = true;

            Log.Trace($"Exit", Common.LOG_APPNAME, startTicks);
        }

        #endregion

        #region Properties

        private IViewModel _viewModel;

        public IViewModel ViewModel
        {
            get { return _viewModel; }

            set
            {
                _viewModel = value;
                DataContext = _viewModel;
            }
        }

        #endregion

        public Options_AZDO_TFS GetOptions()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            Options_AZDO_TFS options = new Options_AZDO_TFS();

            // TODO(crhodes)
            // Put some error handling, validation here.

            //options.GoBackDays = int.Parse(teGoBackDays.Text);
            //options.StartDate = deStartDate.DateTime;
            //options.EndDate = deEndDate.DateTime;
            //options.GetLastActivityDates = (bool)ceGetLastActivityDates.IsChecked;
            //options.SkipIfNoActivity = (bool)ceSkipIfNoActivity.IsChecked;

            if ((bool)ceEnableDelays.IsChecked)
            {
                //options.LoopDelaySeconds = int.Parse(teLoopDelay.Text);
                //options.ItemDelaySeconds = Single.Parse(teItemDelay.Text);
            }

            //options.StartingRow = (int)spnStartingRow.Value;
            //options.StartingColumn = (int)spnStartingColumn.Value;
            //options.OrientOutputVertically = (bool)ceOrientOutputVertically.IsChecked;

            //char[] splitChars = { ';' };
            //options.TeamProjects = cbeTeamProjects.Text.Split(splitChars, StringSplitOptions.None).ToList();
            //options.TeamProjects.Sort();

            //options.WorkItemTypes = cbeWorkItemType.Text.Split(splitChars, StringSplitOptions.None).ToList();
            //options.WorkItemTypes.Sort();

            //options.ShowIndividualItems = (bool)ceShowIndividualItems.IsChecked;
            //options.LoopUpdateInterval = int.Parse(teLoopUpdateInterval.Text);

            //options.RecursionLevel = (int)spnRecursionLevel.Value;

            //options.ShowWorkItemFieldData = (bool)ceShowWorkItemFields.IsChecked;

            //options.ExportXMLTemplate = (bool)ceExportXMLTemplate.IsChecked;
            //options.XMLTemplateFilePath = teXMLTemplateFilePath.Text;
            //options.IncludeGlobalLists = (bool)ceIncludeGlobalLists.IsChecked;

            Log.Trace($"Exit", Common.LOG_APPNAME, startTicks);

            return options;
        }
    }
}
