const getCurrentUser = window.Helper.User.getCurrentUser;
window.Helper.User.getCurrentUser = function() {
	const username = window.Helper.User.getCurrentUserName();
	if (window.Helper.Offline.status === "offline" && window.database.Main_User && username) {
		var d = $.Deferred();
		window.database.Main_User.find(username).then(d.resolve).catch(d.reject);
		return d.promise();
	}
	return getCurrentUser();
};
export {};
