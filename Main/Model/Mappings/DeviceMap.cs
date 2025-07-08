namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	public class DeviceMap : EntityClassMapping<Device>
	{
		public DeviceMap()
		{
			Schema("CRM");
			Table("Device");

			Id(a => a.Id, m =>
			{
				m.Column("DeviceId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.Fingerprint);
			Property(x => x.Token);
			Property(x => x.Username);
			Property(x => x.DeviceInfo);
			Property(x => x.IsTrusted);
		}
	}
}
