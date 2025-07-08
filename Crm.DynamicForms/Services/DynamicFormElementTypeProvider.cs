namespace Crm.DynamicForms.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Modularization;

	public class DynamicFormElementTypeProvider : IDynamicFormElementTypeProvider
	{
		private readonly Dictionary<string, Type> elementTypeCache = new Dictionary<string, Type>();
		private readonly Dictionary<string, DynamicFormElement> elementCache = new Dictionary<string, DynamicFormElement>();

		public virtual Dictionary<string, Type> ElementTypes { get { return elementTypeCache; } }
		
		public virtual string ParseToClient(string formElementTypeName, string value)
		{
			return elementCache[formElementTypeName].ParseToClient(value);
		}
		public virtual string ParseFromClient(string formElementTypeName, string value)
		{
			return value == null ? null : elementCache[formElementTypeName].ParseFromClient(value);
		}

		public DynamicFormElementTypeProvider(IEnumerable<Plugin> activePlugins)
		{
			var elementTypes = activePlugins.SelectMany(x => x.Assembly.GetTypesInheriting<IDynamicFormElement>().Where(t => !t.IsAbstract)).ToArray();

			foreach (var elementType in elementTypes)
			{
				elementTypeCache.Add(elementType.Name, elementType);
				elementCache.Add(elementType.Name, (DynamicFormElement)Activator.CreateInstance(elementType));
			}
		}
	}
}
