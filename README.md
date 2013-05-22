Nasdaq Currency Fetcher
=======================

Fetch up to date currencies from nordic Nasdaq OMX

## Usage
    foreach(var currency in Nasdaq.FetchCurrencies())
    {
        Console.WriteLine(string.Format("{0}: {1} = {2}", currency.Date.ToShortDateString(), currency.Name, currency.Value));
    }

## Requirements
* HtmlAgilityPack
