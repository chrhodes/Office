namespace VNC.AddinHelper
{
    public class Visio
	{
		private Microsoft.Office.Interop.Visio.Application _VisioApplication;

		public Microsoft.Office.Interop.Visio.Application VisioApplication
		{
			get { return _VisioApplication; }
			set { _VisioApplication = value; }
		}

		private bool _enableScreenUpdatesToggle = true;

		public bool EnableScreenUpdatesToggle
		{
			get { return _enableScreenUpdatesToggle; }
			set { _enableScreenUpdatesToggle = value; }
		}

		public static void DisplayInWatchWindow(string outputLine)
		{
			Common.WriteToWatchWindow(string.Format("{0}", outputLine));
		}
	}
}
