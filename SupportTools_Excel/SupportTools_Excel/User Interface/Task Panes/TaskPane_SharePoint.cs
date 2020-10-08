using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Office.Interop.Excel;
using Microsoft.SharePoint.Client;

using ExcelHlp = VNC.AddinHelper.Excel;
using XlHlp =VNC.AddinHelper.Excel;
using VNCSP = VNC.SP;

namespace SupportTools_Excel.User_Interface.Task_Panes
{
    public partial class TaskPane_SharePoint : UserControl
    {
        
        #region Constructors and Load

        public TaskPane_SharePoint()
        {
            InitializeComponent();
        }

        //private void TaskPane_SharePoint_Load(object sender, EventArgs e)
        //{
        //    spSiteCollection_Picker1.ControlChanged += spSiteCollection_Picker1_ControlChanged;
        //}

        #endregion

        //#region Event Handlers

        //private void btnCreateContentType_Click(object sender, EventArgs e)
        //{
        //    var sUri = txtSiteUri.Text;

        //    string schemaXml = "<Field Type='Text' DisplayName='VNCSiteCol1' Name='VNCSiteCol1' Group='VNC' />";

        //    using (var ctx = new ClientContext(sUri))
        //    {
        //        var rootWeb = ctx.Site.RootWeb;

        //        try
        //        {
        //            rootWeb.ContentTypes.Add(new ContentTypeCreationInformation
        //            {
        //                Name = "VNCContentType3",
        //                Group = "VNC"
        //            });

        //            ctx.ExecuteQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.ToString());
        //        }

        //        try
        //        {
        //            var parentContentTypes = ctx.LoadQuery(rootWeb.ContentTypes.Where(ct => ct.Name == "Page"));
        //            ctx.ExecuteQuery();

        //            var parentContentType = parentContentTypes.FirstOrDefault();


        //            if (parentContentType != null)
        //            {
        //                rootWeb.ContentTypes.Add(new ContentTypeCreationInformation
        //                {
        //                    Name = "VNCContentType4",
        //                    Group = "VNC",
        //                    ParentContentType = parentContentType
        //                });

        //                ctx.ExecuteQuery();
        //            }
        //            else
        //            {
        //                throw new InvalidOperationException("Parent Content Type not found");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.ToString());
        //        }

        //    }
        //}

        //private void btnCreateSiteColumn_Click(object sender, EventArgs e)
        //{
        //    var sUri = txtSiteUri.Text;

        //    string schemaXml = "<Field Type='Text' DisplayName='VNCSiteCol1' Name='VNCSiteCol1' Group='VNC' />";
        //    using (var ctx = new ClientContext(sUri))
        //    {
        //        var rootWeb = ctx.Site.RootWeb;

        //        rootWeb.Fields.AddFieldAsXml(schemaXml, true, AddFieldOptions.AddFieldInternalNameHint);
        //        ctx.ExecuteQuery();
        //     }
        //}
        
        //private void btnGetSiteCollectionInfo_Click(object sender, EventArgs e)
        //{
        //    var scUri = spSiteCollection_Picker1.Uri;

        //    try
        //    {
        //        ExcelHlp.ScreenUpdatesOff();

        //        using (var ctx = new ClientContext(scUri))
        //        {
        //            CreateWS_SiteCollection_Info(ctx, false);
        //        }

        //    }
        //    finally
        //    {
        //        ExcelHlp.ScreenUpdatesOn(true);
        //    }

        //}

        //private void btnGetSiteInfo_Click(object sender, EventArgs e)
        //{
        //    var sUri = txtSiteUri.Text;

        //    try
        //    {
        //        ExcelHlp.ScreenUpdatesOff();

        //        using (var ctx = VNCSP.Helper.GetClientContext(sUri))
        //        {
        //            CreateWS_Web_Info(ctx, false);
        //        }

        //    }
        //    finally
        //    {
        //        ExcelHlp.ScreenUpdatesOn(true);
        //    }
        //}

        //private void btnLinkColumnsToContentTypes_Click(object sender, EventArgs e)
        //{
        //    var sUri = txtSiteUri.Text;

