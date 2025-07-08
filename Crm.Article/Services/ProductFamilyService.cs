namespace Crm.Article.Services
{
	using System;
	using Crm.Article.Model;
	using Crm.Article.Services.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;

	public class ProductFamilyService : IProductFamilyService
	{
		private readonly IRepositoryWithTypedId<ProductFamily, Guid> productFamilyRepository;
		public ProductFamilyService(IRepositoryWithTypedId<ProductFamily, Guid> productFamilyRepository)
		{
			this.productFamilyRepository = productFamilyRepository;
		}
		public virtual ProductFamily GetFirstProductFamilyInHierarchy(Guid productFamilyId)
		{
			var productFamily = productFamilyRepository.Get(productFamilyId);
			if (productFamily.Parent == null)
			{
				return productFamily;
			}
			var parentProductFamily = productFamily.Parent;
			while (parentProductFamily.Parent != null)
			{
				parentProductFamily = parentProductFamily.Parent;
			}
			return productFamilyRepository.Get(parentProductFamily.Id);
		}
	}
}
