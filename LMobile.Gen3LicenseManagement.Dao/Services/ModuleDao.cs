using System.Collections.Generic;
using LMobile.Gida;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gen3LicenseManagement.Dao.Contracts;
using LMobile.Gen3LicenseManagement.Dao.Mapping;
using System;
using System.Linq;
using System.Threading;
using System.Net.Mail;

namespace LMobile.Gen3LicenseManagement.Dao.Services
{
  internal class ModuleDao : BaseDao, IModuleDao
  {
    public ModuleDao(GidaSession session)
      : base(session)
    {
    }

    public List<Module> GetModules()
    {
      var query = Session.Query<Module, ModuleMapping>().OrderBy(s => s.Main.Description);
      return query.ReadList();
    }
    public List<Module> GetModules(string projectType)
    {
      var query = Session.Query<Module, ModuleMapping>().OrderBy(s => s.Main.Description);
      query.Where(s => s.Main.ProjectType == projectType);
      return query.ReadList();
    }
    public Module GetModule(int moduleID)
    {
      if (moduleID == 0) return null;
      var query = Session.Query<Module, ModuleMapping>();
      query.Where(s => s.Main.ID == moduleID);
      return query.ReadFirst();
    }

    public List<ModuleProperty> GetModuleProperties()
    {
      var query = Session.Query<ModuleProperty, ModulePropertyMapping>().OrderBy(s => s.Main.PropertyName).OrderBy(s => s.Main.Description);
      return query.ReadList();
    }
    public List<ModuleProperty> GetModuleProperties(int moduleID)
    {
      if (moduleID == 0) return null;

      var ids = Session.Query<ModulePropertiesInModules, ModulePropertiesInModulesMapping>()
          .Where(x => x.Main.ModuleID == moduleID)
          .ReadList().Select(x => x.ModulePropertyID).ToList();

      var query = Session.Query<ModuleProperty, ModulePropertyMapping>();
      query.Where(s =>
        s.Main.ID.In(ids.ToArray()));
      query.OrderBy(s => s.Main.Description);
      return query.ReadList();
    }
    //MLI 2017-11-30: Module search
    public List<ModuleProperty> GetModulePropertiesByName(string p_Name)
    {
      if (string.IsNullOrEmpty(p_Name)) return null;

      string name = string.Format("%{0}%", p_Name.ToUpper());

      var query = Session.Query<ModuleProperty, ModulePropertyMapping>();
      query.Where(s =>
        s.Main.PropertyName.ToUpper().Like(name)
        | s.Main.Description.ToUpper().Like(name));

      query.OrderBy(s => s.Main.Description);
      return query.ReadList();
    }
  public List<ModuleProperty> GetModulePropertiesByName(string p_Name, int p_ProjectID)
    {
      if (string.IsNullOrEmpty(p_Name)) return null;

      string name = string.Format("%{0}%", p_Name.ToUpper());

//select* from[ModuleProperty]  mp
//where 1 = 1
// AND(UPPER(mp.[PropertyName]) LIKE '%20%'      OR UPPER(mp.[Desription]) LIKE '%20%')
//AND Not Exists(select * from[ProjectModuleProperty] pmp where 1 = 1 AND pmp.projectID = 146 and pmp.ModulePropertyID = mp.ID)

      var query = Session.Query<ModuleProperty, ModulePropertyMapping>();
      query.Where(mp => (mp.Main.PropertyName.ToUpper().Like(name) | mp.Main.Description.ToUpper().Like(name))
       & Gql.NotExists(Session.Query().From<ProjectModulePropertyMapping>().Where((pmp) => pmp.Main.ProjectID == p_ProjectID & pmp.Main.ModulePropertyID == mp.Main.ID)));
      

      //query.Where(x => Gql.NotExists(Session.Query()
      //                            .From<ModulePropertyMapping>()
      //                            .InnerJoin<ProjectModulePropertyMapping>((mp, pmp) => pmp.Main.ModulePropertyID == mp.Main.ID 
      //                                                                                  & pmp.Main .ProjectID == p_ProjectID)
      //                            .Where((mp, pmp) => mp.Main.PropertyName.ToUpper().Like(name)
      //                                                                                      | mp.Main.Description.ToUpper().Like(name))
                                  
      //                            )
      //            );
      query.OrderBy(s => s.Main.Description);
      return query.ReadList();
    }

    public List<ModulePropertiesInModules> GetModulePropertiesInModules()
    {
      var query = Session.Query<ModulePropertiesInModules, ModulePropertiesInModulesMapping>().OrderBy(s => s.Main.ModuleID).OrderBy(s => s.Main.ModulePropertyID);
      return query.ReadList();
    }
    public ModuleProperty GetModuleProperty(int modulePropertyID)
    {
      if (modulePropertyID == 0) return null;
      var query = Session.Query<ModuleProperty, ModulePropertyMapping>();
      query.Where(s => s.Main.ID == modulePropertyID);
      return query.ReadFirst();
    }

    public bool DeleteModule(int moduleID)
    {
      if (moduleID == 0) return false;
      
      // First, get the module to ensure it exists and to get its description for the log entry
      var module = GetModule(moduleID);
      if (module == null) return false;
      
      // Delete all associated module properties in modules
      var modulePropertiesInModules = Session.Query<ModulePropertiesInModules, ModulePropertiesInModulesMapping>()
        .Where(x => x.Main.ModuleID == moduleID)
        .ReadList();
      
      foreach (var item in modulePropertiesInModules)
      {
        Session.Delete(item);
      }
      
      // Delete the module itself
      return Session.Delete(module);
    }

    public bool DeleteModuleProperty(int modulePropertyID)
    {
      if (modulePropertyID == 0) return false;
      
      // First, get the module property to ensure it exists
      var moduleProperty = GetModuleProperty(modulePropertyID);
      if (moduleProperty == null) return false;
      
      // Delete all associated module properties in modules entries
      var modulePropertiesInModules = Session.Query<ModulePropertiesInModules, ModulePropertiesInModulesMapping>()
        .Where(x => x.Main.ModulePropertyID == modulePropertyID)
        .ReadList();
      
      foreach (var item in modulePropertiesInModules)
      {
        Session.Delete(item);
      }
      
      // Delete all associated project module properties
      var projectModuleProperties = Session.Query<ProjectModuleProperty, ProjectModulePropertyMapping>()
        .Where(x => x.Main.ModulePropertyID == modulePropertyID)
        .ReadList();
      
      foreach (var item in projectModuleProperties)
      {
        Session.Delete(item);
      }
      
      // Delete the module property itself
      return Session.Delete(moduleProperty);
    }
  }
}
