namespace Crm.Services
{
	using AutoMapper;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using System;

	public class StationSyncService : DefaultSyncService<Station, Guid>
	{
		public StationSyncService(IRepositoryWithTypedId<Station, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
	}
}