namespace Crm.DynamicForms.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Rest.Model;
	using Crm.DynamicForms.Services;
	using Crm.Library.AutoMapper;

	public class DynamicFormResponseMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<DynamicFormResponse, DynamicFormResponseRest>()
				.ForMember(x => x.Value, m =>
				{
					m.PreCondition(x => x.DynamicFormElementType != null);
					m.MapFrom((source, destination, member, context) => context.GetService<IDynamicFormElementTypeProvider>().ParseToClient(source.DynamicFormElementType, source.ValueAsString));
				})
				.ForMember(x => x.DynamicFormKey, m => m.MapFrom(x => x.DynamicFormElement.DynamicFormKey));
			mapper.CreateMap<DynamicFormResponseRest, DynamicFormResponse>()
				.ForMember(x => x.ValueAsString, m =>
				{
					m.MapFrom(x => x.Value);
					m.PreCondition(x => x.DynamicFormElementType != null);
					m.MapFrom((source, destination, member, context) => context.GetService<IDynamicFormElementTypeProvider>().ParseFromClient(source.DynamicFormElementType, source.Value));
				});
		}
	}
}