        //    using (var ctx = new ClientContext(sUri))
        //    {
                
        //        try
        //        {
        //            var rootWeb = ctx.Site.RootWeb;

        //            Field sc1 = rootWeb.Fields.GetByInternalNameOrTitle("VNCSiteCol1");

        //            var parentContentTypes = ctx.LoadQuery(rootWeb.ContentTypes.Where(ct => ct.Name == "VNCContentType4"));
        //            ctx.ExecuteQuery();

        //            var ct4 = parentContentTypes.FirstOrDefault();

        //            ct4.FieldLinks.Add(new FieldLinkCreationInformation
        //            {
        //                Field = sc1
        //            });

        //            ct4.Update(true);

        //            ctx.ExecuteQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.ToString());
        //        }
        //    }
        //}

        //private void spSiteCollection_Picker1_ControlChanged()
        //{
        //    try
        //    {
        //        XlHlp.ScreenUpdatesOff();

        //        // We just picked a new site collection, perhaps update a list of (sub)sites.
        //        PopulateSitesPicker(spSiteCollection_Picker1.Uri);
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

        //#endregion

        //#region Main Methods
        
        //private int AddSection_Libraries(Worksheet ws, ClientContext ctx, Web web, int startingRow)
        //{
        //    ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "Libraries", "SiteName");

        //    Range rng = ws.Cells[startingRow, 1];
        //    int rowsAdded = 0;

        //    var alllists = web.Lists;

        //    IEnumerable<List> doclibs = ctx.LoadQuery(alllists.Where
        //        (list => list.BaseType == BaseType.DocumentLibrary)
        //        );

        //    ctx.ExecuteQuery();

        //    foreach (var list in doclibs)
        //    {
        //        int col = 0;
        //        ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], list.Title);

        //        rowsAdded++;
        //    }

        //    return rowsAdded;
        //}

        //private int AddSection_Lists(Worksheet ws, ClientContext ctx, Web web, int startingRow)
        //{
        //    ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "Lists", "SiteName");
        //    //// The columns in this method need to be kept in sync with CreateTeamProjectsInfo()

        //    Range rng = ws.Cells[startingRow, 1];
        //    int rowsAdded = 0;

        //    var alllists = web.Lists;

        //    IEnumerable<List> lists = ctx.LoadQuery(alllists.Where
        //        (list => list.BaseType == BaseType.GenericList)
        //        );

        //    ctx.ExecuteQuery();

        //    foreach (var list in lists)
        //    {
        //        int col = 0;
        //        ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], list.Title);

        //        rowsAdded++;
        //    }

        //    return rowsAdded;
        //}

        //private int AddSection_SiteGroups(Worksheet ws, ClientContext ctx, Web web, int startingRow)
        //{
        //    ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "Groups", "SiteName");

        //    Range rng = ws.Cells[startingRow, 1];
        //    int rowsAdded = 0;

        //    var siteGroups = web.SiteGroups;

        //    //IEnumerable<List> groups = ctx.LoadQuery(siteGroups.Where(true));

        //    ctx.Load(siteGroups);

        //    ctx.ExecuteQuery();

        //    foreach (var group in siteGroups)
        //    {
        //        int col = 0;
        //        //ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], group.LoginName); // Seems same as Title
        //        ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], group.Title);
        //        ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], group.Id.ToString());
        //        //ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], group.Owner.LoginName);
        //        ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], group.PrincipalType.ToString());

        //        rowsAdded++;

        //        UserCollection users = ctx.Web.SiteGroups.GetById(group.Id).Users;

        //        ctx.Load(users);
        //        ctx.ExecuteQuery();

        //        foreach (var user in users)
        //        {
        //            col = 4;
        //            //ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], user.LoginName);
        //            ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], user.Title);
        //            ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], user.Id.ToString());
        //            //ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], user.UserId.ToString());
        //            ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], user.PrincipalType.ToString());
        //            rowsAdded++;
        //        }
        //        rowsAdded++;

        //    }

        //    return rowsAdded;
        //}

