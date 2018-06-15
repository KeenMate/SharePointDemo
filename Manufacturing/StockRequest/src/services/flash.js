const flash = function (id) {
	var origColor = $(id).css("background-color");
	$(id).css("transition", "1s");
	$(id).css("background-color", "#FF1A1A");
	setTimeout(function () {
		$(id).css("background-color", origColor);
		setTimeout(function () {
			$(id).css("transition", "");
		}, 1000);
	}, 1000);
}
export default flash