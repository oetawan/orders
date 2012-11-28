/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone'], function ($, _, Backbone) {
    return Backbone.View.extend({
        className: 'progress progress-indeterminate',
        render: function () {
            this.$el.html('<div class="win-ring small"></div>');
            return this;
        }
    });
});