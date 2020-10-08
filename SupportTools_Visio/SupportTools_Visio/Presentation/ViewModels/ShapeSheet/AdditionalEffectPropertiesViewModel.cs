
using Prism.Commands;

using SupportTools_Visio.Actions;
using SupportTools_Visio.Presentation.ModelWrappers;

using VNC;

using Visio = Microsoft.Office.Interop.Visio;

namespace SupportTools_Visio.Presentation.ViewModels
{
    public class AdditionalEffectPropertiesViewModel : ShapeSheetSectionBase //, IAdditionalEffectPropertiesViewModel
    {
        public AdditionalEffectPropertiesWrapper AdditionalEffectProperties { get; set; }


        public AdditionalEffectPropertiesViewModel() : base()
        {
            UpdateButtonContent = "Update AdditionalEffectProperties for selected shapes";
        }

        public override void OnUpdateSettingsExecute()
        {
            Log.Trace("Enter", Common.PROJECT_NAME);
            // Wrap a big, OMG, what have I done ???, undo around the whole thing !!!

            int undoScope = Globals.ThisAddIn.Application.BeginUndoScope("UpdateAdditionalEffectProperties");

            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                Visio_Shape.Set_AdditionalEffectProperties_Section(shape, AdditionalEffectProperties.Model);
            }

            Globals.ThisAddIn.Application.EndUndoScope(undoScope, true);
            Log.Trace("Exit", Common.PROJECT_NAME);
        }

        public override void OnLoadCurrentSettingsExecute()
        {
            Visio.Application app = Globals.ThisAddIn.Application;

            Visio.Selection selection = app.ActiveWindow.Selection;

            foreach (Visio.Shape shape in selection)
            {
                AdditionalEffectProperties = new AdditionalEffectPropertiesWrapper(Visio_Shape.Get_AdditionalEffectProperties(shape));
                OnPropertyChanged("AdditionalEffectProperties");
            }
        }
    }
}
