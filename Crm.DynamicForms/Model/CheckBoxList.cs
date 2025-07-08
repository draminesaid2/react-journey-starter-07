namespace Crm.DynamicForms.Model
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.Library.Data.NHibernateProvider.UserTypes;

	using Newtonsoft.Json;

	public class CheckBoxList : DynamicFormElement, IDynamicFormInputElement<List<int>>, IDynamicFormMultipleChoiceElement
	{
		public static string DiscriminatorValue = "CheckBoxList";

		public virtual int MinChoices { get; set; }
		public virtual int MaxChoices { get; set; }

		public virtual bool Required { get; set; }
		public virtual List<int> Response { get; set; }

		public virtual int Choices { get; set; }
		public virtual bool Randomized { get; set; }
		public virtual int Layout { get; set; }

		public override string ParseFromClient(string value)
		{
			var indices = JsonConvert.DeserializeObject<int[]>(value).Select(x => x.ToString());
			return new DelimitedString(indices).ToString();
		}
		public override string ParseToClient(string value)
		{
			var indices = new DelimitedString(value).Where(x => string.IsNullOrEmpty(x) == false).Select(int.Parse).ToArray();
			return JsonConvert.SerializeObject(indices);
		}

		public CheckBoxList()
		{
			MaxChoices = 3;
			Response = new List<int>();
			Choices = 3;
			Size = 2;
		}
	}
}
