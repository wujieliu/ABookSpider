using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ABookSpider.ConsoleApp
{
    public class StarsPageParser : DataParser
    {

        //.//ui[@class='pagination pagination-lg mtb-0']/a[@class='nextpage']/@href
        public override Task InitializeAsync()
        {
            AddRequiredValidator((request =>
            {
                var host = request.RequestUri.Host;
                //var regex = host + "cn/star/.*?/page/.+";
                var regex = host + "/cn/star/";
                return Regex.IsMatch(request.RequestUri.ToString(), regex);
            }));

            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {
            var starsList = context.Selectable.SelectList(Selectors.XPath(".//div[@id='waterfall']/div[@class='item']"));
            foreach (var stars in starsList)
            {
                var url = stars.Select(Selectors.XPath(".//a[@class='movie-box']/@href"))?.Value;
                if (url != null)
                {
                    string[] urlSplit = url.ToString().Split('/');
                    string linkId = urlSplit[urlSplit.Length - 1];
                    var request = context.CreateNewRequest(new Uri(url));
                    request.Properties.Add("linkId", linkId);
                    context.AddFollowRequests(request);
                }
            }
            var nextPage = context.Selectable.Select(Selectors.XPath(".//a[@name='nextpage']/@href"))?.Value;
            if (nextPage != null)
            {
                var request = context.CreateNewRequest(new Uri(nextPage));

                context.AddFollowRequests(request);
            }

            return Task.CompletedTask;
        }
    }
}
