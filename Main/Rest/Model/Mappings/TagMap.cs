namespace Crm.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;

	public class TagMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Tag, string>().ConvertUsing(x => x.Name);

		}
	}
}
