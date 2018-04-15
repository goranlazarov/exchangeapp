using System.Web;
using System.Web.Optimization;

namespace ExchangeApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/chosen.jquery.min.js"

                        ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/js/vendor/modernizr-3.5.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/js/bootstrap.min.js",
                      "~/Scripts/js/bootstrap-datepicker.js",
                      "~/Scripts/js/plugins.js",
                      "~/Scripts/js/popper.min.js",
                      "~/Scripts/js/scripts.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/css/bootstrap.min.css",
                       "~/Content/css/bootstrap-datepicker.css",
                       "~/Content/css/chosen.css",
                        "~/Content/css/screen.css",
                        "~/Content/css/font-awesome.css",
                        "~/Content/css/custom.css",
                        "~/Content/css/combined.css",
                        "~/Content/css/ionicons.css",
                        "~/Content/toastr.css",
                        "~/Content/plugins/datepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                       "~/Scripts/toastr.js*"));
            bundles.Add(new ScriptBundle("~/bundles/script-custom-editor").Include(
                                             "~/Scripts/js/script-custom-editor.js"));

        }


    }
}
