using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using DevExpress.Utils.DirectXPaint;
using DevExpress.Xpf.Editors;
using SupportTools_Visio.Domain;
using dxe = DevExpress.Xpf.Editors;

using VisioHelper = VNC.AddinHelper.Visio;

namespace SupportTools_Visio.User_Interface.User_Controls
{
    public partial class wucEditShape : UserControl
    {
        #region Constructors and Load

        public wucEditShape()
        {
            InitializeComponent();
            LoadControlContents();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VNC.Log.Trace("", Common.LOG_CATEGORY, 0);
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            VNC.Log.Trace("", Common.LOG_CATEGORY, 0);
            VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));
        }

        #endregion

        #region Event Handlers

        private void btnAddConnectionPoints_Click(object sender, RoutedEventArgs e)
        {
            VNC.Log.Trace("", Common.LOG_CATEGORY, 0);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("AddConnectionPoints");

            Actions.Visio_Shape.Add_ConnectionPoints(GetConnectionPointSettings());

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
        }

        private void btnClearConnectionPoints_Click(object sender, RoutedEventArgs e)
        {
            Actions.Visio_Shape.ClearConnectionPoints("All");
        }

        #endregion


        #region Private Methods

        private void LoadControlContents()
        {
            try
            {
                //visioCommand_Picker.PopulateControlFromFile(Common.cCONFIG_FILE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        List<ConnectionPointRow> GetConnectionPointSettings()
        {
            List<ConnectionPointRow> connectionPoints = new List<ConnectionPointRow>();

            #region Top Edge

            if ((bool)ceTEL.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                { 
                    X = "Width*0.0",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceT8LL.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.125",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceTQL.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.25",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceT8LR.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.375",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceTM.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.5",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceT8RL.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.625",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceTQR.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.75",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceT8RR.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.875",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceTER.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            #endregion Top

            #region Bottom

            if ((bool)ceBEL.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.0",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceB8LL.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.125",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceBQL.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.25",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceB8LR.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.375",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceBM.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.5",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceB8RL.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.625",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceBQR.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.75",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceB8RR.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.875",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceBER.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            #endregion Bottom

            #region Left

            if ((bool)ceL8TT.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.0",
                    Y = "Height*0.875",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceLQT.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.0",
                    Y = "Height*0.75",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceL8TB.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.0",
                    Y = "Height*0.625",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceLM.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.0",
                    Y = "Height*0.5",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceL8BT.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.0",
                    Y = "Height*0.375",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceLQB.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.0",
                    Y = "Height*0.25",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceL8BB.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*0.0",
                    Y = "Height*0.125",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            #endregion Left

            #region Right

            if ((bool)ceR8TT.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*0.875",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceRQT.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*0.75",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceR8TB.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*0.625",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceRM.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*0.5",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceR8BT.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*0.375",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceRQB.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*0.25",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            if ((bool)ceR8BB.IsChecked)
            {
                connectionPoints.Add(new ConnectionPointRow
                {
                    X = "Width*1.0",
                    Y = "Height*0.125",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0"
                });
            }

            #endregion Right

            return connectionPoints;
        }

        private void btnInitializeConnectionPoints_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Tag.ToString())
            {
                case "Tops":
                    CheckTops(true);
                    break;

                case "Bottoms":
                    CheckBottoms(true);
                    break;

                case "Lefts":
                    CheckLefts(true);
                    break;

                case "Rights":
                    CheckRights(true);
                    break;

                case "Edges":
                    CheckEdges(true);
                    break;

                case "Middles":
                    CheckMiddles(true);
                    break;

                case "Quarters":
                    CheckQuarters(true);

                    break;

                case "Eighths":
                    CheckEighths(true);
                    break;

                case "All":
                    CheckAll();
                    break;

                case "Clear":
                    ClearAll();
                    break;
            }
        }

        void ClearAll()
        {
            CheckEdges(false);
            CheckMiddles(false);
            CheckQuarters(false);
            CheckEighths(false);
        }

        void CheckAll()
        {
            CheckEdges(true);
            CheckMiddles(true);
            CheckQuarters(true);
            CheckEighths(true);
        }

        void CheckTops(bool isChecked)
        {
            ceTEL.IsChecked = isChecked;
            ceT8LL.IsChecked = isChecked;
            ceTQL.IsChecked = isChecked;
            ceT8LR.IsChecked = isChecked;
            ceTM.IsChecked = isChecked;
            ceT8RL.IsChecked = isChecked;
            ceTQR.IsChecked = isChecked;
            ceT8RR.IsChecked = isChecked;
            ceTER.IsChecked = isChecked;
        }

        void CheckBottoms(bool isChecked)
        {
            ceBEL.IsChecked = isChecked;
            ceB8LL.IsChecked = isChecked;
            ceBQL.IsChecked = isChecked;
            ceB8LR.IsChecked = isChecked;
            ceBM.IsChecked = isChecked;
            ceB8RL.IsChecked = isChecked;
            ceBQR.IsChecked = isChecked;
            ceB8RR.IsChecked = isChecked;
            ceBER.IsChecked = isChecked;
        }

        void CheckLefts(bool isChecked)
        {
            ceTEL.IsChecked = isChecked;

            ceL8TT.IsChecked = isChecked;
            ceLQT.IsChecked = isChecked;
            ceL8TB.IsChecked = isChecked;
            ceLM.IsChecked = isChecked;
            ceL8BT.IsChecked = isChecked;
            ceLQB.IsChecked = isChecked;
            ceL8BB.IsChecked = isChecked;

            ceBEL.IsChecked = isChecked;
        }

        void CheckRights(bool isChecked)
        {
            ceTER.IsChecked = isChecked;

            ceR8TT.IsChecked = isChecked;
            ceRQT.IsChecked = isChecked;
            ceR8TB.IsChecked = isChecked;
            ceRM.IsChecked = isChecked;
            ceR8BT.IsChecked = isChecked;
            ceRQB.IsChecked = isChecked;
            ceR8BB.IsChecked = isChecked;

            ceBER.IsChecked = isChecked;
        }

        private void CheckEighths(bool isChecked)
        {
            ceT8LL.IsChecked = isChecked;
            ceT8LR.IsChecked = isChecked;
            ceT8RL.IsChecked = isChecked;
            ceT8RR.IsChecked = isChecked;

            ceB8LL.IsChecked = isChecked;
            ceB8LR.IsChecked = isChecked;
            ceB8RL.IsChecked = isChecked;
            ceB8RR.IsChecked = isChecked;

            ceL8TT.IsChecked = isChecked;
            ceL8TB.IsChecked = isChecked;
            ceL8BT.IsChecked = isChecked;
            ceL8BB.IsChecked = isChecked;

            ceR8TT.IsChecked = isChecked;
            ceR8TB.IsChecked = isChecked;
            ceR8BT.IsChecked = isChecked;
            ceR8BB.IsChecked = isChecked;
        }

        private void CheckQuarters(bool isChecked)
        {
            ceTQL.IsChecked = isChecked;
            ceTQR.IsChecked = isChecked;

            ceBQL.IsChecked = isChecked;
            ceBQR.IsChecked = isChecked;

            ceLQT.IsChecked = isChecked;
            ceLQB.IsChecked = isChecked;

            ceRQT.IsChecked = isChecked;
            ceRQB.IsChecked = isChecked;
        }

        private void CheckMiddles(bool isChecked)
        {
            ceTM.IsChecked = isChecked;
            ceBM.IsChecked = isChecked;

            ceLM.IsChecked = isChecked;
            ceRM.IsChecked = isChecked;
        }

        private void CheckEdges(bool isChecked)
        {
            ceTEL.IsChecked = isChecked;
            ceTER.IsChecked = isChecked;

            ceBEL.IsChecked = isChecked;
            ceBER.IsChecked = isChecked;
        }
    }
}
