namespace Crm.Article.Services
{
	using System.Linq;
	using AutoMapper;
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;

	public class ProductFamilyDescriptionSyncService : DefaultLookupSyncService<ProductFamilyDescription>
	{
		private readonly ISyncService<ProductFamily> productFamilySyncService;
		public ProductFamilyDescriptionSyncService(IRepositoryWithTypedId<ProductFamilyDescription, int> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<ProductFamily> productFamilySyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.productFamilySyncService = productFamilySyncService;
		}
		public override IQueryable<ProductFamilyDescription> GetAll(User user)
		{
			var productFamilies = productFamilySyncService.GetAll(user);
			return base.GetAll(user)
				.Where(x => productFamilies.Any(y => y.Id.ToString() == x.Key));
		}
		public override ProductFamilyDescription Save(ProductFamilyDescription entity)
		{
			return repository.SaveOrUpdate(entity);
		}

		public override void Remove(ProductFamilyDescription entity)
		{
			repository.Delete(entity);
		}
	}
}