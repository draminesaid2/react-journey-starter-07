namespace Crm.Services.Interfaces
{
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Model.Notes;

	public interface INoteService : IDependency
	{
		IQueryable<T> Filter<T>(IQueryable<T> notes)
			where T : Note;
	}
}