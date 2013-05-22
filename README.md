NasdaqCurrencyFetcher
=====================

Fetch up to date currencies from Nasdaq OMX

Usage
=====
`foreach(var currency in Nasdaq.FetchCurrencies()
`{
`	Console.WriteLine(string.Format("{0}: {1} = {2}", currency.Date, currency.Name, currency.Value))
`}
