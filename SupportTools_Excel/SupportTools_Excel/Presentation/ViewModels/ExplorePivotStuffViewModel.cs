using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Prism.Commands;
using Prism.Events;

using VNC;
using VNC.Core.Events;
using VNC.Core.Mvvm;
using VNC.Core.Services;

using XL = Microsoft.Office.Interop.Excel;
using XlHlp = VNC.AddinHelper.Excel;

namespace SupportTools_Excel.Presentation.ViewModels
{
    public class ExplorePivotStuffViewModel : EventViewModelBase, IExplorePivotStuffViewModel, IInstanceCountVM
    {

        #region Constructors, Initialization, and Load

        public ExplorePivotStuffViewModel(
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService) : base(eventAggregator, messageDialogService)
        {
            Int64 startTicks = Log.CONSTRUCTOR("Enter", Common.LOG_CATEGORY);

            // TODO(crhodes)
            // Save constructor parameters here

            InitializeViewModel();

            Log.CONSTRUCTOR("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private void InitializeViewModel()
        {
            Int64 startTicks = Log.VIEWMODEL("Enter", Common.LOG_CATEGORY);

            InstanceCountVM++;

            // TODO(crhodes)
            //

            SayHelloCommand = new DelegateCommand(
                SayHello, SayHelloCanExecute);

            AddPivotTablesCommand = new DelegateCommand(AddPivotTables, AddPivotTablesCanExecute);
            AddCountColumnsCommand = new DelegateCommand(AddCountColumns, AddCountColumnsCanExecute);

            Message = "ExplorePivotStuffViewModel says hello";

            Log.VIEWMODEL("Exit", Common.LOG_CATEGORY, startTicks);
        }

        #endregion

        #region Enums


        #endregion

        #region Structures


        #endregion

        #region Fields and Properties

        public ICommand SayHelloCommand { get; private set; }


        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Event Handlers

        #region AddPivotTables Command

        public DelegateCommand AddPivotTablesCommand { get; set; }
        public string AddPivotTablesContent { get; set; } = "AddPivotTables";
        public string AddPivotTablesToolTip { get; set; } = "AddPivotTables ToolTip";

        // Can get fancy and use Resources
        //public string AddPivotTablesContent { get; set; } = "ViewName_AddPivotTablesContent";
        //public string AddPivotTablesToolTip { get; set; } = "ViewName_AddPivotTablesContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_AddPivotTablesContent">AddPivotTables</system:String>
        //    <system:String x:Key="ViewName_AddPivotTablesContentToolTip">AddPivotTables ToolTip</system:String>  

        public void AddPivotTables()
        {
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called AddPivotTables";

            // Uncomment this if you are telling someone else to handle this
            // Common.EventAggregator.GetEvent<AddPivotTablesEvent>().Publish();

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<AddPivotTablesEvent>().Subscribe(AddPivotTables);

            // End Cut Four

            XL.Application app = Globals.ThisAddIn.Application;
            XL.Workbook wb = app.ActiveWorkbook;
            XL.Worksheet ws = app.ActiveSheet;
            XL.Chart ch = app.ActiveChart;
            XL.Range rng = app.ActiveCell;
            XL.ListObjects tables = ws.ListObjects;

            var count = tables.Count;

            // NOTE(crhodes)
            // Hum, index starts at 1

            XL.ListObject table = ws.ListObjects.Item[1];

            string safeSheetName = XlHlp.SafeSheetName($"Pivot_{ws.Name}");
            XL.Worksheet pivotSheet = XlHlp.NewWorksheet(safeSheetName, beforeSheetName: ws.Name);

            XL.PivotCache pc = wb.PivotCaches().Create(XL.XlPivotTableSourceType.xlDatabase, SourceData: table, Version: 7);

            XL.PivotTable pt1 = pc.CreatePivotTable(TableDestination: pivotSheet.Cells[3, 1], TableName: $"Pivot1_{table.Name}", DefaultVersion: 7);

            ((XL.PivotField)pt1.PivotFields("Project")).Orientation = XL.XlPivotFieldOrientation.xlRowField;
            ((XL.PivotField)pt1.PivotFields("Project")).Position = 1;
            XL.PivotField pt1f1 = pt1.PivotFields("Project");

            // NOTE(crhodes)
            // Not clear why this doesn't work
            //pt1.AddDataField(pt1.PivotFields("Project"), "Count of Project", XL.XlConsolidationFunction.xlCount);
            // but this does.
            pt1f1.Orientation = XL.XlPivotFieldOrientation.xlDataField;
            pt1f1.Function = XL.XlConsolidationFunction.xlCount;
            pt1f1.LabelRange.Value = "Count";


            XL.PivotTable pt2 = pc.CreatePivotTable(TableDestination: pivotSheet.Cells[3, 4], TableName: $"Pivot2_{table.Name}", DefaultVersion: 7);

            XL.PivotField pt2f1 = pt2.PivotFields("Type");
            pt2f1.Orientation = XL.XlPivotFieldOrientation.xlRowField;
            pt2f1.Position = 1;

            pt2f1.Orientation = XL.XlPivotFieldOrientation.xlDataField;
            pt2f1.Function = XL.XlConsolidationFunction.xlCount;
            pt2f1.LabelRange.Value = "Count";

            //pt2.AddDataField(pt2f1, "Count", XL.XlConsolidationFunction.xlCount);

            XL.PivotTable pt3 = pc.CreatePivotTable(TableDestination: pivotSheet.Cells[3, 7], TableName: $"Pivot3_{table.Name}", DefaultVersion: 7);

            XL.PivotField pt3f1 = pt3.PivotFields("Type");
            pt3f1.Orientation = XL.XlPivotFieldOrientation.xlRowField;
            pt3f1.Position = 1;

            XL.PivotField pt3f2 = pt3.PivotFields("Project");
            pt3f2.Orientation = XL.XlPivotFieldOrientation.xlRowField;
            pt3f2.Position = 2;

            pt3f2.Orientation = XL.XlPivotFieldOrientation.xlDataField;
            pt3f2.Function = XL.XlConsolidationFunction.xlCount;
            pt3f2.LabelRange.Value = "Count";

            ////pt3.AddDataField(pt3f2, "Count", XL.XlConsolidationFunction.xlCount);

            XL.PivotTable pt4 = pc.CreatePivotTable(TableDestination: pivotSheet.Cells[3, 10], TableName: $"Pivot4_{table.Name}", DefaultVersion: 7);

            XL.PivotField pt4f1 = pt4.PivotFields("Project");
            pt4f1.Orientation = XL.XlPivotFieldOrientation.xlRowField;
            pt4f1.Position = 1;

            XL.PivotField pt4f2 = pt4.PivotFields("Type");
            pt4f2.Orientation = XL.XlPivotFieldOrientation.xlRowField;
            pt4f2.Position = 2;

            pt4f2.Orientation = XL.XlPivotFieldOrientation.xlDataField;
            pt4f2.Function = XL.XlConsolidationFunction.xlCount;
            pt4f2.LabelRange.Value = "Count";

            ////pt4.AddDataField(pt4f2, "Count", XL.XlConsolidationFunction.xlCount);

        }

        public bool AddPivotTablesCanExecute()
        {
            // TODO(crhodes)
            // Add any before button is enabled logic.
            return true;
        }

        #endregion

        #region AddCountColumns Command

        public DelegateCommand AddCountColumnsCommand { get; set; }
        public string AddCountColumnsContent { get; set; } = "AddCountColumns";
        public string AddCountColumnsToolTip { get; set; } = "AddCountColumns ToolTip";

        // Can get fancy and use Resources
        //public string AddCountColumnsContent { get; set; } = "ViewName_AddCountColumnsContent";
        //public string AddCountColumnsToolTip { get; set; } = "ViewName_AddCountColumnsContentToolTip";

        // Put these in Resource File
        //    <system:String x:Key="ViewName_AddCountColumnsContent">AddCountColumns</system:String>
        //    <system:String x:Key="ViewName_AddCountColumnsContentToolTip">AddCountColumns ToolTip</system:String>  

        public void AddCountColumns()
        {
            // TODO(crhodes)
            // Do something amazing.
            Message = "Cool, you called AddCountColumns";

            XL.Application app = Globals.ThisAddIn.Application;
            XL.Workbook wb = app.ActiveWorkbook;
            XL.Worksheet ws = app.ActiveSheet;
            XL.Chart ch = app.ActiveChart;
            XL.Range rng = app.ActiveCell;
            XL.ListObjects tables = ws.ListObjects;

            XL.PivotTable pt1 = ws.PivotTables("Pivot1_Jan2020_Created");
            XL.PivotField pt1f1 = pt1.PivotFields("Project");
            pt1f1.Orientation = XL.XlPivotFieldOrientation.xlDataField;
            pt1f1.Function = XL.XlConsolidationFunction.xlCount;
            pt1f1.LabelRange.Value = "Count";
            //pt1f1.Position = 1;

            //((XL.PivotField)pt1.PivotFields("Project")).Orientation = XL.XlPivotFieldOrientation.xlRowField;
            //((XL.PivotField)pt1.PivotFields("Project")).Position = 1;

            //pt1.AddDataField(pt1f1, "Count of Project", XL.XlConsolidationFunction.xlCount);

            //pt4.AddDataField(pt4f2, "Count", XL.XlConsolidationFunction.xlCount);
            // Uncomment this if you are telling someone else to handle this
            // Common.EventAggregator.GetEvent<AddCountColumnsEvent>().Publish();

            // Start Cut Four - Put this in places that listen for event

            //Common.EventAggregator.GetEvent<AddCountColumnsEvent>().Subscribe(AddCountColumns);

            // End Cut Four

        }

        public bool AddCountColumnsCanExecute()
    {
        // TODO(crhodes)
        // Add any before button is enabled logic.
        return true;
    }

    #endregion

    #endregion

    #region Public Methods


    #endregion

    #region Protected Methods


    #endregion

    #region Private Methods

    #region SayHello Command

    private void SayHello()
        {
            Int64 startTicks = Log.EVENT_HANDLER("Enter", Common.LOG_CATEGORY);

            XL.Application app = Globals.ThisAddIn.Application;
            XL.Workbook wb = app.ActiveWorkbook;
            XL.Worksheet ws = app.ActiveSheet;
            XL.Chart ch = app.ActiveChart;
            XL.Range rng = app.ActiveCell;

            Message = $"Hello from {this.GetType()}";

            Log.Trace($"Workbook.PivotCaches: ({wb.PivotCaches().Count})", Common.LOG_CATEGORY);

            foreach (XL.PivotCache item in wb.PivotCaches())
            {
                Log.Trace($"RefreshedDate: ({item.RefreshDate}) RefreshedBy: ({item.RefreshName}) SourceType: ({item.SourceType})", Common.LOG_CATEGORY);
            }

            Log.Trace($"Workbook.PivotTables: ({wb.PivotTables.Count})", Common.LOG_CATEGORY);

            foreach (XL.PivotTable item in wb.PivotTables)
            {
                Log.Trace($"RefreshedDate: ({item.RefreshDate}) RefreshedBy: ({item.RefreshName})", Common.LOG_CATEGORY);
            }

            Log.Trace($"WorkSheet.PivotTables: ({ws.PivotTables().Count})", Common.LOG_CATEGORY);

            foreach (XL.PivotTable item in ws.PivotTables())
            {
                Log.Trace($"Name: ({item.Name})  RefreshedDate: ({item.RefreshDate}) RefreshedBy: ({item.RefreshName})", Common.LOG_CATEGORY);

                foreach (XL.PivotField fld in item.PivotFields())
                {
                    Log.Trace($"   PF Name: ({fld.Name})  Position: ({fld.Position}) Orientation: ({fld.Orientation})", Common.LOG_CATEGORY);
                }

                foreach (XL.PivotField fld in item.RowFields)
                {
                    Log.Trace($"   RF Name: ({fld.Name})  Position: ({fld.Position}) Orientation: ({fld.Orientation})", Common.LOG_CATEGORY);
                }

                foreach (XL.PivotField fld in item.DataFields)
                {
                    Log.Trace($"   DF Name: ({fld.Name})  Position: ({fld.Position}) Orientation: ({fld.Orientation})", Common.LOG_CATEGORY);
                }
            }

            Log.Trace($"Workbook.Charts: ({wb.Charts.Count})", Common.LOG_CATEGORY);

            foreach (var item in wb.Charts)
            {
                Log.Trace($"({item.GetType()})", Common.LOG_CATEGORY);
            }

            Log.Trace($"WorkSheet.ChartObjects: ({ws.ChartObjects().Count})", Common.LOG_CATEGORY);

            foreach (var item in ws.ChartObjects())
            {
                Log.Trace($"({item.GetType()})", Common.LOG_CATEGORY);
            }

            Log.Trace($"WorkSheet.QueryTables: ({ws.QueryTables.Count})", Common.LOG_CATEGORY);

            foreach (XL.QueryTable item in ws.QueryTables)
            {
                Log.Trace($"({item.Name})", Common.LOG_CATEGORY);
            }

            Log.Trace($"WorkSheet.ListObjects: ({ws.ListObjects.Count})", Common.LOG_CATEGORY);

            foreach (XL.ListObject item in ws.ListObjects)
            {
                Log.Trace($"({item.Name})", Common.LOG_CATEGORY);
            }

            Log.EVENT_HANDLER("Exit", Common.LOG_CATEGORY, startTicks);
        }

        private bool SayHelloCanExecute()
        {
            return true;
        }

        #endregion

        #endregion

        #region IInstanceCount

        private static int _instanceCountVM;

        public int InstanceCountVM
        {
            get => _instanceCountVM;
            set => _instanceCountVM = value;
        }

        #endregion
    }
}
