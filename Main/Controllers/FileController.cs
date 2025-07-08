using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using System;
	using System.Drawing;
	using System.IO;

	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Model;
	using Crm.Results;
	using Crm.Services.Interfaces;
	using Crm.ViewModels;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Net.Http.Headers;

	[Authorize]
	public class FileController : Controller
	{
		// Members
		private readonly IFileService fileService;
		private readonly IAppSettingsProvider appSettingsProvider;

		// Methods
		[HttpPost]
		[RequiredPermission(nameof(FileResource), Group = PermissionGroup.WebApi)]
		public virtual ActionResult AddFile(FileResource fileResource)
		{
			if (fileResource == null)
			{
				return new EmptyResult();
			}

			if (Guid.TryParse(fileResource.Filename, out Guid id))
			{
				fileResource.Id = id;
			}

			fileService.SaveFile(fileResource);
			return new EmptyResult();
		}

		[HttpPost]
		[RequiredPermission(PermissionName.Delete, Group = PermissionGroup.File)]
		public virtual ActionResult Delete(Guid id)
		{
			fileService.DeleteFile(id);
			return new EmptyResult();
		}

		[RequiredPermission(PermissionName.GetContent, Group = PermissionGroup.File)]
		public virtual ActionResult File(Guid id)
		{
			var fileResource = fileService.GetFile(id);
			if (fileResource == null)
			{
				return View("Error/Error", ErrorViewModel.NotFound);
			}

			var contentType = String.IsNullOrWhiteSpace(fileResource.ContentType) ? MimeMapping.GetMimeMapping(fileResource.Filename) : fileResource.ContentType; 
			Response.GetTypedHeaders().ContentDisposition = new ContentDispositionHeaderValue("inline") { FileName = fileResource.Filename }; 
			return File(fileResource.Content, contentType);
		}
		
		[RequiredPermission(PermissionName.GetContent, Group = PermissionGroup.File)]
		public virtual ActionResult OpenFile(Guid id)
		{
			var fileResource = fileService.GetFile(id);
			if (fileResource == null)
			{
				return View("Error/Error", ErrorViewModel.NotFound);
			}
			if (fileService.CanOpenWithoutSandbox(fileResource.ContentType, appSettingsProvider.GetValue(MainPlugin.Settings.FileResource.ContentTypesOpenedWithoutSandbox)))
			{
				return File(fileResource.Content, fileResource.ContentType);
			}
			ViewBag.Title = fileResource.Filename;
			ViewBag.FileId = id;
			return View();
		}

		[RequiredPermission(PermissionName.ThumbnailImage, Group = PermissionGroup.File)]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
		public virtual ActionResult ThumbnailImage(Guid id, int width, int height)
		{
			var file = fileService.GetFile(id);

			Image image = null;
			try
			{
				image = Image.FromStream(new MemoryStream(file.Content));
				if (width == 0 && height == 0)
				{
					return new ImageResult(image);
				}

				if (width == 0)
				{
					width = height * image.Width / image.Height;
				}
				else
				{
					height = width * image.Height / image.Width;
				}
				return new ImageResult(image, width, height);
			}
			catch
			{
				image = new Bitmap(1, 1);
				return new ImageResult(image, 1, 1);
			}
			finally
			{
				if (image != null)
				{
					image.Dispose();
				}
			}
		}

		// Constructor
		public FileController(IFileService fileService, IAppSettingsProvider appSettingsProvider)
		{
			this.fileService = fileService;
			this.appSettingsProvider = appSettingsProvider;
		}
	}
}
