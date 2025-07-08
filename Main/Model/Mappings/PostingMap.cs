using Crm.Library.Model;
using NHibernate.Mapping.ByCode.Conformist;

namespace Crm.Model.Mappings
{
	public class PostingMap : SubclassMapping<Posting>
	{
		public PostingMap()
		{
			DiscriminatorValue(PostingCategory.Posting);
		}
	}
}
