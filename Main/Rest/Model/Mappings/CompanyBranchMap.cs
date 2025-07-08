namespace Crm.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;

	public class CompanyBranchMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<CompanyBranch, CompanyBranchRest>();
		}
	}
}
