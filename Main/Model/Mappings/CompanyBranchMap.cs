namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	public class
		CompanyBranchMap : EntityClassMapping<CompanyBranch>
	{
		public CompanyBranchMap()
		{
			Schema("CRM");
			Table("CompanyBranch");

			Id(x => x.Id, m => {
				m.Column("CompanyBranchId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});
			Property(x => x.CompanyKey);
			Property(x => x.Branch1Key);
			Property(x => x.Branch2Key);
			Property(x => x.Branch3Key);
			Property(x => x.Branch4Key);
		}
	}
}
