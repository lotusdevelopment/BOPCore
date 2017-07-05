using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Core
{
    public class RestAuthorizationManager : ServiceAuthorizationManager
    {
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
                    if ((user.Name == "admin" && user.Password == "admin"))
                        if (token.Equals("KIM3G5Tlp3gnW1yxKy3OCB5Kznm5iqlA"))
                            return true;
                    return false;
                }
            }
            catch (Exception)
            {
            }
            WebOperationContext.Current.OutgoingResponse.Headers.Add("WWW-Authenticate: Basic realm=\"BOPAuthService\"");
            throw new WebFaultException(HttpStatusCode.Unauthorized);
        }
    }
}