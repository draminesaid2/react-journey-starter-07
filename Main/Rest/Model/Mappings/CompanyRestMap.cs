namespace Crm.Rest.Model.Mappings
{
	using System.Linq;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;

	public class CompanyRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Company, CompanyRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(t => t.AreaSalesManagerUser, m => m.MapFrom(s => s.AreaSalesManagerObject))
				.ForMember(t => t.Emails, m => m.MapFrom(x => x.Communications.OfType<Email>()))
				.ForMember(t => t.Faxes, m => m.MapFrom(x => x.Communications.OfType<Fax>()))
				.ForMember(t => t.Phones, m => m.MapFrom(x => x.Communications.OfType<Phone>()))
				.ForMember(t => t.Websites, m => m.MapFrom(x => x.Communications.OfType<Website>()))
				.ForMember(t => t.Subsidiaries, m => m.MapFrom(s => s.ClientCompanies))
				.ForMember(t => t.CompanyBranches, m => m.MapFrom(s => s.CompanyBranches))
				.ForMember(t => t.ResponsibleUserUser, m => m.MapFrom(s => s.ResponsibleUserObject));
			mapper.CreateMap<CompanyRest, Company>()
				.IncludeBase<ContactRest, Contact>()
				.ForMember(d => d.IsOwnCompany, m => m.Ignore());
		}
	}
}
