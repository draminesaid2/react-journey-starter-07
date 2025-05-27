using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;
using System.Threading;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class StoredObjectMapping : GidaTableMapping
	{
		public readonly GidaColumnMapping ID;
		public readonly GidaColumnMapping CreateUser;
		public readonly GidaColumnMapping CreateDate;
		public readonly GidaColumnMapping ModifyUser;
		public readonly GidaColumnMapping ModifyDate;
		public readonly GidaColumnMapping UpdateCount;

		public StoredObjectMapping(GidaSession session, string tableName)
			: base(session, tableName)
		{
			var define = this.Define<StoredObject>();
			this.ID = define.Column("ID", o => o.ID, (o, value) => o.ID = value, GidaTableColumn.AutoPrimaryKey);
			this.CreateUser = define.Column("CreateUser", 128, o => o.CreateUser, (o, value) => o.CreateUser = value);
			this.CreateDate = define.Column("CreateDate", o => o.CreateDate, (o, value) => o.CreateDate = value);
			this.ModifyUser = define.Column("ModifyUser", o => o.ModifyUser, (o, value) => o.ModifyUser = value);
			this.ModifyDate = define.Column("ModifyDate", o => o.ModifyDate, (o, value) => o.ModifyDate = value);
			this.UpdateCount = define.Column("UpdateCount", o => o.UpdateCount, (o, value) => o.UpdateCount = value, GidaTableColumn.UpdateCounter);
		}
		protected override void OnInserting(object entity) {
			base.OnInserting(entity);
			var o = (StoredObject) entity;
			o.CreateUser = Thread.CurrentPrincipal.Identity.Name;
			o.CreateDate = DateTime.Now;
		}
		protected override void OnUpdating(object entity) {
			base.OnUpdating(entity);
			var o = (StoredObject)entity;
			o.ModifyUser = Thread.CurrentPrincipal.Identity.Name;
			o.ModifyDate = DateTime.Now;
		}
	}
	
}
