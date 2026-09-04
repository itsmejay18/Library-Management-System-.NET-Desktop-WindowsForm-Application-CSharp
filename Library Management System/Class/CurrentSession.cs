using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Class
{
    internal class CurrentSession
    {
        public static string Email { get; set; }
        public static string Role { get; set; }
        public static string FullName { get; set; }
        public static string Username { get; set; }

        public static int MemberId { get; set; }
    }
}