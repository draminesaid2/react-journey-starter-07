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

	public class PhoneSyncService : DefaultSyncService<Phone, Guid>
	{
		private readonly ICommunicationVisibilityProvider communicationVisibilityProvider;
		public PhoneSyncService(IRepositoryWithTypedId<Phone, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ICommunicationVisibilityProvider communicationVisibilityProvider)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.communicationVisibilityProvider = communicationVisibilityProvider;
		}
		public override Type[] SyncDependencies => new[] { typeof(Contact), typeof(Address) };
		public override IQueryable<Phone> GetAll(User user)
		{
			return communicationVisibilityProvider.FilterByContactVisibility(repository.GetAll(), user);
		}
		public override IQueryable<Phone> Eager(IQueryable<Phone> entities)
		{
			return entities.Fetch(x => x.Contact);
		}
	}
}