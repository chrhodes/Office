﻿using System;

using Prism.Commands;

using SupportTools_Visio.Infrastructure;

using VNC;
using VNC.Core.Mvvm;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class DocumentShapeSheetSectionBase : ViewModelBase
    {
        public DelegateCommand UpdateSettings { get; protected set; }
        public DelegateCommand LoadCurrentSettings { get; protected set; }

        public DelegateCommand Refresh { get; protected set; }

        public DocumentShapeSheetSectionBase()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_APPNAME);

            UpdateSettings = new DelegateCommand(OnUpdateSettingsExecute, OnUpdateSettingsCanExecute);
            LoadCurrentSettings = new DelegateCommand(OnLoadCurrentSettingsExecute, OnLoadCurrentSettingsCanExecute);

            Common.EventAggregator.GetEvent<SelectionChangedEvent>().Subscribe(OnRefresh);

            Log.CONSTRUCTOR("Exit", Common.LOG_APPNAME, startTicks);
        }

        string _message = "";
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

        protected void OnRefresh()
        {
            LoadCurrentSettings.RaiseCanExecuteChanged();
        }

        string _LoadButtonContent = "Load from Active Document";
        public string LoadButtonContent
        {
            get
            {
                return _LoadButtonContent;
            }
            set
            {
                _LoadButtonContent = value;
                OnPropertyChanged();
            }
        }

        string _UpdateButtonContent = "Update Document";
        public string UpdateButtonContent
        {
            get
            {
                return _UpdateButtonContent;
            }
            set
            {
                _UpdateButtonContent = value;
                OnPropertyChanged();
            }
        }

        int _selectedShapeCount;
        public int SelectedShapeCount
        {
            get { return _selectedShapeCount; }
            set
            {
                if (_selectedShapeCount == value)
                    return;
                _selectedShapeCount = value;
                OnPropertyChanged();
                //LoadCurrentSettings.RaiseCanExecuteChanged();
            }
        }

        public virtual void OnUpdateSettingsExecute()
        {
            Message = "OnLoadCurrentSettingsExecute Called";
        }

        public virtual Boolean OnUpdateSettingsCanExecute()
        {
            return true;
        }

        public virtual void OnLoadCurrentSettingsExecute()
        {
            // TODO(crhodes)
            // Validate we have new settings

            Message = "OnLoadCurrentSettingsExecute Called";
        }

        public virtual Boolean OnLoadCurrentSettingsCanExecute()
        {
            //return true;
            Visio.Application app = Globals.ThisAddIn.Application;
            Visio.Document document = app.ActiveDocument;

            Visio.Selection selection = app.ActiveWindow.Selection;

            //var containingMaster = selection.ContainingMaster;
            //var containingMasterID = selection.ContainingMasterID;
            //var containingPage = selection.ContainingPage;
            //var containingPageID = selection.ContainingPageID;
            //var containingShape = selection.ContainingShape;
            //var primaryItem = selection.PrimaryItem;

            //var itemStatus = selection.ItemStatus[0];

            var count = selection.Count;
            SelectedShapeCount = count;

            //var whatAreYou = selection[0];

            //if (Visio_Shape.HasTextTransformSection(shape))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            //// TODO(crhodes)
            //// Check if shape selected


            if (document != null)
            {
                //if (count != 1)
                //{
                //    LoadButtonContent = "Must select single shape to load settings";

                //    return false;
                //}

                //LoadButtonContent = "Load from Active Document";

                return true;
            }
            else
            {
                LoadButtonContent = "No ActiveDocument";

                return false;
            }
        }
    }
}
