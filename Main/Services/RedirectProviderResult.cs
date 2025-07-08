using Microsoft.AspNetCore.Mvc;

namespace Crm.Services
{
	using System;

	using Crm.Controllers;

	public class RedirectProviderResult
	{
		public string Name { get; set; }
		public string Plugin { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public Func<HomeController, ActionResult> ActionResult { get; set; }
		public string Hash { get; set; }
		public string Icon { get; set; }
		public string LoginUrl => Plugin == "Main" ? $"~/{Controller}/{Action}#{Hash}" : $"~/{Plugin}/{Controller}/{Action}#{Hash}";

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((RedirectProviderResult)obj);
		}

		protected bool Equals(RedirectProviderResult other)
		{
			return string.Equals(Name, other.Name) && string.Equals(Plugin, other.Plugin) && string.Equals(Controller, other.Controller) && string.Equals(Action, other.Action);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Name != null ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Plugin != null ? Plugin.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Controller != null ? Controller.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Action != null ? Action.GetHashCode() : 0);
				return hashCode;
			}
		}
	}
}
