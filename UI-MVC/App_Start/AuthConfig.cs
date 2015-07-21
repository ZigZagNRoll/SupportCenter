using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace SC.UI.Web.MVC.App_Start
{
    public class AuthConfig
    {
        public static void InitializeDatabase(bool registerDefaultUsers = false, bool registerDefaultRoles = false)
        {
            WebSecurity.InitializeDatabaseConnection("SupportCenterSecurityDB_SMP", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            
            if (registerDefaultUsers)
            {
                AuthConfig.RegisterDefaultUsers();
            }
            if (registerDefaultRoles)
            {
                AuthConfig.RegisterDefaultRoles();
                
            }
        }

        private static void RegisterDefaultRoles()
        {
            if (!Roles.RoleExists("Demo"))
            {
                Roles.CreateRole("Demo");
                Roles.AddUserToRole("DemoA", "Demo");
                Roles.AddUserToRole("DemoB", "Demo");            }

            if (!Roles.RoleExists("Admin"))
            {
                Roles.CreateRole("Admin");
                Roles.AddUserToRole("Admin", "Admin");
            }

            if (!Roles.RoleExists("User")) Roles.CreateRole("User");
        }

        private static void RegisterDefaultUsers()
        {
            if (!WebSecurity.UserExists("DemoA"))
                WebSecurity.CreateUserAndAccount("DemoA", "DemoA");
            if (!WebSecurity.UserExists("DemoB"))
                WebSecurity.CreateUserAndAccount("DemoB", "DemoB");
            if (!WebSecurity.UserExists("Admin"))
                WebSecurity.CreateUserAndAccount("Admin", "Admin1234!");
        }
    }
}