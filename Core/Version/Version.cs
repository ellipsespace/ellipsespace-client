using System.Reflection;

namespace EllipseSpaceClient.Core.Version
{
    public struct Version
    {
        public static Version zero = new Version(0, 0, 0, 0);

        private short major;
        private short minor;
        private short build;
        private short revision;

        public Version(short major, short minor, short build, short revision)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = revision;
        }

        public Version(string version)
        {
            string[] versionStrings = version.Split('.');
            if (versionStrings.Length != 4)
            {
                this = zero;
                return;
            }

            major = short.Parse(versionStrings[0]);
            minor = short.Parse(versionStrings[1]);
            build = short.Parse(versionStrings[2]);
            revision = short.Parse(versionStrings[3]);
        }

        public static Version Local()
        {
            return new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        public static bool operator >(Version current, Version other)
        {
            if (current.major > other.major)
                return true;
            else
            {
                if (current.minor > other.minor)
                    return true;
                else
                {
                    if (current.build > other.build)
                        return true;

                    else
                    {
                        if (current.revision > other.revision)
                            return true;
                    }
                }
            }

            return false;
        }

        public static bool operator <(Version current, Version other)
        {
            if (current.major < other.major)
                return true;
            else
            {
                if (current.minor < other.minor)
                    return true;
                if (current.build < other.build)
                    return true;

                else
                {
                    if (current.revision < other.revision)
                        return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return $"{major}.{minor}.{build}.{revision}";
        }
    }
}
