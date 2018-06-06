using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area.Data;
using Area.Web.Models;
using System.Reflection;
using Area.Web.Helper;

namespace AddUserApp
{
    class Program
    {
        static void Main(string[] args)
        {
            UserInfo ui = new UserInfo();
            Type type = ui.GetType();
            PropertyInfo[] properties = type.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo pi = properties[i];
                if (pi.PropertyType == typeof(DateTime))
                {
                    DateTime value = DateTime.Now;
                    pi.SetValue(ui, value);
                }
                else if (pi.PropertyType == typeof(bool))
                {
                    bool value = true;
                    pi.SetValue(ui, value);
                }
                else
                {
                    Console.WriteLine(pi.Name + " Giriniz");
                    if (pi.PropertyType == typeof(string))
                    {
                        string value = Console.ReadLine();
                        pi.SetValue(ui, value);
                    }
                    else if (pi.PropertyType == typeof(int))
                    {
                        int value = Convert.ToInt32(Console.ReadLine());
                        pi.SetValue(ui, value);
                    }
                }

            }
            //Bu kodlar controller/user/SaveUser içerisinde var
            using (B2DriveForPostEntities db = new B2DriveForPostEntities())
            {
                try
                {
                    var chkUser = db.Users.Where(x => x.UserName == ui.UserName || x.MailAddress == ui.MailAddress).FirstOrDefault();
                    if (chkUser == null)
                    {
                        Area.Data.User newUser = new Area.Data.User();
                        newUser.CreateDate = DateTime.Now;
                        newUser.IsActive = true;
                        newUser.FirstName = ui.FirstName;
                        newUser.LastName = ui.LastName;
                        newUser.MailAddress = ui.MailAddress;
                        newUser.Phone = ui.Phone;
                        newUser.UserName = ui.UserName;
                        Permission perm = db.Permissions.Where(x => x.Id == ui.PermissionID).FirstOrDefault();
                        if (perm != null)
                        {
                            newUser.Permissions.Add(perm);
                        }
                        db.Users.Add(newUser);
                        db.SaveChanges();

                        Area.Data.UserPassword newUserPassword = new UserPassword();
                        var keyNew = LogHelper.GeneratePassword(10);
                        var password = LogHelper.EncodePassword(ui.Password, keyNew);
                        newUserPassword.CreatedDate = DateTime.Now;
                        newUserPassword.IsActive = true;
                        newUserPassword.UserId = newUser.ID;
                        newUserPassword.Password = password;
                        newUserPassword.Vcode = keyNew;
                        db.UserPasswords.Add(newUserPassword);
                        db.SaveChanges();
                        Console.WriteLine("Eklendi!");
                    }
                    else
                    {
                        Console.WriteLine("Var olan kullanıcı !!");
                    }
                }
                catch
                {
                    Console.WriteLine("Hata oluştu");
                }
            }
            Console.ReadLine();
        }
    }
}
