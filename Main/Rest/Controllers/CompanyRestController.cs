using Microsoft.AspNetCore.Mvc;

namespace Crm.Rest.Controllers
{
	using System;
	using System.Linq;
	using System.Runtime.Serialization;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.ModelBinder;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;
	using Crm.Model;
	using Crm.Services.Interfaces;

	using PermissionGroup = MainPlugin.PermissionGroup;

	[DataContract]
	public class CompanyRestController : RestController<Company>
	{
		public static string InvalidCommTypeExceptionMessage = "Invalid commType {0}. Check your route constraints.";

		private readonly ITagService tagService;
		private readonly ICompanyService companyService;
		private readonly IAddressService addressService;
		private readonly IUserService userService;
		private readonly IRuleValidationService ruleValidationService;
		private readonly IRepositoryWithTypedId<Task, Guid> taskRepository;

		// Methods
		public virtual ActionResult Get(Guid id)
		{
			var company = companyService.GetCompany(id);
			return Rest(company);
		}
		
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.Company)]
		public virtual ActionResult Create(Company company)
		{
			company.IsActive = true;
			var ruleViolations = ruleValidationService.GetRuleViolations(company);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			companyService.SaveCompany(company);
			return Rest(company.Id.ToString());
		}
		
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Company)]
		public virtual ActionResult Update(Company company)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(company);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			companyService.SaveCompany(company);
			return new EmptyResult();
		}
		
		[RequiredPermission(PermissionName.Delete, Group = PermissionGroup.Company)]
		public virtual ActionResult Delete(Guid id)
		{
			companyService.DeleteCompany(id);
			return Content("Company deleted.");
		}

		#region Addresses

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Company)]
		public virtual ActionResult ListAddresses(Guid id)
		{
			var company = companyService.GetCompany(id);
			return Rest(company.Addresses, "Addresses");
		}

		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.Company)]
		public virtual ActionResult CreateAddress(Guid id, Address address)
		{
			//address.ContactId = id;
			address.IsActive = true;

			var ruleViolations = ruleValidationService.GetRuleViolations(address);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			var company = companyService.GetCompany(id);
			company.Addresses.Add(address);
			companyService.SaveCompany(company); // addressService.SaveAddress(address);

			return Rest(address.Id);
		}

		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Company)]
		public virtual ActionResult UpdateAddress(Guid id, Address address)
		{
			var company = companyService.GetCompany(id);
			if (company.Addresses.None(a => a.Id == address.Id))
			{
				return Rest(new RuleViolation("CompanyHasNoAddressWithThisId"));
			}

			var ruleViolations = ruleValidationService.GetRuleViolations(address);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			addressService.SaveAddress(address);

			return new EmptyResult();
		}

		[RequiredPermission(MainPlugin.PermissionName.DeleteAddress, Group = PermissionGroup.Company)]
		public virtual ActionResult DeleteAddress(Guid id, Guid addressId)
		{
			addressService.DeleteAddress(addressId);
			return new EmptyResult();
		}

		#endregion Addresses

		#region Communications

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Company)]
		public virtual ActionResult ListCommunications(Guid id, Guid addressId, string commType)
		{
			var company = companyService.GetCompany(id);

			switch (commType.ToLower())
			{
				case "phones":
					return Rest(company.GetPhonesFor(addressId), "Phones");
				case "emails":
					return Rest(company.GetEmailsFor(addressId), "Emails");
				case "faxes":
					return Rest(company.GetFaxesFor(addressId), "Faxes");
				case "websites":
					return Rest(company.GetWebsitesFor(addressId), "Websites");
				default:
					throw new ApplicationException(InvalidCommTypeExceptionMessage.WithArgs(commType));
			}

			//var address = companyService.GetCompany(id).Addresses.First(a => a.Id == addressId);

			//switch (commType.ToLower())
			//{
			//  case "phones":
			//    return Rest(address.Phones, "Phones");
			//  case "emails":
			//    return Rest(address.Emails, "Emails");
			//  case "faxes":
			//    return Rest(address.Faxes, "Faxes");
			//  case "websites":
			//    return Rest(address.Websites, "Websites");
			//  default:
			//    throw new ApplicationException(InvalidCommTypeExceptionMessage.WithArgs(commType));
			//}
		}

		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.Company)]
		public virtual ActionResult CreateCommunication(Guid id, Guid addressId, [ModelBinder(typeof(IgnoreModelBinder))] Communication communication)
		{
			var company = companyService.GetCompany(id);
			communication.AddressId = addressId;
			communication.ContactId = company.Id;
			company.Communications.Add(communication);

			companyService.SaveCompany(company);

			return new EmptyResult();

			//var address = companyService.GetCompany(id).Addresses.FirstOrDefault(a => a.Id == addressId);

			//if (address == null)
			//{
			//  return Rest(new RuleViolation("CompanyHasNoAddressWithThisId"));
			//}

			//var phone = communication as Phone;
			//var email = communication as Email;
			//var fax = communication as Fax;
			//var website = communication as Website;

			//if (phone != null)
			//{
			//  address.Phones.Add(phone);
			//}
			//if (email != null)
			//{
			//  address.Emails.Add(email);
			//}
			//if (fax != null)
			//{
			//  address.Faxes.Add(fax);
			//}
			//if (website != null)
			//{
			//  address.Websites.Add(website);
			//}

			//addressService.SaveAddress(address);

			//return new EmptyResult();
		}

		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Company)]
		public virtual ActionResult UpdateCommunication(Guid id, Guid addressId, Communication communication)
		{
			var company = companyService.GetCompany(id);

			if (!company.TryRemoveCommunication(communication.Id))
			{
				return Rest(new RuleViolation("AddressHasNoCommunicationWithThisId"));
			}

			communication.AddressId = addressId;
			communication.ContactId = company.Id;

			company.Communications.Add(communication);

			return new EmptyResult();

			//var address = company.Addresses.FirstOrDefault(a => a.Id == addressId);

			//if (address == null)
			//{
			//  return Rest(new RuleViolation("CompanyHasNoAddressWithThisId"));
			//}

			//var phone = communication as Phone;
			//var email = communication as Email;
			//var fax = communication as Fax;
			//var website = communication as Website;

			//if (phone != null)
			//{
			//  if (!address.TryRemovePhone(phone.Id))
			//  {
			//    return Rest(new RuleViolation("AddressHasNoPhoneWithThisId"));
			//  }
			//  address.Phones.Add(phone);
			//}

			//if (email != null)
			//{
			//  if (!address.TryRemoveEmail(email.Id))
			//  {
			//    return Rest(new RuleViolation("AddressHasNoEmailWithThisId"));
			//  }
			//  address.Emails.Add(email);
			//}

			//if (fax != null)
			//{
			//  if (!address.TryRemoveFax(fax.Id))
			//  {
			//    return Rest(new RuleViolation("AddressHasNoFaxWithThisId"));
			//  }
			//  address.Faxes.Add(fax);
			//}

			//if (website != null)
			//{
			//  if (!address.TryRemoveWebsite(website.Id))
			//  {
			//    return Rest(new RuleViolation("AddressHasNoWebsiteWithThisId"));
			//  }
			//  address.Websites.Add(website);
			//}

			//addressService.SaveAddress(address);

			//return new EmptyResult();
		}

		[RequiredPermission(MainPlugin.PermissionName.DeleteCommunication, Group = PermissionGroup.Company)]
		public virtual ActionResult DeleteCommunication(Guid id, Guid addressId, Guid commId, string commType)
		{
			var company = companyService.GetCompany(id);

			if (!company.TryRemoveCommunication(commId))
			{
				return Rest(new RuleViolation("CompanyHasNoCommunicationWithThisId"));
			}

			companyService.SaveCompany(company);

			return new EmptyResult();

			//var address = companyService.GetCompany(id).Addresses.FirstOrDefault(a => a.Id == addressId);

			//if (address == null)
			//{
			//  return Rest(new RuleViolation("CompanyHasNoAddressWithThisId"));
			//}

			//switch (commType.ToLower())
			//{
			//  case "phones":
			//    if (!address.TryRemovePhone(commId))
			//    {
			//      return Rest(new RuleViolation("AddressHasNoPhoneWithThisId"));
			//    }
			//    break;
			//  case "emails":
			//    if (!address.TryRemoveEmail(commId))
			//    {
			//      return Rest(new RuleViolation("AddressHasNoEmailWithThisId"));
			//    }
			//    break;
			//  case "faxes":
			//    if (!address.TryRemoveFax(commId))
			//    {
			//      return Rest(new RuleViolation("AddressHasNoFaxWithThisId"));
			//    }
			//    break;
			//  case "websites":
			//    if (!address.TryRemoveWebsite(commId))
			//    {
			//      return Rest(new RuleViolation("AddressHasNoWebsiteWithThisId"));
			//    }
			//    break;
			//  default:
			//    throw new ApplicationException(InvalidCommTypeExceptionMessage.WithArgs(commType));
			//}

			//addressService.SaveAddress(address);

			//return new EmptyResult();
		}

		#endregion Communications

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Company)]
		public virtual ActionResult ListPersons(Guid id)
		{
			var persons = companyService.GetCompanies().Where(c => c.Id == id).SelectMany(c => c.Staff).ToList(); // .BusinessRelationships contactService.GetCompanyStaff(id));
			return Rest(persons, "Persons");
		}

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Company)]
		public virtual ActionResult ListTasks(Guid id)
		{
		    var tasks = taskRepository.GetAll().Where(x => x.ContactId == id && x.ResponsibleUser == userService.CurrentUser.Id);
			return Rest(tasks, "Tasks");
		}
		
		public virtual ActionResult ListTags(Guid id)
		{
			var tags = tagService.GetTagsByContactIds(new[] { id });
			return Rest(tags, "Tags", "Tag");
		}

		[RequiredPermission(MainPlugin.PermissionName.AssociateTag, Group = PermissionGroup.Company)]
		public virtual ActionResult AssociateTag(Guid id, string tagName)
		{
			tagService.AddTagToContact(id, tagName);
			return new EmptyResult();
		}

		[RequiredPermission(MainPlugin.PermissionName.RemoveTag, Group = PermissionGroup.Company)]
		public virtual ActionResult RemoveTag(Guid id, string tagName)
		{
			tagService.RemoveTagFromContact(id, tagName);
			return new EmptyResult();
		}

		public CompanyRestController(ICompanyService companyService,
			IUserService userService, 
			IAddressService addressService, 
			ITagService tagService, 
			IRuleValidationService ruleValidationService,
			IRepositoryWithTypedId<Task, Guid> taskRepository,
			RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.companyService = companyService;
			this.userService = userService;
			this.addressService = addressService;
			this.tagService = tagService;
			this.ruleValidationService = ruleValidationService;
			this.taskRepository = taskRepository;
		}
	}
}
