using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FirstWPFApp.Models;

namespace FirstWPFApp.Loaders
{
    public class XmlFileLoader : IFileLoader
    {
        public List<Data> Load(string filePath)
        {
            var trades = new List<Data>();
            var doc = XDocument.Load(filePath);

            foreach (var element in doc.Descendants("value"))
            {
                var trade = new Data
                {
                    Date = DateTime.Parse(element.Attribute("date").Value),
                    Open = decimal.Parse(element.Attribute("open").Value, CultureInfo.InvariantCulture),
                    High = decimal.Parse(element.Attribute("high").Value, CultureInfo.InvariantCulture),
                    Low = decimal.Parse(element.Attribute("low").Value, CultureInfo.InvariantCulture),
                    Close = decimal.Parse(element.Attribute("close").Value, CultureInfo.InvariantCulture),
                    Volume = int.Parse(element.Attribute("volume").Value)
                };

                trades.Add(trade);
            }

            return trades;
        }
    }
}
