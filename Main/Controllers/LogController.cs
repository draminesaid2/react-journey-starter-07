namespace Crm.Controllers
{
	using System;

	using log4net;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	public class LogController : Controller
	{
		private readonly ILog logger;
		public LogController(ILog logger)
		{
			this.logger = logger;
		}
		[AllowAnonymous]
		public virtual void LogError(string message, string url, string line, string column, string error)
		{
			var text = $"Message: {message} {Environment.NewLine}Url: {url} {Environment.NewLine}Line: {line} {Environment.NewLine}Column: {column} {Environment.NewLine}Error: {error}";
			logger.Error(text);
		}
	}
}
