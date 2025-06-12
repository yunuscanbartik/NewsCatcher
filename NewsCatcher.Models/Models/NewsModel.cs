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
            }
        }
        public class BBCModel
        {
            [XmlRoot(ElementName = "image")]
            public class Image
            {

                [XmlElement(ElementName = "url")]
                public string Url { get; set; }

                [XmlElement(ElementName = "title")]
                public string Title { get; set; }

                [XmlElement(ElementName = "link")]
                public string Link { get; set; }
            }

            [XmlRoot(ElementName = "link")]
            public class Link
            {

                [XmlAttribute(AttributeName = "href")]
                public string Href { get; set; }

                [XmlAttribute(AttributeName = "rel")]
                public string Rel { get; set; }

                [XmlAttribute(AttributeName = "type")]
                public string Type { get; set; }
            }

            [XmlRoot(ElementName = "guid")]
            public class Guid
            {

                [XmlAttribute(AttributeName = "isPermaLink")]
                public bool IsPermaLink { get; set; }

                [XmlText]
                public string Text { get; set; }
            }

            [XmlRoot(ElementName = "thumbnail")]
            public class Thumbnail
            {

                [XmlAttribute(AttributeName = "width")]
                public int Width { get; set; }

                [XmlAttribute(AttributeName = "height")]
                public int Height { get; set; }

                [XmlAttribute(AttributeName = "url")]
                public string Url { get; set; }
            }

            [XmlRoot(ElementName = "item")]
            public class Item
            {

                [XmlElement(ElementName = "title")]
                public string Title { get; set; }

                [XmlElement(ElementName = "description")]
                public string Description { get; set; }

                [XmlElement(ElementName = "link")]
                public string Link { get; set; }

                [XmlElement(ElementName = "guid")]
                public Guid Guid { get; set; }

                [XmlElement(ElementName = "pubDate")]
                public DateTime PubDate { get; set; }

                [XmlElement(ElementName = "thumbnail")]
                public Thumbnail Thumbnail { get; set; }
            }

            [XmlRoot(ElementName = "channel")]
            public class Channel
            {

                [XmlElement(ElementName = "title")]
                public string Title { get; set; }

                [XmlElement(ElementName = "description")]
                public string Description { get; set; }

                [XmlElement(ElementName = "link")]
                public List<string> Link { get; set; }

                [XmlElement(ElementName = "image")]
                public Image Image { get; set; }

                [XmlElement(ElementName = "generator")]
                public string Generator { get; set; }

                [XmlElement(ElementName = "lastBuildDate")]
                public DateTime LastBuildDate { get; set; }

                [XmlElement(ElementName = "copyright")]
                public string Copyright { get; set; }

                [XmlElement(ElementName = "language")]
                public string Language { get; set; }

                [XmlElement(ElementName = "ttl")]
                public int Ttl { get; set; }

                [XmlElement(ElementName = "item")]
                public List<Item> Item { get; set; }
            }

            [XmlRoot(ElementName = "rss")]
            public class Rss
            {

                [XmlElement(ElementName = "channel")]
                public Channel Channel { get; set; }

                [XmlAttribute(AttributeName = "dc")]
                public string Dc { get; set; }

                [XmlAttribute(AttributeName = "content")]
                public string Content { get; set; }

                [XmlAttribute(AttributeName = "atom")]
                public string Atom { get; set; }

                [XmlAttribute(AttributeName = "version")]
                public double Version { get; set; }

                [XmlAttribute(AttributeName = "media")]
                public string Media { get; set; }

                [XmlText]
                public string Text { get; set; }
            }
        }
    }
}
