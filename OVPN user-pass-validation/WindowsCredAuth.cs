using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace WinOVPN_user_pass_validaton
{
    class WindowsCredAuth
    {
        static internal bool Check(XmlNode xmlwincred)
        {
            bool usersFilter = xmlwincred.SSelectSingleNode("UsersFilter").SGetAttrVal("enabled") == "true" ? true : false;
            bool groupFilter = xmlwincred.SSelectSingleNode("GroupFilter").SGetAttrVal("enabled") == "true" ? true : false;

            if (usersFilter | groupFilter)
            {
                if (usersFilter && !UsersFilterCheck(xmlwincred.SSelectSingleNode("UsersFilter")))
                    usersFilter = false;
                if (groupFilter && !GroupFilterCheck(xmlwincred.SSelectSingleNode("GroupFilter")))
                    groupFilter = false;
                if (!usersFilter & !groupFilter) return false;
            }

            if (ValidateUsernameAndPassword(Program.login,Program.pass))
                return true;

            return false;
        }

        static bool UsersFilterCheck(XmlNode xmlUserFiler)
        {
            foreach (XmlNode user in xmlUserFiler.SSelectNodes("User"))
            {
                if (user.SGetAttrVal("as-regex") == "true" && new Regex(user.InnerText).IsMatch(Program.login)
                    || user.InnerText == Program.login)
                    return true;
            }
            return false;
        }

        static bool GroupFilterCheck(XmlNode xmlGroupFiler)
        {
            var root = new DirectoryEntry($"WinNT://{Environment.MachineName},computer");
            try { root = root.Children.Find(Program.login, "user"); }
            catch { return false; }

            var uGroups = root.Invoke("groups");

            foreach (XmlNode group in xmlGroupFiler.SSelectNodes("Group"))
            {
                foreach (var uGroup in (IEnumerable)uGroups)
                {
                    if (string.Equals(new DirectoryEntry(uGroup).Name, group.InnerText, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }
            return false;

            #region ___old_slow_code___
            //using (PrincipalContext pc = new PrincipalContext((Environment.UserDomainName == Environment.MachineName ? ContextType.Machine : ContextType.Domain), Environment.UserDomainName))
            //{
            //    UserPrincipal up = UserPrincipal.FindByIdentity(pc, Program.login);

            //    foreach (XmlNode group in xmlGroupFiler.SSelectNodes("Group"))
            //    {
            //        if (up.IsMemberOf(GroupPrincipal.FindByIdentity(pc, group.InnerText)))
            //            return true;
            //    }
            //}
            #endregion
        }

        public static bool ValidateUsernameAndPassword(string userName, string password)
        {
            bool result = false;

            ContextType contextType = Environment.UserDomainName == Environment.MachineName ? ContextType.Machine : ContextType.Domain;

            try
            {
                using (PrincipalContext principalContext = new PrincipalContext(contextType))
                {
                    result = principalContext.ValidateCredentials( userName,password );
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

    }
}
