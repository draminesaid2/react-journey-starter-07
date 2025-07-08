const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function () {
	// Original location : .\Plugins\Crm.DynamicForms
	let cssFiles = [
		"Content/js/pdfjs/web/viewer.css"
	];

	generateCssTask(__filename, cssFiles);

};
