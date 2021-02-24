using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;

using Prism.Commands;

using SupportTools_Excel.AzureDevOpsExplorer.Presentation.ModelWrappers;
using SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views;
using SupportTools_Excel.Infrastructure.Presentation.ViewModels;
using System;
using VNC;
using VNC.Core.Mvvm;

namespace SupportTools_Excel.AzureDevOpsExplorer.Presentation.ViewModels
{
    public class QueriesViewModel : ViewModelBase, IAZDOQueriesViewModel
    {
        #region Constructors and Load

        // View First
        // View creates new ViewModel in code or Xaml
        // or ViewModel passed into View constructor

        public QueriesViewModel()
        {
            long startTicks = Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            // TODO(crhodes)
            // Decide if we want defaults
            //AZDOQueries = new AZDOQueriesWrapper(new Domain.AZDOQueries());

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME, startTicks);
        }

        // ViewModel First
        // Calling base(view) wires this ViewModel into the View

        public QueriesViewModel(Queries view) : base(view)
        {
            long startTicks = Log.CONSTRUCTOR("Enter", Common.PROJECT_NAME);

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void InitializeViewModel()
        {
            long startTicks = Log.VIEWMODEL("Enter", Common.PROJECT_NAME);

            //RunQueryCommand = new DelegateCommand(OnRunQueryExecute, OnRunQueryCanExecute);
            //RunTeamProjectQueryCommand = new DelegateCommand(OnRunTeamProjectQueryExecute, OnRunTeamProjectQueryCanExecute);
            //RunTeamProjectQueriesCommand = new DelegateCommand(OnRunTeamProjectQueriesExecute, OnRunTeamProjectQueriesCanExecute);
            QueryChangedCommand = new DelegateCommand(OnQueryChangedExecute, OnQueryChangedCanExecute);
            QueryDoubleClickCommand = new DelegateCommand(OnQueryDoubleClickExecute, OnQueryDoubleClickCanExecute);

            PopulateWorkItemQueries();
            PopulateWorkItemFields();

            Log.VIEWMODEL("Exit", Common.PROJECT_NAME, startTicks);
        }

        private void PopulateWorkItemFields()
        {
            WorkItemFields = new ObservableCollection<string>();

            // HACK(crhodes)
            // Should either retrieve all the fields or drive this in the XML file
            // For now just hard code some fields that will likely be interesting.
            // NB.  Can use short name or ref name, eg. BugReason or Custom.BugReason

            WorkItemFields.Add("Created Date");
            WorkItemFields.Add("Field Issue");
            WorkItemFields.Add("BugReason");
            WorkItemFields.Add("FeatureReason");
            WorkItemFields.Add("IssueReason");
            WorkItemFields.Add("ProductionIssueReason");
            WorkItemFields.Add("ReleaseReason");
            WorkItemFields.Add("RequestReason");
            WorkItemFields.Add("TaskReason");
            WorkItemFields.Add("TestCaseReason");
            WorkItemFields.Add("TestPlanReason");
            WorkItemFields.Add("TestSuiteReason");
            WorkItemFields.Add("UserNeedsReason");
            WorkItemFields.Add("UserStoryReason");
            WorkItemFields.Add("Project ID");
        }

        private void PopulateWorkItemQueries()
        {
            long startTicks = Log.VIEWMODEL("Enter", Common.PROJECT_NAME);

            WorkItemQueries = new List<WorkItemQueryWrapper>();

            XmlTextReader xtr = new XmlTextReader(Common.cCONFIG_FILE);

            XDocument xDocument = XDocument.Load(xtr, LoadOptions.PreserveWhitespace);

            var queries = xDocument.Descendants("TFSQueries");

            WorkItemQueries.Add(
                new WorkItemQueryWrapper(new Domain.WorkItemQuery()
                {
                    Name = "Default",
                    QueryWithTokens = "SELECT @FIELDS FROM WorkItems WHERE [System.TeamProject] = '@PROJECT'"
                }));

            SelectedQuery = WorkItemQueries[0];

            foreach (var query in queries.Elements())
            {
                //var nameV = query.Attribute("Name").Value;
                //var queryV = query.Attribute("Query").Value;

                WorkItemQueries.Add(
                    new WorkItemQueryWrapper(new Domain.WorkItemQuery()
                    {
                        Name = query.Attribute("Name").Value,
                        QueryWithTokens = query.Attribute("Query").Value
                    }));
            }

            Log.VIEWMODEL("Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        public ObservableCollection<string> WorkItemFields
        {
            get;
            set;
        }

         private List<WorkItemQueryWrapper> _workItemQueries;
        public List<WorkItemQueryWrapper> WorkItemQueries
        {
            get => _workItemQueries;
            set
            {
                if (_workItemQueries == value)
                    return;
                _workItemQueries = value;
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

        WorkItemQueryWrapper _selectedQuery;
        public WorkItemQueryWrapper SelectedQuery
        {
            get
            {
                return _selectedQuery;
            }
            set
            {
                _selectedQuery = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Commands

        #region QueryChanged Command

        public DelegateCommand QueryChangedCommand { get; set; }

        public void OnQueryChangedExecute()
        {
            // TODO(crhodes)
            // Do something amazing.
            Message = "Query Changed";
        }

        public bool OnQueryChangedCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region QueryDoubleClick Command

        public DelegateCommand QueryDoubleClickCommand { get; set; }

        public void OnQueryDoubleClickExecute()
        {
            // TODO(crhodes)
            // Do something amazing.
            Message = "Query DoubleClick";
        }

        public bool OnQueryDoubleClickCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion


        #endregion Commands


    }
}