        ////private int DisplayListOf_Libraries(Worksheet ws, int startingRow)
        ////{
        ////    // The columns in this method need to be kept in sync with CreateTeamProjectsInfo()

        ////    Range rng = ws.Cells[startingRow, 1];
        ////    int rowsAdded = 0;

        ////    foreach (CatalogNode projectNode in projectNodes)
        ////    {
        ////        int col = 0;
        ////        ExcelHlp.AddContentToCell(rng.Offset[rowsAdded, col++], projectNode.Resource.DisplayName);

        ////        rowsAdded++;
        ////    }

        ////    return rowsAdded;
        ////}

        //private int AddSection_WebInfo(Worksheet ws, ClientContext ctx, Web web, int startingRow)
        //{
        //    //ctx.Load(web, info => info.HasUniqueRoleAssignments);
        //    //ctx.Load(web1, 
        //    //    info => info.Url, 
        //    //    info => info.MasterUrl, 
        //    //    info => info.Description,
        //    //    info => info.HasUniqueRoleAssignments,
        //    //    info => info.Created);
        //    //ctx.ExecuteQuery();

        //    ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "Url:", ctx.Url);
        //    ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "Title:", web.Title);
        //    ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "Created:", web.Created.ToString());
        //    ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "Description:", web.Description);
        //    //ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "HasUniqueRoleAssignments:", web.HasUniqueRoleAssignments.ToString());
        //    //ExcelHlp.AddTitledInfo(ws.Cells[startingRow++, 1], "MasterUrl:", web.MasterUrl);

        //    return startingRow;
        //}
        
        //private void CreateWS_SiteCollection_Info(ClientContext ctx, bool param1)
        //{
        //    var site = ctx.Site;
        //    var web = ctx.Web;


        //    ctx.Load(site);
        //    ctx.ExecuteQuery();

        //    // Display Site (SiteCollection stuff)

        //    ctx.Load(web);
        //    ctx.ExecuteQuery();

        //    foreach (Web w in web.Webs)
        //    {
        //        XlHlp.DisplayInWatchWindow(w.Title);
        //    }

        //}

        //private void CreateWS_Web_Info(ClientContext ctx, bool param1)
        //{
        //    var web = ctx.Web;

        //    ctx.Load(web);
        //    ctx.ExecuteQuery();

        //    string sheetName = ExcelHlp.SafeSheetName(string.Format("{0}{1}", "SPWeb>", web.Title));
        //    Worksheet ws = ExcelHlp.NewWorksheet(sheetName, beforeSheetName: "FIRST");

        //    int startingRow = 2;

        //    startingRow += AddSection_WebInfo(ws, ctx, web, startingRow);

        //    startingRow += 2;

        //    startingRow += AddSection_Libraries(ws, ctx, web, startingRow);

        //    startingRow += 2;

        //    startingRow += AddSection_Lists(ws, ctx, web, startingRow);

        //    startingRow += 2;

        //    startingRow += AddSection_SiteGroups(ws, ctx, web, startingRow);

        //    startingRow += 2;
        //}

        //#endregion

        //#region Private Methods
        
        //private void LoadControlContents()
        //{
        //    try
        //    {
        //        spSiteCollection_Picker1.PopulateControlFromFile(Common.cCONFIG_FILE);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //private void PopulateSitesPicker(string uri)
        //{
        //    try
        //    {
        //        using (var ctx = new ClientContext(uri))
        //        {
        //            //ctx.Credentials = new NetworkCredential("user", "password", "domain");

        //            var web = ctx.Web;

        //            ctx.Load(web);
        //            ctx.ExecuteQuery();

        //            ctx.Load(web.Webs);
        //            ctx.ExecuteQuery();

        //            foreach (Web w in web.Webs)
        //            {
        //                ctx.Load(w);
        //                ctx.ExecuteQuery();
        //                XlHlp.DisplayInWatchWindow(w.Title);

        //                //using (var wCtx = new ClientContext(w.Url))
        //                //{
        //                //    wCtx.Load(w);
        //                //    wCtx.ExecuteQuery();
        //                //    XlHlp.DisplayInWatchWindow(w.Title);
        //                //}

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        //#endregion
        
    }
}
