using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ISO4217.Generator
{
    public class Currency
    {
        public string Name { get; set; }
        public string ThreeLetterCode { get; set; }
        public string NumericCode { get; set; }
        public string CountryName { get; set; }
        public int MinorUnits { get; set; }
    }

    public class Program
    {
        public static async Task Main()
        {
            // download xml
            var xmlStream = await DownloadXml();

            // parse
            var currencies = XDocument
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

            var codeTemplate = await File.ReadAllTextAsync("./Templates/Code.txt");
            var codesTextArray = currencies
                .Select(x => codeTemplate
                    .Replace("{%LETTERS%}", x.Value.ThreeLetterCode)
                    .Replace("{%CODE%}", x.Value.NumericCode));

            var namesTemplate = await File.ReadAllTextAsync("./Templates/Name.txt");
            var namesTextArray = currencies
                .Select(x => namesTemplate
                    .Replace("{%CODE%}", x.Value.NumericCode)
                    .Replace("{%NAME%}", x.Value.Name));

            var minorUnitsTemplate = await File.ReadAllTextAsync("./Templates/MinorUnit.txt");
            var minorUnitsTextArray = currencies
                .Select(x => minorUnitsTemplate
                    .Replace("{%CODE%}", x.Value.NumericCode)
                    .Replace("{%MINOR_UNITS%}", x.Value.MinorUnits.ToString()));

            var classTemplate = await File.ReadAllTextAsync("./Templates/Class.txt");
            var classText = classTemplate
                .Replace("{%CODES%}", string.Join('\n', codesTextArray))
                .Replace("{%NAMES%}", string.Join('\n', namesTextArray))
                .Replace("{%MINOR_UNITS%}", string.Join('\n', minorUnitsTextArray))
                .Replace("{%DATETIME%}", DateTime.UtcNow.ToString("O"));

            Directory.CreateDirectory("./Output");
            await File.WriteAllTextAsync("./Output/Currency.cs", classText);
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