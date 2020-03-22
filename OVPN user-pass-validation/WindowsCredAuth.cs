using System;
using System.Collections.Generic;
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
               && UsersFilterCheck(xmlwincred.SelectSingleNode("UsersFilter"))
                || xmlwincred.SelectSingleNode("GroupFilter").Attributes["enabled"].Value == "true"
                && GroupFilterCheck(xmlwincred.SelectSingleNode("GroupFilter")))
            { return UserCheckCred(); }



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
            foreach (XmlNode user in xmlUserFiler.SelectNodes("User"))
            {
                if ((user.Attributes["as-regex"].Value != "true" && user.InnerText == Program.login)
                        || (user.Attributes["as-regex"].Value == "true" && new Regex(user.InnerText).IsMatch(Program.login)))
                    return true;
            }
            return false;
        }

        static bool UserCheckCred()
        {
            return false;
        }
    }
}
