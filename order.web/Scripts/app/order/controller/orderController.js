﻿define(['jquery',
        'underscore',
        'backbone',
        'app/order/model/GroupList',
        'app/order/view/GroupListView',
        'app/order/view/ErrorView',
        'app/order/view/ProgressWinRing',
        'app/order/model/ItemList',
        'app/order/model/SearchItemList',
        'app/order/view/ItemListView',
        'bootbox',
        'app/eventAggregator'], function ($, _, Backbone, GroupList, GroupListView, ErrorView, ProgressWinRing, ItemList, SearchItemList, ItemListView, bootbox, EA) {

    return function() {
        var progressWinRing = new ProgressWinRing();
        var groupList = new GroupList();
        var itemList = new ItemList();
        var searchItemList = new SearchItemList();
        var groupListView = new GroupListView({ collection: groupList });
        var itemListView = new ItemListView({ collection: itemList });
        var searchItemListView = new ItemListView({ collection: searchItemList });
        var showError = function (err) {
            var errView = new ErrorView({ model: err });
            $('div.order-error-container').html(errView.render().el);
        };
        var show = function () {
            $('a#back-to-listitem-menu').hide();
            $('div.search-item-view').hide();
            $('div.search-item-view').html(searchItemListView.render().el);
            $('.metro.span12.search-item-view').css('margin-left', '0');
            $('div.group-view').html(groupListView.render().el);
            $('div.item-view').html(itemListView.render().el);
            groupList.fetch({
                beforeSend: function () {
                    $('div.group-view').append(progressWinRing.render().el);
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
                    $('div.item-view').mask("Loading...");
                },
                complete: function () {
                    $('div.item-view').unmask();
                },
                error: function (model, xhr) {
                    showError(new Backbone.Model({ errorMessage: xhr.statusText + " (" + xhr.status + ")" }));
                }
            });
        }

        var searchItem = function (searhQuery) {
            searchItemList.fetch({
                data: { 'searchQuery': searhQuery },
                beforeSend: function () {
                    $('a#back-to-listitem-menu').show();
                    $('div.search-item-view').show();
                    $('div.noitemfound').html('<i><h3>Searching...</h3></i>');
                    $('div.group-view').hide();
                    $('div.item-view').hide();
                    $('div.search-item-view').mask('Loading...');
                },
                complete: function () {
                    $('div.noitemfound').html('<h3>No item found</h3>');
                    $('div.search-item-view').unmask();
                },
                success: function (data) {
                    // add some code here when success
                },
                error: function (model,xhr) {
                    bootbox.modal(xhr.responseText, xhr.statusText);
                }
            });
        }

        EA.on('group:selected', function (data) {
            showItems(data.model);
        }, this);

        $('form#search-item-form').on('submit', function (e) {
            e.preventDefault();
            var searchQuery = $('input.search-query', this).val();
            if ($.trim(searchQuery)) {
                searchItem(searchQuery);
            }
        });

        $('a#back-to-listitem-menu').on('click', function (e) {
            e.preventDefault();
            $('a#back-to-listitem-menu').hide();
            $('div.search-item-view').hide();
            $('div.group-view').show();
            $('div.item-view').show();
        });

        EA.on('order:additem', function (item) {
            console.log(item.toJSON());
        });

        return {
            show: show
        }
    }
});