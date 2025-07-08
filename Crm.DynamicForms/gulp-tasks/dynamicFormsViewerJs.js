const generateJsTask = require("../../../gulp-core/gulptaskCore").generateJsTask;

module.exports = function () {
	// Original location : .\Plugins\Crm.DynamicForms
	let jsFiles = [
		"Content/js/pdfjs/build/pdf.js",
		"Content/js/pdfjs/web/viewer.js",
		"Content/js/pdfjs/web/customToolbar.js"
	];

	generateJsTask(__filename, jsFiles);


};
