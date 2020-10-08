using System.Collections.ObjectModel;

using Microsoft.Office.Interop.Excel;

using Prism.Commands;

using SupportTools_Excel.AzureDevOpsExplorer.Presentation.ModelWrappers;
using SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views;
using SupportTools_Excel.Infrastructure.Presentation.ViewModels;
using System;
using VNC;
using VNC.Core.Mvvm;
using Microsoft.SharePoint.Client;
using System.Collections.Generic;
using System.Xml.Linq;
using SupportTools_Excel.Data;
using System.Xml;
using SupportTools_Excel.AzureDevOpsExplorer.Domain;

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
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            // TODO(crhodes)
            // Decide if we want defaults
            //AZDOQueries = new AZDOQueriesWrapper(new Domain.AZDOQueries());

            InitializeViewModel();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        // ViewModel First
        // Calling base(view) wires this ViewModel into the View

        public QueriesViewModel(Queries view) : base(view)
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeViewModel();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        private void InitializeViewModel()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            //RunQueryCommand = new DelegateCommand(OnRunQueryExecute, OnRunQueryCanExecute);
            //RunTeamProjectQueryCommand = new DelegateCommand(OnRunTeamProjectQueryExecute, OnRunTeamProjectQueryCanExecute);
            //RunTeamProjectQueriesCommand = new DelegateCommand(OnRunTeamProjectQueriesExecute, OnRunTeamProjectQueriesCanExecute);
            QueryChangedCommand = new DelegateCommand(OnQueryChangedExecute, OnQueryChangedCanExecute);
            QueryDoubleClickCommand = new DelegateCommand(OnQueryDoubleClickExecute, OnQueryDoubleClickCanExecute);

            PopulateWorkItemQueries();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        private void PopulateWorkItemQueries()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            WorkItemQueries = new ObservableCollection<WorkItemQueryWrapper>();

            WorkItemQueries.Add(
                new WorkItemQueryWrapper(new Domain.WorkItemQuery() { Name="MyQuery", QueryWithTokens="SELECT SOMETHING FROM SOMEWHERE WHERE STUFF" }));

            //WorkItemQueries2 = new ObservableCollection<WorkItemQueryWrapper>();
            WorkItemQueries3 = new List<WorkItemQueryWrapper>();

            XmlTextReader xtr = new XmlTextReader(Common.cCONFIG_FILE);

            XDocument xDocument = XDocument.Load(xtr, LoadOptions.PreserveWhitespace);
            //XElement xElement = XElement.Load(Common.cCONFIG_FILE, LoadOptions.PreserveWhitespace);

            //XElement xElement = XElement.Load(Common.cCONFIG_FILE);

            var queries = xDocument.Descendants("TFSQueries");

            //var queries2 = xElement.Element("TFSQueries");

            WorkItemQueries3.Add(
                new WorkItemQueryWrapper(new Domain.WorkItemQuery()
                {
                    Name = "Default",
                    QueryWithTokens = "SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = '@PROJECT'"
                }));

            SelectedQuery3 = WorkItemQueries3[0];

            foreach (var query in queries.Elements())
            {
                //var nameV = query.Attribute("Name").Value;
                //var queryV = query.Attribute("Query").Value;

                WorkItemQueries3.Add(
                    new WorkItemQueryWrapper(new Domain.WorkItemQuery()
                    {
                        Name = query.Attribute("Name").Value,
                        QueryWithTokens = query.Attribute("Query").Value
                    }));
            }

            //WorkItemQueries3.Add(
            //    new WorkItemQueryWrapper(new Domain.WorkItemQuery()
            //        {
            //            Name = "Query1",
            //            QueryWithTokens = "SELECT SOMETHING1 FROM SOMEWHERE1 WHERE STUFF1"
            //        }));

            //WorkItemQueries3.Add(
            //    new WorkItemQueryWrapper(new Domain.WorkItemQuery()
            //    {
            //        Name = "Query2",
            //        QueryWithTokens = "SELECT SOMETHING2 FROM SOMEWHERE2 WHERE STUFF2"
            //    }));

            //WorkItemQueries3.Add(
            //    new WorkItemQueryWrapper(new Domain.WorkItemQuery()
            //    {
            //        Name = "Query3",
            //        QueryWithTokens = "SELECT SOMETHING3 FROM SOMEWHERE3 WHERE STUFF3"
            //    }));

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        public ObservableCollection<WorkItemQueryWrapper> WorkItemQueries 
        { 
            get; 
            set; 
        }

        //public ObservableCollection<WorkItemQueryWrapper> WorkItemQueries2 
        //{ 
        //    get; 
        //    set;
        //}

         private List<WorkItemQueryWrapper> _workItemQueries3;       
        public List<WorkItemQueryWrapper> WorkItemQueries3
        {
            get => _workItemQueries3;
            set
            {
                if (_workItemQueries3 == value)
                    return;
                _workItemQueries3 = value;
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

        WorkItemQueryWrapper _selectedQuery2;
        public WorkItemQueryWrapper SelectedQuery2
        {
            get
            {
                return _selectedQuery2;
            }
            set
            {
                _selectedQuery2 = value;
                OnPropertyChanged();
            }
        }

        WorkItemQueryWrapper _selectedQuery3;
        public WorkItemQueryWrapper SelectedQuery3
        {
            get
            {
                return _selectedQuery3;
            }
            set
            {
                _selectedQuery3 = value;
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

        //#region Run Query Command

        //public DelegateCommand RunQueryCommand { get; set; }
        //public string RunQueryContent { get; set; } = "Run Query";
        //public string RunQueryToolTip { get; set; } = "Run Query ToolTip";

        //public void OnRunQueryExecute()
        //{
        //    Common.EventAggregator.GetEvent<RunQueryEvent>().Publish(SelectedQuery.Model);
        //}

        //public bool OnRunQueryCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    return true;
        //}

        //#endregion

        //#region Run TeamProject Query Command

        //public DelegateCommand RunTeamProjectQueryCommand { get; set; }
        //public string RunTeamProjectQueryContent { get; set; } = "Run Query on TeamProject";
        //public string RunTeamProjectQueryToolTip { get; set; } = "Run Query on TeamProject ToolTip";

        //public void OnRunTeamProjectQueryExecute()
        //{
        //    Common.EventAggregator.GetEvent<RunTeamProjectQueryEvent>().Publish(SelectedQuery.Model);
        //}

        //public bool OnRunTeamProjectQueryCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    return true;
        //}

        //#endregion

        //#region Run TeamProject Queries Command

        //public DelegateCommand RunTeamProjectQueriesCommand { get; set; }
        //public string RunTeamProjectQueriesContent { get; set; } = "Run Queries on TeamProject";
        //public string RunTeamProjectQueriesToolTip { get; set; } = "Run Queries on TeamProject ToolTip";

        //public void OnRunTeamProjectQueriesExecute()
        //{
        //    // TODO(crhodes)
        //    // Figure out how to pass a collection of queries
        //    Common.EventAggregator.GetEvent<RunTeamProjectQueriesEvent>().Publish(SelectedQuery.Model);
        //}

        //public bool OnRunTeamProjectQueriesCanExecute()
        //{
        //    // TODO(crhodes)
        //    // Add any before button is enabled logic.
        //    return true;
        //}

        //#endregion

        #endregion Commands


    }
}
