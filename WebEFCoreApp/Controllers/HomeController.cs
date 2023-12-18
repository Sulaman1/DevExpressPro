using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using System.Linq;
using System.Threading.Tasks;
//using AspNetCore.Reporting.Common.Models;
//using AspNetCore.Reporting.Common.Services;
//using AspNetCore.Reporting.MVC.Data;
//using DBContext.Data;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Wizard.Services;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebEFCoreApp.Models;

namespace WebEFCoreApp.Controllers {
    public class HomeController : Controller {
        const string QueryBuilderHandlerUri = "/DXXQBMVC";
        const string ReportDesignerHandlerUri = "/DXXRDMVC";
        const string ReportViewerHandlerUri = "/DXXRDVMVC";
        public IActionResult Index() {
            return View();
        }
        public IActionResult Error() {
            Models.ErrorModel model = new Models.ErrorModel();
            return View(model);
        }
        public IActionResult DesignReport([FromServices] IReportDesignerClientSideModelGenerator clientSideModelGenerator,
                                  [FromServices] IConnectionProviderService connectionProviderService, ReportingControlModel controlModel)
        {
            Models.CustomDesignerModel model = new Models.CustomDesignerModel();
            var report = string.IsNullOrEmpty(controlModel.Id) ? new XtraReport() : null;
            model.DesignerModel = CreateReportDesignerModel(clientSideModelGenerator, connectionProviderService, controlModel.Id, report);
            model.Title = controlModel.Title;

            return View(model);
        }
        public static ReportDesignerModel CreateReportDesignerModel(IReportDesignerClientSideModelGenerator clientSideModelGenerator, IConnectionProviderService connectionProviderService, string reportName, XtraReport report)
        {
            var dataSources = GetAvailableDataSources(connectionProviderService);
            if (report != null)
            {
                return clientSideModelGenerator.GetModel(report, dataSources, ReportDesignerHandlerUri, ReportViewerHandlerUri, QueryBuilderHandlerUri);
            }
            return clientSideModelGenerator.GetModel(reportName, dataSources, ReportDesignerHandlerUri, ReportViewerHandlerUri, QueryBuilderHandlerUri);
        }
        public static Dictionary<string, object> GetAvailableDataSources(IConnectionProviderService connectionProviderService)
        {
            var dataSources = new Dictionary<string, object>();
            // Create a SQL data source with the specified connection string.
            //SqlDataSource ds = new SqlDataSource("NWindConnectionString");
            SqlDataSource ds = new SqlDataSource("Reporting");
            ds.ReplaceService(connectionProviderService, noThrow: true);
            // Create a SQL query to access the Products data table.
            //SelectQuery query = SelectQueryFluentBuilder.AddTable("Products").SelectAllColumnsFromTable().Build("Products");
            //ds.Queries.Add(query);
            //ds.RebuildResultSchema();
            dataSources.Add("Reporting", ds);
            return dataSources;
        }

        public IActionResult Viewer(
            [FromServices] IWebDocumentViewerClientSideModelGenerator clientSideModelGenerator,
            [FromQuery] string reportName) {

            var reportToOpen = string.IsNullOrEmpty(reportName) ? "TestReport" : reportName;
            var model = new Models.ViewerModel {
                ViewerModelToBind = clientSideModelGenerator.GetModel(reportToOpen, WebDocumentViewerController.DefaultUri)
            };
            return View(model);
        }
    }
}
