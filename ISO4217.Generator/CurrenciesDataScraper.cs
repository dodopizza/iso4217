using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ISO4217.Generator
{
    public class CurrenciesDataScraper
    {
        public async Task<Dictionary<string, Currency>> Scrape()
        {
            var xmlStream = await DownloadXml();
            return XDocument
                    .Load(xmlStream).Root!
                .Elements().First()
                .Elements()
                .Select(x => x
                    .Elements()
                    .ToDictionary(el => el.Name.ToString(), el => el.Value.ToString()))
                .Where(x => x.Count == 5)
                .Select(x =>
                {
                    int.TryParse(x["CcyMnrUnts"], out var minorUnits);
                    return new Currency
                    {
                        CountryName = x["CtryNm"],
                        Name = x["CcyNm"],
                        ThreeLetterCode = x["Ccy"],
                        NumericCode = x["CcyNbr"],
                        MinorUnits = minorUnits,
                    };
                })
                .GroupBy(x => x.ThreeLetterCode)
                .ToDictionary(x => x.Key, x => x.First());
        }

        private static async Task<Stream> DownloadXml()
        {
            var httpClient = new HttpClient();
            var httpResponse = await httpClient
                .GetAsync("https://www.currency-iso.org/dam/downloads/lists/list_one.xml");
            return await httpResponse.Content.ReadAsStreamAsync();
        }
    }
}