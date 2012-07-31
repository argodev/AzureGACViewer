using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using GACViewerRole.Model;
using System.GAC;
using System.Text;

namespace GACViewerRole
{
    public class AzureMachineData : DataService<MachineDataService>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            // config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }
    }

    public class MachineDataService
    {
        private List<GacAssembly> assemblies;

        public IQueryable<GacAssembly> Assemblies
        {
            get
            {
                if (assemblies == null)
                {
                    Initialize();
                }

                return this.assemblies.AsQueryable<GacAssembly>();
            }
        }

        private void Initialize()
        {
            assemblies = new List<GacAssembly>();
            var result = from assembly in GetAssemblies() orderby assembly.Name.Name select assembly;

            foreach (var assembly in result)
            {
                assemblies.Add(new GacAssembly() 
                { 
                    Name = assembly.Name.Name,
                    FullName = assembly.Name.FullName,
                    Version = assembly.Name.Version.ToString(),
                    CultureInfo = string.IsNullOrEmpty(assembly.Name.CultureInfo.ToString()) ? "neutral" : assembly.Name.CultureInfo.ToString(),
                    PublicKeyToken = TokenToHumanReadable(assembly.Name.GetPublicKeyToken())
                });
            }
        }

        private string TokenToHumanReadable(byte[] tokenData)
        {
            StringBuilder humanReadable = new StringBuilder();
            
            for (int i=0; i < tokenData.GetLength(0); i++)
            {
                humanReadable.AppendFormat("{0:x}", tokenData[i]);
            }

            return humanReadable.ToString();
        }

        private IEnumerable<AssemblyItem> GetAssemblies()
        {
            IAssemblyEnum assemblyEnum = AssemblyCache.CreateGACEnum();
            IAssemblyName assemblyName;
            while (AssemblyCache.GetNextAssembly(assemblyEnum, out assemblyName) == 0)
                yield return (new AssemblyItem(assemblyName));
        }
    }

    [DataServiceKey("Name")]
    public class GacAssembly
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Version { get; set; }
        public string CultureInfo { get; set; }
        public string PublicKeyToken { get; set; }
    }
}
