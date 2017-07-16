using System.Web;
using System.Web.Optimization;

namespace ArtShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/singleproduct").Include(
                   "~/Content/productview.js"));


            bundles.Add(new ScriptBundle("~/bundles/layoutscript").Include(
                   "~/Content/Vendor/ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js",
                   "~/Content/Vendor/ajax.googleapis.com/ajax/libs/jqueryui/1.10.3/jquery-ui.min.js",
                   "~/Content/Vendor/code.jquery.com/jquery-migrate-1.2.1.min.js"));

            bundles.Add(new StyleBundle("~/Content/scss").Include(
                      "~/Content/Vendor/maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css",
                      "~/Content/Vendor/ajax.googleapis.com/ajax/libs/jqueryui/1.8.6/themes/smoothness/jquery-ui.css",
                      "~/Content/main-1497066374081.css"));
        }
    }
}
