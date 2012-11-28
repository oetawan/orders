/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone'], function ($, _, Backbone) {
    return Backbone.View.extend({
        className: 'alert alert-block',
        template: _.template('<h4><%= errorMessage %></h4>'),
        render: function () {
            this.$el.html('<button type="button" class="close" data-dismiss="alert"></button>');
            this.$el.append(this.template(this.model.toJSON()));

            return this;
        }
    });
});