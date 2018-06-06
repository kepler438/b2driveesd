using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area.Web.Models
{
    public class UserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string MailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PermissionID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}