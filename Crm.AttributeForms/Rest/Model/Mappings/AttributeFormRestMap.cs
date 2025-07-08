namespace Crm.AttributeForms.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Rest.Model;
	using Crm.AttributeForms.Model;
	using Crm.AttributeForms.Rest.Model;
	using Crm.Library.AutoMapper;

	public class ServiceOrderChecklistRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<AttributeForm, AttributeFormRest>()
				.IncludeBase<DynamicFormReference, DynamicFormReferenceRest>();
			mapper.CreateMap<AttributeFormRest, AttributeForm>()
				.IncludeBase<DynamicFormReferenceRest, DynamicFormReference>();

		}
	}
}
