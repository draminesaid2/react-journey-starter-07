namespace Crm.Model
{
	using System;

	using Crm.Model.Interfaces;

	public interface IWithFolder
	{
		Guid? FolderId { get; set; }
		string FolderName { get; }
		Folder Folder { get; set; }
	}

	public class Folder : Contact, IEntityWithTags
	{
	}
}