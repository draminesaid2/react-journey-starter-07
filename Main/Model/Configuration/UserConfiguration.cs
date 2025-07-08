namespace Crm.Model.Configuration
{
	using Crm.Library.EntityConfiguration;
	using Crm.Library.Model;

	using User = Crm.Library.Model.User;

	public class UserConfiguration : EntityConfiguration<User>
	{
		public override void Initialize()
		{
			Property(x => x.FirstName, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.LastName, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.Usergroups, c => c.Filterable(f => f.Definition(new CollectionAutoCompleterFilterDefinition<Usergroup, Usergroup>(x => x.Id, new AutoCompleterFilterDefinition<Usergroup>(null, null, "Main_Usergroup", x => x.Name, x => x.Id, x => x.Name)) { Caption = "Usergroup" })));
			Property(x => x.Email, c => c.Filterable());
			Property(x => x.PersonnelId, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.IdentificationNo, f => f.Filterable());
			Property(x => x.Stations, c => c.Filterable(f => f.Definition(new CollectionAutoCompleterFilterDefinition<Station, Station>(x => x.Id, new AutoCompleterFilterDefinition<Station>("StationAutocomplete", new { Plugin = "Main" }, "Main_Station", "window.Helper.Station.getDisplayName", x => x.Id, x => x.Name)) { Caption = "Station" })));
			Property(x => x.AdName, f => f.Filterable());
			Property(x => x.OpenIdIdentifier, f => f.Filterable());
			Property(x => x.DefaultLanguage, f => f.Filterable(c => c.Caption("Language")));
			Property(x => x.LastLoginDate, c => c.Sortable(s => s.SortCaption("LastLoginLabel")));
		}
		public UserConfiguration(IEntityConfigurationHolder<User> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}