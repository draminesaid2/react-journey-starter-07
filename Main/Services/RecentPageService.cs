namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using log4net;

	public class RecentPageService : IRecentPageService
	{
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;
		private readonly IRepositoryWithTypedId<RecentPage, Guid> recentPageRepository;
		private readonly ILog logger;
		private readonly Func<RecentPage> recentPageFactory;

		public RecentPageService(IRepositoryWithTypedId<RecentPage, Guid> recentPageRepository,ILog logger, IRepositoryWithTypedId<Contact, Guid> contactRepository, Func<RecentPage> recentPageFactory)
		{
			this.recentPageRepository = recentPageRepository;
			this.logger = logger;
			this.contactRepository = contactRepository;
			this.recentPageFactory = recentPageFactory;
		}
		public virtual void AddRecentPage(string username, string title, string url, Guid contactId)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(url))
			{
				return;
			}
			var recentPage = recentPageRepository.GetAll().Where(x => x.Url.ToLower() == url.ToLower() && x.Username == username).FirstOrDefault();
			if (recentPage != null)
			{
				recentPage.Count += 1;
			}
			else
			{
				var contact = contactRepository.Get(contactId);
				recentPage = recentPageFactory();
				recentPage.Title = title;
				recentPage.Url = url;
				recentPage.Username = username;
				recentPage.ContactId = contactId;
				recentPage.Count = 1;
				recentPage.Id = Guid.NewGuid();
				recentPage.AuthData = contact.AuthData != null ? new LMobile.Unicore.EntityAuthData { DomainId = contact.AuthData.DomainId } : null;
			}
			recentPageRepository.SaveOrUpdate(recentPage);
			logger.Debug(string.Format("RecentPage successfully added to the recentPageCache and waiting for processing: ContactId:{0}; Username:{1}; Title:{2}; ModifyDate:{3}", recentPage.ContactId, recentPage.Username, recentPage.Title, recentPage.ModifyDate));
		}
		public virtual void RemoveRecentPages(List<string> urls)
		{
			var recentPages = recentPageRepository.GetAll().Where(x => urls.Select(y => y.ToLower()).Contains(x.Url.ToLower()));
			foreach (RecentPage recentPage in recentPages)
			{
				recentPageRepository.Delete(recentPage);
				logger.Debug(string.Format("RecentPage entry successfully deleted. ContactId:{0}; Username:{1}; Title:{2}; ModifyDate:{3}", recentPage.ContactId, recentPage.Username, recentPage.Title, recentPage.ModifyDate));
			}
		}
		public virtual void RemoveRecentPages(List<string> urls, string username)
		{
			var recentPages = recentPageRepository.GetAll().Where(x => x.Username == username && urls.Select(y => y.ToLower()).ToArray().Contains(x.Url.ToLower()));
			foreach (RecentPage recentPage in recentPages)
			{
				recentPageRepository.Delete(recentPage);
				logger.Debug(string.Format("RecentPage entry successfully deleted. ContactId:{0}; Username:{1}; Title:{2}; ModifyDate:{3}", recentPage.ContactId, recentPage.Username, recentPage.Title, recentPage.ModifyDate));
			}
		}
		public virtual IEnumerable<RecentPage> GetRecentPages(string username, int limit = 15)
		{
			return recentPageRepository.GetAll().Where(x => x.Username == username && !x.IsMaterial)
				.OrderByDescending(x => x.ModifyDate)
				.Take(limit);
		}
		public virtual RecentPage GetRecentPage(Guid contactId, string username)
		{
			return recentPageRepository.GetAll().Where(x => x.ContactId == contactId&&x.Username==username).FirstOrDefault();
		}

		public virtual void RemoveRecentPagesByContactId(Guid contactId)
		{
			var recentPages = recentPageRepository.GetAll().Where(x => x.ContactId == contactId).ToList();
			foreach (var recentPage in recentPages)
			{
				recentPageRepository.Delete(recentPage);
				logger.Debug(string.Format("RecentPage entry successfully deleted. ContactId:{0}; Username:{1}; Title:{2}; ModifyDate:{3}", recentPage.ContactId, recentPage.Username, recentPage.Title, recentPage.ModifyDate));
			}
		}
	}
}
