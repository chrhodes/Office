using Prism.Commands;

using VNC;
using VNC.Core.Mvvm;

using SupportTools_Excel.Presentation.ModelWrappers;
using SupportTools_Excel.Presentation.Views;
using SupportTools_Excel.Infrastructure.Presentation.ViewModels;
using SupportTools_Excel.AzureDevOpsExplorer.Domain;
using SupportTools_Excel.AzureDevOpsExplorer.Presentation.ModelWrappers;
using SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views;

namespace SupportTools_Excel.AzureDevOpsExplorer.Presentation.ViewModels
{
    public class WorkItemsViewModel : ViewModelBase, IAZDOWorkItemsViewModel
    {
        #region Constructors and Load

        // View First

        public WorkItemsViewModel()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeViewModel();

            // TODO(crhodes)
            // Decide if we want defaults
            //AZDOWorkItems = new AZDOWorkItemsWrapper(new Domain.AZDOWorkItems());

            // InitializeRows();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        // ViewModel First
        // Calling base(view) wires this ViewModel into the View

        public WorkItemsViewModel(WorkItems view) : base(view)
        {
            Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeViewModel();

            // InitializeRows();

            //View = view;

            Log.Trace($"Exit", Common.PROJECT_NAME);
        }

        private void InitializeViewModel()
        {
            // TODO(crhodes)
            // Initialize any controls and/or properties that need to be

            WorkItemActionRequest = new WorkItemActionRequestWrapper(
                new WorkItemActionRequest());

            GetWorkItemInfoCommand = new DelegateCommand(OnGetWorkItemInfoExecute, OnGetWorkItemInfoCanExecute);
            AddPivotSummaryCommand = new DelegateCommand(OnAddPivotSummaryExecute, OnAddPivotSummaryCanExecute);

            WorkItemID_DoubleClickCommand = new DelegateCommand(OnWorkItemID_DoubleClick, OnWorkItemID_DoubleClickCanExecute);

            //InitializeRows();
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        private WorkItemActionRequestWrapper _workItemActionRequest;
        public WorkItemActionRequestWrapper WorkItemActionRequest
        {
            get { return _workItemActionRequest; }
            set
            {
                if (_workItemActionRequest == value)
                    return;
                _workItemActionRequest = value;
                OnPropertyChanged();
            }
        }

        string _message = "Click Button to do something";
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        #region WorkItemID_DoubleClick Command

        public DelegateCommand WorkItemID_DoubleClickCommand { get; set; }

        public void OnWorkItemID_DoubleClick()
        {
            // Need to pass wrapper so PropertyChanged gets handled.

            //Common.EventAggregator.GetEvent<WorkItemIDDoubleClickEvent>().Publish(WorkItemActionRequest.Model);
            Common.EventAggregator.GetEvent<WorkItemIDDoubleClickEvent>().Publish(WorkItemActionRequest);
        }

        public bool OnWorkItemID_DoubleClickCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region GetWorkItemInfo Command

        public DelegateCommand GetWorkItemInfoCommand { get; set; }
        public string GetWorkItemInfoContent { get; set; } = "GetWorkItemInfo";
        public string GetWorkItemInfoToolTip { get; set; } = "GetWorkItemInfo ToolTip";
        // Can get fancy and use Resources
        //public string GetWorkItemInfoContent { get; set; } = "ViewName_GetWorkItemInfoContent";
        //public string GetWorkItemInfoToolTip { get; set; } = "ViewName_GetWorkItemInfoContentToolTip";

        // Put these in Resource File

        //    <system:String x:Key="ViewName_GetWorkItemInfoContent">GetWorkItemInfo</system:String>
        //    <system:String x:Key="ViewName_GetWorkItemInfoContentToolTip">GetWorkItemInfo ToolTip</system:String>  

        public void OnGetWorkItemInfoExecute()
        {
            Common.EventAggregator.GetEvent<GetWorkItemInfoEvent>().Publish(WorkItemActionRequest.Model);
        }

        public bool OnGetWorkItemInfoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region AddPivotSummary Command

        public DelegateCommand AddPivotSummaryCommand { get; set; }
        public string AddPivotSummaryContent { get; set; } = "AddPivotSummary";
        public string AddPivotSummaryToolTip { get; set; } = "AddPivotSummary ToolTip";
        // Can get fancy and use Resources
        //public string AddPivotSummaryContent { get; set; } = "ViewName_AddPivotSummaryContent";
        //public string AddPivotSummaryToolTip { get; set; } = "ViewName_AddPivotSummaryContentToolTip";

        // Put these in Resource File

        //    <system:String x:Key="ViewName_AddPivotSummaryContent">AddPivotSummary</system:String>
        //    <system:String x:Key="ViewName_AddPivotSummaryContentToolTip">AddPivotSummary ToolTip</system:String>  

        public void OnAddPivotSummaryExecute()
        {
            //AZDOWorkItemRequest request = new AZDOWorkItemRequest()
            //{ WorkItemSections = cbeWorkItemSections, WorkItemID = teWorkItemID };

            Common.EventAggregator.GetEvent<AddPivotSummaryEvent>().Publish(WorkItemActionRequest.Model);
        }

        public bool OnAddPivotSummaryCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #endregion Commands

    }
}
