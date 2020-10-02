using System;

namespace Dodo.ISO4217
{
    public static class CurrencyExtensions
    {
        public static Currency.Code ToCurrencyCode(this int code) =>
            Enum.IsDefined(typeof(Currency.Code), code)
                ? (Currency.Code) code
                : throw new InvalidOperationException($"\"{code}\" isn't valid {nameof(Currency.Code)}");

        public static Currency.Code ToCurrencyCode(this string code) =>
            Enum.TryParse<Currency.Code>(code, out var @enum) &&
            Enum.IsDefined(typeof(Currency.Code), (int) @enum)
                ? @enum
                : throw new InvalidOperationException($"\"{code}\" isn't valid {nameof(Currency.Code)}");

        public static string GetName(this Currency.Code code) =>
            Currency.GetNameByCode((int) code);

        public static int GetMinorUnits(this Currency.Code code) =>
            Currency.GetMinorUnitsByCode((int) code);
    }
}