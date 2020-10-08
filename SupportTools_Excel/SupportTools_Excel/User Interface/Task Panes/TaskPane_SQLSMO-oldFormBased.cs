using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Office.Interop.Excel;

using Microsoft.SqlServer.Management.Common;
using SMO=Microsoft.SqlServer.Management.Smo;
using SMOWMI=Microsoft.SqlServer.Management.Smo.Wmi;

using SMOH = VNC.SMOHelper;

using XlHlp = VNC.AddinHelper.Excel;
using VNC.AddinHelper;
using DevExpress.Data.Mask;


// TODO
// Consider passing in range instead of startingRow

namespace SupportTools_Excel.User_Interface.Task_Panes
{
    public partial class TaskPane_SQLSMO : UserControl
    {
        SMO.Server _SMOServer;      //  This is the real one
        SMOH.Server _SMOHServer;    // This is the one that hides the access restrictions

        public TaskPane_SQLSMO()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void btnCreateDatabase_Click(object sender, EventArgs e)
        {
            CreateDatabase();
        }

        private void btnCreateDatabaseInfoWorkSheet_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                CreateWS_DatabaseInfo(dataBase);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());  
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);                
            }
        }

        private void btnCreateDatabaseInfoWorksheets_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                CreateAllWorksheetsOf_DatabaseInfo(_SMOHServer);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());               
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);                
            }
        }

        private void btnCreateInstanceInfoWorkSheet_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                CreateWS_InstanceInfo(_SMOHServer, chkListInstanceDetails.Checked);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());                 
            }
            finally
            {
                 XlHlp.ScreenUpdatesOn(true);               
            }
        }

        private void btnCreateStoredProcedureInfoWorksheet_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                SMOH.StoredProcedure storedProcedure = dataBase.StoredProcedures[cbStoredProcedures.Text];
                CreateWS_StoredProcedureInfo(storedProcedure);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());                 
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);                
            }
        }

        private void btnCreateStoredProcedureInfoWorksheets_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                CreateAllWorksheeetsOf_StoredProcedureInfo(dataBase);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());                 
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);                
            }
        }

        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            CreateTable();
        }

        private void btnCreateTableInfoWorksheet_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                SMOH.Table table = dataBase.Tables[cbTables.Text];
                CreateWS_TableInfo(table);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());              
            }
            finally
            {
                 XlHlp.ScreenUpdatesOn(true);                   
            }
        }

        private void btnCreateTableInfoWorkSheets_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                CreateAllWorkSheetsOf_TableInfo(dataBase);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());                 
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);                 
            }
        }

        private void btnCreateViewInfoWorkSheet_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                SMOH.View view = dataBase.Views[cbViews.Text];
                CreateWS_ViewInfo(view);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());                
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);                  
            }
        }

        private void btnCreateViewInfoWorksheets_Click(object sender, EventArgs e)
        {
            try
            {
                XlHlp.ScreenUpdatesOff();
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                CreateAllWorksheetsOf_ViewInfo(dataBase);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());             
            }
            finally
            {
                 XlHlp.ScreenUpdatesOn(true);                    
            }
        }

        private void btnEnumAvailableSqlServers_Click(object sender, EventArgs e)
        {
            EnumAvailableSqlServers();
        }

        private void btnGetServerInfo_Click(object sender, EventArgs e)
        {
            GetServerInfo();
        }

        private void btnListDatabases_Click(object sender, EventArgs e)
        {
            //XlHlp.ScreenUpdatesOff();
            //Worksheet ws = Globals.ThisAddIn.Application.ActiveSheet;
            //int currentRow = Globals.ThisAddIn.Application.ActiveCell.Row;

            //ListDataBases(ws, _SMOServer, currentRow);
            //XlHlp.ScreenUpdatesOn(true);
        }

        private void btnListRows_Table_Click(object sender, EventArgs e)
        {
            try
            {
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                SMOH.Table table = dataBase.Tables[cbTables.Text];
                ListRows(table._table);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());               
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);                   
            }
        }

        private void btnListRows_View_Click(object sender, EventArgs e)
        {
            try
            {
                SMOH.Database dataBase = _SMOHServer.Databases[cbDatabases.Text];
                SMOH.View view = dataBase.Views[cbViews.Text];
                ListRows(view._view);
            }
            catch(Exception ex)
            {
                 MessageBox.Show(ex.ToString());                 
            }
            finally
            {
                XlHlp.ScreenUpdatesOn(true);                 
            }
        }

        private void btnListStoredProcedures_Click(object sender, EventArgs e)
        {
            //XlHlp.ScreenUpdatesOff();
            //Worksheet ws = Globals.ThisAddIn.Application.ActiveSheet;
            //int currentRow = Globals.ThisAddIn.Application.ActiveCell.Row;
            //SMO.Database dataBase = _SMOServer.Databases[cbDatabases.Text];

            //ListStoredProcedures(ws, dataBase, currentRow);
            //XlHlp.ScreenUpdatesOn(true);
        }

        private void btnListTables_Click(object sender, EventArgs e)
        {
            //XlHlp.ScreenUpdatesOff();
            //Worksheet ws = Globals.ThisAddIn.Application.ActiveSheet;
            //int currentRow = Globals.ThisAddIn.Application.ActiveCell.Row;
            //SMO.Database dataBase = _SMOServer.Databases[cbDatabases.Text];

            //ListTables(ws, dataBase, currentRow);
            //XlHlp.ScreenUpdatesOn(true);
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            Logoff();
            btnLogon.Enabled = true;
            btnLogon.BackColor = SystemColors.Control;
            btnLogoff.Enabled = false;
            btnLogoff.BackColor = SystemColors.Control;
            lblInstancName.Text = "";
            gbInstanceOperations.Visible = false;
        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            if (Logon() == true)
            {
                btnLogoff.Enabled = true;
                btnLogoff.BackColor = Color.Green;
                btnLogon.Enabled = false;
                btnLogon.BackColor = Color.Green;
                lblInstancName.Text = ucDBInstanceList.InstanceName;
                gbInstanceOperations.Visible = true;
            }
        }

        private void cbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbTables.Items.Clear();

            foreach(string name in _SMOHServer.Databases[cbDatabases.Text].Tables.Keys)
            {
                cbTables.Items.Add(name);
            }

            cbViews.Items.Clear();

            foreach(SMOH.View view in _SMOHServer.Databases[cbDatabases.Text].Views.Values)
            {
                if(view.IsSystemObject && !ckIncludeSystemStoredProcedures.Checked)
                {
                    continue;
                }

                cbViews.Items.Add(view.Name);
            }

            cbStoredProcedures.Items.Clear();

            foreach(SMOH.StoredProcedure sp in _SMOHServer.Databases[cbDatabases.Text].StoredProcedures.Values)
            {
                if(sp.IsSystemObject == "1" && ! (bool)ckIncludeSystemStoredProcedures.Checked)
                {
                    continue;
                }

                cbStoredProcedures.Items.Add(sp.Name);
            }
        }

        private void ckIncludeDBStoredProcedures_CheckedChanged(object sender, EventArgs e)
        {
            gbStoredProceduresOperations.Visible = ckIncludeDBStoredProcedures.Checked;
        }

        private void ckIncludeDBTables_CheckedChanged(object sender, EventArgs e)
        {
            gbTableOperations.Visible = ckIncludeDBTables.Checked;
        }

        private void ckIncludeDBViews_CheckedChanged(object sender, EventArgs e)
        {
            gbViewOperations.Visible = ckIncludeDBViews.Checked;
        }

        private void ckIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            if (ckIntegratedSecurity.Checked)
            {
                lblUserName.Visible = false;
                cbUserName.Visible = false;

                lblPassword.Visible = false;
                txtPassword.Visible = false;
            }
            else
            {
                lblUserName.Visible = true;
                cbUserName.Visible = true;

                lblPassword.Visible = true;
                txtPassword.Visible = true;
            }
        }

        private void TaskPane_SQLSMO_Load(object sender, EventArgs e)
        {
            ucDBInstanceList.PopulateInstanceListFromFile(@"C:\Temp\SupportTools_Config.xml", "Excel_Config");
        }

        private void ucDBInstanceList_InstanceSelectionChanged_Event()
        {
            txtServerName.Text = ucDBInstanceList.ServerName;

            // A new instance has been selected.  Decide what to do.
            //MessageBox.Show("TODO: InstanceChanged.  How to handle");
        }

        #endregion

        #region Main Function Routines

        private int AddSection_ColumnInfo(Worksheet ws, SMOH.Table table, int startingRow)
        {
            XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "Columns");

            int currentRow = startingRow;
            int col = 1;
            int rowsAdded = 0;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 35, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "DataType");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Maximum\nLength");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Numeric\nPrecision");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Numeric\nScale");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Default");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++,  5, "ID");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "Identity");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "Is\nPrimaryKey");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "Is\nForeignKey");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "Nullable");

            rowsAdded++;

            rowsAdded += DisplayListOf_Columns(ws, table, ++currentRow);

            // -2 because we are starting from 'A' and we have already incremented above
            Char endColumn = (char)('A' + (col - 2));

            string groupRange = string.Format("{0}:{1}", "C", endColumn);
            ws.Columns[groupRange].Group();
            ws.Columns[groupRange].Hidden = true;

            return ++rowsAdded;
        }

        private int AddSection_ColumnInfo(Worksheet ws, SMOH.View view, int startingRow)
        {
            XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "Columns");

            int currentRow = startingRow;
            int col = 1;
            int rowsAdded = 0;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 35, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "DataType");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Maximum\nLength");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Numeric\nPrecision");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Numeric\nScale");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Default");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++,  5, "ID");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "Identity");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "Is\nPrimaryKey");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "Is\nForeignKey");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "Nullable");

            rowsAdded++;

            rowsAdded += DisplayListOf_Columns(ws, view, ++currentRow);

            Char endColumn = (char)('A' + (col - 2));  // -2 because we are starting from 'A' and we have already incremented above

            string groupRange = string.Format("{0}:{1}", "C", endColumn);
            ws.Columns[groupRange].Group();
            ws.Columns[groupRange].Hidden = true;

            return rowsAdded;
        }
  
        private int AddSection_DatabaseInfo(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            Range rng = ws.Cells[startingRow, 1];

            int rowsAdded = 0;

            AddTitledInfo(rng.Offset[rowsAdded++, 0], "As of:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DB Name:", dataBase.Name);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "Instance:", dataBase.Parent);

            AddTitledInfo(rng.Offset[rowsAdded++, 0], "ActiveConnections:", dataBase.ActiveConnections);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AnsiNullDefault:", dataBase.AnsiNullDefault);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AnsiNullsEnabled:", dataBase.AnsiNullsEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AnsiPaddingEnabled:", dataBase.AnsiPaddingEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AnsiWarningsEnabled:", dataBase.AnsiWarningsEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AutoClose:", dataBase.AutoClose);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AutoCreateStatisticsEnabled:", dataBase.AutoCreateStatisticsEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AutoShrink:", dataBase.AutoShrink);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AutoUpdateStatisticsAsync:", dataBase.AutoUpdateStatisticsAsync);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "AutoUpdateStatisticsEnabled:", dataBase.AutoUpdateStatisticsEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "BrokerEnabled:", dataBase.BrokerEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "CaseSensitive:", dataBase.CaseSensitive);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "ChangeTrackingAutoCleanUp:", dataBase.ChangeTrackingAutoCleanUp);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "ChangeTrackingEnabled:", dataBase.ChangeTrackingEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "ChangeTrackingRetentionPeriod:", dataBase.ChangeTrackingRetentionPeriod);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "ChangeTrackingRetentionPeriodUnits:", dataBase.ChangeTrackingRetentionPeriodUnits);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "CloseCursorsOnCommitEnabled:", dataBase.CloseCursorsOnCommitEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "Collation:", dataBase.Collation);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "CompatibilityLevel:", dataBase.CompatibilityLevel);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "ConcatenateNullYieldsNull:", dataBase.ConcatenateNullYieldsNull);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "CreateDate:", dataBase.CreateDate);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DatabaseGuid:", dataBase.DatabaseGuid);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DatabaseSnapshotBaseName:", dataBase.DatabaseSnapshotBaseName);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DataSpaceUsage:", dataBase.DataSpaceUsage);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DateCorrelationOptimization:", dataBase.DateCorrelationOptimization);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DboLogin:", dataBase.DboLogin);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DefaultFileGroup:", dataBase.DefaultFileGroup);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DefaultFileStreamFileGroup:", dataBase.DefaultFileStreamFileGroup);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DefaultFullTextCatalog:", dataBase.DefaultFullTextCatalog);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "DefaultSchema:", dataBase.DefaultSchema);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "HonorBrokerPriority:", dataBase.HonorBrokerPriority);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "ID:", dataBase.ID);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IndexSpaceUsage:", dataBase.IndexSpaceUsage);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsAccessible:", dataBase.IsAccessible);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDatabaseSnapshot:", dataBase.IsDatabaseSnapshot);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDatabaseSnapshotBase:", dataBase.IsDatabaseSnapshotBase);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbAccessAdmin:", dataBase.IsDbAccessAdmin);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbBackupOperator:", dataBase.IsDbBackupOperator);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbDatareader:", dataBase.IsDbDatareader);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbDatawriter:", dataBase.IsDbDatawriter);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbDdlAdmin:", dataBase.IsDbDdlAdmin);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbDenyDatareader:", dataBase.IsDbDenyDatareader);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbDenyDatawriter:", dataBase.IsDbDenyDatawriter);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbOwner:", dataBase.IsDbOwner);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsDbSecurityAdmin:", dataBase.IsDbSecurityAdmin);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsFullTextEnabled:", dataBase.IsFullTextEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsMailHost:", dataBase.IsMailHost);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsManagementDataWarehouse:", dataBase.IsManagementDataWarehouse);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsMirroringEnabled:", dataBase.IsMirroringEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsParameterizationForced:", dataBase.IsParameterizationForced);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsReadCommittedSnapshotOn:", dataBase.IsReadCommittedSnapshotOn);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsSystemObject:", dataBase.IsSystemObject);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsUpdateable:", dataBase.IsUpdateable);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsVarDecimalStorageFormatEnabled:", dataBase.IsVarDecimalStorageFormatEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "LastBackupDate:", dataBase.LastBackupDate);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "LastDifferentialBackupDate:", dataBase.LastDifferentialBackupDate);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "LastLogBackupDate:", dataBase.LastLogBackupDate);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "LocalCursorsDefault:", dataBase.LocalCursorsDefault);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "LogReuseWaitStatus:", dataBase.LogReuseWaitStatus);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringFailoverLogSequenceNumber:", dataBase.MirroringFailoverLogSequenceNumber);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringID:", dataBase.MirroringID);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringPartner:", dataBase.IsParameterizationForced);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringRedoQueueMaxSize:", dataBase.MirroringRedoQueueMaxSize);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringRoleSequence:", dataBase.MirroringRoleSequence);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringSafetyLevel:", dataBase.MirroringSafetyLevel);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringSafetySequence:", dataBase.MirroringSafetySequence);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringStatus:", dataBase.MirroringStatus);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringTimeout:", dataBase.MirroringTimeout);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "MirroringWitness:", dataBase.MirroringWitness);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "NumericRoundAbortEnabled:", dataBase.NumericRoundAbortEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "Owner:", dataBase.Owner);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "PageVerify:", dataBase.PageVerify);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "PrimaryFilePath:", dataBase.PrimaryFilePath);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "QuotedIdentifiersEnabled:", dataBase.QuotedIdentifiersEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "ReadOnly:", dataBase.ReadOnly);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "RecoveryForkGuid:", dataBase.RecoveryForkGuid);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "RecoveryModel:", dataBase.RecoveryModel);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "RecursiveTriggersEnabled:", dataBase.RecursiveTriggersEnabled);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "Size:", dataBase.Size);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "SnapshotIsolationState:", dataBase.SnapshotIsolationState);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "SpaceAvailable:", dataBase.SpaceAvailable);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "State:", dataBase.State);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "Status:", dataBase.Status);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "UserName:", dataBase.UserName);
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "Version:", dataBase.Version);

            rowsAdded++;

            return rowsAdded;
        }

        private int AddSection_Databases(Worksheet ws, SMOH.Server serverInstance, int startingRow)
        {
            XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "Databases");

            int currentRow = startingRow;
            int rowsAdded = 0;
            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 25, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 20, "Owner");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 15, "Create\nDate");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 15, "DataSpace\nUsage");
            //XlHlp.AddColumnToSheet(ref ws, col++, 5,  row, "ID");
            //XlHlp.AddColumnToSheet(ref ws, col++, 35, row, "DatabaseGuid");

            rowsAdded++;

            rowsAdded += DisplayListOf_DataBases(ws, serverInstance, ++currentRow);

            // Create a Table to structure the output
            ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblDatabases_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_DataFileInfo(Worksheet ws, SMOH.FileGroup fileGroup, int startingRow)
        {
            int rowsAdded = 0;
            int currentRow = startingRow;

            XlHlp.AddContentToCell(ws.Cells[currentRow++, 1], "DataFiles");
            rowsAdded++;

            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "FileName");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "AvailableSpace");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "Growth");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "GrowthType");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "MaxSize");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 13, "NumberOf\nDiskReads");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 13, "NumberOf\nDiskWrites");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Size");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "State");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 11, "UsedSpace");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Volume\nFreeSpace");

            rowsAdded++;

            rowsAdded += DisplayListOf_DataFiles(ws, fileGroup, ++currentRow);

            //// Create a Table to structure the output
            //ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tbl_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow + 1, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_EndPoints(Worksheet ws, SMOH.Server server, int startingRow)
        {
            int rowsAdded = 0;
            int currentRow = startingRow;

            XlHlp.AddContentToCell(ws.Cells[currentRow++, 1], "Endpoints");
            rowsAdded++;

            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "EndpointState");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "EndpointType");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "ID");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "IsAdminEndpoint");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "IsSystemObject");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 13, "Owner");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 13, "Protocol");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "ProtocolType");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Urn");

            rowsAdded++;

            rowsAdded += DisplayListOf_Endpoints(ws, server, ++currentRow);

            //// Create a Table to structure the output
            //ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tbl_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow + 1, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }
        private int AddSection_ExtendedPropertyInfo(Worksheet ws, SMO.ExtendedPropertyCollection extendedProperties, int startingRow)
        {
            int rowsAdded = 0;
            int currentRow = startingRow;

            AddTitledInfo(ws.Cells[currentRow++, 1], "ExtendedProperties:", extendedProperties.Count.ToString());

            rowsAdded++;

            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Value");

            rowsAdded++;

            rowsAdded += DisplayListOf_ExtendedProperties(ws, extendedProperties, ++currentRow);

            // Create a Table to structure the output
            //ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblFileGroups_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow + 1, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_FileGroupInfo(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            int rowsAdded = 0;
            int currentRow = startingRow;

            AddTitledInfo(ws.Cells[currentRow++, 1], "FileGroups:", dataBase.FileGroups.Count.ToString());

            //XlHlp.AddContentToCell(ws.Cells[currentRow++, 1], "FileGroups", 14, XlHlp.MakeBold.Yes);
            rowsAdded++;

            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "IsDefault");

            rowsAdded++;

            rowsAdded += DisplayListOf_FileGroups(ws, dataBase, ++currentRow);

            // Create a Table to structure the output
            //ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblFileGroups_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow + 1, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_InstanceInfo(Worksheet ws, SMOH.Server serverInstance, int startingRow, bool showDetail)
        {
            Range rng = ws.Cells[startingRow, 1];

            int rowsAdded = 0;

            AddTitledInfo(rng.Offset[rowsAdded++, 0], "As of:", DateTime.Now.ToString());
            AddTitledInfo(rng.Offset[rowsAdded++, 0], "Instance Name:", serverInstance.Name);

            if (showDetail)
            {
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Backup\nDirectory:", serverInstance.BackupDirectory);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Browser\nService Account:", serverInstance.BrowserServiceAccount);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Browser\nStartMode:", serverInstance.BrowserStartMode);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Build\nClrVersion:", serverInstance.BuildClrVersionString);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "BuildNumber:", serverInstance.BuildNumber);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Collation:", serverInstance.Collation);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ComparisonStyle:", serverInstance.ComparisonStyle);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ComputerNamePhysicalNetBIOS:", serverInstance.ComputerNamePhysicalNetBIOS);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "DefaultFile:", serverInstance.DefaultFile);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "DefaultLog:", serverInstance.DefaultLog);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Edition:", serverInstance.Edition);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ErrorLogPath:", serverInstance.ErrorLogPath);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "FilestreamLevel:", serverInstance.FilestreamLevel);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "FilestreamShareName:", serverInstance.FilestreamShareName);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "InstallDataDirectory:", serverInstance.InstallDataDirectory);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "InstallSharedDirectory:", serverInstance.InstallSharedDirectory);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "InstanceName:", serverInstance.InstanceName);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsCaseSensitive:", serverInstance.IsCaseSensitive);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsClustered:", serverInstance.IsClustered);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsFullTextInstalled:", serverInstance.IsFullTextInstalled);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "IsSingleUser:", serverInstance.IsSingleUser);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "LoginMode:", serverInstance.LoginMode);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "MasterDBPath:", serverInstance.MasterDBPath);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "MasterDBLogPath:", serverInstance.MasterDBLogPath);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "MaxPrecision:", serverInstance.MaxPrecision);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "NamedPipesEnabled:", serverInstance.NamedPipesEnabled);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "NetName:", serverInstance.NetName);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "NumberOfLogFiles:", serverInstance.NumberOfLogFiles);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "OSVersion:", serverInstance.OSVersion);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "PerfMonMode:", serverInstance.PerfMonMode);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "PhysicalMemory:", serverInstance.PhysicalMemory);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "PhysicalMemoryUsageInKB:", serverInstance.PhysicalMemoryUsageInKB);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Platform:", serverInstance.Platform);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Processors:", serverInstance.Processors);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ProcessorUsage:", serverInstance.ProcessorUsage);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Product:", serverInstance.Product);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ProductLevel:", serverInstance.ProductLevel);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ResourceVersion:", serverInstance.ResourceVersionString);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Root\nDirectory:", serverInstance.RootDirectory);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ServerType:", serverInstance.ServerType);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Service\nAccount:", serverInstance.ServiceAccount);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ServiceInstanceId:", serverInstance.ServiceInstanceId);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "ServiceName:", serverInstance.ServiceName);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Service\nStartMode:", serverInstance.ServiceStartMode);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "SqlCharSet\nName:", serverInstance.SqlCharSetName);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "SqlDomain\nGroup:", serverInstance.SqlDomainGroup);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "SqlSortOrder\nName:", serverInstance.SqlSortOrderName);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Status:", serverInstance.Status);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "TcpEnabled:", serverInstance.TcpEnabled);
                AddTitledInfo(rng.Offset[rowsAdded++, 0], "Version:", serverInstance.VersionString);
            }

            return rowsAdded;
        }

        private int AddSection_LinkedServers(Worksheet ws, SMOH.Server serverInstance, int startingRow)
        {
            XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "LinkedServers");

            int currentRow = startingRow;
            int rowsAdded = 0;
            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 25, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 25, "Catalog");

            rowsAdded++;

            rowsAdded += DisplayListOf_LinkedServers(ws, serverInstance, ++currentRow);

            // Create a Table to structure the output
            ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblLinkedServers_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_Logins(Worksheet ws, SMOH.Server serverInstance, int startingRow)
        {
            XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "Logins");

            int currentRow = startingRow;
            int rowsAdded = 0;
            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 25, "Name");

            rowsAdded++;

            rowsAdded += DisplayListOf_Logins(ws, serverInstance, ++currentRow);

            // Create a Table to structure the output
            ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblLogins_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_ServerRoles(Worksheet ws, SMOH.Server serverInstance, int startingRow)
        {
            XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "ServerRoles");

            int currentRow = startingRow;
            int rowsAdded = 0;
            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 25, "Name");

            rowsAdded++;

            rowsAdded += DisplayListOf_ServerRoles(ws, serverInstance, ++currentRow);

            // Create a Table to structure the output
            ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblServerRoles_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_StoredProcedure(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            AddTitledInfo(ws.Cells[startingRow++, 1], "StoredProcedures:", dataBase.StoredProcedures.Count.ToString());
            //XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "StoredProcedures", 14, XlHlp.MakeBold.Yes);

            int currentRow = startingRow;
            int rowsAdded = 0;
            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Owner");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 15, "Create\nDate");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 13, "Date\nLastModified");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 11, "ID");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 12, "MethodName");

            rowsAdded++;

            rowsAdded += DisplayListOf_StoredProcedures(ws, dataBase, ++currentRow);

            // Create a Table to structure the output
            ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblStoredProcedures_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_TableInfo(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            AddTitledInfo(ws.Cells[startingRow++, 1], "Tables:", dataBase.Tables.Count.ToString());

            //XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "Tables", 14, XlHlp.MakeBold.Yes);

            int currentRow = startingRow;
            int rowsAdded = 0;
            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Owner");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 15, "CreateDate");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 13, "Date\nLastModified");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 11, "DataSpace\nUsed"); ;
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 11, "ID");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 11, "RowCount");

            rowsAdded++;

            rowsAdded += DisplayListOf_Tables(ws, dataBase, ++currentRow);

            // Create a Table to structure the output
            ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblTables_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private int AddSection_ViewInfo(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            AddTitledInfo(ws.Cells[startingRow++, 1], "Views:", dataBase.Views.Count.ToString());
            //XlHlp.AddContentToCell(ws.Cells[startingRow++, 1], "Views", 14, XlHlp.MakeBold.Yes);

            int currentRow = startingRow;
            int rowsAdded = 0;
            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 45, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "Owner");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 15, "CreateDate");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 13, "Date\nLastModified");
            XlHlp.AddColumnHeaderToSheetX(ws, currentRow, col++, 10, "ID");

            rowsAdded++;

            rowsAdded += DisplayListOf_Views(ws, dataBase, ++currentRow);

            // Create a Table to structure the output
            ws.ListObjects.Add(XlListObjectSourceType.xlSrcRange, ws.Range[ws.Cells[startingRow, 1], ws.Cells[startingRow + rowsAdded - 1, col - 1]], Type.Missing, XlYesNoGuess.xlYes).Name = string.Format("tblViews_{0}", ws.Name);
            string groupRange = string.Format("{0}:{1}", startingRow, startingRow + rowsAdded - 1);
            ws.Rows[groupRange].Group();
            ws.Rows[groupRange].Hidden = true;

            return rowsAdded;
        }

        private void AddTitledInfo(Range outputStartingRange, string title, string info)
        {
            XlHlp.AddContentToCell(outputStartingRange.Offset[0, 0], title); // XlHlp.MakeBold.Yes);
            XlHlp.AddContentToCell(outputStartingRange.Offset[0, 1], info);
        }

        private void CreateAllWorksheetsOf_DatabaseInfo(SMOH.Server serverInstance)
        {
            foreach (SMOH.Database database in serverInstance.Databases.Values)
            {
                try
                {
                    CreateWS_DatabaseInfo(database);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        
        private void CreateAllWorksheeetsOf_StoredProcedureInfo(SMOH.Database dataBase)
        {
            foreach(SMOH.StoredProcedure storedProcedure in dataBase.StoredProcedures.Values)
            {
                try
                {
                    CreateWS_StoredProcedureInfo(storedProcedure);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CreateAllWorkSheetsOf_TableInfo(SMOH.Database database)
        {
            foreach (SMOH.Table table in database.Tables.Values)
            {
                try
                {
                    CreateWS_TableInfo(table);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CreateAllWorksheetsOf_ViewInfo(SMOH.Database dataBase)
        {
            foreach(SMOH.View view in dataBase.Views.Values)
            {
                try
                {
                    CreateWS_ViewInfo(view);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }   

        private void CreateDatabase()
        {
            if (_SMOServer.Databases[txtNewDatabase.Text] != null)
            {
            	if (DialogResult.OK != MessageBox.Show("Drop Database?", "Database Exists Warning", MessageBoxButtons.OKCancel))
                {
                    return;
                }
                	
                _SMOServer.Databases[txtNewDatabase.Text].Drop();
            }

            SMO.Database newDatabase = new SMO.Database(_SMOServer, txtNewDatabase.Text);
            newDatabase.Create();
            // TODO: Need to update SMOH object list and comboboxes
        }

        private void CreateTable()
        {
            SMO.Database database = _SMOServer.Databases[cbDatabases.Text];

            SMO.Table newTable = new SMO.Table(database, txtNewTable.Text);

            //SMO.DataType varChar = new SMO.DataType(SMO.SqlDataType.VarChar, 100);

            SMO.Column col1 = new SMO.Column(newTable, "col1", SMO.DataType.VarChar(100));
            
            SMO.Column col2 = new SMO.Column(newTable, "col2", SMO.DataType.DateTime);
            SMO.Column col3 = new SMO.Column(newTable, "col3", SMO.DataType.Int);

            newTable.Columns.Add(col1);
            newTable.Columns.Add(col2);
            newTable.Columns.Add(col3);
            newTable.Create();

            //database.Tables.Add(newTable);

        }

        private void CreateWS_DatabaseInfo(SMOH.Database dataBase)
        {
            long startTicks = Common.WriteToDebugWindow("CreateDatabaseInfoWorkSheet(Start)");

            string sheetName = XlHlp.SafeSheetName("D>" + dataBase.Name);
            Worksheet ws = XlHlp.NewWorksheet(sheetName, afterSheetName: "LAST");

            // Output starts here.  Each Display method returns the output end point.

            int startingRow = 2;

            startingRow += AddSection_DatabaseInfo(ws, dataBase, startingRow);

            startingRow += AddSection_ExtendedPropertyInfo(ws, dataBase.ExtendedProperties, startingRow);

            startingRow += AddSection_FileGroupInfo(ws, dataBase, startingRow);

            if (ckIncludeDBTables.Checked)
            {
                startingRow += 2;
                startingRow += AddSection_TableInfo(ws, dataBase, startingRow);
            }

            if (ckIncludeDBViews.Checked)
            {
                startingRow += 2;
                startingRow += AddSection_ViewInfo(ws, dataBase, startingRow);
            }

            if (ckIncludeDBStoredProcedures.Checked)
            {
                startingRow += 2;
                startingRow += AddSection_StoredProcedure(ws, dataBase, startingRow);
            }

            Common.WriteToDebugWindow("CreateDatabaseInfoWorkSheet(Start)", startTicks);
        }

        private void CreateWS_InstanceInfo(SMOH.Server serverInstance, bool showDetails)
        {
            long startTicks = Common.WriteToDebugWindow("CreateInstanceInfoWorksheet(Start)");

            string sheetName = XlHlp.SafeSheetName("I>" + serverInstance.Name);
            Worksheet ws = XlHlp.NewWorksheet(sheetName, beforeSheetName: "FIRST");

            // Output starts here.  Each Display method returns the output end point.

            int startingRow = 2;

            startingRow += AddSection_InstanceInfo(ws, serverInstance, startingRow, showDetails);
            startingRow++;
            startingRow += AddSection_Databases(ws, serverInstance, startingRow);
            startingRow += 2;
            startingRow += +AddSection_Logins(ws, serverInstance, startingRow);
            startingRow += 2;
            startingRow += AddSection_ServerRoles(ws, serverInstance, startingRow);
            startingRow += 2;
            startingRow += AddSection_LinkedServers(ws, serverInstance, startingRow);
            startingRow += 2;
            startingRow += AddSection_EndPoints(ws, serverInstance, startingRow);
            startingRow += 2;

            Common.WriteToDebugWindow("CreateInstanceInfoWorksheet(End)", startTicks);
        }

        private void CreateWS_StoredProcedureInfo(SMOH.StoredProcedure storedProcedure)
        {
            long startTicks = Common.WriteToDebugWindow("CreateStoredProcedureInfoWorksheet(Start)");

            string sheetName = XlHlp.SafeSheetName("S>" + storedProcedure.Name);
            Worksheet ws = XlHlp.NewWorksheet(sheetName, afterSheetName: "LAST");
            int fontSize = 8;

            AddTitledInfo(ws.Cells[2, 1], "As of:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            AddTitledInfo(ws.Cells[3, 1], "DB Name:", storedProcedure.Name);


            XlHlp.AddContentToCell(ws.Cells[6, 1], "Parameters");
            int row = 7;
            int col = 1;

            XlHlp.AddColumnHeaderToSheetX(ws, row, col++, 25, "Name");
            XlHlp.AddColumnHeaderToSheetX(ws, row, col++, 20, "DataType");
            XlHlp.AddColumnHeaderToSheetX(ws, row, col++, 10, "Maximum\nLength");
            XlHlp.AddColumnHeaderToSheetX(ws, row, col++, 10, "Numeric\nPrecision");
            XlHlp.AddColumnHeaderToSheetX(ws, row, col++, 10, "Numeric\nScale");
            XlHlp.AddColumnHeaderToSheetX(ws, row, col++, 10, "Default\nValue");

            row += DisplayListOf_StoredProcedureParameters(ws, storedProcedure, ++row);

            row++;

            AddTitledInfo(ws.Cells[row++, 1], "Header:", "");
            XlHlp.AddContentToCell(ws.Cells[row++, 2], storedProcedure.TextHeader);
            AddTitledInfo(ws.Cells[row++, 1], "Body:", "");
            XlHlp.AddContentToCell(ws.Cells[row++, 2], storedProcedure.TextBody);

            Common.WriteToDebugWindow("CreateStoredProcedureInfoWorksheet(End)", startTicks);
        }

        private void CreateWS_TableInfo(SMOH.Table table)
        {
            long startTicks = Common.WriteToDebugWindow("CreateTableInfoWorkSheet(Start)");

            string sheetName = XlHlp.SafeSheetName("T>" + table.Name);
            Worksheet ws = XlHlp.NewWorksheet(sheetName, afterSheetName: "LAST");

            AddTitledInfo(ws.Cells[2, 1], "As of:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            AddTitledInfo(ws.Cells[3, 1], "Name:", table.Name);

            int startingRow = 6;
            startingRow += AddSection_ExtendedPropertyInfo(ws, table.ExtendedProperties, startingRow);

            AddSection_ColumnInfo(ws, table,startingRow);

            Common.WriteToDebugWindow("CreateTableInfoWorkSheet(End)", startTicks);
        }

        private void CreateWS_ViewInfo(SMOH.View view)
        {
            long startTicks = Common.WriteToDebugWindow("CreateViewInfoWorkSheet(Start)");

            string sheetName = XlHlp.SafeSheetName("V>" + view.Name);
            Worksheet ws = XlHlp.NewWorksheet(sheetName, afterSheetName: "LAST");

            AddTitledInfo(ws.Cells[2, 1], "As of:", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            AddTitledInfo(ws.Cells[3, 1], "Name:", view.Name);

            int startingRow = 6;

            startingRow += AddSection_ExtendedPropertyInfo(ws, view.ExtendedProperties, startingRow);
            AddSection_ColumnInfo(ws, view, startingRow);

            Common.WriteToDebugWindow("CreateViewInfoWorkSheet(End)", startTicks);
        }

        private int DisplayListOf_Columns(Worksheet ws, SMOH.Table table, int startingRow)
        {
            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.Column column in table.Columns.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.DataType);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.MaximumLength);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.NumericPrecision);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.NumericScale);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Default);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.ID);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Identity);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.InPrimaryKey);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.IsForeignKey);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Nullable);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_Columns(Worksheet ws, SMOH.View view, int startingRow)
        {
            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.Column column in view.Columns.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.DataType);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.MaximumLength);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.NumericPrecision);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.NumericScale);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Default);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.ID);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Identity);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.InPrimaryKey);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.IsForeignKey);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], column.Nullable);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_DataBases(Worksheet ws, SMOH.Server serverInstance, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.Database dataBase in serverInstance.Databases.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataBase.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataBase.Owner);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataBase.CreateDate);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataBase.DataSpaceUsage);

                //XlHlp.AddContentToCell(rng.Offset[row, col++], dataBase.ID.ToString());
                //XlHlp.AddContentToCell(rng.Offset[row, col++], dataBase.DatabaseGuid.ToString());

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_DataFiles(Worksheet ws, SMOH.FileGroup fileGroup, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.DataFile dataFile in fileGroup.DataFiles.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.FileName);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.AvailableSpace);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.Growth);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.GrowthType);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.MaxSize);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.NumberOfDiskReads);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.NumberOfDiskWrites);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.Size);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.State);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.UsedSpace);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], dataFile.VolumeFreeSpace);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_Endpoints(Worksheet ws, SMOH.Server server, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.Endpoint endPoint in server.Endpoints.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.EndpointState);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.EndpointType);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.ID);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.IsAdminEndpoint);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.IsSystemObject);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.Owner);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.Protocol);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.ProtocolType);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], endPoint.Urn);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_ExtendedProperties(Worksheet ws, SMO.ExtendedPropertyCollection extendedProperties, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMO.ExtendedProperty extendedProperty in extendedProperties)
            {
                //int row = 0;
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], extendedProperty.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], extendedProperty.Value.ToString());

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_FileGroups(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.FileGroup fileGroup in dataBase.FileGroups.Values)
            {
                //int row = 0;
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], fileGroup.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], fileGroup.IsDefault);
                
                rowsAdded++;

                rowsAdded += AddSection_DataFileInfo(ws, fileGroup, startingRow + rowsAdded);
                rng = ws.Cells[startingRow + rowsAdded, 1];
            }

            return rowsAdded;
        }

        private int DisplayListOf_LinkedServers(Worksheet ws, SMOH.Server serverInstance, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.LinkedServer linkedServer in serverInstance.LinkedServers.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], linkedServer.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], linkedServer.Catalog);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_Logins(Worksheet ws, SMOH.Server serverInstance, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.Login login in serverInstance.Logins.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], login.Name);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_ServerRoles(Worksheet ws, SMOH.Server serverInstance, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateInstanceInfoWorksheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.ServerRole serverRole in serverInstance.ServerRoles.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], serverRole.Name);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_StoredProcedureParameters(Worksheet ws, SMOH.StoredProcedure storedProcedure, int startingRow)
        {
            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach(SMOH.StoredProcedureParameter parameter in storedProcedure.Parameters.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], parameter.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], parameter.DataType);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], parameter.MaximumLength);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], parameter.NumericPrecision);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], parameter.NumericScale);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], parameter.DefaultValue);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_StoredProcedures(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.StoredProcedure storedProcedure in dataBase.StoredProcedures.Values)
            {
                if (storedProcedure.IsSystemObject == "1" && ! ckIncludeSystemStoredProcedures.Checked)
                {
                    continue;
                }

                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], storedProcedure.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], storedProcedure.Owner);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], storedProcedure.CreateDate);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], storedProcedure.DateLastModified);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], storedProcedure.ID);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], storedProcedure.MethodName);

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_Tables(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateDatabaseInfoWorkSheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach (SMOH.Table table in dataBase.Tables.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], table.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], table.Owner);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], table.CreateDate);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], table.DateLastModified);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], table.DataSpaceUsed);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], table.ID.ToString());
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], table.RowCount.ToString());

                rowsAdded++;
            }

            return rowsAdded;
        }

        private int DisplayListOf_Views(Worksheet ws, SMOH.Database dataBase, int startingRow)
        {
            // The columns in this method need to be kept in sync with CreateDatabaseInfoWorkSheet()

            Range rng = ws.Cells[startingRow, 1];
            int rowsAdded = 0;

            foreach(SMOH.View view in dataBase.Views.Values)
            {
                int col = 0;

                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], view.Name);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], view.Owner);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], view.CreateDate);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], view.DateLastModified);
                XlHlp.AddContentToCell(rng.Offset[rowsAdded, col++], view.ID.ToString());

                rowsAdded++;
            }

            return rowsAdded;
        }

        private void EnumAvailableSqlServers()
        {
            SMO.SmoApplication smoApplication = new SMO.SmoApplication();

            foreach (var sqlServer in SMO.SmoApplication.EnumAvailableSqlServers().AsEnumerable())
            {
                sqlServer.Field<string>("Name");
                sqlServer.Field<string>("Server");
                sqlServer.Field<string>("Instance");
                sqlServer.Field<bool>("IsClustered");
                sqlServer.Field<string>("Version");
                sqlServer.Field<bool>("IsLocal");
            }
        }

        private void GetServerInfo()
        {
            SMOWMI.ManagedComputer computer = new SMOWMI.ManagedComputer(txtServerName.Text);
            Common.WriteToDebugWindow(String.Format("Name:{0}", computer.Name));


        }

        private void ListRows(Microsoft.SqlServer.Management.Smo.View view)
        {
            throw new NotImplementedException();
        }

        private void ListRows(Microsoft.SqlServer.Management.Smo.Table table)
        {
            throw new NotImplementedException();
        }

        private void Logoff()
        {
            if (_SMOServer != null)
            {
            	_SMOServer.ConnectionContext.Disconnect();

                // May want to keep these around
                _SMOHServer = null;
            }

            cbDatabases.Items.Clear();
        }

        private bool Logon()
        {
            bool result = false;

            _SMOServer = new SMO.Server(ucDBInstanceList.InstanceName);

            if (ckIntegratedSecurity.Checked)
            {
                _SMOServer.ConnectionContext.LoginSecure = true;
            }
            else
            {
                _SMOServer.ConnectionContext.LoginSecure = false;
                //_SMOServer.ConnectionContext.Login = txtUserName.Text;
                _SMOServer.ConnectionContext.Login = cbUserName.Text;
                _SMOServer.ConnectionContext.Password = txtPassword.Text;
            }

            try
            {
                // Don't have to explicitly connect.  Connection is established when needed and pooling is used.
                // If connect, no pooling.

                //_SMOServer.ConnectionContext.Connect();

                // Load the SMOHelper Server object with information.
                // This allows us to not worry about access privileges in this code.
                // The values will be available or marked as "<No Access>"
                _SMOHServer = new SMOH.Server(_SMOServer);

                cbDatabases.Items.Clear();

                foreach (string name in _SMOHServer.Databases.Keys)
                {
                    cbDatabases.Items.Add(name);
                }

                result = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return result;
        }

        #endregion

        private void ucDBInstanceList_Load(object sender, EventArgs e)
        {

        }

        private void chkListInstanceDetails_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
