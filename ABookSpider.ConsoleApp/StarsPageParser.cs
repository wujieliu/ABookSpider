using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
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
                var regex = host + "cn/star/.*?/page/.+";
                return Regex.IsMatch(request.RequestUri.ToString(), regex);
            }));
           
            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {
            throw new NotImplementedException();
        }
    }
}
