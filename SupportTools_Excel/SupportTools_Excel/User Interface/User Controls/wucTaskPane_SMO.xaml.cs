
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Data.Mask;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Windows;

using System.Windows.Controls;
using VNC.AddinHelper;
using SMO = Microsoft.SqlServer.Management.Smo;

using SMOH = VNC.SMOHelper;
using XlHlp = VNC.AddinHelper.Excel;

namespace SupportTools_Excel.User_Interface.User_Controls
{
    /// <summary>
    /// Interaction logic for wucTaskPane_SMO.xaml
    /// </summary>
    public partial class wucTaskPane_SMO : UserControl
    {
        #region Fields and Properties

        SMO.Server _SMOServer;      // This is the real one
        SMOH.Server _SMOHServer;    // This is the one that Hides the access restrictions
                                    // by catching not found exceptions

        char[] splitSemicolonChar = { ';' };
        char[] splitPipeChar = { '|' };
        char[] splitSpaceChar = { ' ' };

        #endregion

        #region Constructors and Load

        public wucTaskPane_SMO()
        {
            InitializeComponent();
            LoadControlContents();
        }

        private void LoadControlContents()
        {
            try
            {
                wucSQLInstance_Picker1.PopulateControlFromFile(Common.cCONFIG_FILE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            wucSQLInstance_Picker1.ControlChanged += wucSQLInstance_Picker1_ControlChanged;
        }

        private void wucSQLInstance_Picker1_ControlChanged()
        {
            VNC.AddinHelper.Common.WriteToDebugWindow("wucSQLInstance_Picker1.ControlChanged");
            // TODO(crhodes)
            // Need to rebind the combobox
        }

        #endregion

        #region Event Handlers

        private void btnCreateDatabaseInfoWorkSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();

                foreach (string databaseName in cbeDatabases.Text.Split(splitSemicolonChar, StringSplitOptions.None))
                {
                    SMOH.Database dataBase = _SMOHServer.Databases[databaseName];
                    CreateWS_DatabaseInfo(dataBase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);
            }
        }

        //private void btnCreateDatabaseInfoWorkSheets_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        XlHlp.ScreenUpdatesOff();
        //        CreateAllWorksheetsOf_DatabaseInfo(_SMOHServer);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    finally
        //    {
        //        XlHlp.ScreenUpdatesOn(true);
        //    }
        //}

        private void btnCreateInstanceInfoWorkSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                CreateWS_InstanceInfo(_SMOHServer, (bool)ceListInstanceDetails.IsChecked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);
            }
        }

        private void btnCreateStoredProcedureInfoWorkSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                //SMOH.Database dataBase = _SMOHServer.Databases[cbeDatabases.Text];
                //SMOH.StoredProcedure storedProcedure = dataBase.StoredProcedures[cbeStoredProcedures.Text];
                //CreateWS_StoredProcedureInfo(storedProcedure);

