namespace Crm.Services
{
	using System;
	using System.Linq;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Model;
	using Crm.Model.Notes;
	using Crm.Model.Relationships;

	public class CrmMerger : IMerger<Company>, IMerger<Contact>, IMerger<Person>
	{
		private readonly IRepositoryWithTypedId<Address, Guid> addressRepository;
		private readonly IRepositoryWithTypedId<Bravo, Guid> bravoRepository;
		private readonly IRepositoryWithTypedId<BusinessRelationship, Guid> businessRelationshipRepository;
		private readonly IRepositoryWithTypedId<Communication, Guid> communicationRepository;
		private readonly IRepositoryWithTypedId<Company, Guid> companyRepository;
		private readonly IRepositoryWithTypedId<Note, Guid> noteRepository;
		private readonly IRepositoryWithTypedId<Person, Guid> personRepository;
		private readonly IRepository<Tag> tagRepository;
		private readonly IRepositoryWithTypedId<Task, Guid> taskRepository;

		public CrmMerger(IRepositoryWithTypedId<Communication, Guid> communicationRepository, IRepositoryWithTypedId<Address, Guid> addressRepository, IRepositoryWithTypedId<Company, Guid> companyRepository, IRepositoryWithTypedId<Note, Guid> noteRepository, IRepositoryWithTypedId<Person, Guid> personRepository, IRepository<Tag> tagRepository, IRepositoryWithTypedId<Task, Guid> taskRepository, IRepositoryWithTypedId<Bravo, Guid> bravoRepository, IRepositoryWithTypedId<BusinessRelationship, Guid> businessRelationshipRepository)
		{
			this.addressRepository = addressRepository;
			this.communicationRepository = communicationRepository;
			this.companyRepository = companyRepository;
			this.noteRepository = noteRepository;
			this.personRepository = personRepository;
			this.tagRepository = tagRepository;
			this.taskRepository = taskRepository;
			this.bravoRepository = bravoRepository;
			this.businessRelationshipRepository = businessRelationshipRepository;
		}

		public virtual void Merge(Company loser, Company winner)
		{
			MergeAddresses(loser, winner);
			MergeStaff(loser, winner);
			MergeBusinessRelationships(loser, winner);
			companyRepository.SaveOrUpdate(winner);
			companyRepository.SaveOrUpdate(loser);
		}
		public virtual void Merge(Contact loser, Contact winner)
		{
			MergeNotes(loser, winner);
			MergeTags(loser, winner);
			MergeTasks(loser, winner);
			MergeBravos(loser, winner);
		}
		public virtual void Merge(Person loser, Person winner)
		{
			MergeCommunications(loser, winner);
		}

		protected virtual void MergeBravos(Contact loser, Contact winner)
		{
			var loserBravos = bravoRepository.GetAll().Where(x => x.ContactId == loser.Id);
			foreach (var loserBravo in loserBravos)
			{
				loserBravo.ContactId = winner.Id;
				bravoRepository.SaveOrUpdate(loserBravo);
			}
		}
		protected virtual void MergeAddresses(Company loser, Company winner)
		{
			foreach (var loserAddress in loser.Addresses)
			{
				loserAddress.CompanyId = winner.Id;
				loserAddress.IsCompanyStandardAddress = false;
				var communications = communicationRepository.GetAll().Where(x => x.AddressId == loserAddress.Id && x.ContactId == loser.Id);
				addressRepository.SaveOrUpdate(loserAddress);
				foreach (var communication in communications)
				{
					communication.ContactId = winner.Id;
					communicationRepository.SaveOrUpdate(communication);
				}
			}
			winner.Communications.AddItemsNotContained(loser.Communications);
			loser.Communications.Clear();
			winner.Addresses.AddItemsNotContained(loser.Addresses);
			loser.Addresses.Clear();
		}
		protected virtual void MergeStaff(Company loser, Company winner)
		{
			foreach (var loserStaff in loser.Staff)
			{
				loserStaff.ParentId = winner.Id;
				loserStaff.StandardAddressKey = winner.StandardAddress.Id;
				personRepository.SaveOrUpdate(loserStaff);
			}
			winner.Staff.AddItemsNotContained(loser.Staff);
			loser.Staff.Clear();
		}
		protected virtual void MergeBusinessRelationships(Company loser, Company winner)
		{
			var loserChildBusinessRelationships = businessRelationshipRepository.GetAll().Where(x => x.ChildId == loser.Id);
			foreach (var loserChildBusinessRelationship in loserChildBusinessRelationships)
			{
				loserChildBusinessRelationship.ChildId = winner.Id;
				businessRelationshipRepository.SaveOrUpdate(loserChildBusinessRelationship);
			}
			var loserParentBusinessRelationships = businessRelationshipRepository.GetAll().Where(x => x.ParentId == loser.Id);
			foreach (var loserParentBusinessRelationship in loserParentBusinessRelationships)
			{
				loserParentBusinessRelationship.ParentId = winner.Id;
				businessRelationshipRepository.SaveOrUpdate(loserParentBusinessRelationship);
			}
		}
		protected virtual void MergeCommunications(Person loser, Person winner)
		{
			var loserCommunications = communicationRepository.GetAll().Where(x => x.ContactId == loser.Id);
			foreach (var loserCommunication in loserCommunications)
			{
				loserCommunication.ContactId = winner.Id;
				loserCommunication.AddressId = winner.StandardAddressKey;
				communicationRepository.SaveOrUpdate(loserCommunication);
			}
			winner.Communications.AddItemsNotContained(loser.Communications);
			loser.Communications.Clear();
		}
		protected virtual void MergeNotes(Contact loser, Contact winner)
		{
			var loserNotes = noteRepository.GetAll().Where(x => x.ContactId == loser.Id);
			foreach (var loserNote in loserNotes)
			{
				loserNote.ContactId = winner.Id;
				noteRepository.SaveOrUpdate(loserNote);
			}
			winner.Notes.AddItemsNotContained(loser.Notes);
			loser.Notes.Clear();
		}
		protected virtual void MergeTags(Contact loser, Contact winner)
		{
			var loserTags = tagRepository.GetAll().Where(x => x.ContactKey == loser.Id);
			var winnerTags = tagRepository.GetAll().Where(x => x.ContactKey == winner.Id);
			foreach (var loserTag in loserTags)
			{
				if (winnerTags.Any(x => x.Name == loserTag.Name))
				{
					tagRepository.Delete(loserTag);
				}
				else
				{
					loserTag.ContactKey = winner.Id;
					tagRepository.SaveOrUpdate(loserTag);
				}
			}
		}
		protected virtual void MergeTasks(Contact loser, Contact winner)
		{
			var loserTasks = taskRepository.GetAll().Where(x => x.ContactId == loser.Id);
			foreach (var loserTask in loserTasks)
			{
				loserTask.ContactId = winner.Id;
				taskRepository.SaveOrUpdate(loserTask);
			}
		}
	}
}
