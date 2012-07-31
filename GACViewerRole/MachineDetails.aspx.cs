using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Collections;

namespace GACViewerRole
{
    public partial class MachineDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MemoryStatus stat = new MemoryStatus();
            GlobalMemoryStatus(out stat);

            long ram = (long)stat.TotalPhysical;

            StringBuilder builder = new StringBuilder();
            builder.Append("<pre>");

            builder.AppendLine(" ===== MEMORY STATUS ==== ");
            builder.AppendLine("Physical");
            builder.AppendLine(string.Format("{0,-30} {1,15}", "  Total:", MakeHumanReadable((long)stat.TotalPhysical)));
            builder.AppendLine(string.Format("{0,-30} {1,15}", "  Available:", MakeHumanReadable((long)stat.AvailablePhysical)));
            builder.AppendLine("Page File");
            builder.AppendLine(string.Format("{0,-30} {1,15}", "  Total:", MakeHumanReadable((long)stat.TotalPageFile)));
            builder.AppendLine(string.Format("{0,-30} {1,15}", "  Available:", MakeHumanReadable((long)stat.AvailablePageFile)));
            builder.AppendLine(string.Empty);
            builder.AppendLine("Virtual");
            builder.AppendLine(string.Format("{0,-30} {1,15}", "  Total:", MakeHumanReadable((long)stat.TotalVirtual)));
            builder.AppendLine(string.Format("{0,-30} {1,15}", "  Available:", MakeHumanReadable((long)stat.AvailableVirtual)));

            builder.AppendLine(string.Empty);
            builder.AppendLine(string.Empty);
            builder.AppendLine(" ===== HARD DRIVES STATUS ==== ");


            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                builder.AppendLine(string.Format("Drive {0}", d.Name));
                builder.AppendLine(string.Format("  Drive type: {0}", d.DriveType));

                if (d.IsReady == true)
                {
                    builder.AppendLine(string.Format("  Volume label: {0}", d.VolumeLabel));
                    builder.AppendLine(string.Format("  File system: {0}", d.DriveFormat));
                    builder.AppendLine(string.Format("{0,-30} {1,11}",
                        "  Available space to current user:",
                        MakeHumanReadable(d.AvailableFreeSpace)));

                    builder.AppendLine(string.Format("{0,-30} {1,15}",
                        "  Total available space:",
                        MakeHumanReadable(d.TotalFreeSpace)));

                    builder.AppendLine(string.Format("{0,-30} {1,15}",
                        "  Total size of drive:",
                        MakeHumanReadable(d.TotalSize)));
                }

                builder.AppendLine(string.Empty);
            }

            builder.AppendLine(string.Empty);
            builder.AppendLine(string.Empty);
            builder.AppendLine(" ===== MISC SYSTEM INFORMATION ===== ");

            builder.AppendLine(string.Format("{0,-22} {1}", "Machine Name:", System.Environment.MachineName));
            builder.AppendLine(string.Format("{0,-22} {1}", "User Domain:", System.Environment.UserDomainName));
            builder.AppendLine(string.Format("{0,-22} {1}", "UserName:", System.Environment.UserName));
            builder.AppendLine(string.Format("{0,-22} {1}", "OS Version:", System.Environment.OSVersion));
            builder.AppendLine(string.Format("{0,-22} {1}", "64 Bit OS:", System.Environment.Is64BitOperatingSystem));
            builder.AppendLine(string.Format("{0,-22} {1}", "64 Bit Process:", System.Environment.Is64BitProcess));
            builder.AppendLine(string.Format("{0,-22} {1}", "Processor Count:", System.Environment.ProcessorCount));
            builder.AppendLine(string.Format("{0,-22} {1}", "System Directory:", System.Environment.SystemDirectory));
            builder.AppendLine(string.Format("{0,-22} {1}", "System Page Size:", System.Environment.SystemPageSize));
            builder.AppendLine(string.Format("{0,-22} {1}", "Working Set:", System.Environment.WorkingSet));
            builder.AppendLine(string.Format("{0,-22} {1}", "Interactive Process:", System.Environment.UserInteractive));
            builder.AppendLine(string.Format("{0,-22} {1}", "CLR Version:", System.Environment.Version));

            // environment variables?
            builder.AppendLine(string.Empty);
            builder.AppendLine(string.Empty);
            builder.AppendLine(" ===== ENVIRONMENT VARIABLES ===== ");
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();

            foreach (DictionaryEntry de in environmentVariables)
            {
                builder.AppendLine(string.Format("{0,-24} {1}", de.Key, de.Value));
            }

            builder.AppendLine("</pre>");

            this.machineblob.Text = builder.ToString();
        }

        static string MakeHumanReadable(long memoryValue)
        {
            string output = string.Empty;
            double tempValue = memoryValue;

            output = string.Format("{0:N2} B ", tempValue);

            // convert to KB
            if ((tempValue > 1024))
            {
                tempValue = tempValue / 1024;
                output = string.Format("{0:N2} KB", tempValue);
            }

            // convert to MB
            if ((tempValue > 1024))
            {
                tempValue = tempValue / 1024;
                output = string.Format("{0:N2} MB", tempValue);
            }

            // convert to GB
            if ((tempValue > 1024))
            {
                tempValue = tempValue / 1024;
                output = string.Format("{0:0.00} GB", tempValue);
            }

            return output;
        }

        public struct MemoryStatus
        {
            public uint Length;
            public uint MemoryLoad;
            public uint TotalPhysical;
            public uint AvailablePhysical;
            public uint TotalPageFile;
            public uint AvailablePageFile;
            public uint TotalVirtual;
            public uint AvailableVirtual;
        }

        [DllImport("kernel32.dll")]
        public static extern void GlobalMemoryStatus(out MemoryStatus stat);
    }
}