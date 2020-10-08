using System.Windows.Forms;

namespace SupportTools_Visio.User_Interface.Forms
{
    public partial class frmWPFUserInterface : Form
    {
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private System.Windows.Controls.UserControl wucUserControl;
        

        public frmWPFUserInterface()
        {
            InitializeComponent();
            LoadDefaultXamlUI();
        }

        public frmWPFUserInterface(string wpfUserControlName)
        {
            InitializeComponent();

            // TODO(crhodes)
            // Load a user interface by name.  See below

        }

        void LoadDefaultXamlUI()
        {
            elementHost = new System.Windows.Forms.Integration.ElementHost();
            wucUserControl = new VNC.WPF.Presentation.Views.CylonEyeBall();

            SuspendLayout();

            elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            elementHost.Location = new System.Drawing.Point(0, 0);
            elementHost.Name = "elementHost";
            elementHost.Size = new System.Drawing.Size(431, 637);
            elementHost.TabIndex = 0;
            elementHost.Text = "elementHost";
            elementHost.Child = this.wucUserControl;

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(431, 637);
            Controls.Add(elementHost);
            Name = "frmWPFUserInterface";
            Text = "Visio Commands";

            ResumeLayout(false);
        }
    }
}
