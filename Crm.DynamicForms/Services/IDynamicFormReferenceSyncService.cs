namespace Crm.DynamicForms.Services
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.DynamicForms.Model;
	using Crm.Library.AutoFac;
	using Crm.Library.Model;

	public interface IDynamicFormReferenceSyncService : IDependency
	{
		IQueryable<DynamicFormReference> GetAllDynamicFormReferences(User user, IDictionary<string, int?> groups);
	}
}
