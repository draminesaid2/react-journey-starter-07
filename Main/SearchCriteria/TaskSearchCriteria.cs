namespace Crm.SearchCriteria
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Extensions;

	public class TaskSearchCriteria : TimeSpanSearchCriteria
	{
		public string ResponsibleUser { get; set; }
		public List<string> ResponsibleUsers { get; set; }
		public Guid UserGroup { get; set; }
		public string GroupBy { get; set; }
		public Guid? ContactId { get; set; }
		public Guid? NoteId { get; set; }
		public DateTime? FromCreateDate { get; set; }
		public DateTime? ToCreateDate { get; set; }

		public bool HasCriteria
		{
			get
			{
				return
					ResponsibleUser.IsNotNullOrEmpty() ||
					UserGroup.IsNotNull() ||
					ContactId.HasValue ||
					FromDate.HasValue ||
					ToDate.HasValue ||
					FromCreateDate.HasValue ||
					ToCreateDate.HasValue;
			}
		}
	}
}
