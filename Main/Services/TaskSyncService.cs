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

	public class TaskSyncService : DefaultSyncService<Task, Guid>
	{
		public TaskSyncService(IRepositoryWithTypedId<Task, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(Contact) }; }
		}
		public override IQueryable<Task> GetAll(User user)
		{
			return repository.GetAll();
		}
	}
}