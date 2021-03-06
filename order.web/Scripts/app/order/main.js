﻿(function () {

    var root = this;
    load3rdPartyModules();
    loadPluginsAndBoot();

    function load3rdPartyModules() {
        define('jquery', [], function () { return jQuery; });
        define('underscore', [], function () { return _; });
        define('backbone', [], function () { return Backbone; });
        define('bootbox', [], function () { return bootbox; });
    }

    function loadPluginsAndBoot() {
        requirejs(['/Scripts/jquery.loadmask.min.js',
                   '/Scripts/moment.min.js',
                   '/Scripts/jquery.formatCurrency-1.4.0.min.js',
                   '/Scripts/i18n/jquery.formatCurrency.all.js'], boot);
    }

    function boot() {
        requirejs.config({
            baseUrl: 'Scripts'
        });
        require([
            'jquery',
            'underscore',
            'backbone',
            'app/order/controller/orderController'], function ($, _, Backbone, orderController) {
                $(function () {
                    orderController().show();
                });
        });
    }

})();