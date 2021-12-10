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
    public class MovieParser : DataParser
    {
        public override Task InitializeAsync()
        {
            AddRequiredValidator((request =>
            {
                var host = request.RequestUri.Host;
                var regex = host + "/cn/movie";
                return Regex.IsMatch(request.RequestUri.ToString(), regex);
            }));
            // if you want to collect every pages
            // AddFollowRequestQuerier(Selectors.XPath(".//div[@class='pager']"));
            return Task.CompletedTask;
        }

        protected override Task ParseAsync(DataFlowContext context)
        {
            var container = context.Selectable.Select(Selectors.XPath(".//body/div[@class='container']"));
            object linkId;
            context.Request.Properties.TryGetValue("linkId", out linkId);
            string stringLinkId = linkId.ToString();
            var title = container.Select(Selectors.XPath(".//h3"))?.Value;
            var bigImageUrl = container.Select(Selectors.XPath(".//div[@class='row movie']/div[@class='col-md-9 screencap']/a[@class='bigImage']/@href"))?.Value;
            var info = container.Select(Selectors.XPath(".//div[@class='row movie']/div[@class='col-md-3 info']"));
            var infoPList = info.SelectList(Selectors.XPath(".//p"));

            //string A_ID;

            //string release_date;

            //string length;

            //string director;

            //string director_link_id;

            string studio = "";

            string studioUrl = "";

            //string studio_link_id;

            string label = "";

            string labelUrl = "";

            string series = "";

            string seriesUrl = "";

            string genres = "";

            string stars = "";

            MovieEntity movieEntity = new MovieEntity();
            var A_ID = info.Select(Selectors.XPath(".//p[span[@class='header' and text()='识别码:']]"))?.Value.Replace("识别码:", "").Trim();
            var release_date = info.Select(Selectors.XPath(".//p[span[@class='header' and text()='发行时间:']]/text()"))?.Value.Trim();
            var length = info.Select(Selectors.XPath(".//p[span[@class='header' and text()='长度:']]/text()"))?.Value.Trim();
            var director = info.Select(Selectors.XPath(".//p[span[@class='header' and text()='导演:']]/a/text()"))?.Value.Trim();
            var directorUrl = info.Select(Selectors.XPath(".//p[span[@class='header' and text()='导演:']]/a/@href"))?.Value;
            for (int i = 0; i < infoPList.Count(); i++)
            {
                var studioTitle = infoPList.ElementAt(i).Select(Selectors.XPath(".//p[@class='header' and text()='制作商: ']"));
                if (i + 1 < infoPList.Count()&& studioTitle!=null)
                {
                    studioUrl = infoPList.ElementAt(i+1).Select(Selectors.XPath(".//a/@href"))?.Value;
                    studio = infoPList.ElementAt(i + 1).Select(Selectors.XPath(".//a/text()"))?.Value;
                }
                var labelTitle = infoPList.ElementAt(i).Select(Selectors.XPath(".//p[@class='header' and text()='发行商: ']"));
                if (i + 1 < infoPList.Count() && labelTitle!=null)
                {
                    labelUrl = infoPList.ElementAt(i+1).Select(Selectors.XPath(".//a/@href"))?.Value;
                    label = infoPList.ElementAt(i + 1).Select(Selectors.XPath(".//a/text()"))?.Value;
                }
                var seriesTitle = infoPList.ElementAt(i).Select(Selectors.XPath(".//p[@class='header' and text()='系列:']"));
                if (i + 1 < infoPList.Count()&& seriesTitle!=null)
                {
                    seriesUrl = infoPList.ElementAt(i + 1).Select(Selectors.XPath(".//a/@href"))?.Value;
                    series = infoPList.ElementAt(i + 1).Select(Selectors.XPath(".//a/text()"))?.Value;
                }
            }

            var genresSelect = info.SelectList(Selectors.XPath(".//p/span[@class='genre']"));
            if (genresSelect != null)
            {


                foreach (var item in genresSelect)
                {
                    genres += item.Select(Selectors.XPath(".//a/text()"))?.Value + "|";
                }
            }

            var starsSelect = container.SelectList(Selectors.XPath(".//div[@id='avatar-waterfall']/a[@class='avatar-box']"));
            if (starsSelect != null)
            {
                List<MoveStarsRelation> MoveStarsRelationList = new List<MoveStarsRelation>();
                foreach (var item in starsSelect)
                {
                    var starUrl = item.Select(Selectors.XPath(".//a/@href"))?.Value;
                    var star = item.Select(Selectors.XPath(".//span/text()"))?.Value;
                    string[] starUrlSplit = starUrl.ToString().Split('/');
                    string starsLinkId = starUrlSplit[starUrlSplit.Length - 1];
                    MoveStarsRelation moveStarsRelation = new MoveStarsRelation();
                    moveStarsRelation.StarLinkId = starsLinkId;
                    moveStarsRelation.MovieLinkId = stringLinkId;
                    MoveStarsRelationList.Add(moveStarsRelation);
                      stars += star + "|";
                }
                var MoveStarsRelationTypeName = typeof(MoveStarsRelation);
                context.AddData(MoveStarsRelationTypeName, MoveStarsRelationList);
            }

            var imageSelect = container.SelectList(Selectors.XPath(".//div[@id='sample-waterfall']/a[@class='sample-box']"));
            if (imageSelect != null)
            {
                List< MovieImage > MovieImageList=new List< MovieImage >();
                for (int i = 0; i < imageSelect.Count(); i++)
                {
                    var imageUrl = imageSelect.ElementAt(i).Select(Selectors.XPath(".//a/@href"))?.Value;

                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        //var request = context.CreateNewRequest(new Uri(imageUrl));
                        //context.AddFollowRequests(request);
                        MovieImage movieImage = new MovieImage();
                        movieImage.MovieLinkId = stringLinkId;
                        movieImage.Image = imageUrl;
                        movieImage.SortIndex = i + 1;
                        MovieImageList.Add(movieImage);
                    }

                }
                movieEntity.image_len = imageSelect.Count();
                var MovieImageName = typeof(MovieImage);
                context.AddData(MovieImageName, MovieImageList);
            }
      

            movieEntity.BigImage = bigImageUrl;
            movieEntity.stars = stars;
            movieEntity.LinkId = stringLinkId;
            movieEntity.Name = title;
            movieEntity.A_ID = A_ID;
            movieEntity.release_date = release_date;
            movieEntity.length = length;
            movieEntity.director = director;
            if (directorUrl != null)
            {

                string[] directorUrlSplit = directorUrl.ToString().Split('/');
                string directorLinkId = directorUrlSplit[directorUrlSplit.Length - 1];
                movieEntity.director_link_id = directorLinkId;
            }
            movieEntity.studio = studio;
            if (studioUrl != null)
            {
                string[] studioUrlSplit = studioUrl.ToString().Split('/');
                string studioLinkId = studioUrlSplit[studioUrlSplit.Length - 1];
                movieEntity.studio_link_id = studioLinkId;
            }
            //string studio_link_id;

            movieEntity.label = label;
            if (labelUrl != null)
            {
                string[] labelUrlSplit = labelUrl.ToString().Split('/');
                string labelLinkId = labelUrlSplit[labelUrlSplit.Length - 1];
                movieEntity.label_link_id = labelLinkId;
            }

            if (seriesUrl != null)
            {
                movieEntity.series = series;
                string[] seriesUrlSplit = seriesUrl.ToString().Split('/');
                string seriesLinkId = seriesUrlSplit[seriesUrlSplit.Length - 1];
                movieEntity.series_link_id = seriesLinkId;
            }
            movieEntity.genres = genres;


            var typeName = typeof(MovieEntity);
            context.AddData(typeName, movieEntity);


            //for (int i = 0; i < starsList.Count(); i++)
            //{
            //    var title = starsList.ElementAt(i).Select(Selectors.XPath(".//div[@class='photo-info']/span"))?.Value;
            //    var url = starsList.ElementAt(i).Select(Selectors.XPath(".//a[@class='avatar-box text-center']/@href"))?.Value;
            //    var headUrl = starsList.ElementAt(i).Select(Selectors.XPath(".//div[@class='photo-frame']/img/@src"))?.Value;

            //    if (!string.IsNullOrWhiteSpace(url))
            //    {
            //        if (url.Contains("/"))
            //        {
            //            string[] urlSplit = url.ToString().Split('/');
            //            string starsLinkId = urlSplit[urlSplit.Length - 1];
            //            var request = context.CreateNewRequest(new Uri(url));
            //            request.Properties.Add("title", title);
            //            request.Properties.Add("url", url);
            //            request.Properties.Add("starsLinkId", starsLinkId);
            //            request.Properties.Add("index", i + 1);
            //            context.AddFollowRequests(request);
            //        }

            //    }

            //    if (!string.IsNullOrWhiteSpace(headUrl))
            //    {
            //        var request = context.CreateNewRequest(new Uri(headUrl));
            //        context.AddFollowRequests(request);
            //    }
            //}
            //foreach (var stars in starsList)
            //{

            //}

            return Task.CompletedTask;
        }
    }
}
