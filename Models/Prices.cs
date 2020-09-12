using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IlirGashi_ConsoleApp.Models
{
    public class Prices
    {
        public string Item { get; set; }
        public double Price { get; set; }

        public Prices() { }

        ///
        ///@return List<Prices>
        ///
        public List<Prices> ReadPricesFromFile(string FileName)
        {
            try
            {
                Console.WriteLine("Init method::ReadPricesFromFile");
                List<Prices> pricesList = new List<Prices>();
                if (string.IsNullOrEmpty(FileName))
                    return pricesList;

                string pathPrices = Path.Combine(Environment.CurrentDirectory, @"Data\", FileName);
                using (StreamReader reader = new StreamReader(pathPrices))
                {
                    var result = reader.ReadToEnd();

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    XmlNodeList xmlNodeList = doc.GetElementsByTagName("ItemPrice");

                    foreach (XmlNode xmlNodePrice in xmlNodeList)
                    {
                        if (xmlNodePrice.HasChildNodes)
                            pricesList.Add(new Prices { Item = xmlNodePrice["Item"].InnerText, Price = double.Parse(xmlNodePrice["Price"].InnerText) });
                    }
                }
                Console.WriteLine("End method::ReadPricesFromFile");
                return pricesList;
            }
            catch(Exception e)
            {
                Console.WriteLine("method::ReadPricesFromFile Exception: {0}", e.Message);
                return new List<Prices>();
            }
        }
    }

  
}
