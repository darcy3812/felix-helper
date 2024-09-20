using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using FelixHelper.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FelixHelper.Services
{
    public class PromotionService
    {
        private readonly FtpService _ftpService;
        private const string HeaderPromoTextId = "headerPromoText";
        private const string HeaderPromoDatesId = "headerPromoDates";
        private const string PromoTextId = "PromoText";        

        public PromotionService(FtpService ftpService)
        {
            _ftpService = ftpService;
        }

        public bool UpdatePromotion(PromotionModel promotion)
        {
            using (var stream = new MemoryStream())
            {
                var downloadResult = _ftpService.DownloadFile(stream);
                if (!downloadResult)
                {
                    return false;
                }
                stream.Position = 0;

                using (IHtmlDocument doc = ProccessHtml(promotion, stream))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    using (var sw = new StreamWriter(stream, System.Text.Encoding.UTF8))
                    {
                        sw.Write(doc.DocumentElement.OuterHtml);
                        sw.Flush();
                        stream.Seek(0, SeekOrigin.Begin);

                        return _ftpService.UploadFile(stream);
                    }
                }
            }
        }

        private IHtmlDocument ProccessHtml(PromotionModel promotion, MemoryStream stream)
        {
            HtmlParser parser = new HtmlParser();
            var doc = parser.ParseDocument(stream);
            var headerPromoTextElement = doc.GetElementById(HeaderPromoTextId);
            headerPromoTextElement.TextContent = ReplaceMonth(headerPromoTextElement.TextContent, promotion.Month);

            var headerPromoDatesElement = doc.GetElementById(HeaderPromoDatesId);
            headerPromoDatesElement.TextContent = promotion.Days;

            var promoTextElement = doc.GetElementById(PromoTextId);
            promoTextElement.TextContent = ReplacePromoText(promoTextElement.TextContent, promotion);
            return doc;
        }

        private string ReplaceMonth(string promoText, string newMonth)
        {
            var words = promoText.Split(' ');
            var month = words.First(_ => _.Contains(':')).Trim(':');

            return promoText.Replace(month, newMonth);
        }

        private string ReplacePromoText(string promoText, PromotionModel promotion)
        {
            promoText = ReplaceMonth(promoText, promotion.Month);
            return ReplaceDays(promoText, promotion.Days);
        }

        private string ReplaceDays(string promoText, string days)
        {
            var text = promoText.Split(':')[0];

            return $"{text}: {days}";
        }
    }
}
