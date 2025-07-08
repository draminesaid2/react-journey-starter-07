namespace Crm.BusinessRules.NoteRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model.Notes;

	public class TextRequired : RequiredRule<Note>
	{
		public TextRequired()
		{
			Init(n => n.Text);
		}
	}
}