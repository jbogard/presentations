using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Before.Features.Offers.AssignOffer;
using Before.Features.Offers.ExpireOffer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Before
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    var foo = typeof(AssignOfferService);
                    var bar = typeof(ExpireOfferService);
                    var f = nameof(AssignOfferService.Assign);
                    var g = nameof(ExpireOfferService.Expire);
                });
    }
}
