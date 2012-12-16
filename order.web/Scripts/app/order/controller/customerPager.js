define(['jquery',
        'underscore',
        'backbone'],
function ($, _, Backbone, ChangePasswordView) {

    return function () {
        var pageSize = 3;
        var count = 0;
        var fetchingNumberOfCustomers;
        var fetchingNextPage;
        
        var fetchNumberOfCustomers = function () {
            if(fetchingNumberOfCustomers)
                fetchingNumberOfCustomers.abort();

            fetchingNumberOfCustomers = $.ajax('/Customer/Count', {
                dataType: 'json',
                cache: false,
                beforeSend: function () {
                    
                },
                complete: function () {
                    fetchingNumberOfCustomers = null;
                },
                success: function (data) {
                    count = data.count;
                    showNextPageIfNecessary();
                },
                error: function (xhr,status,err) {
                    bootbox.alert(xhr.responseText);
                }
            });
        };

        var showNextPageIfNecessary = function () {
            $('div.next-page-button-container').remove();
            if ($('div.zain-listviewitem').length < count) {
                $('div.zain-listview-container').append('<div class="next-page-button-container"><a href="#" class="next-page-button" aria-hidden="true" data-icon=""></a></div>');
            }
        };

        var bindNextPageButton = function () {
            $('div.zain-listview-container').delegate('a.next-page-button', 'click', function (e) {
                e.preventDefault();
                fetchNextPage();
            });
        }

        var fetchNextPage = function () {
            if (fetchingNextPage)
                fetchingNextPage.abort();

            fetchingNextPage = $.ajax('/Customer/NextPage', {
                dataType: 'json',
                cache: false,
                data: { skip: $('div.zain-listviewitem').length },
                beforeSend: function () {
                    $('div.zain-listview-container').mask('Loading next page...');
                },
                complete: function () {
                    fetchingNextPage = null;
                    $('div.zain-listview-container').unmask();
                },
                success: function (data) {
                    _.each(data, addItem, this);
                    showNextPageIfNecessary();
                },
                error: function (xhr, status, err) {
                    bootbox.alert(xhr.responseText);
                }
            });
        };

        var addItem = function (item) {
            var html = _.template('<div class="mediumListIconTextItem zain-listviewitem">\
                        <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFoAAABaCAYAAAA4qEECAAACqElEQVR4Xu3bvY8BQRzG8d9GiJda6ES0lEL8+yoK0YlatBQSide7m0nmgnDGWV93yaOyt7PzrM8+O7vNJYvF4sP0eblAIuiXG/sAQTPOgoacBS1oSgDK0RotaEgAilGjBQ0JQDFqtKAhAShGjRY0JADFqNGChgSgGDVa0JAAFKNGCxoSgGLUaEFDAlCMGi1oSACKUaMFDQlAMWq0oCEBKEaNFjQkAMWo0YKGBKAYNVrQkAAUo0YLGhKAYtRoQUMCUIwaLWhIAIp5S6Nns5lNp1P/E5MksV6vZ4VCwW9//TuejcdjOx6PfrvZbFq1Wr3L8Yo574Y+MACHDpDlctlarZaNRiNbr9fW6XQ8er/ft2Kx6LdP92Wz2Zs/6xVzPmAYNRSHDs2r1+vWaDQsbLvm7nY73/Rr+3K5nG96qVTyF2Eymdh8PveN/+m4n/bF3ClRihGD/gy0w3WY16ADvGv4crm0brdrw+Hwu/m3Ll7MnBFGqQzBocNtHprp8NzfHMp2u/UtvWx02N7v935pORwOZ2v7M3OmohgxCQ7tzun0weVa7ABrtdrdRrtjw4WpVCp+jQ+fZ+aMcHp6yFugT886do1262lornswbjabm28kj8z5tGDkBDj0tds85q0jk8nYYDDwy0a73fbNdt/dg3G1Wp09KNN4k4n0ix6GQ7szC28M7ns+n/dY4fXt1nt0OOZy/Q5LyG/mjFZKYeBboFM47383haChSyZoQUMCUIwaLWhIAIpRowUNCUAxarSgIQEoRo0WNCQAxajRgoYEoBg1WtCQABSjRgsaEoBi1GhBQwJQjBotaEgAilGjBQ0JQDFqtKAhAShGjRY0JADFqNGChgSgGDVa0JAAFKNGCxoSgGLUaEFDAlCMGi1oSACKUaMFDQlAMWo0BP0Jeyr5I6MnsB4AAAAASUVORK5CYII=" class="mediumListIconTextItem-Image" data-src="holder.js/90x90" alt="90x90" style="width: 90px; height: 90px; background-color: rgb(238, 238, 238);">\
                        <div class="mediumListIconTextItem-Detail">\
                            <h3 class="zain-heading"><%= CustomerName %></h3>\
                            <span class="label label-info"><%= LicenseId %></span>\
                        <div>\
                            <span class="label label-warning"><%= ServiceBusNamespace %></span>\
                            <span class="label label-warning"><%= Issuer %></span>\
                        </div>\
                        <div><span class="label label-danger"><%= SecretKey %></span></div>\
                            <div class="zain-action-group">\
                                <a class="btn btn-primary" href="/Customer/EditCustomer/<%= Id %>" id="btn-edit-customer">Edit</a>\
                                <a class="btn btn-warning" href="/Customer/DeleteCustomer/<%= Id %>" id="btn-delete-customer">Delete</a>\
                            </div>\
                        </div>\
                       </div>', item);
            $('div.zain-listview-container').append(html);
        };

        var start = function () {
            bindNextPageButton();
            fetchNumberOfCustomers();
        }

        return {
            start: start
        }
    }
});