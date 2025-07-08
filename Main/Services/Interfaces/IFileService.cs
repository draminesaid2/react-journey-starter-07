namespace Crm.Services.Interfaces
{
	using System.Collections.Generic;

	using Crm.Library.AutoFac;
	using Crm.Model;
	using System;
	public interface IFileService : IDependency
	{
		// Properties
		long MaxFileSize { get; }

		// Methods
		FileResource GetFile(Guid id);
		void SaveFile(FileResource resource);
		FileResource CreateAndSaveFileResource(byte[] content, string contentType, string fileName);
		void DeleteFile(Guid resource);
		void DeleteFiles(IEnumerable<FileResource> files);
		bool Exists(Guid id);
		bool CanOpenWithoutSandbox(string contentType, string[] allowedFileTypes);
	}
}