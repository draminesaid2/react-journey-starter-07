namespace Crm.DynamicForms.Model
{
	using System;
	using System.Globalization;

	using Crm.DynamicForms.Model.BaseModel;

	public class Date : DynamicFormElement, IDynamicFormInputElement<DateTime?>
	{
		public static string DiscriminatorValue = "Date";

		public virtual bool Required { get; set; }
		public virtual DateTime? Response { get; set; }
		public override string ParseFromClient(string value)
		{
			if (DateTime.TryParse(value, out var dateTime))
			{
				return dateTime.ToLocalTime().Date.ToString(CultureInfo.InvariantCulture.DateTimeFormat);
			}
			return null;
		}
		public override string ParseToClient(string value)
		{
			if (DateTime.TryParse(value, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal, out var dateTime))
			{
				return dateTime.ToUniversalTime().ToString("O");
			}
			return null;
		}

		public Date()
		{
			Size = 4;
	}
	}
}
