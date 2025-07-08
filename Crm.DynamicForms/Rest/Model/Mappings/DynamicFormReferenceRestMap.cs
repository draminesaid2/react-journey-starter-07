namespace Crm.DynamicForms.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Rest.Model;
	using Crm.Library.AutoMapper;

	public class DynamicFormReferenceRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<DynamicFormReference, DynamicFormReferenceRest>();
			mapper.CreateMap<DynamicFormReferenceRest, DynamicFormReference>()
				.ForMember(d => d.ReferenceType, m => m.Ignore());
		}
	}
}
