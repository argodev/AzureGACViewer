using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GACViewerRole.Model
{
    public class ProjectReference
    {
        public ProjectReference(String include, Boolean? privateFlag, Boolean available)
        {
            FullName = include;
            Private = privateFlag;
            Available = available;
        }

        public Boolean? Private { get; private set; }
        public String FullName { get; private set; }
        public Boolean Available { get; private set; }
    }
}