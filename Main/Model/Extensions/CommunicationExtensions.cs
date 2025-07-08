namespace Crm.Model.Extensions
{
	using System;
	using Crm.Library.Extensions;

	public static class CommunicationExtensions
	{
		public static string DataOrPhoneNumber(this Communication comm) 
		{
			if (comm.GetType() != typeof(Fax) && comm.GetType() != typeof(Phone))
			{
				return comm.Data;
			}
			return String.Format("{0} {1} {2}",
				comm.Country.IsNotNull() ? "+" + comm.Country.CallingCode : comm.CallingCode.IsNotNullOrEmpty() ? "+" + comm.CallingCode : "",
				comm.AreaCode.IsNotNullOrWhiteSpace() ? String.Format("{0}", comm.AreaCode) : "",
				comm.Data)
					.Trim();
		}
	}
}