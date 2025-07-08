namespace Crm.Rest.Model.Mappings
{
	using System;
	using System.Linq;
	using System.Reflection;

	using AutoMapper;

	using Crm.Library.AutoMapper;
	using Crm.Library.BaseModel.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Signalr;

	using Microsoft.AspNetCore.OData.Query;

	public class UserRestMap : IAutoMap
	{
		public virtual void CreateMap(IProfileExpression mapper)
		{
			mapper.CreateMap<User, UserRest>()
				.ForMember(d => d.Avatar, m => m.PreCondition(ctx => 
					(ctx.Items.TryGetValue("$option", out var value) && value is ODataQueryOptions options && options.Request.Query["expandAvatar"] == "true") ||
					(ctx.Items.TryGetValue("$expand", out var expands) && expands is MemberInfo[] memberInfos && memberInfos.Any(x => x == typeof(UserRest).GetMember(nameof(UserRest.Avatar))[0]))
				))
				.ForMember(d => d.GeneralToken, m => m.MapFrom((source, destination, member, context) => context.GetService<IUserService>().CurrentUser != null && (context.GetService<IAuthorizationManager>().IsAdministrator(context.GetService<IUserService>().CurrentUser) || context.GetService<IUserService>().CurrentUser.UserId == source.UserId) ? source.GeneralToken : null))
				.ForMember(d => d.UsergroupObjects, m => m.MapFrom(s => s.Usergroups))
				.ForMember(d => d.UsergroupIds, m => m.MapFrom(s => s.Usergroups.Select(u => u.Id)))
				.ForMember(d => d.StationIds, m => m.MapFrom(s => s.Stations.Select(u => u.Id)))
				.ForMember(x => x.DomainId, m => m.MapFrom(x => x.GetAuthData() != null ? x.GetAuthData().DomainId : (Guid?)null))
				;
			mapper.CreateMap<User, UserSignalRInformation>()
				.ForMember(x => x.Connected, m => m.MapFrom((source, dest, member, context) => context.GetService<ISignalrProfiler>().ConnectedUsers.Contains(source)))
				.ForMember(x => x.JavaScriptLogLevel, m => m.MapFrom((source, dest, member, context) => context.GetService<ISignalrProfiler>().JavaScriptLogSettings(source)))
				.ForMember(x => x.LocalDatabaseLogs, m => m.MapFrom((source, dest, member, context) => context.GetService<ISignalrProfiler>().LocalDatabaseLogs(source)))
				.ForMember(x => x.LocalStorageLogs, m => m.MapFrom((source, dest, member, context) => context.GetService<ISignalrProfiler>().LocalStorageLogs(source)))
				;
			mapper.CreateMap<UserRest, User>()
				.ForMember(d => d.Usergroups, m => m.MapFrom(s => s.UsergroupObjects));
		}
	}
}
