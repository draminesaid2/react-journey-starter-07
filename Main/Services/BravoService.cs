namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class BravoService : IBravoService
	{
		// Members
		private readonly IRepositoryWithTypedId<Bravo, Guid> bravoRepository;

		public virtual IEnumerable<string> GetUsedBravoCategories()
		{
			return bravoRepository.GetAll().Select(c => c.CategoryKey).Distinct();
		}

		public BravoService(IRepositoryWithTypedId<Bravo, Guid> bravoRepository)
		{
			this.bravoRepository = bravoRepository;
		}
	}
}
