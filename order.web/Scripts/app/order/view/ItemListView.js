/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/order/view/ItemView', 'app/eventAggregator'], function ($, _, Backbone, ItemView, EA) {
    return Backbone.View.extend({
        className: 'listview-container grid-layout zain-listview-container',
        initialize: function () {
            this.collection.on('reset', this.addAllItems, this);
        },
        render: function () {
            this.addAllItems();
            return this;
        },
        addAllItems: function () {
            this.$el.empty();
            this.collection.forEach(function (item) {
                this.addItem(item);
            }, this);
            if (this.collection.length === 0) {
                this.$el.append('<div class="noitemfound"><h3>No item found</h3></div>');
            }
        },
        addItem: function (item) {
            var view = new ItemView({ model: item });
            view.render();
            this.$el.append(view.el);
            this.$el.append('<hr class="row-separator"/>');
        }
    });
});