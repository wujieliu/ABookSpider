using ABookSpider.Entity;
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
    internal class GenreParser : DataParser
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator((request =>
            {
                var host = request.RequestUri.Host;
                var regex = host + "/cn/genre";
                return request.RequestUri.ToString() == "https://" + regex;
            }));
            // if you want to collect every pages
            // AddFollowRequestQuerier(Selectors.XPath(".//div[@class='pager']"));
            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {

            var titleList = context.Selectable.SelectList(Selectors.XPath(".//div[@class='container-fluid pt-10']/h4")).ToList();
            var nameListList = context.Selectable.SelectList(Selectors.XPath(".//div[@class='container-fluid pt-10']/div[@class='row genre-box']")).ToList();

            List<GenreEntity> genreList = new List<GenreEntity>();
            for (int i = 0; i < titleList.Count(); i++)
            {
                var title = titleList[i]?.Value;
                var linkIdList = nameListList[i].SelectList(Selectors.XPath(".//a/@href")).ToList();
                var nameList = nameListList[i].SelectList(Selectors.XPath(".//a")).ToList();

                for (int j = 0; j < linkIdList.Count(); j++)
                {
                    var name = nameList[j]?.Value;
                    var linkIdUrl = linkIdList[j]?.Value;
                    if (!string.IsNullOrWhiteSpace(linkIdUrl))
                    {
                        if (linkIdUrl.Contains("/"))
                        {
                            string[] urlSplit = linkIdUrl.ToString().Split('/');
                            string linkId = urlSplit[urlSplit.Length - 1];

                            GenreEntity genre = new GenreEntity();
                            genre.Name = name;
                            genre.Title = title;
                            genre.LinkId = linkId;
                            genre.SortName = j + 1;
                            genre.SortTitle = i + 1;
                            genreList.Add(genre);
                        }
                    }
                }

            }
            var typeName = typeof(GenreEntity);
            context.AddData(typeName, genreList);

            return Task.CompletedTask;
        }
    }
}
