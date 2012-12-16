(function () {

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
            '/scripts/app/order/controller/manageAccountController.js',
            '/scripts/app/order/controller/customerPager.js'], function ($, _, Backbone, manageAccountController, customerPager) {
                $(function () {
                    var ctrl = manageAccountController();
                    var csPager = customerPager();
                    ctrl.start();
                    csPager.start();
                });
        });
    }

})();