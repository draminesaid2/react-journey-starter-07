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
	using Crm.Services.Interfaces;

	using NHibernate.Linq;

	public class FaxSyncService : DefaultSyncService<Fax, Guid>
	{
		private readonly ICommunicationVisibilityProvider communicationVisibilityProvider;
		public FaxSyncService(IRepositoryWithTypedId<Fax, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ICommunicationVisibilityProvider communicationVisibilityProvider)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.communicationVisibilityProvider = communicationVisibilityProvider;
		}
		public override Type[] SyncDependencies => new[] { typeof(Contact), typeof(Address) };
		public override IQueryable<Fax> GetAll(User user)
		{
			return communicationVisibilityProvider.FilterByContactVisibility(repository.GetAll(), user);
		}
		public override IQueryable<Fax> Eager(IQueryable<Fax> entities)
		{
			return entities.Fetch(x => x.Contact);
		}
	}
}