namespace Crm.DynamicForms.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.Rest.Model;
	using Crm.Library.AutoMapper;
	using Crm.Library.Rest;

	using NHibernate;

	public class DynamicFormElementMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<DynamicFormElement, DynamicFormElementRest>()
				.ConvertUsing((source, destination, context) => (DynamicFormElementRest)context.Mapper.Map(source, destination, source.GetType(), context.GetService<RestTypeProvider>().GetRestTypeFor(NHibernateUtil.GetClass(source))));

			mapper.CreateMap<DynamicFormElement, CheckBoxListRest>();
			mapper.CreateMap<DynamicFormElement, DateRest>();
			mapper.CreateMap<DynamicFormElement, DropDownRest>();
			mapper.CreateMap<DynamicFormElement, FileAttachmentDynamicFormElementRest>();
			mapper.CreateMap<DynamicFormElement, ImageRest>();
			mapper.CreateMap<DynamicFormElement, LiteralRest>();
			mapper.CreateMap<DynamicFormElement, MultiLineTextRest>();
			mapper.CreateMap<DynamicFormElement, NumberRest>();
			mapper.CreateMap<DynamicFormElement, PageSeparatorRest>();
			mapper.CreateMap<DynamicFormElement, RadioButtonListRest>();
			mapper.CreateMap<DynamicFormElement, SectionSeparatorRest>();
			mapper.CreateMap<DynamicFormElement, SignaturePadRest>();
			mapper.CreateMap<DynamicFormElement, SignaturePadWithPrivacyPolicyRest>();
			mapper.CreateMap<DynamicFormElement, SingleLineTextRest>();
			mapper.CreateMap<DynamicFormElement, TimeRest>();
		}
	}
}
