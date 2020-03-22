using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace OVPN_user_pass_validation
{
    class WindowsCredAuth
    {
        static internal bool Check(XmlNode xmlwincred)
        {
            if (xmlwincred.SelectSingleNode("UsersFilter").Attributes["enabled"].Value == "true"
               && !UsersFilterCheck(xmlwincred.SelectSingleNode("UsersFilter"))
                || xmlwincred.SelectSingleNode("GroupFilter").Attributes["enabled"].Value == "true"
                && !GroupFilterCheck(xmlwincred.SelectSingleNode("GroupFilter")))
            { return false; }

            if (ValidateUsernameAndPassword(Program.login,Program.pass))
                return true;

            return false;
        }

        static bool UsersFilterCheck(XmlNode xmlUserFiler)
        {
            foreach (XmlNode user in xmlUserFiler.SelectNodes("User"))
            {
                if ((user.Attributes["as-regex"].Value != "true" && user.InnerText == Program.login)
                        || (user.Attributes["as-regex"].Value == "true" && new Regex(user.InnerText).IsMatch(Program.login)))
                    return true;
            }
            return false;
        }

        static bool GroupFilterCheck(XmlNode xmlGroupFiler)
        {
            using (PrincipalContext pc = new PrincipalContext((Environment.UserDomainName == Environment.MachineName ? ContextType.Machine : ContextType.Domain), Environment.UserDomainName))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, Program.login);

                foreach (XmlNode group in xmlGroupFiler.SelectNodes("Group"))
                {
                    if (up.IsMemberOf(GroupPrincipal.FindByIdentity(pc, group.InnerText)))
                        return true;
                }
            }
            return false;
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
