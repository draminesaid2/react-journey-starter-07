namespace Crm.DynamicForms.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class DynamicFormConfiguration : EntityConfiguration<DynamicForm>
	{
		public override void Initialize()
		{
			Property(x => x.Category, c => c.Filterable());
			Property(x => x.Title, c =>
			{
				c.Filterable();
				c.Sortable();
			});
			Property(x => x.Description, c => c.Filterable());
		}

		public DynamicFormConfiguration(IEntityConfigurationHolder<DynamicForm> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
