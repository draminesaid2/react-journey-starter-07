namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;

	public class CompanyBranchSyncService : DefaultSyncService<CompanyBranch, Guid>
	{
		private readonly ISyncService<Company> companySyncService;
		public CompanyBranchSyncService(IRepositoryWithTypedId<CompanyBranch, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<Company> companySyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.companySyncService = companySyncService;
		}
		public override IQueryable<CompanyBranch> GetAll(User user, IDictionary<string, int?> groups)
		{
			var company = companySyncService.GetAll(user, groups);
			return repository.GetAll()
				.Where(x => company.Any(y => y.Id == x.CompanyKey));
		}
	}
}
