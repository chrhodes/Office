﻿using System;

using Microsoft.Office.Tools.Ribbon;

using SupportTools_Visio.Presentation.ViewModels;
using SupportTools_Visio.Presentation.Views;

using VNC;
using VNC.Core.Presentation;
using VNC.WPF.Presentation.Dx.Views;

namespace SupportTools_Visio
{
    public partial class Ribbon
    {
        #region Event Handlers

        #region WPF Events - Custom

        public static DxThemedWindowHost commandCockpitHost = null;

        private void btnCommandCockpit_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref commandCockpitHost,
                "Command Cockpit (XML Commands)",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                (CommandCockpit)Common.ApplicationBootstrapper.Container.Resolve(typeof(CommandCockpit))
            );

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static DxThemedWindowHost linqToExcelHost = null;

        private void btnLinqToExcel_Click
            (object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref linqToExcelHost,
                "Linq to Excel",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                (Presentation.Views.LinqToExcel)Common.ApplicationBootstrapper.Container.Resolve(typeof(Presentation.Views.LinqToExcel))
            );

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static DxThemedWindowHost duplicatePageHost = null;

        private void btnDuplicatePage_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref duplicatePageHost,
            "Duplicate Page",
            Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
            //Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
            ShowWindowMode.Modeless_Show,
            new Presentation.Views.DuplicatePage(new DuplicatePageViewModel()));

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static DxThemedWindowHost renamePagesHost = null;

        private void btnRenamePages_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref renamePagesHost,
                "Rename Paqe(s)",
                Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
                ShowWindowMode.Modeless_Show,
                (RenamePageViewModel)Common.ApplicationBootstrapper.Container.Resolve(typeof(RenamePageViewModel))
            );

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static DxThemedWindowHost movePagesHost = null;

        private void btnMovePages_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref movePagesHost,
                "Move Paqe(s)",
                Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
                ShowWindowMode.Modeless_Show,
                (MovePageViewModel)Common.ApplicationBootstrapper.Container.Resolve(typeof(MovePageViewModel))
            );

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }



        public static DxThemedWindowHost editControlRowsHost = null;

        private void btnEditControlRows_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref editControlRowsHost,
                "Edit Control Rows",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditControlRows(new Presentation.ViewModels.EditControlRowsViewModel()));

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static DxThemedWindowHost editParagraphHost = null;

        //public static VNC.Core.Xaml.Presentation.WindowHost editControlPointsHost = null;
        private void btnEditParagraph_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref editParagraphHost,
                "Edit Paragraph",
                300, 600,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditParagraph(new Presentation.ViewModels.EditParagraphViewModel()));

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private Presentation.Views.EditControlPoints editControlPointsUC = null;

        public static VNC.WPF.Presentation.Dx.Views.DxThemedWindowHost editTextHost = null;

        // static VNC.Core.Xaml.Presentation.WindowHost editTextHost = null;
        private Presentation.Views.EditText editTextUC = null;

        public static DxThemedWindowHost editControlPointsHost = null;

        private void btnEditControlPoints_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref editControlPointsHost,
                "Edit Shape Control Points Text",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditControlPoints());

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void btnEditText_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref editTextHost,
                "Edit Text",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.EditText(new Presentation.ViewModels.EditTextViewModel()));

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #endregion Event Handlers
    }
}