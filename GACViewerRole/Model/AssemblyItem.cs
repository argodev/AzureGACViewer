using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.GAC;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data.Services.Common;

namespace GACViewerRole.Model
{
    [DataServiceKey("FullName")]
    public class AssemblyItem : IComparable
    {
        public AssemblyItem(IAssemblyName assemblyName)
        {
            Interface = assemblyName;
        }

        private IAssemblyName Interface { get; set; }

        private AssemblyName _name;

        public AssemblyName Name
        {
            get
            {
                if (_name == null)
                {
                    Initialize();
                }

                return _name;
            }
        }

        private void Initialize()
        {
            _name = new AssemblyName();
            _name.Name = AssemblyCache.GetName(Interface);
            _name.Version = AssemblyCache.GetVersion(Interface);
            _name.CultureInfo = AssemblyCache.GetCulture(Interface);
            _name.SetPublicKeyToken(AssemblyCache.GetPublicKeyToken(Interface));
        }
        

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return (Name.Name.CompareTo(obj));
        }

        #endregion
    }
}