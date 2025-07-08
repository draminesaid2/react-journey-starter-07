const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function () {
	let cssFiles = [
		"Content/style/DynamicFormDesigner.less"
	];
	generateCssTask(__filename, cssFiles);
};
