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
            [XmlRoot("rss", Namespace = "")] // XML'in kök elementi <rss>. Namespace="" ile varsayılan ad alanını belirtir.
            public class Rss
            {
                [XmlElement("channel")] // <rss> içindeki <channel> elementi.
                public Channel Channel { get; set; } // Channel nesnesini tutar.
            }

            public class Channel
            {
                [XmlElement("item")] // <channel> içindeki <item> elementleri (haberler).
                public List<Item> Item { get; set; } // Haber öğelerinin listesi.
            }

            public class Item
            {
                [XmlElement("title")] // <item> içindeki <title> elementi.
                public string Title { get; set; } // Haber başlığı (ör. "Örnek Haber Başlığı").

                [XmlElement("description")] // <item> içindeki <description> elementi.
                public string Description { get; set; } // Haber açıklaması (ör. "Örnek haber açıklaması...").

                [XmlElement("link")] // <item> içindeki <link> elementi.
                public string Link { get; set; } // Haberin URL'si (ör. "https://www.bbc.com/turkce/haber").

                [XmlElement("guid")] // <item> içindeki <guid> elementi.
                public Guid Guid { get; set; } // Haberin benzersiz kimliği.

                [XmlElement("pubDate")] // <item> içindeki <pubDate> elementi.
                public string PubDate { get; set; } // Haberin yayın tarihi (ör. "Mon, 30 Jun 2025 13:00:00 GMT").

                [XmlElement("thumbnail", Namespace = "http://search.yahoo.com/mrss/")] // <item> içindeki <media:thumbnail> elementi, media ad alanıyla.
                public Thumbnail Thumbnail { get; set; } // Haberin küçük resmi.
            }

            public class Guid
            {
                [XmlAttribute("isPermaLink")] // <guid> içindeki isPermaLink özniteliği (ör. "true").
                public string IsPermaLink { get; set; } // GUID'nin permalink olup olmadığını belirtir.

                [XmlText] // <guid> elementinin metin içeriği.
                public string Value { get; set; } // GUID değeri (ör. "https://www.bbc.com/turkce/haber").
            }

            public class Thumbnail
            {
                [XmlAttribute("width")] // <media:thumbnail> içindeki width özniteliği.
                public int Width { get; set; } // Küçük resmin genişliği (ör. 120).

                [XmlAttribute("height")] // <media:thumbnail> içindeki height özniteliği.
                public int Height { get; set; } // Küçük resmin yüksekliği (ör. 68).

                [XmlAttribute("url")] // <media:thumbnail> içindeki url özniteliği.
                public string Url { get; set; } // Küçük resmin URL'si (ör. "https://ichef.bbci.co.uk/images/haber.jpg").
            }
        }
    }
}
