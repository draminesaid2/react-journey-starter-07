namespace Crm.Article.Model
{
	using System;
	using System.Collections.Generic;

	public class ArticleGroupWithChildren
	{
		private readonly string key;
		private readonly string value;
		private readonly string base64Image;
		private readonly bool isEmpty;

		public string Key { get { return key; } }
		public string Value { get { return value; } }
		public string Base64Image { get { return base64Image; } }
		public bool IsEmpty { get { return isEmpty; } }
		public IList<ArticleGroupWithChildren> Subgroups { get; set; }

		public ArticleGroupWithChildren(string lookupKey, string lookupValue, string lookupBase64Image = "", bool lookupIsEmpty = false)
		{
			Subgroups = new List<ArticleGroupWithChildren>();
			key = lookupKey;
			value = lookupValue ?? String.Empty;
			base64Image = lookupBase64Image;
			isEmpty = lookupIsEmpty;
		}
	}
}