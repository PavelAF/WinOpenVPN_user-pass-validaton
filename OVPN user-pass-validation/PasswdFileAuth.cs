using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVPN_user_pass_validation
{
    class PasswdFileAuth
    {
    /*
                    if (!FileExistReadable(passwdFile)) { Environment.ExitCode = 1; return; }

    string[] cred;
            try { cred = File.ReadAllLines(args[0], Encoding.UTF8); }
                catch { Environment.ExitCode = 1; return; }
            if (cred.Length != 2) { Environment.ExitCode = 1; return; }

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
