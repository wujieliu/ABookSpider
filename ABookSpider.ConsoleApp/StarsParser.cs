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
    public class StarsParser : DataParser
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator((request =>
            {
                var host = request.RequestUri.Host;
                var regex = host + "/cn/actresses/page";
                return Regex.IsMatch(request.RequestUri.ToString(), regex);
            }));
            // if you want to collect every pages
            // AddFollowRequestQuerier(Selectors.XPath(".//div[@class='pager']"));
            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {//*[@id="waterfall"]/div[6]
            var starsList = context.Selectable.SelectList(Selectors.XPath(".//div[@id='waterfall']/div[@class='item']"));
            foreach (var stars in starsList)
            {
                var title = stars.Select(Selectors.XPath(".//div[@class='photo-info']/span"))?.Value;
                var url = stars.Select(Selectors.XPath(".//a[@class='avatar-box text-center']/@href"))?.Value;
                var headUrl = stars.Select(Selectors.XPath(".//div[@class='photo-frame']/img/@src"))?.Value;

                if (!string.IsNullOrWhiteSpace(url))
                {
                    if (url.Contains("/"))
                    {
                        string[] urlSplit = url.ToString().Split('/');
                        string starsLinkId = urlSplit[urlSplit.Length - 1];
                        var request = context.CreateNewRequest(new Uri(url));
                        request.Properties.Add("title", title);
                        request.Properties.Add("url", url);
                        request.Properties.Add("starsLinkId", starsLinkId);

                        context.AddFollowRequests(request);
                    }

                }

                if (!string.IsNullOrWhiteSpace(headUrl))
                {
                    var request = context.CreateNewRequest(new Uri(headUrl));
                    context.AddFollowRequests(request);
                }
            }

            return Task.CompletedTask;
        }
    }
}
