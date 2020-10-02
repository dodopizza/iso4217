// ReSharper disable InconsistentNaming
using System.Collections.Generic;

namespace Dodo.ISO4217
{
    public static class Currency
    {
        public enum Code
        {
            RUB = 643,
            EUR = 978,
        }

        private static readonly IReadOnlyDictionary<int, string> Names = new Dictionary<int, string>(180)
        {
            [643] = "Russian Ruble",
            [978] = "Euro",
        };

        public static string GetNameByCode(int code) =>
            Names[code];

        private static readonly IReadOnlyDictionary<int, int> MinorUnits = new Dictionary<int, int>(180)
        {
            [643] = 2,
            [978] = 2,
        };

        public static int GetMinorUnitsByCode(int code) =>
            MinorUnits[code];
    }
}