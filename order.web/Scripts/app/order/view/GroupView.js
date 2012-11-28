/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone','app/eventAggregator'], function ($, _, Backbone, EA) {
    return Backbone.View.extend({
        tagName: 'li',
        template: _.template('<a href="#"><%= Name %></a>'),
        events: {
            'click': function (e) {
                EA.trigger('group:selected', { view: this, model: this.model });
            }
        },
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this;
        }
    });
});