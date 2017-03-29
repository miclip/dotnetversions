using dotnetversions.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace dotnetversions.Helpers
{
    public class DotnetVersions
    {
        public static IList<DotnetVersion> GetAllDotnetVersions()
        {
            var versions = new List<DotnetVersion>();
            versions.AddRange(GetPre45VersionFromRegistry());
            versions.AddRange(Get45PlusFromRegistry());
            return versions;
        }


        private static IList<DotnetVersion> Get45PlusFromRegistry()
        {
            var versions = new List<DotnetVersion>();

            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    var version = new DotnetVersion();
                    version.Name = CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
                    versions.Add(version);
                }
              
            }

            return versions;
        }

        private static IList<DotnetVersion> GetPre45VersionFromRegistry()
        {
            var versions = new List<DotnetVersion>();

            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5 
                // or later, you can use:
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {
                        var version = new DotnetVersion();
                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        version.VersionKeyName = versionKeyName;
                        version.Name = (string)versionKey.GetValue("Version", "");
                        version.ServicePack = versionKey.GetValue("SP", "").ToString();
                        version.Install = versionKey.GetValue("Install", "").ToString();
                        
                        if (version.Name != "")
                            continue;
                        
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            if (version.SubVersions == null)
                                version.SubVersions = new List<DotnetVersion>();

                            var subVersion = new DotnetVersion();
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            subVersion.VersionKeyName = subKeyName;
                            subVersion.Name = (string)subKey.GetValue("Version", "");
                            if (subVersion.Name != "")
                                subVersion.ServicePack = subKey.GetValue("SP", "").ToString();

                            subVersion.Install = subKey.GetValue("Install", "").ToString();
                            version.SubVersions.Add(subVersion);
                        }

                        versions.Add(version);
                    }
                }
            }

            return versions;
        }

        // Checking the version using >= will enable forward compatibility.
        private static string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 394802)
                return "4.6.2 or later";
            if (releaseKey >= 394254)
            {
                return "4.6.1";
            }
            if (releaseKey >= 393295)
            {
                return "4.6";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5";
            }
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }
    }
}
