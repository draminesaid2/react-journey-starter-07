using Microsoft.AspNetCore.Mvc;

namespace Crm.Results
{

	using Crm.ViewModels;

	public class ErrorResult : ViewResult
	{

		public ErrorResult(ErrorViewModel errorViewModel)
		{
			ViewName = "Error/Error";
			ViewData.Model = errorViewModel;
		}
	}
}