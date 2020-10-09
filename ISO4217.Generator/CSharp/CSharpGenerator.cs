using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ISO4217.Generator.CSharp
{
    public class CSharpGenerator : IGenerator
    {
        private static async Task GeneratePackage(
            string version,
            string templatesDir,
            string nugetDir)
        {
            const string projFilename = "ISO3166.csproj";
            var projTemplate = await File.ReadAllTextAsync($"{templatesDir}/Nuget/{projFilename}.txt");
            var projContent = projTemplate.Replace("<Version></Version>", $"<Version>{version}</Version>");
            await File.WriteAllTextAsync(nugetDir + $"/{projFilename}", projContent);

            const string extFilename = "CountryExtensions.cs";
            var extContent = await File.ReadAllTextAsync($"{templatesDir}/Nuget/{extFilename}.txt");
            await File.WriteAllTextAsync(nugetDir + $"/{extFilename}", extContent);
        }

        private static async Task GenerateCode(
            Dictionary<string, Currency> currencies,
            string templatesDir,
            string nugetDir)
        {
            var codeTemplate = await File.ReadAllTextAsync($"{templatesDir}/Code/Code.txt");
            var codesTextArray = currencies
                .Select(x => codeTemplate
                    .Replace("{%LETTERS%}", x.Value.ThreeLetterCode)
                    .Replace("{%CODE%}", x.Value.NumericCode));
            var namesTemplate = await File.ReadAllTextAsync($"{templatesDir}/Code/Name.txt");
            var namesTextArray = currencies
                .Select(x => namesTemplate
                    .Replace("{%CODE%}", x.Value.NumericCode)
                    .Replace("{%NAME%}", x.Value.Name));
            var minorUnitsTemplate = await File.ReadAllTextAsync($"{templatesDir}/Code/MinorUnit.txt");
            var minorUnitsTextArray = currencies
                .Select(x => minorUnitsTemplate
                    .Replace("{%CODE%}", x.Value.NumericCode)
                    .Replace("{%MINOR_UNITS%}", x.Value.MinorUnits.ToString()));
            var classTemplate = await File.ReadAllTextAsync($"{templatesDir}/Code/Class.cs.txt");
            var classText = classTemplate
                .Replace("{%CODES%}", string.Join('\n', codesTextArray))
                .Replace("{%NAMES%}", string.Join('\n', namesTextArray))
                .Replace("{%MINOR_UNITS%}", string.Join('\n', minorUnitsTextArray))
                .Replace("{%DATETIME%}", DateTime.UtcNow.ToString("O"));
            await File.WriteAllTextAsync($"{nugetDir}/Currency.cs", classText);
        }

        private static async Task GenerateTests(
            string templatesDir,
            string testsDir)
        {
            const string projFilename = "ISO4217.UnitTests.csproj";
            var projTemplate = await File.ReadAllTextAsync($"{templatesDir}/Tests/{projFilename}.txt");
            await File.WriteAllTextAsync(testsDir + $"/{projFilename}", projTemplate);

            const string extFilename = "ExtensionsTests.cs";
            var extTemplate = await File.ReadAllTextAsync($"{templatesDir}/Tests/{extFilename}.txt");
            await File.WriteAllTextAsync(testsDir + $"/{extFilename}", extTemplate);
        }

        private static async Task GenerateSolution(
            string templatesDir,
            string outputDir)
        {
            const string slnFilename = "ISO4217.sln";
            var slnTemplate = await File.ReadAllTextAsync($"{templatesDir}/Solution/{slnFilename}.txt");
            await File.WriteAllTextAsync(outputDir + $"/{slnFilename}", slnTemplate);

            const string licenseFilename = "LICENSE";
            File.Copy($"./{licenseFilename}", outputDir + $"/{licenseFilename}");

            const string readmeFilename = "README.md";
            File.Copy($"./{readmeFilename}", outputDir + $"/{readmeFilename}");

            const string logoFilename = "dodopizza-logo.png";
            File.Copy($"./{logoFilename}", outputDir + $"/{logoFilename}");
        }

        public async Task Generate(Dictionary<string, Currency> currencies)
        {
            const string version = "0.0.1-alpha1";

            const string templatesDir = "./ISO4217.Generator/CSharp/Templates";
            const string outputDir = "./Output/CSharp";

            const string nugetDir = outputDir + "/ISO4217";
            const string testsDir = outputDir + "/ISO4217.UnitTests";

            Directory.CreateDirectory(nugetDir);
            Directory.CreateDirectory(testsDir);

            await GeneratePackage(version, templatesDir, nugetDir);
            await GenerateCode(currencies, templatesDir, nugetDir);
            await GenerateTests(templatesDir, testsDir);
            await GenerateSolution(templatesDir, outputDir);
        }
    }
}