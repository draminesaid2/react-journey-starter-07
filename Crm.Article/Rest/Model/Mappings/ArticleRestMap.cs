namespace Crm.Article.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Article.Model;
	using Crm.Library.AutoMapper;
	using Crm.Model;
	using Crm.Rest.Model;

	public class ArticleRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Article, ArticleRest>()
				.IncludeBase<Contact, ContactRest>();

			mapper.CreateMap<ArticleRest, Article>()
				.IncludeBase<ContactRest, Contact>();
		}
	}
}
