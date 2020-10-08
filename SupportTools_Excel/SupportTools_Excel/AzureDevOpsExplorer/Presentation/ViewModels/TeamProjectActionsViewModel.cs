﻿using Prism.Commands;

using SupportTools_Excel.AzureDevOpsExplorer.Domain;
using SupportTools_Excel.AzureDevOpsExplorer.Presentation.ModelWrappers;
using SupportTools_Excel.AzureDevOpsExplorer.Presentation.Views;
using SupportTools_Excel.Infrastructure.Presentation.ViewModels;

using VNC;
using VNC.Core.Mvvm;

namespace SupportTools_Excel.AzureDevOpsExplorer.Presentation.ViewModels
{
    public class TeamProjectActionsViewModel : ViewModelBase, IAZDOTeamProjectActionsViewModel
    {
        #region Constructors and Load

        // View First
        // View creates new ViewModel in code or Xaml
        // or ViewModel passed into View constructor

        public TeamProjectActionsViewModel()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            // TODO(crhodes)
            // Decide if we want defaults
            //AZDOTeamProjectActions = new AZDOTeamProjectActionsWrapper(new Domain.AZDOTeamProjectActions());

            InitializeViewModel();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        // ViewModel First
        // Calling base(view) wires this ViewModel into the View

        public TeamProjectActionsViewModel(TeamProjectActions view) : base(view)
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            InitializeViewModel();

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        private void InitializeViewModel()
        {
            long startTicks = Log.Trace($"Enter", Common.PROJECT_NAME);

            GetTPInfoCommand = new DelegateCommand(OnGetTPInfoExecute, OnGetTPInfoCanExecute);
            GetTPXMLCommand = new DelegateCommand(OnGetTPXMLExecute, OnGetTPXMLCanExecute);

            TeamProjectActionRequest = new TeamProjectActionRequestWrapper(
                new TeamProjectActionRequest());

            Log.Trace($"Exit", Common.PROJECT_NAME, startTicks);
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        private TeamProjectActionRequestWrapper _teamProjectActionRequest;
        public TeamProjectActionRequestWrapper TeamProjectActionRequest
        {
            get { return _teamProjectActionRequest; }
            set
            {
                if (_teamProjectActionRequest == value)
                    return;
                _teamProjectActionRequest = value;
                OnPropertyChanged();
            }
        }

        //public System.Collections.ObjectModel.ObservableCollection<string> BSSectionsSelected { get; set; }
        //public System.Collections.ObjectModel.ObservableCollection<string> TMSectionsSelected { get; set; }
        //public System.Collections.ObjectModel.ObservableCollection<string> TPSectionsSelected { get; set; }
        //public System.Collections.ObjectModel.ObservableCollection<string> VCSSectionsSelected { get; set; }
        //public System.Collections.ObjectModel.ObservableCollection<string> WISSectionsSelected { get; set; }

        #endregion

        #region Commands

        #region GetTPInfo Command

        public DelegateCommand GetTPInfoCommand { get; set; }
        public string GetTPInfoContent { get; set; } = "Get Team Project Information";
        public string GetTPInfoToolTip { get; set; } = "Creates One WorkSheet containing selected Sections for Each Selected Team Project";

        public void OnGetTPInfoExecute()
        {
            Common.EventAggregator.GetEvent<GetTeamProjectInfoEvent>().Publish(TeamProjectActionRequest.Model);
        }

        public bool OnGetTPInfoCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region GetTPXML Command

        public DelegateCommand GetTPXMLCommand { get; set; }
        public string GetTPXMLContent { get; set; } = "Get Team Project XML";
        public string GetTPXMLToolTip { get; set; } = "Gets XML Definition for Team Project using Hosted XML Process Model";

        public void OnGetTPXMLExecute()
        {
            // TODO(crhodes)
            // May not need request

            Common.EventAggregator.GetEvent<GetTeamProjectXMLEvent>().Publish(TeamProjectActionRequest.Model);
        }

        public bool OnGetTPXMLCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #endregion Commands

    }
}
