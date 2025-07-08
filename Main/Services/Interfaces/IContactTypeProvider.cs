namespace Crm.Services.Interfaces
{
	using System.Collections.Generic;

	using Crm.Library.AutoFac;

	public interface IContactTypeProvider : ISingletonDependency
	{
		List<string> ContactTypes { get; }
	}
}