namespace Crm.Rest.Model.Mappings
{
	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Model;

	public class TaskRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Task, TaskRest>()
				.ForMember(x => x.ContactType, m => m.MapFrom(x => x.Contact.ContactType))
				.ForMember(t => t.ResponsibleUserUser, m => m.MapFrom(s => s.ResponsibleUserObject))
				.ForMember(t => t.ResponsibleUserGroupKey, m => m.MapFrom(s => s.ResponsibleGroupKey))
				.ForMember(x => x.ResponsibleUserGroup, m => m.MapFrom(t => t.ResponsibleGroup != null ? t.ResponsibleGroup.Name : null));
			mapper.CreateMap<TaskRest, Task>()
				.ForMember(t => t.ResponsibleGroupKey, m => m.MapFrom(s => s.ResponsibleUserGroupKey));
		}
	}
}