                foreach (string databaseName in cbeDatabases.Text.Split(splitSemicolonChar, StringSplitOptions.None))
                {
                    SMOH.Database database = _SMOHServer.Databases[databaseName];
                    SMOH.StoredProcedure storedProcedure = database.StoredProcedures[cbeStoredProcedures.Text];
                    CreateWS_StoredProcedureInfo(storedProcedure);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);
            }
        }

        //private void btnCreateStoredProcedureInfoWorkSheets_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        XlHlp.ScreenUpdatesOff();
        //        //SMOH.Database dataBase = _SMOHServer.Databases[cbeDatabases.Text];
        //        //CreateAllWorksheeetsOf_StoredProcedureInfo(dataBase);

        //        foreach (string databaseName in cbeDatabases.Text.Split(splitSemicolonChar, StringSplitOptions.None))
        //        {
        //            SMOH.Database database = _SMOHServer.Databases[databaseName];
        //            CreateAllWorksheeetsOf_StoredProcedureInfo(database);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    finally
        //    {
        //        XlHlp.ScreenUpdatesOn(true);
        //    }
        //}
        private void btnCreateTableInfoMasterWorkSheet_Click(object sender, RoutedEventArgs e)
        {
            if(! ValidUISelections()) { return; }

            bool orientVertical = GetDisplayOrientation();

            try
            {
                XlHlp.ScreenUpdatesOff();
                
                // This method knows how to handle multiple names, e.g. AML;EASE;FOO;BAR
                CreateWS_TableInfoMaster(cbeDatabases.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);
            }
        }

        private void btnCreateTableInfoWorkSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();

                //SMOH.Database dataBase = _SMOHServer.Databases[cbeDatabases.Text];
                //SMOH.Table table = dataBase.Tables[cbeTables.Text];
                //CreateWS_TableInfo(table);

                foreach (string databaseName in cbeDatabases.Text.Split(splitSemicolonChar, StringSplitOptions.None))
                {
                    SMOH.Database database = _SMOHServer.Databases[databaseName];

                    foreach (string tableName in cbeTables.Text.Split(splitSemicolonChar, StringSplitOptions.None))
                    {
                        string[] values = tableName.Split(splitSpaceChar, StringSplitOptions.None);

                        if (databaseName == values[1])
                        {
                            SMOH.Table table = database.Tables[values[0]];
                            CreateWS_TableInfo(table, database.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);
            }
        }

        //private void btnCreateTableInfoWorkSheets_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        XlHlp.ScreenUpdatesOff();

        //        //SMOH.Database dataBase = _SMOHServer.Databases[cbeDatabases.Text];
        //        //CreateAllWorkSheetsOf_TableInfo(dataBase);

        //        foreach (string databaseName in cbeDatabases.Text.Split(splitSemicolonChar, StringSplitOptions.None))
        //        {
        //            SMOH.Database database = _SMOHServer.Databases[databaseName];

        //            CreateAllWorkSheetsOf_TableInfo(database);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    finally
        //    {
        //        XlHlp.ScreenUpdatesOn(true);
        //    }
        //}

        private void btnCreateViewInfoWorkSheet_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLoadDatabaseContentComboBoxes_Click(object sender, RoutedEventArgs e)
        {
            foreach (string databaseName in cbeDatabases.Text.Split(splitSemicolonChar, StringSplitOptions.None))
            {
                if (databaseName == "")
                {
                    return;
                }

                SMOH.Database dataBase = _SMOHServer.Databases[databaseName];

                if ((bool)ceIncludeDBTables.IsChecked)
                {
                    UpdateDBTablesComboBox(databaseName);
                }

                if ((bool)ceIncludeDBViews.IsChecked)
                {
                    UpdateDBViewsComboBox(databaseName);
                }

                if ((bool)ceIncludeDBStoredProcedures.IsChecked)
                {
                    UpdateDBStoredProceduresComboBox(databaseName);
                }
            }

            // We may have added more than one database into the combobox
            // Sort so the Table Names are next to each other.

            List<string> items = new List<string>();

            foreach (var item in cbeTables.Items)
            {
                items.Add(item.ToString());
            }

            items.Sort();

            cbeTables.ItemsSource = items;
        }

        private void btnLogoff_Click(object sender, RoutedEventArgs e)
        {
            Logoff();
            //btnLogon.Enabled = true;
            //btnLogon.BackColor = SystemColors.Control;
            //btnLogoff.Enabled = false;
            //btnLogoff.BackColor = SystemColors.Control;
            //lblInstancName.Text = "";
            //gbInstanceOperations.Visible = false;
        }

        private void btnLogon_Click(object sender, RoutedEventArgs e)
        {
            if (Logon() == true)
            {
                //btnLogoff.Enabled = true;
                //btnLogoff.BackColor = Color.Green;
                //btnLogon.Enabled = false;
                //btnLogon.BackColor = Color.Green;
                //lblInstancName.Text = ucDBInstanceList.InstanceName;
                //gbInstanceOperations.Visible = true;
            }
        }


        private void cbeDatabases_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //foreach (string databaseName in cbeDatabases.Text.Split(splitSemicolonChar, StringSplitOptions.None))
            //{
            //    if (databaseName == "")
            //    {
            //        return;
            //    }

            //    SMOH.Database dataBase = _SMOHServer.Databases[databaseName];

            //    if ((bool)ceIncludeDBTables.IsChecked)
            //    {
            //        UpdateDBTablesComboBox(databaseName);
            //    }

            //    if ((bool)ceIncludeDBViews.IsChecked)
            //    {
            //        UpdateDBViewsComboBox(databaseName);
            //    }

            //    if ((bool)ceIncludeDBStoredProcedures.IsChecked)
            //    {
            //        UpdateDBStoredProceduresComboBox(databaseName);
            //    }
            //}

            //List<string> items = new List<string>();

            //foreach (var item in cbeTables.Items)
            //{
            //    items.Add(item.ToString());
            //}

            //items.Sort();

            //cbeTables.ItemsSource = items;
        
        }

        //private static List<RoleNameID> Sort(List<RoleNameID> list)
        //{
        //    return list.OrderBy(role => role.Name).ToList();
        //    DevExpress.Xpf.Editors.ListItemCollection items = cbeTables.Items;
        //}

        private void UpdateDBStoredProceduresComboBox(string databaseName)
        {
            XlHlp.DisplayInWatchWindow(string.Format("  {0}", "Adding StoredProcedures to combobox ..."));

            // NB.  We need to use the StoredProcedure.Values here as we want to be able to filter out
            // System Stored Procedures

            foreach (SMOH.StoredProcedure sp in _SMOHServer.Databases[databaseName].StoredProcedures.Values)
            {
                if (sp.IsSystemObject == "1" && !(bool)ceIncludeSystemStoredProcedures.IsChecked)
                {
                    continue;
                }

                cbeStoredProcedures.Items.Add(string.Format("{0} {1}", sp.Name, databaseName));
                XlHlp.DisplayInWatchWindow(string.Format("  - {0}", sp.Name));
            }
        }

        private void UpdateDBTablesComboBox(string databaseName)
        {
            XlHlp.DisplayInWatchWindow(string.Format("  {0}", "Adding Tables to combobox ..."));

            foreach (string tableName in _SMOHServer.Databases[databaseName].Tables.Keys)
            {
                cbeTables.Items.Add(string.Format("{0} {1}", tableName, databaseName));
                XlHlp.DisplayInWatchWindow(string.Format("  - {0}", tableName));
            }
        }

        private void UpdateDBViewsComboBox(string databaseName)
        {
            XlHlp.DisplayInWatchWindow(string.Format("  {0}", "Adding Views to combobox ..."));

            // NB.  We need to use the Views.Values here as we want to be able to filter out
            // System Views

            foreach (SMOH.View view in _SMOHServer.Databases[databaseName].Views.Values)
            {
                if (view.IsSystemObject && !(bool)ceIncludeSystemStoredProcedures.IsChecked)
                {
                    continue;
                }

                cbeViews.Items.Add(string.Format("{0} {1}", view.Name, databaseName));
                XlHlp.DisplayInWatchWindow(string.Format("  - {0}", view.Name));
            }
        }

        #endregion

        #region Main Function Routines

        #region CreateWS_*

        private void CreateAllWorksheetsOf_DatabaseInfo(SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (SMOH.Database database in serverInstance.Databases.Values)
            {
                try
                {
                    CreateWS_DatabaseInfo(database);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CreateAllWorksheeetsOf_StoredProcedureInfo(SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (SMOH.StoredProcedure storedProcedure in dataBase.StoredProcedures.Values)
            {
                try
                {
                    CreateWS_StoredProcedureInfo(storedProcedure);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CreateAllWorkSheetsOf_TableInfo(SMOH.Database database)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (SMOH.Table table in database.Tables.Values)
            {
                try
                {
                    CreateWS_TableInfo(table, database.Name);
                    XlHlp.DisplayInWatchWindow(string.Format("  - Table >{0}<", table));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CreateAllWorksheetsOf_ViewInfo(SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (SMOH.View view in dataBase.Views.Values)
            {
                try
                {
                    CreateWS_ViewInfo(view);
                    XlHlp.DisplayInWatchWindow(string.Format("  - View >{0}<", view));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CreateWS_DatabaseInfo(SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            long startTicks = Common.WriteToDebugWindow("CreateDatabaseInfoWorkSheet(Start)");

            string sheetName = XlHlp.SafeSheetName("D>" + dataBase.Name);
            Microsoft.Office.Interop.Excel.Worksheet ws = XlHlp.NewWorksheet(sheetName, afterSheetName: "LAST");

            // Output starts here.  Each Display method returns the output end point.

            XlHlp.XlLocation insertAt = new XlHlp.XlLocation(ws, row: 5, column: 1, orientVertical: true);

            insertAt = AddSection_DatabaseInfo(insertAt, dataBase);

            insertAt = AddSection_ExtendedPropertyInfo(insertAt, dataBase.ExtendedProperties);

            insertAt = AddSection_FileGroupInfo(insertAt, dataBase);

            if ((bool)ceIncludeDBTables.IsChecked)
            {
                insertAt.IncrementRows(2);
                insertAt = AddSection_TableInfo(insertAt, dataBase);
            }

            if ((bool)ceIncludeDBViews.IsChecked)
            {
                insertAt.IncrementRows(2);
                insertAt = AddSection_ViewInfo(insertAt, dataBase);
            }

            if ((bool)ceIncludeDBStoredProcedures.IsChecked)
            {
                insertAt.IncrementRows(2);
                insertAt = AddSection_StoredProcedure(insertAt, dataBase);
            }

            Common.WriteToDebugWindow("CreateDatabaseInfoWorkSheet(Start)", startTicks);
        }

        private void CreateWS_InstanceInfo(SMOH.Server serverInstance, bool showDetails)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            long startTicks = Common.WriteToDebugWindow("CreateInstanceInfoWorksheet(Start)");

            string sheetName = XlHlp.SafeSheetName("I>" + serverInstance.Name);
           Microsoft.Office.Interop.Excel.Worksheet ws = XlHlp.NewWorksheet(sheetName, beforeSheetName: "FIRST");

            // Output starts here.  Each Display method returns the output end point.

            XlHlp.XlLocation insertAt = new XlHlp.XlLocation(ws, row: 5, column: 1, orientVertical: true);

            insertAt = AddSection_InstanceInfo(insertAt, serverInstance, showDetails);

            insertAt.IncrementRows(2);

            insertAt = AddSection_Databases(insertAt, serverInstance);

            insertAt.IncrementRows(2);

            insertAt = AddSection_Logins(insertAt, serverInstance);

            insertAt.IncrementRows(2);

            insertAt = AddSection_ServerRoles(insertAt, serverInstance);

            insertAt.IncrementRows(2);

            insertAt = AddSection_LinkedServers(insertAt, serverInstance);

            insertAt.IncrementRows(2);

            insertAt = AddSection_EndPoints(insertAt, serverInstance);

            insertAt.IncrementRows(2);

            Common.WriteToDebugWindow("CreateInstanceInfoWorksheet(End)", startTicks);
        }

        private void CreateWS_StoredProcedureInfo(SMOH.StoredProcedure storedProcedure)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            long startTicks = Common.WriteToDebugWindow("CreateStoredProcedureInfoWorksheet(Start)");

            string sheetName = XlHlp.SafeSheetName("S>" + storedProcedure.Name);
            Microsoft.Office.Interop.Excel.Worksheet ws = XlHlp.NewWorksheet(sheetName, afterSheetName: "LAST");
            int fontSize = 8;

            XlHlp.XlLocation insertAt = new XlHlp.XlLocation(ws, row: 5, column: 1, orientVertical: true);

            XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "As of:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DB Name:", storedProcedure.Name);

            XlHlp.AddContentToCell(insertAt.AddRowX(), "Parameters");

            insertAt.ClearOffsets();

            XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "Name");
            XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 20, "DataType");
            XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Maximum\nLength");
            XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Numeric\nPrecision");
            XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Numeric\nScale");
            XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Default\nValue");

            insertAt = DisplayListOf_StoredProcedureParameters(insertAt, storedProcedure);

            insertAt.IncrementRows();

            insertAt.ClearOffsets();

            XlHlp.AddLabeledInfoX(insertAt.AddOffsetColumnX(), "Header:", "");
            XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), storedProcedure.TextHeader);

            insertAt.ClearOffsets();

            XlHlp.AddLabeledInfoX(insertAt.AddOffsetColumnX(), "Body:", "");
            XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), storedProcedure.TextBody);

            Common.WriteToDebugWindow("CreateStoredProcedureInfoWorksheet(End)", startTicks);
        }

        private void CreateWS_TableInfo(SMOH.Table table, string databaseName)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            long startTicks = Common.WriteToDebugWindow("CreateTableInfoWorkSheet(Start)");

            // This exceeds the Max Worksheet name when we prepend databaseName :(

            //string sheetName = XlHlp.SafeSheetName(string.Format("{0} T>{1}", databaseName, table.Name));
            string sheetName = XlHlp.SafeSheetName("T>" + table.Name);
            Microsoft.Office.Interop.Excel.Worksheet ws = XlHlp.NewWorksheet(sheetName, afterSheetName: "LAST");

            XlHlp.XlLocation insertAt = new XlHlp.XlLocation(ws, row: 5, column: 1, orientVertical: true);

            XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "As of:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Name:", table.Name);

            insertAt = AddSection_ExtendedPropertyInfo(insertAt, table.ExtendedProperties);

            insertAt.MarkStart(XlHlp.MarkType.GroupTable);

            AddSection_ColumnInfo(insertAt, table);

            //insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tbl_{0}", table.Name));

            insertAt.Group(insertAt.OrientVertical, hide: true);

            insertAt.EndSectionAndSetNextLocation(insertAt.OrientVertical);

            Common.WriteToDebugWindow("CreateTableInfoWorkSheet(End)", startTicks);
        }

        private void CreateWS_TableInfoMaster(string databaseNames)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0} ({1})",
                System.Reflection.MethodInfo.GetCurrentMethod().Name, databaseNames));

            try
            {


                string sheetName = XlHlp.SafeSheetName(string.Format("{0}{1}", "MS>", "NeedGoodName"));
                Microsoft.Office.Interop.Excel.Worksheet ws = XlHlp.NewWorksheet(sheetName, beforeSheetName: "FIRST");

                XlHlp.XlLocation insertAt = new XlHlp.XlLocation(ws, row: 5, column: 1, orientVertical: true);

                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Table Column Information for ", databaseNames);

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "Database");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "Table");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "Column");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "DataType");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "MaximumLength");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "NumericPrecision");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "NumericScale");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "IsPrimaryKey");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "IsForeignKey");
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), "Nullable");

                insertAt.IncrementRows();

                if (0 == cbeTables.Text.Length)
                {
                    insertAt.ClearOffsets();
                    XlHlp.DisplayInWatchWindow(string.Format("No tables selected, aborting."));
                    XlHlp.AddContentToCell(insertAt.GetCurrentRange(), "No table selected, aborting.  Check cbeTables");

                    return;
                }

                foreach (string databaseName in cbeDatabases.Text.Split(splitSemicolonChar, StringSplitOptions.None))
                {
                    SMOH.Database database = _SMOHServer.Databases[databaseName];

                    foreach (string tableName in cbeTables.Text.Split(splitSemicolonChar, StringSplitOptions.None))
                    {
                        // TODO(crhodes)
                        // Need to make this smart enough to split out db;tablename

                        string[] values = tableName.Split(splitSpaceChar, StringSplitOptions.None);

                        if (databaseName == values[1])
                        {
                            insertAt = AddSection_Table_Column_Info(insertAt, database, values[0]);
                        }
                    }
                }

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblMasterInfo_{0}", databaseNames.Replace(";", "")));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.EndSectionAndSetNextLocation(insertAt.OrientVertical);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CreateWS_ViewInfo(SMOH.View view)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            long startTicks = Common.WriteToDebugWindow("CreateViewInfoWorkSheet(Start)");

            string sheetName = XlHlp.SafeSheetName("V>" + view.Name);
            Microsoft.Office.Interop.Excel.Worksheet ws = XlHlp.NewWorksheet(sheetName, afterSheetName: "LAST");

            XlHlp.XlLocation insertAt = new XlHlp.XlLocation(ws, row: 5, column: 1, orientVertical: true);

            insertAt.MarkStart(XlHlp.MarkType.GroupTable);

            XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "As of:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Name:", view.Name);

            insertAt = AddSection_ExtendedPropertyInfo(insertAt, view.ExtendedProperties);

            AddSection_ColumnInfo(insertAt, view);

            Common.WriteToDebugWindow("CreateViewInfoWorkSheet(End)", startTicks);
        }
   
        #endregion

        #region AddSection_*
        
        private XlHlp.XlLocation AddSection_ColumnInfo(XlHlp.XlLocation insertAt, SMOH.Table table)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "Columns");

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 20f, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 7.8f, "DataType");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 7.8f, "Maximum\nLength");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 7f, "Numeric\nPrecision");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 7f, "Numeric\nScale");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 7f, "Default");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 4f, "ID");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 8f, "Identity");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 11f, "Is\nPrimaryKey");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 11f, "Is\nForeignKey");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 7f, "Nullable");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_Columns(insertAt, table);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblTableColumns_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_ColumnInfo(XlHlp.XlLocation insertAt, SMOH.View view)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "Columns");

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 35, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "DataType");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Maximum\nLength");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Numeric\nPrecision");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Numeric\nScale");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Default");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(),  5, "ID");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 12, "Identity");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 12, "Is\nPrimaryKey");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 12, "Is\nForeignKey");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 12, "Nullable");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_Columns(insertAt, view);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblViewColummns_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_DatabaseInfo(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "As of:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DB Name:", dataBase.Name);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Instance:", dataBase.Parent);

                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ActiveConnections:", dataBase.ActiveConnections);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AnsiNullDefault:", dataBase.AnsiNullDefault);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AnsiNullsEnabled:", dataBase.AnsiNullsEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AnsiPaddingEnabled:", dataBase.AnsiPaddingEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AnsiWarningsEnabled:", dataBase.AnsiWarningsEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AutoClose:", dataBase.AutoClose);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AutoCreateStatisticsEnabled:", dataBase.AutoCreateStatisticsEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AutoShrink:", dataBase.AutoShrink);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AutoUpdateStatisticsAsync:", dataBase.AutoUpdateStatisticsAsync);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "AutoUpdateStatisticsEnabled:", dataBase.AutoUpdateStatisticsEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "BrokerEnabled:", dataBase.BrokerEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "CaseSensitive:", dataBase.CaseSensitive);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ChangeTrackingAutoCleanUp:", dataBase.ChangeTrackingAutoCleanUp);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ChangeTrackingEnabled:", dataBase.ChangeTrackingEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ChangeTrackingRetentionPeriod:", dataBase.ChangeTrackingRetentionPeriod);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ChangeTrackingRetentionPeriodUnits:", dataBase.ChangeTrackingRetentionPeriodUnits);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "CloseCursorsOnCommitEnabled:", dataBase.CloseCursorsOnCommitEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Collation:", dataBase.Collation);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "CompatibilityLevel:", dataBase.CompatibilityLevel);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ConcatenateNullYieldsNull:", dataBase.ConcatenateNullYieldsNull);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "CreateDate:", dataBase.CreateDate);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DatabaseGuid:", dataBase.DatabaseGuid);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DatabaseSnapshotBaseName:", dataBase.DatabaseSnapshotBaseName);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DataSpaceUsage:", dataBase.DataSpaceUsage);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DateCorrelationOptimization:", dataBase.DateCorrelationOptimization);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DboLogin:", dataBase.DboLogin);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DefaultFileGroup:", dataBase.DefaultFileGroup);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DefaultFileStreamFileGroup:", dataBase.DefaultFileStreamFileGroup);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DefaultFullTextCatalog:", dataBase.DefaultFullTextCatalog);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DefaultSchema:", dataBase.DefaultSchema);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "HonorBrokerPriority:", dataBase.HonorBrokerPriority);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ID:", dataBase.ID);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IndexSpaceUsage:", dataBase.IndexSpaceUsage);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsAccessible:", dataBase.IsAccessible);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDatabaseSnapshot:", dataBase.IsDatabaseSnapshot);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDatabaseSnapshotBase:", dataBase.IsDatabaseSnapshotBase);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbAccessAdmin:", dataBase.IsDbAccessAdmin);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbBackupOperator:", dataBase.IsDbBackupOperator);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbDatareader:", dataBase.IsDbDatareader);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbDatawriter:", dataBase.IsDbDatawriter);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbDdlAdmin:", dataBase.IsDbDdlAdmin);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbDenyDatareader:", dataBase.IsDbDenyDatareader);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbDenyDatawriter:", dataBase.IsDbDenyDatawriter);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbOwner:", dataBase.IsDbOwner);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsDbSecurityAdmin:", dataBase.IsDbSecurityAdmin);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsFullTextEnabled:", dataBase.IsFullTextEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsMailHost:", dataBase.IsMailHost);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsManagementDataWarehouse:", dataBase.IsManagementDataWarehouse);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsMirroringEnabled:", dataBase.IsMirroringEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsParameterizationForced:", dataBase.IsParameterizationForced);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsReadCommittedSnapshotOn:", dataBase.IsReadCommittedSnapshotOn);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsSystemObject:", dataBase.IsSystemObject);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsUpdateable:", dataBase.IsUpdateable);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsVarDecimalStorageFormatEnabled:", dataBase.IsVarDecimalStorageFormatEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "LastBackupDate:", dataBase.LastBackupDate);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "LastDifferentialBackupDate:", dataBase.LastDifferentialBackupDate);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "LastLogBackupDate:", dataBase.LastLogBackupDate);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "LocalCursorsDefault:", dataBase.LocalCursorsDefault);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "LogReuseWaitStatus:", dataBase.LogReuseWaitStatus);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringFailoverLogSequenceNumber:", dataBase.MirroringFailoverLogSequenceNumber);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringID:", dataBase.MirroringID);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringPartner:", dataBase.IsParameterizationForced);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringRedoQueueMaxSize:", dataBase.MirroringRedoQueueMaxSize);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringRoleSequence:", dataBase.MirroringRoleSequence);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringSafetyLevel:", dataBase.MirroringSafetyLevel);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringSafetySequence:", dataBase.MirroringSafetySequence);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringStatus:", dataBase.MirroringStatus);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringTimeout:", dataBase.MirroringTimeout);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MirroringWitness:", dataBase.MirroringWitness);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "NumericRoundAbortEnabled:", dataBase.NumericRoundAbortEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Owner:", dataBase.Owner);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "PageVerify:", dataBase.PageVerify);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "PrimaryFilePath:", dataBase.PrimaryFilePath);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "QuotedIdentifiersEnabled:", dataBase.QuotedIdentifiersEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ReadOnly:", dataBase.ReadOnly);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "RecoveryForkGuid:", dataBase.RecoveryForkGuid);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "RecoveryModel:", dataBase.RecoveryModel);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "RecursiveTriggersEnabled:", dataBase.RecursiveTriggersEnabled);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Size:", dataBase.Size);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "SnapshotIsolationState:", dataBase.SnapshotIsolationState);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "SpaceAvailable:", dataBase.SpaceAvailable);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "State:", dataBase.State);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Status:", dataBase.Status);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "UserName:", dataBase.UserName);
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Version:", dataBase.Version);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_Databases(XlHlp.XlLocation insertAt, SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "Databases");

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 20, "Owner");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "Create\nDate");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "Size");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "DataSpace\nUsage");

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "Tables");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "Views");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "StoredProcedures");
                //XlHlp.AddColumnToSheet(ref ws, col++, 5,  row, "ID");
                //XlHlp.AddColumnToSheet(ref ws, col++, 35, row, "DatabaseGuid");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_DataBases(insertAt, serverInstance);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblDatabases_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }


        private XlHlp.XlLocation AddSection_DataFileInfo(XlHlp.XlLocation insertAt, SMOH.FileGroup fileGroup)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "DataFiles");
                insertAt.IncrementRows();

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "FileName");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "AvailableSpace");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "Growth");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "GrowthType");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "MaxSize");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 13, "NumberOf\nDiskReads");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 13, "NumberOf\nDiskWrites");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Size");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "State");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 11, "UsedSpace");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Volume\nFreeSpace");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_DataFiles(insertAt, fileGroup);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblDataFile_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_EndPoints(XlHlp.XlLocation insertAt, SMOH.Server server)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "Endpoints");
                insertAt.IncrementRows();

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "EndpointState");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "EndpointType");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "ID");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "IsAdminEndpoint");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "IsSystemObject");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 13, "Owner");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 13, "Protocol");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "ProtocolType");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Urn");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_Endpoints(insertAt, server);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblEndPoints_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }
        private XlHlp.XlLocation AddSection_ExtendedPropertyInfo(XlHlp.XlLocation insertAt, SMO.ExtendedPropertyCollection extendedProperties)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ExtendedProperties:", extendedProperties.Count.ToString());

                insertAt.IncrementRows();

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Value");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_ExtendedProperties(insertAt, extendedProperties);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblExtendedPropertyInfo_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_FileGroupInfo(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "FileGroups:", dataBase.FileGroups.Count.ToString());

                //XlHlp.AddContentToCell(ws.Cells[currentRow++, 1], "FileGroups", 14, XlHlp.MakeBold.Yes);
                insertAt.IncrementRows();

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "IsDefault");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_FileGroups(insertAt, dataBase);

                //insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblFileGroupInfo_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_InstanceInfo(XlHlp.XlLocation insertAt, SMOH.Server serverInstance, bool showDetail)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "As of:", DateTime.Now.ToString());
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Instance Name:", serverInstance.Name);

                if (showDetail)
                {
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Backup\nDirectory:", serverInstance.BackupDirectory);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Browser\nService Account:", serverInstance.BrowserServiceAccount);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Browser\nStartMode:", serverInstance.BrowserStartMode);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Build\nClrVersion:", serverInstance.BuildClrVersionString);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "BuildNumber:", serverInstance.BuildNumber);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Collation:", serverInstance.Collation);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ComparisonStyle:", serverInstance.ComparisonStyle);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ComputerNamePhysicalNetBIOS:", serverInstance.ComputerNamePhysicalNetBIOS);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DefaultFile:", serverInstance.DefaultFile);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "DefaultLog:", serverInstance.DefaultLog);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Edition:", serverInstance.Edition);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ErrorLogPath:", serverInstance.ErrorLogPath);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "FilestreamLevel:", serverInstance.FilestreamLevel);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "FilestreamShareName:", serverInstance.FilestreamShareName);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "InstallDataDirectory:", serverInstance.InstallDataDirectory);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "InstallSharedDirectory:", serverInstance.InstallSharedDirectory);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "InstanceName:", serverInstance.InstanceName);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsCaseSensitive:", serverInstance.IsCaseSensitive);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsClustered:", serverInstance.IsClustered);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsFullTextInstalled:", serverInstance.IsFullTextInstalled);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "IsSingleUser:", serverInstance.IsSingleUser);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "LoginMode:", serverInstance.LoginMode);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MasterDBPath:", serverInstance.MasterDBPath);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MasterDBLogPath:", serverInstance.MasterDBLogPath);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "MaxPrecision:", serverInstance.MaxPrecision);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "NamedPipesEnabled:", serverInstance.NamedPipesEnabled);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "NetName:", serverInstance.NetName);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "NumberOfLogFiles:", serverInstance.NumberOfLogFiles);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "OSVersion:", serverInstance.OSVersion);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "PerfMonMode:", serverInstance.PerfMonMode);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "PhysicalMemory:", serverInstance.PhysicalMemory);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "PhysicalMemoryUsageInKB:", serverInstance.PhysicalMemoryUsageInKB);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Platform:", serverInstance.Platform);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Processors:", serverInstance.Processors);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ProcessorUsage:", serverInstance.ProcessorUsage);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Product:", serverInstance.Product);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ProductLevel:", serverInstance.ProductLevel);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ResourceVersion:", serverInstance.ResourceVersionString);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Root\nDirectory:", serverInstance.RootDirectory);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ServerType:", serverInstance.ServerType);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Service\nAccount:", serverInstance.ServiceAccount);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ServiceInstanceId:", serverInstance.ServiceInstanceId);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "ServiceName:", serverInstance.ServiceName);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Service\nStartMode:", serverInstance.ServiceStartMode);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "SqlCharSet\nName:", serverInstance.SqlCharSetName);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "SqlDomain\nGroup:", serverInstance.SqlDomainGroup);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "SqlSortOrder\nName:", serverInstance.SqlSortOrderName);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Status:", serverInstance.Status);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "TcpEnabled:", serverInstance.TcpEnabled);
                    XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Version:", serverInstance.VersionString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_LinkedServers(XlHlp.XlLocation insertAt, SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "LinkedServers");

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "Catalog");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_LinkedServers(insertAt, serverInstance);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblLinkedServers_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_Logins(XlHlp.XlLocation insertAt, SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
               System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "Logins");

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "Name");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_Logins(insertAt, serverInstance);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblLogins_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_ServerRoles(XlHlp.XlLocation insertAt, SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow($"{System.Reflection.MethodInfo.GetCurrentMethod().Name}");

            try
            {
                XlHlp.AddContentToCell(insertAt.AddRowX(), "ServerRoles");

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 25, "Name");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_ServerRoles(insertAt, serverInstance);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblServerRoles_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_StoredProcedure(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "StoredProcedures:", dataBase.StoredProcedures.Count.ToString());

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Owner");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "Create\nDate");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 13, "Date\nLastModified");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 11, "ID");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 12, "MethodName");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_StoredProcedures(insertAt, dataBase);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblStoredProcedure_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_Table_Column_Info(XlHlp.XlLocation insertAt, VNC.SMOHelper.Database database, string tableName)
        {
            XlHlp.DisplayInWatchWindow(string.Format("Database >{0}< Adding table >{1}< info ...", database.Name, tableName));

            try
            {
                SMOH.Table table = database.Tables[tableName];

                foreach (SMOH.Column column in table.Columns.Values)
                {
                    insertAt.ClearOffsets();

                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), database.Name);
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), table.Name);

                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Name);
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.DataType);
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.MaximumLength);
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.NumericPrecision);
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.NumericScale);
                    //XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Default);
                    //XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.ID);
                    //XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Identity);
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.InPrimaryKey);
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.IsForeignKey);
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Nullable);

                    insertAt.IncrementRows();
                }

                //insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_TableInfo(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Tables:", dataBase.Tables.Count.ToString());

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Owner");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "CreateDate");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 13, "Date\nLastModified");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 11, "DataSpace\nUsed"); ;
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 11, "ID");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 11, "RowCount");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_Tables(insertAt, dataBase);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblTableInfo_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }

        private XlHlp.XlLocation AddSection_ViewInfo(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            try
            {
                XlHlp.AddLabeledInfoX(insertAt.AddRowX(), "Views:", dataBase.Views.Count.ToString());

                insertAt.MarkStart(XlHlp.MarkType.GroupTable);

                insertAt.ClearOffsets();

                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 45, "Name");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "Owner");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 15, "CreateDate");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 13, "Date\nLastModified");
                XlHlp.AddColumnHeaderToSheetX(insertAt.AddOffsetColumnX(), 10, "ID");

                insertAt.IncrementRows();

                insertAt = DisplayListOf_Views(insertAt, dataBase);

                insertAt.MarkEnd(XlHlp.MarkType.GroupTable, string.Format("tblViewInfo_{0}", insertAt.workSheet.Name));

                insertAt.Group(insertAt.OrientVertical, hide: true);

                insertAt.IncrementRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return insertAt;
        }
        #endregion

        #region Display_*

        private XlHlp.XlLocation DisplayListOf_Columns(XlHlp.XlLocation insertAt, SMOH.Table table)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (SMOH.Column column in table.Columns.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.DataType);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.MaximumLength);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.NumericPrecision);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.NumericScale);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Default);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.ID);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Identity);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.InPrimaryKey);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.IsForeignKey);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Nullable);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_Columns(XlHlp.XlLocation insertAt, SMOH.View view)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (SMOH.Column column in view.Columns.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.DataType);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.MaximumLength);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.NumericPrecision);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.NumericScale);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Default);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.ID);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Identity);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.InPrimaryKey);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.IsForeignKey);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), column.Nullable);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_DataBases(XlHlp.XlLocation insertAt, SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            foreach (SMOH.Database dataBase in serverInstance.Databases.Values)
            {
                XlHlp.DisplayInWatchWindow(string.Format("Adding Database Info for ({0})", dataBase.Name));
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataBase.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataBase.Owner);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataBase.CreateDate);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataBase.Size);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataBase.DataSpaceUsage);

                // These can throw exceptions if not access

                try
                {
                    // This is quick but returns 0 for counts

                    SMO.Database db = new SMO.Database(_SMOServer, dataBase.Name);
                    db.Refresh();

                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), db.Tables.Count.ToString());
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), db.Views.Count.ToString());
                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), db.StoredProcedures.Count.ToString());

                    Int64 count = 0;

                    foreach (var item in db.Tables)
                    {
                        count++;
                    }

                    XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), count.ToString());

                    //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), dataBase.Tables.Count().ToString());
                    //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), dataBase.Views.Count().ToString());
                    //XlHlp.AddContentToCell(insertAt.AddOffsetColumn(), dataBase.StoredProcedures.Count().ToString());
                }
                catch (Exception ex)
                {
                    // Quietly ignore
                }

                //XlHlp.AddContentToCell(rng.Offset[row, col++], dataBase.ID.ToString());
                //XlHlp.AddContentToCell(rng.Offset[row, col++], dataBase.DatabaseGuid.ToString());

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_DataFiles(XlHlp.XlLocation insertAt, SMOH.FileGroup fileGroup)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            foreach (SMOH.DataFile dataFile in fileGroup.DataFiles.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.FileName);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.AvailableSpace);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.Growth);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.GrowthType);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.MaxSize);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.NumberOfDiskReads);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.NumberOfDiskWrites);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.Size);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.State);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.UsedSpace);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), dataFile.VolumeFreeSpace);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_Endpoints(XlHlp.XlLocation insertAt, SMOH.Server server)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            foreach (SMOH.Endpoint endPoint in server.Endpoints.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.EndpointState);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.EndpointType);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.ID);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.IsAdminEndpoint);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.IsSystemObject);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.Owner);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.Protocol);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.ProtocolType);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), endPoint.Urn);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_ExtendedProperties(XlHlp.XlLocation insertAt, SMO.ExtendedPropertyCollection extendedProperties)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            foreach (SMO.ExtendedProperty extendedProperty in extendedProperties)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), extendedProperty.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), extendedProperty.Value.ToString());

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_FileGroups(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            foreach (SMOH.FileGroup fileGroup in dataBase.FileGroups.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), fileGroup.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), fileGroup.IsDefault);

                insertAt.IncrementRows();

                insertAt = AddSection_DataFileInfo(insertAt, fileGroup);
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_LinkedServers(XlHlp.XlLocation insertAt, SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            foreach (SMOH.LinkedServer linkedServer in serverInstance.LinkedServers.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), linkedServer.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), linkedServer.Catalog);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_Logins(XlHlp.XlLocation insertAt, SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            foreach (SMOH.Login login in serverInstance.Logins.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), login.Name);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_ServerRoles(XlHlp.XlLocation insertAt, SMOH.Server serverInstance)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            foreach (SMOH.ServerRole serverRole in serverInstance.ServerRoles.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), serverRole.Name);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_StoredProcedureParameters(XlHlp.XlLocation insertAt, SMOH.StoredProcedure storedProcedure)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (SMOH.StoredProcedureParameter parameter in storedProcedure.Parameters.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), parameter.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), parameter.DataType);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), parameter.MaximumLength);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), parameter.NumericPrecision);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), parameter.NumericScale);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), parameter.DefaultValue);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_StoredProcedures(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            foreach (SMOH.StoredProcedure storedProcedure in dataBase.StoredProcedures.Values)
            {
                if (storedProcedure.IsSystemObject == "1" && ! (bool) ceIncludeSystemStoredProcedures.IsChecked)
                {
                    continue;
                }

                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), storedProcedure.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), storedProcedure.Owner);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), storedProcedure.CreateDate);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), storedProcedure.DateLastModified);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), storedProcedure.ID);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), storedProcedure.MethodName);

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_Tables(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateDatabaseInfoWorkSheet()

            foreach (SMOH.Table table in dataBase.Tables.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), table.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), table.Owner);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), table.CreateDate);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), table.DateLastModified);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), table.DataSpaceUsed);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), table.ID);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), table.RowCount.ToString());

                insertAt.IncrementRows();
            }

            return insertAt;
        }

        private XlHlp.XlLocation DisplayListOf_Views(XlHlp.XlLocation insertAt, SMOH.Database dataBase)
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));

            // The columns in this method need to be kept in sync with CreateDatabaseInfoWorkSheet()

            foreach (SMOH.View view in dataBase.Views.Values)
            {
                insertAt.ClearOffsets();

                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), view.Name);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), view.Owner);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), view.CreateDate);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), view.DateLastModified);
                XlHlp.AddOffsetContentToCell(insertAt.AddOffsetColumn(), view.ID.ToString());

                insertAt.IncrementRows();
            }

            return insertAt;
        }
        
        #endregion

        private void Logoff()
        {
            if (_SMOServer != null)
            {
                _SMOServer.ConnectionContext.Disconnect();

                // May want to keep these around
                _SMOHServer = null;
            }

            cbeDatabases.Items.Clear();
            cbeTables.Items.Clear();
            cbeViews.Items.Clear();
            cbeStoredProcedures.Items.Clear();
        }

        private bool Logon()
        {
            XlHlp.DisplayInWatchWindow(string.Format("{0}",
                System.Reflection.MethodInfo.GetCurrentMethod().Name));
            bool result = false;

            _SMOServer = new SMO.Server(wucSQLInstance_Picker1.FullName);

            if (true == ceIntegratedSecurity.IsChecked)
            {
                _SMOServer.ConnectionContext.LoginSecure = true;
            }
            else
            {
                _SMOServer.ConnectionContext.LoginSecure = false;
                //_SMOServer.ConnectionContext.Login = txtUserName.Text;
                _SMOServer.ConnectionContext.Login = teUserName.Text;
                _SMOServer.ConnectionContext.Password = tePassword.Text;
                _SMOServer.ConnectionContext.Password = pbePassword.Text;

            }

            try
            {
                // Don't have to explicitly connect.  Connection is established when needed and pooling is used.
                // If connect, no pooling.

                //_SMOServer.ConnectionContext.Connect();

                // Load the SMOHelper Server object with information.
                // This allows us to not worry about access privileges in this code.
                // Any values will be available or marked as "<No Access>"

                XlHlp.DisplayInWatchWindow(string.Format("  {0}", "Initializing new SMOHServer ..."));

                _SMOHServer = new SMOH.Server(_SMOServer);

                int count;

                cbeDatabases.Items.Clear();

                if (wucSQLInstance_Picker1.Databases != null)
                {
                    // Limit the databases to the ones in the XML.
                    // NB. We have to populate the Databases Property on SMOH.Server ourselves.

                    //count = wucSQLInstance_Picker1.Databases.Count;


                    //XlHlp.DisplayInWatchWindow(string.Format("  {0} ({1}) {2}", "Adding", count, "Databases to combobox ..."));

                    //_SMOHServer.Databases = new Dictionary<string, SMOH.Database>();

                    //count = _SMOHServer.Databases.Keys.Count;

                    foreach (var name in wucSQLInstance_Picker1.Databases)
                    {
                        XlHlp.DisplayInWatchWindow(string.Format("  - {0}", name));

                        //SMO.Database realDatabase = new SMO.Database(_SMOServer, name);
                        //SMOH.Database database = new SMOH.Database(realDatabase);
                        //_SMOHServer.Databases.Add(name, database);

                        cbeDatabases.Items.Add(name);
                    }
                }
                else
                {
                    // Get the list of databases from the Instance that was selected.
                    count = _SMOHServer.Databases.Keys.Count;

                    XlHlp.DisplayInWatchWindow(string.Format("  {0} ({1}) {2}", "Adding", count, "Databases to combobox ..."));

                    foreach (string name in _SMOHServer.Databases.Keys)
                    {
                        cbeDatabases.Items.Add(name);
                        XlHlp.DisplayInWatchWindow(string.Format("  - {0}", name));
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return result;
        }

        #endregion

        #region Utility Routines



        #endregion

        #region Private Methods

        private bool GetDisplayOrientation()
        {
            return (bool)ceOrientOutputVertically.IsChecked;
        }

        private bool ValidUISelections()
        {
            //if (cbeTeamProjectCollections.SelectedText.Length > 0)
            //{
            return true;
            //}
            //else
            //{
            //    MessageBox.Show("Must Select Team Project Collection first", "UI Selection Incomplete");
            //    return false;
            //}
        }

        #endregion
    }
}
