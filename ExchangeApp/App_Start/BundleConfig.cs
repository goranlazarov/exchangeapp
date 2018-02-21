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
                        "~/Scripts/js/vendor/jquery-3.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/js/vendor/modernizr-3.5.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/js/bootstrap.min.js",
                      "~/Scripts/js/active.js",
                      "~/Scripts/js/plugins.js",
                      "~/Scripts/js/popper.min.js",
                      "~/Scripts/js/scripts.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/css/bootstrap.min.css",
                    "~/Content/css/custom.css",
                    "~/Content/css/plugins.css",
                    "~/Content/css/style.css",
                    "~/Content/css/plugins/animate.min.css",
                    "~/Content/css/plugins/animated-headline.css",
                     "~/Content/css/plugins/calender.css",
                    "~/Content/css/plugins/datepicker.min.css",
                     "~/Content/css/plugins/fakeloader.css",
                    "~/Content/css/plugins/flaticon.css",
                     "~/Content/css/plugins/font-awesome.min.css",
                    "~/Content/css/plugins/meanmenu.css",
                     "~/Content/css/plugins/pe-icon-7-stroke.css",
                     "~/Content/css/plugins/plyr.min.css",
                    "~/Content/css/plugins/selectpicker.min.css",
                     "~/Content/css/plugins/slick.min.css",
                        "~/Content/css/plugins/youtubepopup.min.css"
                        "~/Content/toastr.css"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                       "~/Scripts/toastr.js*"));
                  
        }


    }
}
