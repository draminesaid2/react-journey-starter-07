namespace Crm.Services
{
	using System.Collections.Generic;
	using System.IO;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Environment.FileSystems.Storage;
	using Crm.Library.Helper;
	using Crm.Model;
	using Crm.Services.Interfaces;
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Linq;

	using Crm.Library.Extensions;

	using log4net;

	public class FileService : IFileService
	{
		public static long MaximumFileSize;

		// Members
		private readonly IRepositoryWithTypedId<FileResource, Guid> fileRepository;
		private readonly IStorageFolder storage;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<FileResource> fileResourceFactory;
		private static readonly ILog Logger = LogManager.GetLogger(typeof(FileService));

		// Properties
		public virtual long MaxFileSize
		{
			get { return MaximumFileSize; }
		}

		// Methods
		public virtual FileResource GetFile(Guid id)
		{
			var fileResource = fileRepository.Get(id);

			if(fileResource == null)
			{
				return null;
			}

			// Backwards compatibility, this is just needed as long as still files exist that aren't transferred to
			// the new SQL Server FILESTREAM Storage, in fact this needs to be around as long as no one accesses
			// files in the dusty attic
			if (fileResource.Content == null)
			{
				var file = storage.RetrieveBytes(fileResource.Path);
				if (file != null && file.Length > 0)
				{
					fileResource.Content = file;
					fileRepository.SaveOrUpdate(fileResource);
				}
			}

			return fileResource;
		}
		public virtual void SaveFile(FileResource resource)
		{
			OrientImage(resource);
			fileRepository.SaveOrUpdate(resource);
		}
		public virtual FileResource CreateAndSaveFileResource(byte[] content, string contentType, string fileName)
		{
			var fileResource = fileResourceFactory();
			fileResource.Content = content;
			fileResource.ContentType = contentType;
			fileResource.Filename = fileName;
			fileResource.Length = content.Length;
			SaveFile(fileResource);
			return fileResource;
		}
		public virtual void DeleteFile(Guid fileId)
		{
			var file = fileRepository.Get(fileId);
			DeleteFiles(new[] { file });
		}
		public virtual void DeleteFiles(IEnumerable<FileResource> files)
		{
			foreach (FileResource file in files)
			{
				fileRepository.Delete(file);
				if (file.Content == null)
				{
					if (file.Path != null)
					{
						storage.DeleteFile(file.Path);
						var directory = Path.GetDirectoryName(file.Path);
						if (storage.DirectoryExists(directory) && storage.DirectoryIsEmpty(directory))
							storage.DeleteDirectory(directory);
					}
				}
			}
		}
		public virtual bool Exists(Guid id)
		{
			var resource = fileRepository.Get(id);
			return (resource.Content != null || storage.FileExists(resource.Path));
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
		public virtual void OrientImage(FileResource fileResource)
		{
			if (fileResource.Content != null && fileResource.Content.Length > 0 && fileResource.ContentType.IsNotNullOrEmpty() && fileResource.ContentType == "image/jpeg")
			{
				using (var memoryStream = new MemoryStream(fileResource.Content))
				{
					var image = Image.FromStream(memoryStream);
					if (Array.IndexOf(image.PropertyIdList, 274) > -1)
					{
						var orientation = (int)image.GetPropertyItem(274).Value[0];
						switch (orientation)
						{
							case 1:
								return;
							case 2:
								image.RotateFlip(RotateFlipType.RotateNoneFlipX);
								break;
							case 3:
								image.RotateFlip(RotateFlipType.Rotate180FlipNone);
								break;
							case 4:
								image.RotateFlip(RotateFlipType.Rotate180FlipX);
								break;
							case 5:
								image.RotateFlip(RotateFlipType.Rotate90FlipX);
								break;
							case 6:
								image.RotateFlip(RotateFlipType.Rotate90FlipNone);
								break;
							case 7:
								image.RotateFlip(RotateFlipType.Rotate270FlipX);
								break;
							case 8:
								image.RotateFlip(RotateFlipType.Rotate270FlipNone);
								break;
						}

						image.RemovePropertyItem(274);
					}

					using (var memoryStream2 = new MemoryStream())
					{
						try
						{
							ImageCodecInfo imageEncoder = ImageCodecInfo.GetImageEncoders().FirstOrDefault(encoder => encoder.MimeType == fileResource.ContentType);
							var imageFormat = imageEncoder != null ? new ImageFormat(imageEncoder.FormatID) : ImageFormat.Jpeg;
							image.Save(memoryStream2, imageFormat);
							fileResource.Content = memoryStream2.ToArray();
						}
						catch(Exception e)
						{
							Logger.Warn(e.Message + "\n" + e.StackTrace);
						}
					}
				}
			}
		}

		public virtual bool CanOpenWithoutSandbox(string contentType, string[] allowedFileTypes) =>
			allowedFileTypes.Length == 0
			|| allowedFileTypes.Contains(contentType)
			|| (allowedFileTypes.Contains("image/*") && contentType.StartsWith("image/"));

		// Constructor
		protected FileService(Func<FileResource> fileResourceFactory)
		{
			this.fileResourceFactory = fileResourceFactory;
		}
		public FileService(IRepositoryWithTypedId<FileResource, Guid> fileRepository, IStorageFolder storage, IAppSettingsProvider appSettingsProvider, Func<FileResource> fileResourceFactory)
		{
			this.fileRepository = fileRepository;
			this.storage = storage;
			this.appSettingsProvider = appSettingsProvider;
			this.fileResourceFactory = fileResourceFactory;
			MaximumFileSize = appSettingsProvider.GetValue(MainPlugin.Settings.System.MaxFileLengthInKb);
		}
	}
}
