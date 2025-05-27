
﻿using System.Collections.Generic;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Contracts
{
  public interface IModuleDao
  {
    List<Module> GetModules();
    List<Module> GetModules(string projectType);
    Module GetModule(int moduleID);
    bool DeleteModule(int moduleID); // Added new method

    List<ModuleProperty> GetModuleProperties();
    List<ModuleProperty> GetModuleProperties(int moduleID);
    //MLI 2017-11-30: Module search
    List<ModuleProperty> GetModulePropertiesByName(string p_Name);
    List<ModuleProperty> GetModulePropertiesByName(string p_Name, int p_ProjecID);
    List<ModulePropertiesInModules> GetModulePropertiesInModules();
    ModuleProperty GetModuleProperty(int modulePropertyID);
  }
}
