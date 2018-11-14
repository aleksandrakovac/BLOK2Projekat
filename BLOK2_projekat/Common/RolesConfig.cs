using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum Permissions { Read = 0, Generate = 1, Delete = 2, DeleteAll = 3};
    public enum Roles { Reader = 0, AlarmGenerator = 1, AlarmAdmin = 2};

    public class RolesConfig
    {
        private static ResourceManager resourceManager = null;
        private static ResourceSet resourceSet = null;
        private static object resourceLock = new object();

        static string[] ReaderPermissions =  GetReader();
        static string[] GeneratorPermissions = GetGenerator();
        static string[] AdminPermissions = GetAdmin();
        static string[] Empty = new string[] { };

        private static ResourceManager ResourceManager
        {
            get
            {
                lock(resourceLock)
                {
                    if (resourceManager == null)
                        resourceManager = new ResourceManager(typeof(RolesConfigFile).FullName, Assembly.GetExecutingAssembly()); 
                    return resourceManager;
                }
            }
        }

        private static ResourceSet ResourceSet
        {
            get
            {
                lock(resourceLock)
                {
                    if(resourceSet == null)
                    {
                        resourceSet = ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                    }
                    return resourceSet;
                }
            }
        }

        public static string[] GetReader()
        {
            string x = ResourceManager.GetString(Roles.Reader.ToString());
            string[] permissionsStr = { };
            if (x != null)
            {               
                permissionsStr = x.Split(',');
                return permissionsStr;
            }

            return permissionsStr;
        }

        public static string[] GetGenerator()
        {
            string x = ResourceManager.GetString(Roles.AlarmGenerator.ToString());
            string[] permissionsStr = { };
            if (x != null)
            {
                permissionsStr = x.Split(',');
                return permissionsStr;
            }

            return permissionsStr;
        }

        public static string[] GetAdmin()
        {
            string x = ResourceManager.GetString(Roles.AlarmAdmin.ToString());
            string[] permissionsStr = { };
            if (x != null)
            {
                permissionsStr = x.Split(',');
                return permissionsStr;
            }

            return permissionsStr;
        }

        public static string[] GetPermissions(string role)
        {
            switch(role)
            {
                case "Reader": return ReaderPermissions;
                case "AlarmGenerator": return GeneratorPermissions;
                case "AlarmAdmin": return AdminPermissions;
                default: return Empty;
            }
        }

    }
}
