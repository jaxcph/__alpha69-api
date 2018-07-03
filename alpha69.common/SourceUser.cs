using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using System.Security.Cryptography;
using System.Text;

namespace alpha69.common
{
    public class SourceUser
    {
        public string Id { get; set; }
        public string Domain { get; set; }
        public string Login { get; set; }
        public string Roles { get; set; }
        public string IPAddress { get; set; }
        public int UserScore { get; set; }


        public SourceUser()
        {

        }

        public bool HasRole(string roleName)
        {

            if (string.IsNullOrEmpty(Roles) || string.IsNullOrEmpty(roleName))
                return false;

            roleName = roleName.ToLower().Trim();
            Roles = Roles.ToLower().Trim();

            string[] ra = Roles.Split(",",StringSplitOptions.RemoveEmptyEntries);
            foreach(var r in ra)
            {
                var a = r.Trim();
                if (a==roleName)
                    return true;
            }

            return false;

        }

    }
}