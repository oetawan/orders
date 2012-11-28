/// <reference path="../../../backbone.js" />
/// <reference path="../../../underscore.js" />

define(['jquery', 'underscore', 'backbone', 'app/order/view/GroupView', 'app/eventAggregator'], function ($, _, Backbone, GroupView, EA) {
    return Backbone.View.extend({
        tagName: 'ul',
        className: 'nav nav-tabs nav-stacked',
        initialize: function () {
            this.collection.on('reset', this.addAllGroups, this);
            this.listViewItems = [];
            EA.on('group:selected', this.toggleActiveListViewItem, this);
        },
        render: function () {
            this.addAllGroups();
            return this;
        },
        addAllGroups: function () {
            this.$el.empty();
            this.listViewItems = [];
            this.collection.forEach(function (item) {
                this.addGroupItem(item);
            }, this);
            if (this.listViewItems.length > 0) {
                EA.trigger('group:selected', { view: this.listViewItems[0], model: this.listViewItems[0].model });
            }
        },
        addGroupItem: function (item) {
            var view = new GroupView({ model: item });
            this.listViewItems.push(view);
            view.render();
            this.$el.append(view.el);
        },
        toggleActiveListViewItem: function (data) {
            $('.active', this.$el).removeClass('active');
            $(data.view.el).addClass('active');
        }
    });
});