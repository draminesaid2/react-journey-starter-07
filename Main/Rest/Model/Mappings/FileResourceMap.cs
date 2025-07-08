namespace Crm.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;

	public class FileResourceMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<FileResource, FileResourceRest>().ForMember(x => x.Content, m => m.Ignore());
		}
	}
}
