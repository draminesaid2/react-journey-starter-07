namespace Crm.BusinessRules.StationRules
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Validation;

	public class NameUnique : Rule<Station>
	{
		private readonly IRepositoryWithTypedId<Station, Guid> stationRepository;

		protected override bool IsIgnoredFor(Station station)
		{
			return station.Name.IsNullOrEmpty();
		}

		public override bool IsSatisfiedBy(Station station)
		{
			return !stationRepository.GetAll().Any(x => x.Name == station.Name && x.Id != station.Id);
		}

		protected override RuleViolation CreateRuleViolation(Station station)
		{
			return RuleViolation(station, s => s.Name);
		}

		// Constructor
		public NameUnique(IRepositoryWithTypedId<Station, Guid> stationRepository)
			: base(RuleClass.Unique)
		{
			this.stationRepository = stationRepository;
		}
	}
}
