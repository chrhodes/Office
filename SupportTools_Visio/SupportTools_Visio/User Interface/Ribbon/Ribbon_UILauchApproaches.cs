﻿using System;
using System.Windows;

using Microsoft.Office.Tools.Ribbon;

using Prism.Events;

using SupportTools_Visio.Presentation.ViewModels;
using SupportTools_Visio.Presentation.Views;

using VNC;
using VNC.Core.Presentation;
using VNC.Core.Services;
using VNC.WPF.Presentation.Dx.Views;
using VNC.WPF.Presentation.Views;

namespace SupportTools_Visio
{
    public partial class Ribbon
    {
        #region Event Handlers

        #region UI Launch Events

        private DxThemedWindowHost themedWindowHostModal = null;

        private void btnThemedWindowHostModal_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref themedWindowHostModal,
                "ThemedWindowHost (Modal)",
                Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
                ShowWindowMode.Modal_ShowDialog);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private DxThemedWindowHost themedWindowHostModeless = null;

        private void btnThemedWindowHost_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref themedWindowHostModeless,
                "ThemedWindowHost (Modeless)",
                Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
                ShowWindowMode.Modeless_Show);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private DxWindowHost dxWindowHost = null;

        private void btnDxWindowHost_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxWindowHost.DisplayUserControlInHost(ref dxWindowHost,
                "DxWindowHost Test",
                Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
                ShowWindowMode.Modeless_Show);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static WindowHost windowHostLocal = null;

        private void btnWindowHostLocal_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            WindowHost.DisplayUserControlInHost(ref windowHostLocal,
                "WindowHost (local) Test",
                Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
                ShowWindowMode.Modeless_Show);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void btnWindowHostVNC_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            ShowEmptyHost(windowHostVNC, "WindowHost (VNC)", ShowWindowMode.Modeless_Show);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private static void ShowEmptyHost(Window host, string title, ShowWindowMode mode)
        {
            long startTicks = Log.PRESENTATION("Enter", Common.LOG_CATEGORY);

            if (host is null)
            {
                host = new DxThemedWindowHost();
                host.Height = Common.DEFAULT_WINDOW_HEIGHT_SMALL;
                host.Width = Common.DEFAULT_WINDOW_WIDTH_SMALL;
                host.Title = title;
            }

            if (mode == ShowWindowMode.Modal_ShowDialog)
            {
                long endTicks2 = Log.PRESENTATION("Exit", Common.LOG_CATEGORY, startTicks);

                host.Title = $"{host.GetType()} loadtime: {Log.GetDuration(startTicks, endTicks2)}";

                host.ShowDialog();
            }
            else
            {
                host.Show();
            }

            long endTicks = Log.PRESENTATION("Exit", Common.LOG_CATEGORY, startTicks);

            host.Title = $"{host.GetType()} loadtime: {Log.GetDuration(startTicks, endTicks)}";
        }

        #endregion UI Launch Events

        #region WPF UI Events

        #region WPF Events - Custom

        public static WindowHost cylonHost = null;

        private void btnLaunchCylon_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            WindowHost.DisplayUserControlInHost(ref cylonHost,
                "I am a Cylon loaded by name",
                Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
                ShowWindowMode.Modeless_Show,
                "VNC.WPF.Presentation.Views.CylonEyeBall, VNC.WPF.Presentation");

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static WindowHost cylonHost2 = null;

        private void btnLaunchCylon2_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            WindowHost.DisplayUserControlInHost(ref cylonHost2,
                "I am a Cylon loaded by type",
                Common.DEFAULT_WINDOW_WIDTH_SMALL, Common.DEFAULT_WINDOW_HEIGHT_SMALL,
                ShowWindowMode.Modeless_Show,
                new CylonEyeBall());

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        DxLayoutControl _dxLayoutControl;

        public DxLayoutControl DxLayoutControl
        {
            get
            {
                if (_dxLayoutControl is null)
                {
                    _dxLayoutControl = new DxLayoutControl();
                }

                return _dxLayoutControl;
            }
            set
            {
                _dxLayoutControl = value;
            }
        }

        private DxThemedWindowHost dxLayoutControlHost = null;

        private void btnDxLayoutControl_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref dxLayoutControlHost,
                "DxLayoutControl Test",
                Common.DEFAULT_WINDOW_WIDTH_LARGE, Common.DEFAULT_WINDOW_HEIGHT_LARGE,
                ShowWindowMode.Modeless_Show,
                DxLayoutControl);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        DxDockLayoutControl _dxDockLayoutControl;

        public DxDockLayoutControl DxDockLayoutControl
        {
            get
            {
                if (_dxDockLayoutControl is null)
                {
                    _dxDockLayoutControl = new DxDockLayoutControl();
                }

                return _dxDockLayoutControl;
            }
            set
            {
                _dxDockLayoutControl = value;
            }
        }

        private DxThemedWindowHost dxDockLayoutControlHost = null;

        private void btnDxDockLayoutControl_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref dxDockLayoutControlHost,
                "DxDockLayoutControl Test",
                Common.DEFAULT_WINDOW_WIDTH_LARGE, Common.DEFAULT_WINDOW_HEIGHT_LARGE,
                ShowWindowMode.Modeless_Show,
                DxDockLayoutControl);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        DxDockLayoutManagerControl _dxDockLayoutControlManager;

        public DxDockLayoutManagerControl DxDockLayoutManagerControl
        {
            get
            {
                if (_dxDockLayoutControlManager is null)
                {
                    _dxDockLayoutControlManager = new DxDockLayoutManagerControl();
                }

                return _dxDockLayoutControlManager;
            }
            set
            {
                _dxDockLayoutControlManager = value;
            }
        }

        private DxThemedWindowHost dxDockLayoutManagerControlHost = null;

        private void btnDxDockLayoutManagerControl_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref dxDockLayoutManagerControlHost,
                "DxDocLayoutManagerControl Test",
                Common.DEFAULT_WINDOW_WIDTH_LARGE, Common.DEFAULT_WINDOW_HEIGHT_LARGE,
                ShowWindowMode.Modeless_Show,
                DxDockLayoutManagerControl);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        Presentation.Views.PrismRegionTest _prismRegionTest;

        public Presentation.Views.PrismRegionTest PrismRegionTest
        {
            get
            {
                if (_prismRegionTest is null)
                {
                    _prismRegionTest = new Presentation.Views.PrismRegionTest();
                }

                return _prismRegionTest;
            }
            set
            {
                _prismRegionTest = value;
            }
        }

        private DxThemedWindowHost prismRegionTestHost = null;

        private void btnPrismRegionTest_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref prismRegionTestHost,
                "Prism Region Test", 
                Common.DEFAULT_WINDOW_WIDTH_LARGE, Common.DEFAULT_WINDOW_HEIGHT_LARGE,
                ShowWindowMode.Modeless_Show,
                PrismRegionTest);

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #endregion WPF UI Events

        #region MVVM Examples

        public static DxThemedWindowHost vncMVVM_V1_Host = null;

        private void btnVNC_MVVM_V1_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V1_Host,
                "MVVM View First (View is passed new ViewModel)",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.Views.ViewA(
                    new Presentation.ViewModels.ViewAViewModel(
                        (IEventAggregator)Common.ApplicationBootstrapper.Container.Resolve(typeof(EventAggregator)),
                        (IMessageDialogService)Common.ApplicationBootstrapper.Container.Resolve(typeof(MessageDialogService))
                    )
                )
            );

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        public static DxThemedWindowHost vncMVVM_VM1_Host = null;

        private void btnVNC_MVVM_VM1_Click(object sender, RibbonControlEventArgs e)
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_VM1_Host,
                "MVVM ViewModel First (ViewModel is passed new View)",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                new Presentation.ViewModels.ViewAViewModel(
                    new Presentation.Views.ViewA(),
                    (IEventAggregator)Common.ApplicationBootstrapper.Container.Resolve(typeof(EventAggregator)),
                    (IMessageDialogService)Common.ApplicationBootstrapper.Container.Resolve(typeof(MessageDialogService))
                )
            );

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        //++ NOTE(crhodes)
        //++ Use Container to Resolve View and ViewModel for us
        // If try to Register in SupportTools_VisioModule, get StackOverflow
        // Go learn why some day.

        public static DxThemedWindowHost vncMVVM_V2_Host = null;

        private void btnVNC_MVVM_V2_Click(object sender, RibbonControlEventArgs e)
        {
            // NOTE(crhodes)
            // This does not wire View to ViewModel

            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_V2_Host,
                "MVVM View First (View is passed new ViewModel) DI Resolve",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                (ViewA)Common.ApplicationBootstrapper.Container.Resolve(typeof(ViewA))
            );
        }

        public static DxThemedWindowHost vncMVVM_VM2_Host = null;

        private void btnVNC_MVVM_VM2_Click(object sender, RibbonControlEventArgs e)
        {
            DxThemedWindowHost.DisplayUserControlInHost(ref vncMVVM_VM2_Host,
                "MVVM ViewModel First (ViewModel is passed new View) DI Resolve",
                Common.DEFAULT_WINDOW_WIDTH, Common.DEFAULT_WINDOW_HEIGHT,
                ShowWindowMode.Modeless_Show,
                (ViewAViewModel)Common.ApplicationBootstrapper.Container.Resolve(typeof(ViewAViewModel))
            );
        }

        #endregion

        #endregion Event Handlers
    }
}