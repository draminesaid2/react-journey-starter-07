namespace Customer.Kagema.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model;
	using Crm.Model.Lookups;
	using Crm.Model.Notes;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;
	using Crm.Services;
	using Crm.Services.Interfaces;
	using NHibernate.Linq;

	/// <summary>
	/// Kagema-specific NoteSyncService that filters notes to only sync files from technician's assigned service orders
	/// This reduces the amount of data synchronized to mobile devices by only including relevant documents
	/// </summary>
	public class KagemaNoteSyncService : NoteSyncService
	{
		private readonly IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository;
		private readonly IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository;

		public KagemaNoteSyncService(
			IRepositoryWithTypedId<Note, Guid> repository, 
			RestTypeProvider restTypeProvider, 
			IRestSerializer restSerializer, 
			IPluginProvider pluginProvider, 
			IMapper mapper, 
			INoteService noteService, 
			ILookupManager lookupManager,
			IRepositoryWithTypedId<ServiceOrderHead, Guid> serviceOrderRepository,
			IRepositoryWithTypedId<ServiceOrderDispatch, Guid> dispatchRepository)
			: base(repository, restTypeProvider, restSerializer, pluginProvider, mapper, noteService, lookupManager)
		{
			this.serviceOrderRepository = serviceOrderRepository;
			this.dispatchRepository = dispatchRepository;
		}

		public override IQueryable<Note> GetAll(User user, IDictionary<string, int?> groups)
		{
			// Get the base filtered notes
			var baseNotes = base.GetAll(user, groups);

			// Get technician's assigned service orders (non-closed)
			var technicianServiceOrders = GetTechnicianAssignedServiceOrders(user);

			// Filter notes to only include those related to technician's service orders
			var filteredNotes = baseNotes.Where(note => 
				// Include notes that don't have a reference key (general notes)
				note.ReferenceKey == null ||
				// Include notes related to technician's assigned service orders
				technicianServiceOrders.Any(order => order.Id == note.ReferenceKey) ||
				// Include notes related to dispatches for technician's service orders
				dispatchRepository.GetAll()
					.Where(dispatch => dispatch.DispatchedUsername == user.Id && 
							 technicianServiceOrders.Any(order => order.Id == dispatch.OrderId))
					.Any(dispatch => dispatch.Id == note.ReferenceKey)
			);

			return filteredNotes;
		}

		/// <summary>
		/// Gets service orders assigned to the technician that are not closed
		/// </summary>
		/// <param name="user">The technician user</param>
		/// <returns>Service orders assigned to the technician</returns>
		private IQueryable<ServiceOrderHead> GetTechnicianAssignedServiceOrders(User user)
		{
			// Define non-closed status keys (excluding completed, invoiced, etc.)
			var openStatusKeys = new[]
			{
				ServiceOrderStatus.NewKey,
				ServiceOrderStatus.ReadyForSchedulingKey,
				ServiceOrderStatus.ScheduledKey,
				ServiceOrderStatus.PartiallyReleasedKey,
				ServiceOrderStatus.ReleasedKey,
				ServiceOrderStatus.InProgressKey,
				ServiceOrderStatus.PartiallyCompletedKey,
				ServiceOrderStatus.PostProcessingKey,
				ServiceOrderStatus.ReadyForInvoiceKey
			};

			// Get service orders assigned to the technician through dispatches
			var assignedOrderIds = dispatchRepository.GetAll()
				.Where(dispatch => dispatch.DispatchedUsername == user.Id)
				.Select(dispatch => dispatch.OrderId)
				.Distinct();

			// Return service orders that are:
			// 1. Assigned to the technician via dispatch
			// 2. Not in closed status
			return serviceOrderRepository.GetAll()
				.Where(order => assignedOrderIds.Contains(order.Id) && 
							   openStatusKeys.Contains(order.StatusKey));
		}
	}
}