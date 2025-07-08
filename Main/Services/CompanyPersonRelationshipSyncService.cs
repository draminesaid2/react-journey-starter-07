namespace Crm.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model.Relationships;
	using Crm.Model;
	using Crm.Library.Model;

	public class CompanyPersonRelationshipSyncService : DefaultSyncService<CompanyPersonRelationship, Guid>
	{
		public CompanyPersonRelationshipSyncService(IRepositoryWithTypedId<CompanyPersonRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper) : base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override Type[] SyncDependencies => new[] { typeof(Company), typeof(Person) };
		public override CompanyPersonRelationship Save(CompanyPersonRelationship entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override IQueryable<CompanyPersonRelationship> GetAll(User user)
		{
			return repository.GetAll()
					.Where(x => x.Parent.IsActive && x.Child.IsActive && x.Child.ContactType == "Person" && x.Parent.ContactType == "Company");
		}
	}
}
