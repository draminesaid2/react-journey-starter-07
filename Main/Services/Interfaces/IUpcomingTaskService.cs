using Crm.Library.AutoFac;
using Crm.Library.Model;
using Crm.Model;
using System.Collections.Generic;

namespace Crm.Services.Interfaces
{
	using System.Text;

	public interface IUpcomingTaskService : IDependency
	{
		string GenerateMessageBody(User user, int todayTaskCount, int overdueTaskCount);
		void SaveMessage(User user, string messageBody);
		List<Task> GetTasksForToday(string username);
		List<Task> GetOverdueTasks(string username);
		StringBuilder GenerateLink(StringBuilder sb, User user, LMobile.Unicore.Domain domain = null);
	}
}
