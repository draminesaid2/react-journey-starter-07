namespace Crm.Services
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Extensions;
	using Crm.Library.Modularization;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class ContactTypeProvider : IContactTypeProvider
	{
		public virtual List<string> ContactTypes { get; }

		public ContactTypeProvider(IEnumerable<Plugin> activePlugins)
		{
			ContactTypes = activePlugins.SelectMany(x => x.Assembly.GetTypesInheriting<Contact>()).Select(x => x.Name).ToList();
		}
	}
}
