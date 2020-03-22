using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OVPN_user_pass_validation
{
    class PasswdFileAuth
    {
        static internal bool Check(XmlNode xmlpasswd)
        {
            string passwdPath, passwdFileFormat;

            passwdPath = xmlpasswd["PasswdFilePath"].GetAttribute("value");
            passwdFileFormat = xmlpasswd["PasswdFileEntryFormatOverride"].GetAttribute("value");

            if (passwdFileFormat == null)
                passwdFileFormat = Properties.Settings.Default.def_PasswdFileEntryFormat;
            if (passwdPath == null)
                return false;



            //if (!Program.FileExistReadable(xmlpasswd.)) { return false; }
            return true;
        }
    /*

            Regex regex = new Regex(String.Format(matchMask, cred[0], cred[1]));

            using (StreamReader stream = new StreamReader(passwdFile, Encoding.UTF8))
            {
                while (!stream.EndOfStream)
                {
                    if (regex.IsMatch(stream.ReadLine()))
                    {
                        stream.Close();
                        stream.Dispose();
                        Environment.ExitCode = 0;
                        return;
                    }
                }
            }
    */
    }
}
