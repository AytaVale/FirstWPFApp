using FirstWPFApp;
using FirstWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CsvFileLoader : IFileLoader
{
    public List<Data> Load(string filePath)
    {
        var DataList = new List<Data>();

        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1)) // нужно запомнить: пропускает заголовок.
        {
            var parts = line.Split(',');
            if (parts.Length == 6)
            {
                try
                {
                    var data = new Data
                    {
                        Date = DateTime.Parse(parts[0], CultureInfo.InvariantCulture),
                        Open = decimal.Parse(parts[1], CultureInfo.InvariantCulture),
                        High = decimal.Parse(parts[2], CultureInfo.InvariantCulture),
                        Low = decimal.Parse(parts[3], CultureInfo.InvariantCulture),
                        Close = decimal.Parse(parts[4], CultureInfo.InvariantCulture),
                        Volume = int.Parse(parts[5], CultureInfo.InvariantCulture)
                    };
                    DataList.Add(data);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
        return DataList;
    }
}
