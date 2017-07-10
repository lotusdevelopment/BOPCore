using Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ViewModels.General;

namespace Core.InnerLogic
{
    public class BopLogic
    {
        private readonly BopDb _db = new BopDb();
        private readonly Mailing _mail = new Mailing();
        private readonly Serializers _srz = new Serializers();

        public async Task<bool> LogIn(string userName, string password)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    var pass = _srz.CalculateMD5Hash(password);
                    var query = "select count(*) from dbo.users where user_cellphone = '" + userName + "' and user_password = '" + pass + "'";
                    var sr = _db.Database.SqlQuery<int>(query).FirstOrDefault();
                    return (sr > 0);
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }

        public async Task<GeneralUser> GetUser(string userName)
        {
            return await Task<GeneralUser>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetUser '" + userName + "'";
                    var user = _db.Database.SqlQuery<GeneralUser>(query).FirstOrDefault();
                    return user;
                }
                catch (Exception ex)
                {
                    return null;
                }
            });
        }

        public async Task ForgotPassword(string userName)
        {
            await Task.Factory.StartNew(() =>
           {
               try
               {
                   var query = "exec sp_GetUser '" + userName + "'";
                   var user = _db.Database.SqlQuery<GeneralUser>(query).FirstOrDefault();
                   if (user == null) return;
                   var password = System.Web.Security.Membership.GeneratePassword(10, 0);
                   var ePassword = _srz.EncryptMd5(password);
                   query = "exec sp_FotgotPassword '" + ePassword + "', '" + user.UserId + "'";
                   var sr = _db.Database.SqlQuery<object>(query);
                   _mail.SendForgotPasswordmail(password, user);
               }
               catch (Exception ex)
               {
                   return;
               }
           });
        }

        public async Task<bool> UpdateUser(GeneralUser user)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }
    }
}