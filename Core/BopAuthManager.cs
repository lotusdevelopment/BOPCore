using Core.DataBase;
using Core.InnerLogic;
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
        private readonly GeneralDb _db = new GeneralDb();
        private readonly Serializers _srz = new Serializers();

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
                    var recycledHash = _srz.CalculateMD5Hash(string.Concat(timestamp, entity.PublicKey, entity.PrivateKey));
                    return (recycledHash.Equals(hash));
                }
            }
            catch (Exception Ex)
            {
            }
            WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"BOPAuthService\"");
            throw new WebFaultException(HttpStatusCode.Unauthorized);
        }
    }
}