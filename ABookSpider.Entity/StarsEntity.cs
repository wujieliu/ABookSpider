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
    [Schema("abook", "a_stars")]
    [EntitySelector(Expression = ".//div[@class='avatar-box']", Type = SelectorType.XPath)]
    //[GlobalValueSelector(Expression = ".//a[@class='current']", Name = "类别", Type = SelectorType.XPath)]
    //[GlobalValueSelector(Expression = "//path", Name = "LinkId", Type = SelectorType.XPath)]
    [FollowRequestSelector(Expressions = new[] { "cn/star']" },SelectorType =SelectorType.Regex)]
    public class StarsEntity : EntityBase<StarsEntity>
    {

        //       "linkid" TEXT(16) NOT NULL,
        //"name" TEXT(50),
        //"name_history" TEXT(50),
        //"birthday" TEXT(12),
        //"height" TEXT(10),
        //"cup" TEXT(10),
        //"bust" TEXT(10),
        //"waist" TEXT(10),
        //"hips" TEXT(10),
        //"hometown" TEXT(50),
        //"hobby" TEXT(50),
        //"headimg" TEXT(200),

        protected override void Configure()
        {
            HasKey(x => x.LinkId);
        }

        //public int id { get; set; }

        [Required]
        [StringLength(100)]
        [ValueSelector(Expression = "starsLinkId", Type = SelectorType.Environment)]
        public string LinkId { get; set; }

        [Required]
        [ValueSelector(Expression = ".//div[@class='photo-info']/span")]
        public string Name { get; set; }

        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'年龄:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "年龄:")]
        public string Age { get; set; }

        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'生日:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "生日:")]
        public string Birthday { get; set; }

        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'身高:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "身高:")]
        public string Height { get; set; }

        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'罩杯:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "罩杯:")]
        public string Cup { get; set; }

        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'胸围:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "胸围:")]
        public string Bust { get; set; }

        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'腰围:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "腰围:")]
        public string Waist { get; set; }

        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'臀围:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "臀围:")]
        public string Hipline { get; set; }
        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'出生地:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "出生地:")]
        public string Birthplace { get; set; }

        [ValueSelector(Expression = ".//div[@class='photo-info']/p[contains(text(),'爱好:')]")]
        [ReplaceFormatter(NewValue = "", OldValue = "爱好:")]
        public string Hobby { get; set; }

        [Required]
        [ValueSelector(Expression = ".//div[@class='photo-frame']/img/@src")]
        public string Headimg { get; set; }

        //public int Id { get; set; }

        //[Required]
        //[StringLength(200)]
        //[ValueSelector(Expression = "类别", Type = SelectorType.Environment)]
        //public string Category { get; set; }

        //[Required]
        //[StringLength(200)]
        //[ValueSelector(Expression = "网站", Type = SelectorType.Environment)]
        //public string WebSite { get; set; }

        //[StringLength(200)]
        //[ValueSelector(Expression = "Title", Type = SelectorType.Environment)]
        //[ReplaceFormatter(NewValue = "", OldValue = " - 博客园")]
        //public string Title { get; set; }

        //[StringLength(40)]
        //[ValueSelector(Expression = "GUID", Type = SelectorType.Environment)]
        //public string Guid { get; set; }

        //[ValueSelector(Expression = ".//h2[@class='news_entry']/a")]
        //public string News { get; set; }

        //[ValueSelector(Expression = ".//h2[@class='news_entry']/a/@href")]
        //public string Url { get; set; }

        //[ValueSelector(Expression = ".//div[@class='entry_summary']")]
        //[TrimFormatter]
        //public string PlainText { get; set; }

        //[ValueSelector(Expression = "DATETIME", Type = SelectorType.Environment)]
        //public DateTime CreationTime { get; set; }
    }
}
