using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace WebApplication1.ParserCore.Rozetka
{
    class RozetkaParser : IParser<List<SmartphoneParams>>
    {
        public List<SmartphoneParams> Parse(IHtmlDocument document)
        {
            List<SmartphoneParams> sm = new List<SmartphoneParams>();

            var names = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("g-i-tile-i-title clearfix"));
            var descriptions = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("short-description"));
            var images = document.QuerySelectorAll("img")
                .Where(item => item.ParentElement.ClassName.Contains("responsive-img centering-child-img"))
                .Select(item => item.Attributes["src"]?.Value);

            for (int i = 0; i < names.Count(); i++)
            {
                sm.Add(new SmartphoneParams()
                {
                    Name = names.ToList()[i].TextContent.Trim(),
                    Description = descriptions.ToList()[i].TextContent.Trim(),
                    Image = ImageToByteArray(
                        DownloadImage(images.ToList()[i]))
                });
            }

            return sm;
        }

        private Image DownloadImage(string fromUrl)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                using (Stream stream = webClient.OpenRead(fromUrl))
                {
                    return Image.FromStream(stream);
                }
            }
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}