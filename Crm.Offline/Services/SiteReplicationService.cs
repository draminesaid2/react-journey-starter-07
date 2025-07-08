namespace Crm.Offline.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model.Site;

	using Main.Replication.Model;
	using Main.Replication.Services.Implementation;

	using NHibernate;

	public class SiteReplicationService : ReplicationService<Site, Guid>
	{
		public override IQueryable<Site> GetNewEntities(IQueryable<Site> entities, Guid clientId)
		{
			return entities.Where(x => !GetReplications(clientId,false).Any(y => y.EntityGuidId == x.UId));
		}
		public override IQueryable<Site> GetUpdatedEntities(IQueryable<Site> entities, Guid clientId)
		{
			return entities.Where(x => GetReplications(clientId, false).Any(y => !(y.EntityModifyDate.Date.Equals(x.ModifiedAt.Date) && y.EntityModifyDate.Hour.Equals(x.ModifiedAt.Hour) && y.EntityModifyDate.Minute.Equals(x.ModifiedAt.Minute) && y.EntityModifyDate.Second.Equals(x.ModifiedAt.Second))));
		}
		public override IEnumerable<Guid> GetPendingEntityIds(IQueryable<Site> entities, Guid clientId)
		{
			return GetNewEntities(entities, clientId).Concat(GetUpdatedEntities(entities, clientId)).Select(x => x.UId);
		}
		public override IEnumerable<Guid> GetExpiredEntityIds(IQueryable<Site> entities, Guid clientId)
		{
			return new List<Guid>().AsQueryable();
		}
		public override Lazy<bool> AnyEntityToSync(IQueryable<Site> entities, Guid clientId)
		{
			return new Lazy<bool>(() => GetPendingEntityIds(entities, clientId).Any());
		}
		public SiteReplicationService(ISessionFactory sessionFactory, IRepositoryWithTypedId<ReplicatedClient, Guid> replicatedClientRepository, IRepositoryWithTypedId<ReplicatedEntity, Guid> replicatedEntityRepository)
			: base(sessionFactory, replicatedClientRepository, replicatedEntityRepository)
		{
		}
	}
}
