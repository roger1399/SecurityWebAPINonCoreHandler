using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebAPINonCoreHandler.Handlers
{
    public class AuthHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Not Allowed!!");
            }
            else
            {
                string authToken = request.Headers.Authorization.Parameter;
                string decodedauthtoken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string[] usernamepass = decodedauthtoken.Split(':');
                string username = usernamepass[0];
                string pass = usernamepass[1];

                if (username == "roger" && pass == "apiit")
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.Unauthorized, "Not Allowed!!");
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            //Return the response back up the chain
            return response;


        }
    }
}