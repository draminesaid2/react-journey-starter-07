namespace Crm.DynamicForms.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Rest.Model;
	using Crm.Library.AutoMapper;

	public class DynamicFormRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<DynamicForm, DynamicFormRest>();
			mapper.CreateMap<DynamicFormRest, DynamicForm>();
		}
	}
}
