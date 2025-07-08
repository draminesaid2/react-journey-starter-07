namespace Crm.Rest.Model.Mappings
{
	using System;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.BaseModel;
	using Crm.Model;

	public class ContactRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Contact, ContactRest>()
				.ForMember(x => x.ParentName, m => m.MapFrom(x => x.Parent != null ? x.Parent.Name : null))
				.ForMember(x => x.ParentType, m => m.MapFrom(x => x.Parent != null ? x.Parent.ContactType : null))
				.ForMember(x => x.VisibleToUserIds, m => m.MapFrom(x => x.Visibility == Visibility.Users ? x.VisibleToUserIds : Array.Empty<string>()))
				.ForMember(x => x.VisibleToUsergroupIds, m => m.MapFrom(x => x.Visibility == Visibility.UserGroups ? x.VisibleToUsergroupIds : Array.Empty<Guid>()));
			mapper.CreateMap<ContactRest, Contact>()
				.ForMember(x => x.VisibleToUserIds, m => m.MapFrom((x, y, c) => x.Visibility == Visibility.Users || y.Visibility == Visibility.Users ? x.VisibleToUserIds : Array.Empty<string>()))
				.ForMember(x => x.VisibleToUsergroupIds, m => m.MapFrom((x, y, c) => x.Visibility == Visibility.UserGroups || y.Visibility == Visibility.UserGroups ? x.VisibleToUsergroupIds : Array.Empty<Guid>()));
		}
	}
}
