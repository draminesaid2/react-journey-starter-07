namespace Crm.Services.Interfaces
{
	using System.Collections.Generic;
	using Crm.Library.AutoFac;

	public interface IBravoService : ITransientDependency
	{
		IEnumerable<string> GetUsedBravoCategories();
	}
}
