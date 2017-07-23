using System.Web;
using System.Web.Optimization;

namespace Apps
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/vendor/js/modernizr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/vendor/js/jquery.min.js",
                        "~/Scripts/OwnJs.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/vendor/plugins/switchery.min.css",
                      "~/Content/vendor/css/bootstrap.min.css",
                      "~/Content/vendor/css/core.css",
                      "~/Content/vendor/css/icons.css",
                      "~/Content/vendor/css/components.css",
                      "~/Content/vendor/css/pages.css",
                      "~/Content/vendor/plugins/bootstrap-datepicker/dist/css/bootstrap-datepicker.css",
                      "~/Content/vendor/css/menu.css",
                      "~/Content/vendor/plugins/multi-select.css",
                      "~/Content/vendor/plugins/datatables/jquery.dataTables.min.css",
                      "~/Content/vendor/plugins/datatables/buttons.bootstrap.min.css",
                      "~/Content/vendor/plugins/datatables/fixedHeader.bootstrap.min.css",
                      "~/Content/vendor/plugins/datatables/responsive.bootstrap.min.css",
                      "~/Content/vendor/plugins/datatables/scroller.bootstrap.min.css",
                      "~/Content/vendor/plugins/bootstrap-sweetalert/sweet-alert.css", 
                      "~/Content/Own.css",
                      "~/Content/vendor/css/responsive.css"));

            bundles.Add(new ScriptBundle("~/bundles/LotusScripts").Include(
                        "~/Scripts/vendor/js/bootstrap.min.js",
                        "~/Scripts/vendor/js/detect.js",
                        "~/Scripts/vendor/js/fastclick.js",
                        "~/Scripts/vendor/js/jquery.slimscroll.js",
                        "~/Scripts/vendor/js/jquery.blockUI.js",
                        "~/Scripts/vendor/js/waves.js",
                        "~/Scripts/vendor/js/wow.min.js",
                        "~/Scripts/vendor/js/jquery.nicescroll.js",
                        "~/Scripts/vendor/js/jquery.scrollTo.min.js",
                        "~/Scripts/vendor/plugins/switchery.min.js",
                        "~/Scripts/vendor/plugins/jquery.waypoints.js",
                        "~/Scripts/vendor/plugins/jquery.counterup.min.js",
                        "~/Scripts/vendor/plugins/raphael-min.js",
                        "~/Scripts/vendor/plugins/bootstrap-wizard/jquery.bootstrap.wizard.js",
                        "~/Scripts/vendor/plugins/jquery-validation/dist/jquery.validate.min.js",
                        "~/Scripts/vendor/plugins/bootstrap-datepicker/dist/js/bootstrap-datepicker.js",
                         "~/Scripts/vendor/plugins/bootstrap-sweetalert/sweet-alert.js",
                         "~/Scripts/vendor/plugins/bootstrap-sweetalert/sweet-alert.min.js",
                        "~/Scripts/vendor/plugins/select2/select2.min.js",
                        "~/Scripts/vendor/plugins/jquery.multi-select.js",
                        "~/Scripts/vendor/plugins/jquery.quicksearch.js",
                        "~/Scripts/vendor/plugins/datatables/jquery.dataTables.min.js",
                        "~/Scripts/vendor/plugins/datatables/dataTables.bootstrap.js",
                        "~/Scripts/vendor/plugins/datatables/dataTables.buttons.min.js",
                        "~/Scripts/vendor/plugins/datatables/buttons.bootstrap.min.js",
                        "~/Scripts/vendor/plugins/datatables/jszip.min.js",
                        "~/Scripts/vendor/plugins/datatables/pdfmake.min.js",
                        "~/Scripts/vendor/plugins/datatables/vfs_fonts.js",
                        "~/Scripts/vendor/plugins/datatables/buttons.html5.min.js",
                        "~/Scripts/vendor/plugins/datatables/buttons.print.min.js",
                        "~/Scripts/vendor/plugins/datatables/dataTables.fixedHeader.min.js",
                        "~/Scripts/vendor/plugins/datatables/dataTables.keyTable.min.js",
                        "~/Scripts/vendor/plugins/datatables/dataTables.responsive.min.js",
                        "~/Scripts/vendor/plugins/datatables/responsive.bootstrap.min.js",
                        "~/Scripts/vendor/plugins/datatables/dataTables.scroller.min.js",
                        "~/Scripts/vendor/js/jquery.core.js",
                        "~/Scripts/vendor/js/jquery.app.js"));
        }
    }
}
