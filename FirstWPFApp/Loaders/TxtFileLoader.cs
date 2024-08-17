using FirstWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FirstWPFApp.Loaders
{
    public class TxtFileLoader : IFileLoader
    {
        public List<Data> Load(string filePath)
        {
            var trades = new List<Data>();

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var values = line.Split(';');

                var trade = new Data
                {
                    Date = DateTime.ParseExact(values[0], "yyyy-M-d", CultureInfo.InvariantCulture),
                    Open = decimal.Parse(values[1], CultureInfo.InvariantCulture),
                    High = decimal.Parse(values[2], CultureInfo.InvariantCulture),
                    Low = decimal.Parse(values[3], CultureInfo.InvariantCulture),
                    Close = decimal.Parse(values[4], CultureInfo.InvariantCulture),
                    Volume = int.Parse(values[5])
                };

                trades.Add(trade);
            }

            return trades;
        }
    }
}