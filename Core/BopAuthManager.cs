using Core.DataBase;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ViewModels.General;

namespace Core
{
    public class BopAuthManager : ServiceAuthorizationManager
    {
        private readonly BopDb _db = new BopDb();

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                var timestamp = WebOperationContext.Current.IncomingRequest.Headers["timestamp"];
                var publicKey = WebOperationContext.Current.IncomingRequest.Headers["publicKey"];
                var hash = WebOperationContext.Current.IncomingRequest.Headers["hash"];
                if ((authHeader != null) && (authHeader != string.Empty))
                {
                    var svcCredentials = ASCIIEncoding.ASCII
                        .GetString(Convert.FromBase64String(authHeader.Substring(6)))
                        .Split(':');
                    var user = new
                    {
                        Name = svcCredentials[0],
                        Password = svcCredentials[1]
                    };
                    var query = @"select enti_name as Entity, enti_User as Systemuser, enti_PublicKey as PublicKey, 
                                enti_PrivateKey as PrivateKey
                                from dbo.entity
                                where enti_User = '" + user.Name + "'" +
                                @"and enti_Password = '" + user.Password + "'" +
                                @"and enti_PublicKey = '" + publicKey + "'";
                    var entity = _db.Database.SqlQuery<GenericBusAuth>(query).FirstOrDefault();
                    if (entity == null) return false;
                    var recycledHash = CalculateMD5Hash(string.Concat(timestamp, entity.PublicKey, entity.PrivateKey));
                    return (recycledHash.Equals(hash)) ? true : false;
                }
            }
            catch (Exception)
            {
            }
            WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"BOPAuthService\"");
            throw new WebFaultException(HttpStatusCode.Unauthorized);
        }

        private string CalculateMD5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}