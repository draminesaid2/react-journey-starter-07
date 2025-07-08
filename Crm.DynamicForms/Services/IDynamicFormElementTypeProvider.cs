namespace Crm.DynamicForms.Services
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.AutoFac;

	public interface IDynamicFormElementTypeProvider : ISingletonDependency
	{
		Dictionary<string, Type> ElementTypes { get; }
		string ParseToClient(string formElementTypeName, string value);
		string ParseFromClient(string formElementTypeName, string value);
	}
}