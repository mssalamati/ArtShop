using System.Web;
using System.Web.Optimization;

namespace Blog
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

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
       "~/Scripts/angular.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
          "~/Scripts/modules-9fa0e7afd9c8b2a8eac97ecf9d682f2d.js",
          "~/Scripts/froogaloop.js", "~/Scripts/jquery.cookie.js", "~/Scripts/1061.js", "~/Scripts/4.8.1.js", "~/Scripts/1.12.4.js", "~/Scripts/1.4.1.js", 
          "~/Scripts/jquery.flexslider-min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/font-awesome.min.css",
                      "~/Content/site.css",
                      "~/Content/style.css",
                      "~/Content/recent-tweets.css",
                      "~/Content/perfect-pullquotes.css",
                      "~/Content/instag-slider.css",
                      "~/Content/dashicons.min.css",
                      "~/Content/css_2.css",
                      "~/Content/easy-pull-quotes-public.css",
                      "~/Content/flexslider.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/default").Include(
                       "~/Content/Admin/styles/webfont.css",
                       "~/Content/Admin/styles/climacons-font.css",
                       "~/Content/Admin/vendor/bootstrap/dist/css/bootstrap.css",
                       "~/Content/Admin/styles/bootstrap-rtl.css",
                       "~/Content/Admin/styles/font-awesome.css",
                       "~/Content/Admin/styles/card.css",
                       "~/Content/Admin/styles/sli.css",
                       "~/Content/Admin/styles/animate.css",
                       "~/Content/Admin/styles/app.css",
                       "~/Content/Admin/styles/app.skins.css"));
        }
    }
}
