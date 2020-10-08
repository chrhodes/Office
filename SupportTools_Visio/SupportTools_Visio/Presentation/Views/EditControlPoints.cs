using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using SupportTools_Visio.Domain;

using VisioHelper = VNC.AddinHelper.Visio;
using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class EditControlPoints : UserControl
    {
        #region Constructors and Load

        public EditControlPoints()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            InitializeComponent();
            LoadControlContents();
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            //VNC.Log.Trace("", Common.LOG_APPNAME, 0);
            //VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
            //    System.Reflection.MethodInfo.GetCurrentMethod().Name));
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            //VNC.Log.Trace("", Common.LOG_APPNAME, 0);
            //VisioHelper.DisplayInWatchWindow(string.Format("{0}()",
            //    System.Reflection.MethodInfo.GetCurrentMethod().Name));
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        #endregion

        #region Event Handlers

        private void btnAddConnectionPoints_Click(object sender, RoutedEventArgs e)
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            //VNC.Log.Trace("", Common.LOG_APPNAME, 0);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("AddConnectionPoints");

            Actions.Visio_Shape.Add_ConnectionPoints(GetConnectionPointSettings());

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
        }

        private void btnClearConnectionPoints_Click(object sender, RoutedEventArgs e)
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            string tag = ((Button)sender).Tag.ToString();

            Actions.Visio_Shape.ClearConnectionPoints(tag);
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

        List<Domain.ConnectionPointRow> GetConnectionPointSettings()
        {
            List<Domain.ConnectionPointRow> connectionPoints = new List<Domain.ConnectionPointRow>();

            #region Top Edge

            if ((bool)ceTEL.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "TEL",
                    X = "Width*0.0",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top,Left"
                });
            }

            if ((bool)ceT8LL.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "T8LL",
                    X = "Width*0.125",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top"
                });
            }

            if ((bool)ceTQL.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "TQL",
                    X = "Width*0.25",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top"
                });
            }

            if ((bool)ceT8LR.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "T8LR",
                    X = "Width*0.375",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top"
                });
            }

            if ((bool)ceTM.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "TM",
                    X = "Width*0.5",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top"
                });
            }

            if ((bool)ceT8RL.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "T8RL",
                    X = "Width*0.625",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top"
                });
            }

            if ((bool)ceTQR.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "TQR",
                    X = "Width*0.75",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top"
                });
            }

            if ((bool)ceT8RR.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "T8RR",
                    X = "Width*0.875",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top"
                });
            }

            if ((bool)ceTER.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "TER",
                    X = "Width*1.0",
                    Y = "Height*1.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Top,Right"
                });
            }

            #endregion Top

            #region Bottom

            if ((bool)ceBEL.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "BEL",
                    X = "Width*0.0",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom,Left"
                });
            }

            if ((bool)ceB8LL.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "B8LL",
                    X = "Width*0.125",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom"
                });
            }

            if ((bool)ceBQL.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "BQL",
                    X = "Width*0.25",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom"
                });
            }

            if ((bool)ceB8LR.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "B8LR",
                    X = "Width*0.375",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom"
                });
            }

            if ((bool)ceBM.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "BM",
                    X = "Width*0.5",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom"
                });
            }

            if ((bool)ceB8RL.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "B8RL",
                    X = "Width*0.625",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom"
                });
            }

            if ((bool)ceBQR.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "BQR",
                    X = "Width*0.75",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom"
                });
            }

            if ((bool)ceB8RR.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "B8RR",
                    X = "Width*0.875",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom"
                });
            }

            if ((bool)ceBER.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "BER",
                    X = "Width*1.0",
                    Y = "Height*0.0",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Bottom,Right"
                });
            }

            #endregion Bottom

            #region Left

            if ((bool)ceL8TT.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "L8TT",
                    X = "Width*0.0",
                    Y = "Height*0.875",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Left"
                });
            }

            if ((bool)ceLQT.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "LQT",
                    X = "Width*0.0",
                    Y = "Height*0.75",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Left"
                });
            }

            if ((bool)ceL8TB.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "L8TB",
                    X = "Width*0.0",
                    Y = "Height*0.625",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Left"
                });
            }

            if ((bool)ceLM.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "LM",
                    X = "Width*0.0",
                    Y = "Height*0.5",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Left"
                });
            }

            if ((bool)ceL8BT.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "L8BT",
                    X = "Width*0.0",
                    Y = "Height*0.375",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Left"
                });
            }

            if ((bool)ceLQB.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "LQB",
                    X = "Width*0.0",
                    Y = "Height*0.25",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Left"
                });
            }

            if ((bool)ceL8BB.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "L8BB",
                    X = "Width*0.0",
                    Y = "Height*0.125",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Left"
                });
            }

            #endregion Left

            #region Right

            if ((bool)ceR8TT.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "R8TT",
                    X = "Width*1.0",
                    Y = "Height*0.875",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Right"
                });
            }

            if ((bool)ceRQT.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "RQT",
                    X = "Width*1.0",
                    Y = "Height*0.75",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Right"
                });
            }

            if ((bool)ceR8TB.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "R8TB",
                    X = "Width*1.0",
                    Y = "Height*0.625",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Right"
                });
            }

            if ((bool)ceRM.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "RM",
                    X = "Width*1.0",
                    Y = "Height*0.5",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Right"
                });
            }

            if ((bool)ceR8BT.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "R8BT",
                    X = "Width*1.0",
                    Y = "Height*0.375",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Right"
                });
            }

            if ((bool)ceRQB.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "RQB",
                    X = "Width*1.0",
                    Y = "Height*0.25",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Right"
                });
            }

            if ((bool)ceR8BB.IsChecked)
            {
                connectionPoints.Add(new Domain.ConnectionPointRow
                {
                    Name = "R8BB",
                    X = "Width*1.0",
                    Y = "Height*0.125",
                    DirX = "0 in",
                    DirY = "0 in",
                    Type = "0",
                    D = "Right"
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

        private void btnInitializeConnectionPoints_Click(string tag)
        {
            switch (tag)
            {
                case "Tops":
                    CheckTops(false);
                    break;

                case "Bottoms":
                    CheckBottoms(false);
                    break;

                case "Lefts":
                    CheckLefts(false);
                    break;

                case "Rights":
                    CheckRights(false);
                    break;

                case "All":
                    ClearAll();
                    break;

                default:
                    MessageBox.Show($"Unknown tag: {tag}");
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
