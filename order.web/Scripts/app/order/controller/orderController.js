define(['jquery',
        'underscore',
        'backbone',
        'app/order/model/GroupList',
        'app/order/view/GroupListView',
        'app/order/view/ErrorView',
        'app/order/view/ProgressWinRing',
        'app/order/model/ItemList',
        'app/order/view/ItemListView',
        'app/eventAggregator'], function ($, _, Backbone, GroupList, GroupListView, ErrorView, ProgressWinRing, ItemList, ItemListView, EA) {

    return function() {
        var progressWinRing = new ProgressWinRing();
        var groupList = new GroupList();
        var itemList = new ItemList();
        var groupListView = new GroupListView({ collection: groupList });
        var itemListView = new ItemListView({collection: itemList});
        var showError = function (err) {
            var errView = new ErrorView({ model: err });
            $('div.order-error-container').html(errView.render().el);
        };
        var show = function () {
            $('div.metro.span2').html(groupListView.render().el);
            $('div.metro.span10').html(itemListView.render().el);
            groupList.fetch({
                beforeSend: function () {
                    $('div.metro.span2').append(progressWinRing.render().el);
                },
                complete: function () {
                    progressWinRing.remove();
                },
                error: function (model,xhr) {
                    showError(new Backbone.Model({errorMessage: xhr.statusText + " (" + xhr.status + ")"}));
                }
            });
        };
        var showItems = function (item) {
            itemList.fetch({
                data: {
                    groupId: item.get('GroupingId')
                },
                beforeSend: function () {
                    $('div.metro.span10').mask("Loading...");
                },
                complete: function () {
                    $('div.metro.span10').unmask();
                },
                error: function (model, xhr) {
                    showError(new Backbone.Model({ errorMessage: xhr.statusText + " (" + xhr.status + ")" }));
                }
            });
        }

        EA.on('group:selected', function (data) {
            showItems(data.model);
        }, this);

        return {
            show: show
        }
    }
});