using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace OVPN_user_pass_validation
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || !FileExistReadable(args[0]) || new FileInfo(args[0]).Length > 160)
                { Environment.ExitCode = 1; return; }

            string passwdFile = @"ovpn_passwd.txt";
            string matchMask = @"^{0} : {1}$";

            string xmlPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            xmlPath = xmlPath.Substring(0, xmlPath.LastIndexOf('.')) + ".xml";

            if (FileExistReadable(xmlPath))
            {
                XmlDocument conf = new XmlDocument();
                conf.Load(xmlPath);
                XmlNode confNode = conf.SelectSingleNode("/configuration");
                passwdFile = confNode["passwd_file"].GetAttribute("value");
                matchMask = confNode["passwd_format"].GetAttribute("value");
            }


            Environment.ExitCode = 1;
            return;
        }


        static bool FileExistReadable(string filepath)
        {
            if (!System.IO.File.Exists(filepath)) return false;
            try { File.Open(filepath, FileMode.Open, FileAccess.Read).Dispose(); return true; }
                catch { return false; }
        }

        public static bool ValidateUsernameAndPassword(string userName, SecureString securePassword)
        {
            bool result = false;

            ContextType contextType = ContextType.Machine;

            if (InDomain())
            {
                contextType = ContextType.Domain;
            }



            try
            {
                using (PrincipalContext principalContext = new PrincipalContext(contextType))
                {
                    result = principalContext.ValidateCredentials(
                        userName,
                        new NetworkCredential(string.Empty, securePassword).Password
                    );
                }
            }
            catch (PrincipalOperationException)
            {
                // Account disabled? Considering as Login failed
                result = false;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        ///     Validate: computer connected to domain?   
        /// </summary>
        /// <returns>
        ///     True -- computer is in domain
        ///     <para>False -- computer not in domain</para>
        /// </returns>
        public static bool InDomain()
        {
            bool result = true;

            try
            {
                Domain domain = Domain.GetComputerDomain();
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                result = false;
            }

            return result;
        }

    }

}
