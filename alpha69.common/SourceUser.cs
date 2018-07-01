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
        public int userScore { get; set; }

    }
}