﻿(function ($) {

    $(function () {
        $("#settings").click(function (e) {
            e.preventDefault();
            $('#charms').charms('showSection', 'theme-charms-section');
        });
    });
    
})(jQuery);