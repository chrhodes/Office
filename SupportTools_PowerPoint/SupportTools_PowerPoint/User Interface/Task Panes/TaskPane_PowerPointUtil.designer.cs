﻿namespace SupportTools_PowerPoint.User_Interface.Task_Panes
{
    partial class TaskPane_PowerPointUtil
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.wucTaskPane_PowerPointUtil1 = new SupportTools_PowerPoint.User_Interface.User_Controls.wucTaskPane_PowerPointUtil();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(384, 150);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.wucTaskPane_PowerPointUtil1;
            // 
            // TaskPane_PowerPointUtil
            // 
            this.Controls.Add(this.elementHost1);
            this.Name = "TaskPane_PowerPointUtil";
            this.Size = new System.Drawing.Size(384, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private User_Controls.wucTaskPane_PowerPointUtil wucTaskPane_PowerPointUtil1;
        //private System.Windows.Forms.Integration.ElementHost elementHost1;
    }
}
