using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.GAC;
using System.Reflection;
using GACViewerRole.Model;
using System.Runtime.InteropServices;

namespace GACViewerRole
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gridView_Init(object sender, EventArgs e)
        {
            GridView gridView = (GridView)sender;
            gridView.DataSource = (from assembly in Assemblies() orderby assembly.Name.Name select assembly);
            gridView.DataBind();
        }

        private IEnumerable<AssemblyItem> Assemblies()
        {
            IAssemblyEnum assemblyEnum = AssemblyCache.CreateGACEnum();
            IAssemblyName assemblyName;
            while (AssemblyCache.GetNextAssembly(assemblyEnum, out assemblyName) == 0)
                yield return (new AssemblyItem(assemblyName));
        }
    }
}
