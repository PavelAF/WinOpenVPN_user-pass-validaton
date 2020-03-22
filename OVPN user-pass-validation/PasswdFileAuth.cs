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
            string passwdPath, passwdFileFormat;
            
            passwdPath = xmlpasswd.SSelectSingleNode("PasswdFilePath").SGetAttrVal("value");
            passwdFileFormat = xmlpasswd.SSelectSingleNode("PasswdFileEntryFormatOverride").SGetAttrVal("value");

            if (passwdFileFormat == null)
                passwdFileFormat = Properties.Settings.Default.def_PasswdFileEntryFormat;
            if (passwdPath == null || !Program.FileExistReadable(passwdPath))
                return false;

            //Regex regex = new Regex(String.Format(passwdFileFormat, Program.login, Program.pass));

            using (StreamReader stream = new StreamReader(passwdPath, Encoding.UTF8))
            {
                while (!stream.EndOfStream)
                {
                    if (stream.ReadLine() == passwdFileFormat)
                    {
                        stream.Close();
                        stream.Dispose();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
