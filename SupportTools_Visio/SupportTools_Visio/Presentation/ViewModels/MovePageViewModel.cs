﻿using System;
using System.Windows.Input;

using Prism.Commands;
using Prism.Events;

using SupportTools_Visio.Presentation.Views;

using VNC;
using VNC.Core.Mvvm;
using VNC.Core.Services;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class MovePageViewModel : EventViewModelBase, IMovePageViewModel, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public MovePageViewModel(
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public MovePageViewModel(
            MovePage view,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            View = view;
            View.ViewModel = this;
            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // TODO(crhodes)
            //

            SayHelloCommand = new DelegateCommand(
                SayHello, SayHelloCanExecute);

            Message = "MovePageViewModel says hello";

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums


        #endregion

        #region Structures


        #endregion

        #region Fields and Properties

        public ICommand SayHelloCommand { get; private set; }

        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers



        #endregion

        #region Public Methods


        #endregion

        #region Protected Methods


        #endregion

        #region Private Methods

        private bool SayHelloCanExecute()
        {
            return true;
        }

        private void SayHello()
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            Message = "Hello";

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region IInstanceCount

        private static int _instanceCountVM;

        public int InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
