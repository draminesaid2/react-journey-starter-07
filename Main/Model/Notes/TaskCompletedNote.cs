namespace Crm.Model.Notes
{
	public class TaskCompletedNote : Note
	{
		public override string ImageTextKey
		{
			get { return "TaskIconText"; }
		}
		public override string PermanentLabelResourceKey
		{
			get { return "TaskCompletedBy"; }
		}
	}
}