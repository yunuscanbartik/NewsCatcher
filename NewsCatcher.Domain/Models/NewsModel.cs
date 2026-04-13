using System.Xml.Serialization;

namespace NewsCatcher.Models.Models
{
    public class NewsModel
    {
        public class BrowseModel
        {
            public class Request
            {
                public int? NewsId { get; set; }
            }
            public class Return : ReturnModel
            {
                public List<ReturnData> Data { get; set; }
            }
            public class ReturnData
            {
                public int? NewsId { get; set; }
                public string? Title { get; set; }
                public string? Content { get; set; }
                public string? Summary { get; set; }
                public int? CategoryId { get; set; }
                public DateTime? ShareDate { get; set; }
                public string? SourceName { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
                public string? ThumbnailUrl { get; set; }
                public string? Link { get; set; }
                public string? GuId { get; set; }
            }
        }
        public class CreateModel
        {
            public class Request
            {
                public string? Title { get; set; }
                public string? Content { get; set; }
                public string? Summary { get; set; }
                public int? CategoryId { get; set; }
                public string? SourceName { get; set; }
                public string? ThumbnailUrl { get; set; }
                public string? Link { get; set; }
                public string? GuId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int? NewsId { get; set; }
                public string? Title { get; set; }
                public string? Content { get; set; }
                public string? Summary { get; set; }
                public int? CategoryId { get; set; }
                public DateTime? ShareDate { get; set; }
                public string? SourceName { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
                public string? ThumbnailUrl { get; set; }
                public string? Link { get; set; }
                public string? GuId { get; set; }
            }
        }

        public class UpdateModel
        {
            public class Request
            {
                public int? NewsId { get; set; }
                public string? Title { get; set; }
                public string? Content { get; set; }
                public string? Summary { get; set; }
                public int? CategoryId { get; set; }
                public string? SourceName { get; set; }
                public string? ThumbnailUrl { get; set; }
                public string? Link { get; set; }
                public string? GuId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int? NewsId { get; set; }
                public string? Title { get; set; }
                public string? Content { get; set; }
                public string? Summary { get; set; }
                public int? CategoryId { get; set; }
                public DateTime? ShareDate { get; set; }
                public string? SourceName { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
                public string? ThumbnailUrl { get; set; }
                public string? Link { get; set; }
                public string? GuId { get; set; }
            }
        }
        public class DeleteModel
        {
            public class Request
            {
                public int? NewsId { get; set; }
            }
            public class Return : ReturnModel
            {
                public ReturnData? Data { get; set; }
            }
            public class ReturnData
            {
                public int? NewsId { get; set; }
                public string? Title { get; set; }
                public string? Content { get; set; }
                public string? Summary { get; set; }
                public int? CategoryId { get; set; }
                public DateTime? ShareDate { get; set; }
                public string? SourceName { get; set; }
                public DateTime? CreatedDate { get; set; }
                public DateTime? UpdatedDate { get; set; }
                public string? ThumbnailUrl { get; set; }
                public string? Link { get; set; }
                public string? GuId { get; set; }
            }
        }
        public class BBCModel
        {
            [XmlRoot(ElementName = "rss")]
            public class Rss
            {
                [XmlElement("channel")]
                public Channel Channel { get; set; }

                [XmlAttribute("version")]
                public string Version { get; set; }
            }

            public class Channel
            {
                [XmlElement("title")]
                public string Title { get; set; }

                [XmlElement("description")]
                public string Description { get; set; }

                [XmlElement("link")]
                public string Link { get; set; }

                [XmlElement("lastBuildDate")]
                public string LastBuildDateStr { get; set; }

                [XmlIgnore]
                public DateTime LastBuildDate
                {
                    get
                    {
                        return DateTime.ParseExact(LastBuildDateStr, "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    set { LastBuildDateStr = value.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'"); }
                }

                [XmlElement("item")]
                public List<Item> Item { get; set; }
            }

            public class Item
            {
                [XmlElement("title")]
                public string Title { get; set; }

                [XmlElement("description")]
                public string Description { get; set; }

                [XmlElement("link")]
                public string Link { get; set; }

                [XmlElement("pubDate")]
                public string PubDateStr { get; set; }

                [XmlIgnore]
                public DateTime PubDate
                {
                    get
                    {
                        return DateTime.ParseExact(PubDateStr, "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    set { PubDateStr = value.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'"); }
                }

                [XmlElement("guid")]
                public string Guid { get; set; }

                [XmlElement("thumbnail", Namespace = "http://search.yahoo.com/mrss/")]
                public Thumbnail? Thumbnail { get; set; }
            }

            public class Thumbnail
            {
                [XmlAttribute("url")]
                public string Url { get; set; }

                [XmlAttribute("width")]
                public int Width { get; set; }

                [XmlAttribute("height")]
                public int Height { get; set; }
            }
        }
        public class CNNModel
        {
            [XmlRoot(ElementName = "rss")]
            public class Rss
            {
                [XmlElement("channel")]
                public Channel Channel { get; set; }

                [XmlAttribute("version")]
                public string Version { get; set; }
            }
            public class Channel
            {
                [XmlElement("title")]
                public string Title { get; set; }

                [XmlElement("link")]
                public string Link { get; set; }

                [XmlElement("description")]
                public string Description { get; set; }

                [XmlElement("language")]
                public string Language { get; set; }

                [XmlElement("category")]
                public string Category { get; set; }

                [XmlElement("item")]
                public List<Item> Item { get; set; }
            }
            public class Item
            {
                [XmlElement("guid")]
                public RssItemGuidElement ItemGuid { get; set; }

                [XmlElement("link")]
                public string Link { get; set; }

                [XmlElement("title")]
                public string Title { get; set; }

                [XmlElement("description")]
                public string Description { get; set; }

                [XmlElement("pubDate")]
                public string PubDateStr { get; set; }

                [XmlIgnore]
                public DateTime PubDate
                {
                    get
                    {
                        return DateTime.ParseExact(PubDateStr, "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    set { PubDateStr = value.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'"); }
                }

                [XmlElement("image")]
                public string Image { get; set; }
            }
            public class RssItemGuidElement
            {
                [XmlAttribute("isPermaLink")]
                public bool IsPermaLink { get; set; }

                [XmlText]
                public string Value { get; set; }
            }
            public class AtomLink
            {
                [XmlAttribute("href")]
                public string Href { get; set; }
            }
        }
    }
}



