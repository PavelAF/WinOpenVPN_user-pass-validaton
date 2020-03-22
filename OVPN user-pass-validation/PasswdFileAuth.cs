using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace OVPN_user_pass_validation
{
    class PasswdFileAuth
    {
        static internal bool Check(XmlNode xmlpasswd)
        {
            string passwdPath, loginPassCheck;
            
            passwdPath = xmlpasswd.SSelectSingleNode("PasswdFilePath").SGetAttrVal("value");
            loginPassCheck = xmlpasswd.SSelectSingleNode("PasswdFileEntryFormatOverride").SGetAttrVal("value");

            if (loginPassCheck == null)
                loginPassCheck = Properties.Settings.Default.def_PasswdFileEntryFormat;
            if (passwdPath == null || !Program.FileExistReadable(passwdPath))
                return false;

            //Regex regex = new Regex(String.Format(passwdFileFormat, Program.login, Program.pass));

            string loginPassLo = String.Format(loginPassCheck, Program.login.ToLower(), Program.pass);
            loginPassCheck = String.Format(loginPassCheck, Program.login, Program.pass);

            using (StreamReader stream = new StreamReader(passwdPath, Encoding.UTF8))
            {
                while (!stream.EndOfStream)
                {
                    string str = stream.ReadLine();
                    if (str == loginPassCheck || str == loginPassLo)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
