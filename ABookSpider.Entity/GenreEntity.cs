using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow.Parser.Formatters;
using DotnetSpider.DataFlow.Storage;
using DotnetSpider.Selector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABookSpider.Entity
{
    [Schema("abook", "a_genre")]
    [EntitySelector(Expression = ".//div[@class='container-fluid pt-10']", Type = SelectorType.XPath)]
    //[GlobalValueSelector(Expression = ".//a[@class='current']", Name = "类别", Type = SelectorType.XPath)]
    //[GlobalValueSelector(Expression = "//path", Name = "LinkId", Type = SelectorType.XPath)]
    //[FollowRequestSelector(Expressions = new[] { "cn/genre']" }, SelectorType = SelectorType.Regex)]
    public class GenreEntity : EntityBase<GenreEntity>
    {
        protected override void Configure()
        {
            HasKey(x => x.LinkId);
        }

        //[Required]
        //[StringLength(100)]
        //[ValueSelector(Expression = ".//div[@class='row genre-box']/a/@href")]
        //[SplitFormatter(Separator =new []{"/"})]
        public string LinkId { get; set; }

        //[StringLength(100)]
        //[ValueSelector(Expression = "name", Type = SelectorType.Environment)]
        public string Name { get; set; }
        //[StringLength(100)]
        //[ValueSelector(Expression = "title", Type = SelectorType.Environment)]
        public string Title { get; set; }
        //[ValueSelector(Expression = "sort", Type = SelectorType.Environment)]
        public int SortName { get; set; }

        public int SortTitle { get; set; }

    }
}
