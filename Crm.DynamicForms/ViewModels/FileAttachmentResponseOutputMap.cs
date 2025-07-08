namespace Crm.DynamicForms.ViewModels
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;

	using Newtonsoft.Json;

	public class FileAttachmentResponseOutputMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<FileResource, FileResource>();
			mapper.CreateMap<string, FileAttachmentResponseOutput>().ConstructUsing(Map);
		}
		protected virtual FileAttachmentResponseOutput Map(string source, ResolutionContext context)
		{
			var ids = JsonConvert.DeserializeObject<FileAttachmentResponse>(source);
			if (source == null || ids?.Any() != true)
			{
				return new FileAttachmentResponseOutput();
			}

			var fileResourceRepository = context.GetService<IRepositoryWithTypedId<FileResource, Guid>>();
			var fileResources = fileResourceRepository.GetAll()
				.Where(x => ids.Contains(x.Id))
				.AsEnumerable()
				.Select(x => context.Mapper.Map<FileResource>(x))
				.ToList();
			var result = new FileAttachmentResponseOutput();
			result.AddRange(fileResources);
			return result;
		}
	}
}
