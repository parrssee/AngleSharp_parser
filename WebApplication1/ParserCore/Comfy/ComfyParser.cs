using AngleSharp.Html.Dom;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.ParserCore.Comfy
{
    class ComfyParser : IParser<string[]>
    {
        public string[] Parse(IHtmlDocument document)
        {
            var list = new List<string>();
            var items = document.QuerySelectorAll("a").Where(item => item.ClassName != null && item.ClassName.Contains("product-item__name-link js-gtm-product-title"));

            foreach (var item in items)
            {
                list.Add(item.InnerHtml.Trim());
            }

            return list.ToArray();
        }
    }
}