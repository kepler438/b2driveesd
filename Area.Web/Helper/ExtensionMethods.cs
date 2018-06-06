using Area.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area.Web.Helper
{
    public static class ExtensionMethods
    {
        public static string GetPersonelForVisit(int? personID)
        {
            if (personID <= 0)
                return "";
            using (B2DriveForPostEntities db = new B2DriveForPostEntities())
            { 
                var user = db.Users.Where(p => p.ID == personID).FirstOrDefault();
                return user.FirstName + " " + user.LastName;
            } 
        }

      
    }
}