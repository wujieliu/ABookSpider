using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Storage;
using DotnetSpider.Selector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABookSpider.Entity
{
    [Schema("abook", "a_movie")]
    [EntitySelector(Expression = ".//div[@class='container']", Type = SelectorType.XPath)]
    [FollowRequestSelector(Expressions = new[] { "cn/movie/']" }, SelectorType = SelectorType.Regex)]
    public class MovieEntity : EntityBase<MovieEntity>
    {
        protected override void Configure()
        {
            HasKey(x => x.LinkId);
        }

        [Required]
        [StringLength(40)]
        [Column("link_id")]
        [ValueSelector(Expression = "linkId", Type = SelectorType.Environment)]
        public string LinkId { get; set; }

        //[StringLength(100)]
        //[ValueSelector(Expression = "name", Type = SelectorType.Environment)]
        [Column("name")]
        [ValueSelector(Expression = ".//h3")]
        public string Name { get; set; }
        public string BigImage { get; set; }

        public string A_ID { get; set; }


        public string release_date { get; set; }

        public string length { get; set; }

        public string director { get; set; }

        public string director_link_id { get; set; }

        public string studio { get; set; }

        public string studio_link_id { get; set; }

        public string label { get; set; }

        public string label_link_id { get; set; }



        public string series { get; set; }

        public string series_link_id { get; set; }


        public string genres { get; set; }

        public string stars { get; set; }


        public int image_len { get; set; }



    }
}
