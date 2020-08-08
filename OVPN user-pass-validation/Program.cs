using System;
using System.IO;
using System.Text;
using System.Xml;

namespace WinOVPN_user_pass_validaton
{
    class Program
    {
        static internal string login, pass;
        
        static void Main(string[] args)
        {
            if (args.Length != 1 || !FileExistReadable(args[0]) || new FileInfo(args[0]).Length > 200)
                { Environment.ExitCode = 1; return; }

            string xmlPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string xmlPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            //string xmlPath = Environment.GetCommandLineArgs()[0];
            //string xmlPath = @"D:\Documents\Visual Studio 2017\Projects\OVPN user-pass-validation\OVPN user-pass-validation\bin\x64\Release\OVPN user-pass-validation.exe";
            xmlPath = xmlPath.Substring(0, xmlPath.LastIndexOf('.')) + ".xml";

            XmlDocument conf = new XmlDocument();
            try
            {
                if (FileExistReadable(xmlPath))
                {
                    conf.Load(xmlPath);
                }
                else { throw new Exception("xml read/exist err"); }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Source+": "+e.Message);
                Environment.ExitCode = 1;
                return;
            }

            string[] cred;
            try { cred = File.ReadAllLines(args[0], Encoding.UTF8); }
            catch { Environment.ExitCode = 1; return; }
            if (cred.Length != 2) { Environment.ExitCode = 1; return; }
            login = cred[0];
            pass = cred[1];


            XmlNode confNode = conf.SSelectSingleNode("/configuration");
            if (confNode.SSelectSingleNode("PasswdFileAuth").SGetAttrVal("enabled") == "true"
                && PasswdFileAuth.Check(confNode.SSelectSingleNode("PasswdFileAuth")))
                { Environment.ExitCode = 0; return; }
            if (confNode.SSelectSingleNode("WindowsCredAuth").SGetAttrVal("enabled") == "true"
                && WindowsCredAuth.Check(confNode.SSelectSingleNode("WindowsCredAuth")))
                { Environment.ExitCode = 0; return; }
            Environment.ExitCode = 1;
            return;
        }
        

        internal static bool FileExistReadable(string filepath)
        {
            if (!System.IO.File.Exists(filepath)) return false;
            try { File.Open(filepath, FileMode.Open, FileAccess.Read).Dispose(); return true; }
                catch { return false; }
        }

    }

}
