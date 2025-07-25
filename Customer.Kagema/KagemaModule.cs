﻿using Autofac;
using Autofac.Core;

using Crm.Library.AutoFac;
using Crm.Service.BackgroundServices;
using Crm.Services;
using Customer.Kagema.BackgroundServices;
using Customer.Kagema.Services;

namespace Customer.Kagema
{

	public class KagemaModule : Module
	{
		protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
		{
			base.AttachToComponentRegistration(componentRegistry, registration);
			registration.ReplaceRegistration<DispatchReportSenderAgent, KagemaDispatchReportSenderAgent>();
			registration.ReplaceRegistration<ServiceOrderDocumentSaverAgent, KagemaServiceOrderDocumentSaverAgent>();
			registration.ReplaceRegistration<NoteSyncService, KagemaNoteSyncService>();
		}
	}
}
