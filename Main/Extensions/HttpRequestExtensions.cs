namespace Crm.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;

	using Microsoft.AspNetCore.Http;

	public static class HttpRequestExtensions
	{
		public static List<FileResource> CreateFileResources(this HttpRequest request, Func<FileResource> fileResourceFactory, IResourceManager resourceManager, int maxFileLengthInKb)
		{
			var userService = request.HttpContext.GetService<IUserService>();
			var maxRequestLength = maxFileLengthInKb * 1024;
			var fileResources = new List<FileResource>();

			for (var i = 0; i < request.Form.Files.Count; i++)
			{
				var postedFileBase = request.Form.Files[i];

				if (postedFileBase == null) continue;
				if (postedFileBase.Length == 0) continue;

				if (postedFileBase.Length > maxRequestLength)
					throw new ArgumentException(resourceManager.GetTranslation("FileTooLarge"));

				var fileResource = TransformToFileResource(postedFileBase, fileResourceFactory(), userService.CurrentUser.Id);
				fileResources.Add(fileResource);
			}

			return fileResources;
		}
		public static string ExtractBackUrlForCurrentUrl(this HttpRequest request)
		{
			var returnUrl = request.GetFormValue("ReturnToRemember");
			if (returnUrl != null)
			{
				return returnUrl;
			}
			returnUrl = request.GetTypedHeaders().Referer?.ToString();
			if (returnUrl != null && !returnUrl.Contains("/Account/Login") && (request.Path.Value == null || !returnUrl.EndsWith(request.Path.Value)))
			{
				return returnUrl;
			}

			return null;
		}
		public static FileResource TransformToFileResource(IFormFile postedFileBase, FileResource fileResource, string userId)
		{
			fileResource.ContentType = postedFileBase.ContentType;
			fileResource.Length = postedFileBase.Length;
			fileResource.Filename = Path.GetFileName(postedFileBase.FileName);
			fileResource.Content = postedFileBase.OpenReadStream().ReadAllBytes();
			fileResource.CreateDate = DateTime.Now;
			fileResource.ModifyDate = DateTime.Now;
			fileResource.CreateUser = userId;
			fileResource.ModifyUser = userId;
			return fileResource;
		}
	}
}
