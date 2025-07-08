namespace Crm.Article.Services.Interfaces
{
	using Crm.Article.Model;
	using Crm.Library.AutoFac;
	using System;

	public interface IProductFamilyService : ITransientDependency
	{
		ProductFamily GetFirstProductFamilyInHierarchy(Guid productFamilyId);
	}
}