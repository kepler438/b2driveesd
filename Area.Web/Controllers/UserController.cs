using Area.Data;
using Area.Data.Enums;
using Area.Web.Helper;
using Area.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Area.Web.Controllers
{
    public class UserController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();
        // GET: User
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logoff()
        {
            Session["UserId"] = null;
            return RedirectToAction("Login");
        }
        public ActionResult LoginInfoControl(LoginInfo info)
        {
            var getUser = (from s in db.Users where s.UserName == info.username || s.MailAddress == info.username select s).FirstOrDefault();
            if (getUser != null)
            {
                UserPassword pass = db.UserPasswords.Where(x => x.UserId == getUser.ID && x.IsActive == true).FirstOrDefault();
                string hashCode = pass.Vcode;
                var encodingPasswordString = LogHelper.EncodePassword(info.password, hashCode);
                if (object.Equals(pass.Password, encodingPasswordString))
                {
                    Session["UserId"] = getUser.ID;
                    int UserType = getUser.Permissions.First().Id;
                    if (UserType == (int)EnumUserType.Admin)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if(UserType == (int)EnumUserType.Personnel)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (UserType == (int)EnumUserType.Supervisor)
                    {
                        return RedirectToAction("Index", "SpHome");
                    }
                    else if (UserType == (int)EnumUserType.Customer)
                    {
                        return RedirectToAction("Index", "CustomerHome");
                    }

                }
            }
            TempData["UserErrorMessage"] = "Hatalı Kullanıcı Adı ya da Şifre Girdiniz.";
            return RedirectToAction("Login");
        }
        public bool SaveUser(UserInfo info)
        {
            try
            {
                var chkUser = db.Users.Where(x => x.UserName == info.UserName || x.MailAddress == info.MailAddress).FirstOrDefault();
                if (chkUser == null)
                {
                    Data.User newUser = new Data.User();
                    newUser.CreateDate = DateTime.Now;
                    newUser.IsActive = true;
                    newUser.FirstName = info.FirstName;
                    newUser.LastName = info.LastName;
                    newUser.MailAddress = info.MailAddress;
                    newUser.Phone = info.Phone;
                    newUser.UserName = info.UserName;
                    newUser.Permissions.Add(new Permission { Id = info.PermissionID });
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    Data.UserPassword newUserPassword = new UserPassword();
                    var keyNew = LogHelper.GeneratePassword(10);
                    var password = LogHelper.EncodePassword(info.Password, keyNew);
                    newUserPassword.CreatedDate = DateTime.Now;
                    newUserPassword.IsActive = true;
                    newUserPassword.UserId = newUser.ID;
                    newUserPassword.Password = password;
                    db.UserPasswords.Add(newUserPassword);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}