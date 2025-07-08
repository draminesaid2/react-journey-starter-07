namespace Crm.DynamicForms.Model
{
	using System.Globalization;

	using Crm.DynamicForms.Model.BaseModel;

	public class Number : DynamicFormElement, IDynamicFormInputElement<double?>
	{
		public static string DiscriminatorValue = "Number";

		public virtual decimal MinValue { get; set; }
		public virtual decimal MaxValue { get; set; }

		public virtual bool Required { get; set; }
		public virtual double? Response { get; set; }
		public virtual int RowSize { get; set; }

		public override string ParseFromClient(string value)
		{
			if (value == null)
				return null;

			double val;
			if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
				return null;

			return val.ToString(CultureInfo.InvariantCulture);
		}

		public Number()
		{
			RowSize = 1;
			Size = 4;
		}
	}
}
