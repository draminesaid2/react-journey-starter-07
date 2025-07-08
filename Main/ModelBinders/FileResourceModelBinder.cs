namespace Crm.ModelBinders
{
	using System;
	using System.Linq;

	using Crm.Extensions;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.ModelBinder;
	using Crm.Model;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ModelBinding;

	[ModelBinderFor(typeof(FileResource))]
	public class FileResourceModelBinder : CrmModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var request = controllerContext.HttpContext.Request;
			if (request.Form.Files.Count != 1)
			{
				throw new ArgumentException("request must contain exactly one file");
			}

			var appSettingsProvider = controllerContext.HttpContext.GetService<IAppSettingsProvider>();
			var maxFileLength = appSettingsProvider.GetValue(MainPlugin.Settings.System.MaxFileLengthInKb);

			var fileResourceFactory = controllerContext.HttpContext.GetService<Func<FileResource>>();
			var resourceManager = controllerContext.HttpContext.GetService<IResourceManager>();
			return request.CreateFileResources(fileResourceFactory, resourceManager, maxFileLength).Single();
		}
	}
}