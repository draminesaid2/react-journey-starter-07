namespace Crm.Article.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Article.Model;
	using Crm.Library.AutoMapper;
	using Crm.Model;
	using Crm.Rest.Model;

	public class ProductFamilyRestMap :IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<ProductFamily, ProductFamilyRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(t => t.ChildProductFamilies, m => m.MapFrom(s => s.ChildProductFamilies))
				.ForMember(t=>t.ParentProductFamily, m=> m.MapFrom(s => s.ParentProductFamily))
				.ForMember(d => d.ResponsibleUserUser, m => m.MapFrom(s => s.ResponsibleUserObject));

			mapper.CreateMap<ProductFamilyRest, ProductFamily>()
				.IncludeBase<ContactRest, Contact>();
		}
	}
}
