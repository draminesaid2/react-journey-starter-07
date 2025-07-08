namespace Crm.BusinessRules.PersonRules
{
    using System;

    using Crm.Library.Data.Domain.DataInterfaces;
    using Crm.Library.Validation;
    using Crm.Model;

	public class AddressRequired : Rule<Person>
	{
	    private readonly IRepositoryWithTypedId<Address, Guid> addressRepository;
	    public AddressRequired(IRepositoryWithTypedId<Address, Guid> addressRepository) : base(RuleClass.Required)
	    {
	        this.addressRepository = addressRepository;
	    }
	    protected override RuleViolation CreateRuleViolation(Person entity)
	    {
	        return new RuleViolation(entity, "Address", "Address", RuleClass.Required);
	    }
	    public override bool IsSatisfiedBy(Person entity)
	    {
	        return entity.StandardAddressKey != default(Guid) && addressRepository.Get(entity.StandardAddressKey).IsActive;
	    }
	}
}
