namespace Crm.Web
{
	using System;
	using Autofac;
	using AutoMapper;
	using Crm.Library.Model.Site;
	using Crm.Services.Interfaces;
	using LMobile.Unicore;

	public class SiteModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c => c.Resolve<ISiteService>().CurrentSite).As<Site>();
			builder.Register<Func<Site>>(
				c =>
				{
					var mapper = c.Resolve<IMapper>();
					var domain = c.Resolve<Domain>();
					var site = mapper.Map(domain, new Site());
					return () => site;
				}).As<Func<Site>>();
		}
	}
}