using System;
using System.Linq;
using Core.DataBase;
using ViewModels;
using Core.Resources;

namespace Core
{
    public class Authorizations
    {
        private readonly GeneralDb _db = new GeneralDb();
        private readonly Encryption _encryption = new Encryption();

        public bool Authorized(AuthorizeModel credentials)
        {
            if (credentials == null) return false;
            return credentials.IsEncrypted ? EncryptedVerification(credentials) : Verification(credentials);
        }

        private bool EncryptedVerification(AuthorizeModel credentials)
        {
            var arr = new AuthorizeModel
            {
                User = _encryption.Decrypt(credentials.User),
                Password = _encryption.Decrypt(credentials.Password)
            };
            return Verification(arr);
        }

        private bool Verification(AuthorizeModel credentials)
        {
            try
            {
                var query = "exec Sp_GetEntity '" + credentials.User + "', '" + credentials.Password + "'";
                var rtn = Convert.ToInt32(_db.Database.SqlQuery<string>(query).FirstOrDefault());
                return rtn == 1;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}