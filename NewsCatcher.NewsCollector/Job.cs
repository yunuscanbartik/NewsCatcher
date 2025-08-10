using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NewsCatcher.NewsCollector
{
    public class Job : ICustomJob
    {
        public async Task<bool> Execute(JobObject jobObject)
        {




            try
            {
                using (var httpClient = new HttpClient()) // HTTP isteklerini yapabilmek için değişkene atıyorum.
                {
                    var response = await httpClient.GetAsync(feedUrl); //URL e get isteği göndererek haberleri alıyorum ve response değişkenine atıyorum.

                    var xmlContent = await response.Content.ReadAsStringAsync(); //response içeriğini string olarak okuyorum. 

                    var serializer = new XmlSerializer(typeof(NewsModel.BBCModel.Rss)); // xml i parçalayarak nesneye dönüştürmek için XmlSerializer kullanıyorum.
                    using (var stringReader = new StringReader(xmlContent)) // xml içeriğini string olarak okuyabilmek için StringReader kullanıyorum.
                    {
                        var rss = (NewsModel.BBCModel.Rss)serializer.Deserialize(stringReader);
                        return rss.Channel?.Item;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new List<NewsModel.BBCModel.Item>();
            }


            var URL = jobObject.URL;
            //MODELI BURADA AL
            List<T>  a
            //  GİT URLDEN OKU AMK
            
           //  URLD+EN OKUDUN MU MAPLE AMK
            
           //  RABBITMQYA PASLA AMK
            
        }

        public T GetT()

        
    }
}
