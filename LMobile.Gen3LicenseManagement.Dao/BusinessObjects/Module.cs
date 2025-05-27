using System;
using System.Collections.Generic;
namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
  public class Module : StoredModule
  {
    public static implicit operator Module(ModuleProperty v)
    {
      throw new NotImplementedException();
    }
  }
}
