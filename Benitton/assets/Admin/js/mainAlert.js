(function () {
	"use strict";

	var treeviewMenu = $('.app-menu');

	// Toggle Sidebar
	$('[data-toggle="sidebar"]').click(function(event) {
		event.preventDefault();
		$('.app').toggleClass('sidenav-toggled');
	});

	// Activate sidebar treeview toggle
	$("[data-toggle='treeview']").click(function(event) {
		event.preventDefault();
		if(!$(this).parent().hasClass('is-expanded')) {
			treeviewMenu.find("[data-toggle='treeview']").parent().removeClass('is-expanded');
		}
		$(this).parent().toggleClass('is-expanded');
	});

	// Set initial active toggle
	$("[data-toggle='treeview.'].is-expanded").parent().toggleClass('is-expanded');

	//Activate bootstrip tooltips
	$("[data-toggle='tooltip']").tooltip();

})();

function alertG(msg) {
    new PNotify({
        title: 'Success',
        text: msg,
        delay: 5000,
        type: 'success',
        addclass: 'pnotify-right'
    });
}
function alertR(msg) {
    new PNotify({
        title: 'Error',
        text: msg,
        delay: 5000,
        type: 'error',
        addclass: 'pnotify-right'
    });
}

function alertInfo(msg) {
    new PNotify({
        title: 'Info',
        text: msg,
        delay: 5000,
        type: 'info',
        addclass: 'pnotify-right'
    });
}
