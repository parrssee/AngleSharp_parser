using AngleSharp.Html.Dom;

namespace WebApplication1.ParserCore
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
