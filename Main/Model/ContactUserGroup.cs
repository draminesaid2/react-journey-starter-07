﻿namespace Crm.Model
{
	using System;

	public class ContactUserGroup
	{
		public virtual Guid UsergroupKey { get; protected set; }
		public virtual Guid ContactKey { get; protected set; }
		public override bool Equals(object obj) => base.Equals(obj);
		public override int GetHashCode() => base.GetHashCode();
	}
}