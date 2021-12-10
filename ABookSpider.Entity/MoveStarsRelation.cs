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
    [Schema("abook", "a_movie_star_relation")]
    [EntitySelector(Expression = ".//div[@class='container']", Type = SelectorType.XPath)]
    //[FollowRequestSelector(Expressions = new[] { "cn/movie/']" }, SelectorType = SelectorType.Regex)]
    public class MoveStarsRelation : EntityBase<MoveStarsRelation>
    {
        protected override void Configure()
        {
            HasKey(x => x.id);

        }

        public int id { get; set; }

        public string MovieLinkId { get; set; }

        public string StarLinkId { get; set; }
    }
}
