namespace Crm.ViewModels
{
	using System.Collections.Generic;

	using Crm.Services;

	public class ClientSelectionViewModel : CrmModel
	{
		public List<RedirectProviderResult> RedirectProviderResults { get; set; }
	}
}
