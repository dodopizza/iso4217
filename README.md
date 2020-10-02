# Dodo.ISO4217

The generator scrapes data from https://www.currency-iso.org/dam/downloads/lists/list_one.xml then generates c# code

## Code examples

```
const int currencyCodeInt = 643;
const string currencyCodeString = "RUB";
const string currencyCodeIntString = "643";

// code enum
var currencyCode = currencyCodeInt.ToCurrencyCode(); // -> Currency.Code.RUB
var currencyCode = currencyCodeString.ToCurrencyCode(); // -> Currency.Code.RUB
var currencyCode = currencyCodeIntString.ToCurrencyCode(); // -> Currency.Code.RUB

// names
var currencyName = currencyCode.GetName(); // -> "Russian Ruble"

// int code
var currencyCodeInt = (int) currencyCode; // -> 643

// int codes
var currencyCodeIntString = currencyCode.ToString("D"); // -> "643"

// codes as strings
var currencyCodeLetters = currencyCode.ToString(); // -> "RUB"

// minor units
var minorUnits = currencyCode.GetMinorUnits(); // -> 2
```

## New version

1. Edit ISO4217.Generator code
2. Update version in ISO4217.csproj
3. Commit and push
4. PROFIT
