using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

public class Nasdaq
{
    const String NasdaqUrl = "http://www.nasdaqomxnordic.com/webproxy/DataFeedProxy.aspx";

    public static IEnumerable<Currency> FetchCurrencies()
    {
        var doc = CreateHtmlDocument();
        var lines = doc.DocumentNode.SelectNodes("//*/tbody/tr");
        foreach (var line in lines)
        {
            yield return ParseLine(line);
        }
    }

    private static Currency ParseLine(HtmlNode line)
    {
        var id = line.SelectSingleNode("td[1]").InnerText;
        var last = GetValueFromLine(line);
        var date = DateTime.Parse(line.SelectSingleNode("td[3]").InnerText);
        return new Currency
                   {
                       Name = id,
                       Value = last,
                       Date = date
                   };
    }

    private static double GetValueFromLine(HtmlNode line)
    {
        var valueNode = line.SelectSingleNode("td[2]").InnerText.Replace('.', ',');
        double last;
        double.TryParse(valueNode, out last);
        return last;
    }

    private static HtmlDocument CreateHtmlDocument()
    {
        var response = CreateConnection();
        var reader = new StreamReader(response.GetResponseStream());
        var table = reader.ReadToEnd();
        var doc = new HtmlDocument();
        doc.LoadHtml(table);
        return doc;
    }

    private static WebResponse CreateConnection()
    {
        var request = WebRequest.Create(NasdaqUrl);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        PostData(request);
        var response = request.GetResponse();
        return response;
    }

    private static void PostData(WebRequest request)
    {
        using (var writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(
                "xmlquery=%3Cpost%3E%0A%3Cparam+name%3D%22SubSystem%22+value%3D%22Prices%22%2F%3E%0A%3Cparam+name%3D%22Action%22+value%3D%22GetMarket%22%2F%3E%0A%3Cparam+name%3D%22Market%22+value%3D%22CURRENCY%22%2F%3E%0A%3Cparam+name%3D%22Exchange%22+value%3D%22NMF%22%2F%3E%0A%3Cparam+name%3D%22inst__a%22+value%3D%220%2C1%2C2%2C37%2C86%22%2F%3E%0A%3Cparam+name%3D%22ext_xslt%22+value%3D%22%2FnordicV3%2Finst_simple_table.xsl%22%2F%3E%0A%3Cparam+name%3D%22ext_xslt_hiddenattrs%22+value%3D%22%2Cid%2Cnm%2C%22%2F%3E%0A%3Cparam+name%3D%22ext_xslt_lang%22+value%3D%22en%22%2F%3E%0A%3Cparam+name%3D%22XPath%22+value%3D%22%2F%2Finst%22%2F%3E%0A%3Cparam+name%3D%22ext_xslt_tableId%22+value%3D%22currencyTable%22%2F%3E%0A%3Cparam+name%3D%22ext_xslt_options%22+value%3D%22%2Cnoflag%2Cnolink%2C%22%2F%3E%0A%3Cparam+name%3D%22app%22+value%3D%22%2Faktier%2Fvaluta%2F%22%2F%3E%0A%3C%2Fpost%3E");
        }
    }

    public class Currency
    {
        public String Name { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}