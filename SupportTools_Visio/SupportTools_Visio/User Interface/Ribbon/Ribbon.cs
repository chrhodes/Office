using Microsoft.Office.Tools.Ribbon;

namespace SupportTools_Visio
{
    public partial class Ribbon
    {
        // NOTE(crhodes)
        // This was moved out of designer so we can call bootstrapper.

        public Ribbon()
           : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
            var bootstrapper = new Application.Bootstrapper();
            bootstrapper.Run();
        }

        public static VNC.WPF.Presentation.Views.WindowHost windowHostVNC = null;

        #region Event Handlers

        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        #endregion Event Handlers

    }
}