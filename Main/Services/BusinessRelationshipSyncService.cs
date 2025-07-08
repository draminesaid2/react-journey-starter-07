namespace Crm.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model;
	using Crm.Model.Relationships;

	public class BusinessRelationshipSyncService : DefaultSyncService<BusinessRelationship, Guid>
	{
		public BusinessRelationshipSyncService(IRepositoryWithTypedId<BusinessRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override Type[] SyncDependencies => new[] { typeof(Company) };
		public override IQueryable<BusinessRelationship> GetAll(User user)
		{
			return repository.GetAll()
					.Where(x => x.Parent.IsActive && x.Child.IsActive&& x.Child.ContactType == "Company" && x.Parent.ContactType == "Company");
		}
		public override BusinessRelationship Save(BusinessRelationship entity)
		{
			return repository.SaveOrUpdate(entity);
		}
	}
}