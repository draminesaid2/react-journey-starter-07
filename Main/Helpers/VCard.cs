namespace Crm.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Crm.Library.Extensions;

	public class VCard
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Title { get; set; }
		public string Salutation { get; set; }
		public string Organization { get; set; }
		public string JobTitle { get; set; }
		public IList<string> StreetAddressLines { get; set; }
		public IList<string> NoteLines { get; set; }
		public string Zip { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string CountryName { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string HomePage { get; set; }
		public byte[] Image { get; set; }

		public override string ToString()
		{
			//var hasFirstName = FirstName.IsNotNullOrEmpty();

			var builder = new StringBuilder();
			builder.AppendLine("BEGIN:VCARD");
			builder.AppendLine("VERSION:2.1");

			// Name
			builder.AppendLine("N:" + LastName + ";" + FirstName + ";;" + Title + ";" + Salutation);

			// Full name
			builder.AppendLine("FN:" + (Salutation.IsNotNullOrEmpty() ? "" : Salutation + " ") + (Title.IsNotNullOrEmpty() ? "" : Title + " ") + FirstName + " " + LastName);

			// Address
			builder.Append("ADR;WORK;ENCODING=QUOTED-PRINTABLE;PREF:f1;");
            //Office
            builder.Append(";");
            //Street
            builder.Append(StreetAddressLines.ToString(", ") + ";");
			builder.Append(City + ";");
			builder.Append(State + ";");
			builder.Append(Zip + ";");
			builder.AppendLine(CountryName);

			// Other data
			builder.AppendLine("ORG:" + Organization);
			builder.AppendLine("TITLE:" + JobTitle);
			builder.AppendLine("TEL;WORK;VOICE:" + Phone);
			builder.AppendLine("TEL;WORK;FAX:" + Fax);
			builder.AppendLine("TEL;CELL;VOICE:" + Mobile);
			builder.AppendLine("URL:" + HomePage);
			builder.AppendLine("URL:" + HomePage);
			builder.AppendLine("EMAIL;PREF;INTERNET:" + Email);
			builder.AppendLine("NOTE;ENCODING=QUOTED-PRINTABLE:" + NoteLines.ToString(", "));

			// Add image
			if (Image != null && Image.Length > 0)
			{
				builder.AppendLine("PHOTO;ENCODING=BASE64;TYPE=JPEG:");
				builder.AppendLine(Convert.ToBase64String(Image));
				builder.AppendLine(string.Empty);
			}

			builder.AppendLine("END:VCARD");

			return builder.ToString();
		}
		public VCard()
		{
			NoteLines = new List<string>();
			StreetAddressLines = new List<string>();
		}
	}
}