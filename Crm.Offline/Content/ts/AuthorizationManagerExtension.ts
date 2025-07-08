///<reference path="../../../../Content/@types/index.d.ts"/>

import {AuthorizationManager} from "../../../../Content/ts/AuthorizationManager";

function getImportedPermissions(username, permissionName) {
	const result = window.AuthorizationManager.users[username][permissionName].ImportedPermissions || [];
	let importedPermissions = [];
	result.forEach(x => {
		importedPermissions = importedPermissions.concat(getImportedPermissions(username, x));
	});
	importedPermissions.forEach(x => {
		if (result.indexOf(x) === -1) {
			result.push(x);
		}
	});
	return result;
}

window.AuthorizationManager = new class extends AuthorizationManager {
	userHasPermission(username, permissionName) {
		let hasPermission = super.userHasPermission(username, permissionName);
		if (window.Helper.Offline.status === "online") {
			return hasPermission;
		}
		if (permissionName.indexOf("WebAPI::") === 0) {
			hasPermission = hasPermission && super.userHasPermission(username, permissionName.replace("WebAPI::", "Sync::"));
		}
		if (hasPermission) {
			getImportedPermissions(username, permissionName).forEach((importedPermission) => {
				if (importedPermission.indexOf("WebAPI::") === 0) {
					hasPermission = hasPermission && super.userHasPermission(username, importedPermission.replace("WebAPI::", "Sync::"));
				}
			});
		}
		return hasPermission;
	}
}();

