using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.DataBase;
using Core.Resources;
using LotusViewModels.General;
using ViewModels.ILotusAssistance;
using LotusViewModels.Request;
using LotusViewModels.Response;
using Newtonsoft.Json;
using ViewModels;
using System.Web.Configuration;

namespace Core
{
    public class Account : IAccount
    {
        private readonly GeneralDb _db = new GeneralDb();        
        private readonly Mailing _mail = new Mailing();
        
        public async Task<LoginResponse> LoginSystem(LoginRequest login)
        {
            return await Task<LoginResponse>.Factory.StartNew(() =>
            {
                var password = GetMd5Password(login.Password);
                var query = "exec sp_GetUser '" + login.User + "', '" + password + "', '1'";
                var user = _db.Database.SqlQuery<User>(query).FirstOrDefault();
                if (user == null)
                    return new LoginResponse
                    {
                        Valid = false
                    };
                var rtn = new LoginResponse
                {
                    Valid = true,
                    User = user,
                    Company = GetCompanyInfo(user.UserId).Result,
                    Apps = GetApps(user.UserId).Result,
                    Roles = GetRoles(user.UserId).Result
                };
                return rtn;
            });
        }

        private async Task<Empresas> GetCompanyInfo(string uId)
        {
            return await Task<Empresas>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetCompany '" + uId + "', 'null', '1'";
                    return _db.Database.SqlQuery<Empresas>(query).FirstOrDefault();
                }
                catch (Exception e)
                {
                    return new Empresas();
                }
            });
        }

        private async Task<List<Roles>> GetRoles(string uId)
        {
            return await Task<List<Roles>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetRoles '" + uId + "', '1'";
                    return _db.Database.SqlQuery<Roles>(query).ToList();
                }
                catch (Exception)
                {
                    return new List<Roles>();
                }
            });
        }

        private async Task<List<Apps>> GetApps(string uId)
        {
            return await Task<List<Apps>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetApps '" + uId + "', '1'";
                    var rtn = _db.Database.SqlQuery<Apps>(query).ToList();
                    foreach (var app in rtn)
                        app.Menu = GetMenuxApp(uId, app.AppId).Result;
                    return rtn;
                }
                catch (Exception)
                {
                    return new List<Apps>();
                }
            });
        }

        private async Task<List<Menu>> GetMenuxApp(string uId, int appId)
        {
            return await Task<List<Menu>>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_GetMenuxApp '" + uId + "', '" + appId + "'";
                    return _db.Database.SqlQuery<Menu>(query).ToList();
                }
                catch (Exception)
                {
                    return new List<Menu>();
                }
            });
        }

        public async Task<string> CreateUser(User createUser)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    var password = GetMd5Password(createUser.Password);
                    var dt = createUser.Birthdate != null ? Convert.ToDateTime(createUser.Birthdate) : DateTime.Now;
                    var query = "exec sp_createUser '" + createUser.Flag + "', '" + createUser.UserIcard +
                    "', '" + createUser.Name + "', '" + createUser.UserLastname + "', '" + createUser.UserIcardtitular +
                    "', '" + createUser.UserUc + "', '" + createUser.UserEmail + "', '" + password + "', '" + createUser.LanguageId +
                    "', '" + createUser.SellerCode + "', '" + dt + "', '" + createUser.Gender +
                    "', '" + createUser.CellPhone + "', '" + createUser.Address + "', '" + createUser.CountryId +
                    "', '" + createUser.GroupId + "', '" + createUser.RoleId + "'";
                    var rtn = _db.Database.SqlQuery<string>(query).FirstOrDefault();
                    if (!rtn.ToUpper().Equals("USER SUCCESSFULLY REGISTERED"))
                        return rtn;
                    query = "exec sp_GetUser '" + createUser.UserEmail + "', '" + null + "', '2'";
                    var usr = _db.Database.SqlQuery<User>(query).FirstOrDefault();
                    createUser.UserId = usr.UserId;
                    Task.Factory.StartNew(() => _mail.SendCreationEmail(createUser));
                    return rtn;
                }
                catch (Exception e)
                {
                    return "Fallo: " + e.Message;
                }
            });
        }

        public async Task<bool> ChangePassword(User user)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    var password = GetMd5Password(user.Password);
                    var query = "exec sp_ChangePassword '" + user.UserEmail + "', '" + password + "'";
                    var rtn = _db.Database.SqlQuery<object>(query);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            });
        }

        public async Task<string> ResetAccount(User user)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    var password = CreateRandomPassword();
                    var nPassword = GetMd5Password(user.Password);
                    var query = "exec sp_ResetPassword '" + user.UserEmail + "', '" + nPassword + "'";
                    var rtn = _db.Database.SqlQuery<object>(query);
                    query = "exec sp_GetUser '" + user.UserEmail + "', '" + null + "', '2'";
                    var nUser = _db.Database.SqlQuery<User>(query).FirstOrDefault();
                    Task.Factory.StartNew(() => _mail.SendUpdateEmail(nUser));
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            });
        }

        private string GetMd5Password(string password)
        {
            var encodedPassword2 = new UTF8Encoding().GetBytes(password);
            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword2);
            var encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            return encoded;
        }

        private bool ComparePasswords(string password, string systemPassword)
        {

            return (string.CompareOrdinal(GetMd5Password(password), systemPassword) == 0);
        }

        public string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789*/$&()=";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<string> UpdateLastActivity(User user)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    /*var userR = _db.users.FirstOrDefault(x => x.user_email.ToUpper().Equals(user.UserEmail.ToUpper()));
                    if (userR == null) return "";
                    userR.user_last_activity = DateTime.Now;
                    _db.SaveChanges();*/
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            });
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                try
                {
                    var query = "exec sp_UpdateUserInformation '" + user.UserId + "', '" + user.ProfilePicture + "', '"
                    + user.Address + "', '" + user.CellPhone + "', '" + user.LanguageId + "'";
                    var rtn = _db.Database.SqlQuery<object>(query);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            });
        }

        public void UpdateProfilePicture(string url, string id)
        {
            try
            {
                var route = WebConfigurationManager.AppSettings["ProfilePicturesRoute"].ToString();
                var query = "exec sp_UpdateProfilePicture '" + id + "', '" + url + "'";
                var rtn = _db.Database.SqlQuery<object>(query);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                //
            }
        }

        private string CreateRandomPassword(int passwordLength = 10)
        {
            var allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            var chars = new char[passwordLength];
            var rd = new Random();
            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
    }
}
