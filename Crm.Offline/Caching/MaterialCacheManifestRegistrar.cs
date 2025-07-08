namespace Crm.Offline.Caching
{

	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Offline;
	using Crm.Library.Offline.Interfaces;

	public class MaterialCacheManifestRegistrar : CacheManifestRegistrar<MaterialCacheManifest>
	{
		public MaterialCacheManifestRegistrar(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
		}
		protected override void Initialize()
		{
			CacheJs( "offlineMaterialJs");
			Cache("Offline", "Files");
			Cache("~/Content/js/sqlite-wasm/sqlite3-opfs-async-proxy.js");
			Cache("~/Content/js/sqlite-wasm/sqlite3-websql-polyfill-worker.js");
			Cache("~/Content/js/sqlite-wasm/sqlite3.js");
			Cache("~/Content/js/sqlite-wasm/sqlite3.wasm");
		}
	}
}