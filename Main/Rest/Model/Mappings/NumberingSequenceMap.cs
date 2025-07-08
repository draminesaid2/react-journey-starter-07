namespace Crm.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;
	using Crm.Rest.Controllers;

	public class NumberingSequenceMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<NumberingSequence, NumberingSequenceController.NumberingSequenceInfo>();
		}
	}
}
