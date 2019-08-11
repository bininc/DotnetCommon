using Microsoft.Build.Framework;
using System;

namespace BuildVersion.Task
{

    public class GetCurrentBuildVersion : Microsoft.Build.Utilities.Task
    {
        [Output]
        public string Version { get; set; }

        public string BaseVersion { get; set; }

        public override bool Execute()
        {
            var originalVersion = System.Version.Parse(BaseVersion ?? "1.0.0");

            Version = GetCurrentBuildVersionString(originalVersion);

            return true;
        }

        private static string GetCurrentBuildVersionString(Version baseVersion)
        {
            DateTime d = DateTime.Now;
            return new Version(baseVersion.Major, baseVersion.Minor,
                (DateTime.Today - new DateTime(2000, 1, 1)).Days,
                (int)new TimeSpan(d.Hour, d.Minute, d.Second).TotalSeconds / 2).ToString();
        }
    }
}
