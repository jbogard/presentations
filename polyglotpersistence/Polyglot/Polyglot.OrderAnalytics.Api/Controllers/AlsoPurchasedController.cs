using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Polyglot.OrderAnalytics.Api.Controllers
{
    public class AlsoPurchasedController : ApiController
    {
        public int[] Get(int id)
        {
            return new[] {988, 907, 858};
        }
    }
}
