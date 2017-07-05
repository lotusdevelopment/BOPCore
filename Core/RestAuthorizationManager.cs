using Core.DataBase;
using System;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ViewModels.General;

namespace Core
{
    public class RestAuthorizationManager : ServiceAuthorizationManager
    {
        private readonly BopDb _db = new BopDb();

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                var token = WebOperationContext.Current.IncomingRequest.Headers["TknId"];
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
                                @"and enti_PublicKey = '" + token + "'";
                    var entity = _db.Database.SqlQuery<GenericBusAuth>(query).FirstOrDefault();
                    return (entity != null) ? true : false;
                }
            }
            catch (Exception ex)
            {
            }
            WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"BOPAuthService\"");
            throw new WebFaultException(HttpStatusCode.Unauthorized);
        }
    }
}