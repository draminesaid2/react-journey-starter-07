namespace Crm.Model.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Model;

	public static class UserExtensions
	{
		public static string GetDropboxAddress(this User user, string dropboxDomain)
		{
			if (user == null || string.IsNullOrWhiteSpace(user.DropboxToken) || string.IsNullOrWhiteSpace(dropboxDomain))
			{
				return null;
			}

			return string.Format("{0}@{1}", user.DropboxToken, dropboxDomain);
		}

		public static User GetByUserId(this IEnumerable<User> users, Guid userId)
		{
			return GetByUserId(users?.AsQueryable(), userId);
		}
		public static User GetByUserId(this IQueryable<User> users, Guid userId)
		{
			return users?.FirstOrDefault(x => x.UserId == userId);
		}
		public static User GetById(this IEnumerable<User> users, string id)
		{
			return GetById(users?.AsQueryable(), id);
		}
		public static User GetById(this IQueryable<User> users, string id)
		{
			if (id != null)
			{
				return users?.FirstOrDefault(x => x.Id.ToLower() == id.ToLower());
			}
			return null;
		}
		public static User GetByEmail(this IEnumerable<User> users, string email)
		{
			return GetByEmail(users?.AsQueryable(), email);
		}
		public static User GetByEmail(this IQueryable<User> users, string email)
		{
			if (email != null)
			{
				return users?.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
			}
			return null;
		}
		public static User GetByDropboxToken(this IEnumerable<User> users, string dropboxToken)
		{
			return GetByDropboxToken(users?.AsQueryable(), dropboxToken);
		}
		public static User GetByDropboxToken(this IQueryable<User> users, string dropboxToken)
		{
			if (dropboxToken != null)
			{
				return users?.FirstOrDefault(x => x.DropboxToken != null && x.DropboxToken.ToLower() == dropboxToken.ToLower());
			}
			return null;
		}
		public static User GetByGeneralToken(this IEnumerable<User> users, string generalToken)
		{
			return GetByGeneralToken(users?.AsQueryable(), generalToken);
		}
		public static User GetByGeneralToken(this IQueryable<User> users, string generalToken)
		{
			if (generalToken != null)
			{
				return users?.FirstOrDefault(x => x.GeneralToken != null && x.GeneralToken.ToLower() == generalToken.ToLower());
			}
			return null;
		}
	}
}