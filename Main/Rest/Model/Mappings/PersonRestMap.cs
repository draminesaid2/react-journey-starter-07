namespace Crm.Rest.Model.Mappings
{
	using System.Linq;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;

	public class PersonRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Person, PersonRest>()
				.IncludeBase<Contact, ContactRest>()
				.ForMember(t => t.Emails, m => m.MapFrom(x => x.Communications.OfType<Email>()))
				.ForMember(t => t.Faxes, m => m.MapFrom(x => x.Communications.OfType<Fax>()))
				.ForMember(t => t.Phones, m => m.MapFrom(x => x.Communications.OfType<Phone>()))
				.ForMember(t => t.Websites, m => m.MapFrom(x => x.Communications.OfType<Website>()))
				.ForMember(t => t.Parent, m => m.MapFromProxy(x => x.Parent).As<Company>())
				.ForMember(t => t.ResponsibleUserUser, m => m.MapFrom(s => s.ResponsibleUserObject));

			mapper.CreateMap<PersonRest, Person>()
				.IncludeBase<ContactRest, Contact>();
		}
	}
}
