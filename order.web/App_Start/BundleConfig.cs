﻿using System.Web;
using System.Web.Optimization;

namespace order.web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery",
                "//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsextlibs").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.blockUI.js",
                "~/Scripts/jquery.form.js",
                "~/Scripts/jquery.mousewheel.js",
                "~/Scripts/jquery.scrollTo.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/bootmetro.js",
                "~/Scripts/bootmetro-charms.js",
                "~/Scripts/bootbox.js",
                "~/Scripts/daterangepicker.js",
                "~/Scripts/holder.js",
                "~/Scripts/mustache.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsapp").Include(
                "~/Scripts/app/home.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsregister").Include(
                "~/Scripts/app/register.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsorder").Include(
                "~/Scripts/underscore.min.js",
                "~/Scripts/backbone.min.js",
                "~/Scripts/jquery.formatCurrency-1.4.0.min.js",
                "~/Scripts/i18n/jquery.formatCurrency.all.js"));

            /*bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));*/

            /*bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));*/

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css")); 
            bundles.Add(new StyleBundle("~/Content/css/base").Include("~/Content/base.css"));
            bundles.Add(new StyleBundle("~/Content/css/bootmetro").Include(
                "~/Content/bootmetro/css/bootstrap.css",
                "~/Content/bootmetro/css/bootstrap-responsive.css",
                "~/Content/bootmetro/css/bootmetro.css",
                "~/Content/bootmetro/css/bootmetro-tiles.css",
                "~/Content/bootmetro/css/bootmetro-charms.css",
                "~/Content/bootmetro/css/metro-ui-light.css",
                "~/Content/bootmetro/css/icomoon.css",
                "~/Content/bootmetro/css/datepicker.css",
                "~/Content/bootmetro/css/daterangepicker.css"));
            bundles.Add(new StyleBundle("~/Content/css/order").Include(
                "~/Content/order.css", 
                "~/Content/jquery.loadmask.css"));

            bundles.Add(new StyleBundle("~/Content/css/manageaccount").Include(
                "~/Content/manageaccount.css",
                "~/Content/jquery.loadmask.css"));

            /*bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));*/
        }
    }
}