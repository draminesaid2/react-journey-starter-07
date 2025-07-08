namespace Crm.Rest.Model.Mappings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Autofac;

	using AutoMapper;

	using Crm.Library.AutoFac.Extensions;
	using Crm.Library.AutoMapper;
	using Crm.Library.Extensions;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model.Notes;

	public class NoteRestMap : IAutoMap
	{
		private readonly Dictionary<string, Type> noteTypes;

		public NoteRestMap(IPluginProvider pluginProvider)
		{
			noteTypes = pluginProvider.ActivePluginDescriptors.SelectMany(x => x.Assembly.GetTypesInheriting<Note>()).ToDictionary(k => k.Name);
		}
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Note, NoteRest>()
				.ForMember(x => x.ContactType, m => m.MapFrom(x => x.Contact.ContactType))
				.ForMember(x => x.ContactName, m => m.MapFrom(x => x.Contact.Name))
				.ForMember(x => x.User, m => m.MapFrom(x => x.CreateUserObject));
			mapper.CreateMap<NoteRest, Note>()
				.ConstructUsing((note, context) =>
				{
					if (noteTypes.TryGetValue(note.NoteType, out var noteType) == false)
					{
						noteType = typeof(Note);
					}
					return (Note)context.GetService<ILifetimeScope>().InitializeEntity(Activator.CreateInstance(noteType));
				});
		}
	}
}
