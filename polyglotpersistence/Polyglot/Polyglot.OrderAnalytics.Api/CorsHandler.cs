using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Polyglot.OrderAnalytics.Api
{
    public class CorsHandler : DelegatingHandler
    {
        const string Origin = "Origin";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            bool isCorsRequest = request.Headers.Contains(Origin);
            if (isCorsRequest)
            {
                return base.SendAsync(request, cancellationToken).ContinueWith(t =>
                {
                    HttpResponseMessage resp = t.Result;
                    resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                    return resp;
                });
            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            } 
        }
    }
}