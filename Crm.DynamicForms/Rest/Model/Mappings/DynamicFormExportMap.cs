namespace Crm.DynamicForms.Rest.Model.Mappings
{
	using System.Linq;

	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.DynamicForms.Rest.Model;
	using Crm.Library.AutoMapper;

	public class DynamicFormExportMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<DynamicFormLocalization, DynamicFormLocalizationRest>();
			mapper.CreateMap<DynamicFormLocalizationRest, DynamicFormLocalization>();
			mapper.CreateMap<DynamicForm, DynamicFormExport>()
				.ForMember(x => x.DynamicForm, m => m.MapFrom(x => x))
				.ForMember(x => x.Localizations, m => m.MapFrom(x => x.Localizations))
				.ForMember(x => x.FileResources, m => m.MapFrom(s => s.Elements.OfType<Image>().Where(y => y.FileResourceId.HasValue).ToArray()))
				.AfterMap(
					(x, y) =>
					{
						y.DynamicForm.Elements = null;
						y.DynamicForm.Languages = null;
					})
				;
			mapper.CreateMap<DynamicFormExport, DynamicForm>();
		}
	}
}
