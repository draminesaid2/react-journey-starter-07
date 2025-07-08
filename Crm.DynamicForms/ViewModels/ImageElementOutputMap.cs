namespace Crm.DynamicForms.ViewModels
{
	using System;

	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.Library.AutoMapper;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;

	public class ImageElementOutputMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<Image, FileResource>().ConvertUsing((source, dest, context) => Map(source, context));
		}
		protected virtual FileResource Map(Image source, ResolutionContext context)
		{
			if (source?.FileResourceId == null)
			{
				return null;
			}

			var fileResourceRepository = context.GetService<IRepositoryWithTypedId<FileResource, Guid>>();
			var fileResource = fileResourceRepository.Get(source.FileResourceId.Value);
			return context.Mapper.Map<FileResource>(fileResource);
		}
	}
}
