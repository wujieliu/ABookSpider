using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Storage;
using DotnetSpider.Selector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABookSpider.Entity
{
    [Schema("abook", "a_movie_image")]
    [EntitySelector(Expression = ".//div[@class='container']", Type = SelectorType.XPath)]
    public class MovieImage : EntityBase<MovieImage>
    {
        protected override void Configure()
        {
            HasKey(x => x.id);
        }

        public int id { get; set; }

        public string MovieLinkId { get; set; }

        public string Image { get; set; }

        public int SortIndex { get; set; }
    }
}
