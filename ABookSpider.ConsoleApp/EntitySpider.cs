using ABookSpider.Entity;
using DotnetSpider;
using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Storage;
using DotnetSpider.Downloader;
using DotnetSpider.Http;
using DotnetSpider.Infrastructure;
using DotnetSpider.MySql;
using DotnetSpider.Scheduler;
using DotnetSpider.Scheduler.Component;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABookSpider.ConsoleApp
{
    //https://avmoo.casa/cn/star/44ea747cc9811733
    public class EntitySpider : Spider
    {
        public static async Task RunAsync()
        {
            var builder = Builder.CreateDefaultBuilder<EntitySpider>(options =>
            {
                // 每秒 1 个请求
                options.Speed = 1;
            });
            builder.UseDownloader<HttpClientDownloader>();
            builder.UseSerilog();

            builder.IgnoreServerCertificateError();
            builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();
            await builder.Build().RunAsync();


            //var builder = Builder.CreateDefaultBuilder<GithubSpider>();
            //builder.UseSerilog();
            //builder.UseDownloader<HttpClientDownloader>();
            //builder.UseQueueDistinctBfsScheduler<HashSetDuplicateRemover>();
            //await builder.Build().RunAsync();
        }
        public EntitySpider(IOptions<SpiderOptions> options, DependenceServices services, ILogger<Spider> logger) : base(options, services, logger)
        {
        }
        

        protected override async Task InitializeAsync(CancellationToken stoppingToken = default)
        {
            AddDataFlow(GetDefaultStorage());
            AddDataFlow(new StarsParser());
            AddDataFlow(new ImageStorage());
            AddDataFlow(new DataParser<StarsEntity>());
            //AddDataFlow(new GenreParser());

            AddDataFlow(GetDefaultStorage());
            for (int i = 0; i < 206; i++)
            {
                await AddRequestsAsync(
                              new Request(
                                  "https://avmoo.casa/cn/actresses/page/" + (i + 1),new Dictionary<string, object>{ { "page", i + 1 } }));
            }
          
            //await AddRequestsAsync(
            //    new Request(
            //        " https://avmoo.casa/cn/genre"));

        }

        protected override SpiderId GenerateSpiderId()
        {
            return new("ABookSpider", "ABookSpider");
        }
    }
}
