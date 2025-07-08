namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Library.AutoFac;
	using Crm.Model;

	public interface IPersonService : ITransientDependency
	{
		IQueryable<Person> GetPersons();
		Person GetPerson(Guid personId);
		void SavePerson(Person person);
		void DeletePerson(Guid personId);
		IEnumerable<string> GetUsedBusinessTitles();
		IEnumerable<string> GetUsedDepartmentTypes();
		IEnumerable<string> GetUsedSalutations();
		IEnumerable<string> GetUsedSalutationLetters();
		IEnumerable<string> GetUsedTitles();
	}
}
