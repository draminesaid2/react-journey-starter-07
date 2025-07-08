using System;
using System.Linq;

namespace Crm.Article.Services
{
	using AutoMapper;

	using Crm.Article.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;

	public class ProductFamilySyncService : DefaultSyncService<ProductFamily, Guid>
	{
		private readonly IVisibilityProvider visibilityProvider;
		public ProductFamilySyncService(IRepositoryWithTypedId<ProductFamily, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IVisibilityProvider visibilityProvider)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.visibilityProvider = visibilityProvider;
		}
		public override IQueryable<ProductFamily> GetAll(User user)
		{
			var query = repository.GetAll();
			return visibilityProvider.FilterByVisibility(query);
		}
	}
}