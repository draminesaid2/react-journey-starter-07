namespace Crm.Services.Interfaces
{
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Model.Notes;

	public interface INoteFilter : IDependency
	{
		IQueryable<Note> Filter(IQueryable<Note> notes);
	}
}