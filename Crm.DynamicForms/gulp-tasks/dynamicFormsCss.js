const generateCssTask = require("../../../gulp-core/gulptaskCore").generateCssTask;

module.exports = function() {
    // Original location : .\Plugins\Crm.DynamicForms
    let cssFiles = [
        "../../Content/style/font-awesome/font-awesome.css",
        "Content/style/Response.less",
        "../../Content/style/TemplateReport.less"
    ];

    generateCssTask(__filename, cssFiles);

};
