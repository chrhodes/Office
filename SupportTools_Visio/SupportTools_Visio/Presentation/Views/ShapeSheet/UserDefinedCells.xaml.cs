﻿using System;
using System.Windows.Controls;

using VNC;

namespace SupportTools_Visio.Presentation.Views
{
    public partial class UserDefinedCells : UserControl
    {

        #region Constructors and Load

        public UserDefinedCells()
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            InitializeComponent();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion
    }
}
