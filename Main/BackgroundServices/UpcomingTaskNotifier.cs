namespace Crm.BackgroundServices
{
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Services.Interfaces;
	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class UpcomingTaskNotifier : JobBase
	{
		private readonly IUserService userService;
		private readonly IUpcomingTaskService upcomingTaskService;

		protected override void Run(IJobExecutionContext context)
		{
			var users = userService.GetActiveUsers();
			foreach (User user in users)
			{
				if (receivedShutdownSignal)
				{
					break;
				}
				var todayTaskCount = upcomingTaskService.GetTasksForToday(user.Id).Count;
				var overdueTaskCount = upcomingTaskService.GetOverdueTasks(user.Id).Count;

				if (todayTaskCount <= 0 && overdueTaskCount <= 0)
				{
					continue;
				}

				var messageBody = upcomingTaskService.GenerateMessageBody(user, todayTaskCount, overdueTaskCount);
				upcomingTaskService.SaveMessage(user, messageBody);
			}
		}
		
		public UpcomingTaskNotifier(
			IUserService userService,
			ISessionProvider sessionProvider,
			ILog logger,
			IHostApplicationLifetime hostApplicationLifetime,
			IUpcomingTaskService upcomingTaskService)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.userService = userService;
			this.upcomingTaskService = upcomingTaskService;
		}
	}
}
