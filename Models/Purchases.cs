using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlirGashi_ConsoleApp.Models
{
    public class Purchases
    {
        public string Customer { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<string> Items { get; set; }

        public Purchases()
        {
            Items = new List<string>();
        }

        ///
        ///@return List<Purchases>
        ///
        public List<Purchases> ReadPurchasesFromFile(string FileName)
        {
            try
            {
                Console.WriteLine("Init method::ReadPurchasesFromFile");
                List<Purchases> purchasesList = new List<Purchases>();
                if (string.IsNullOrEmpty(FileName))
                    return purchasesList;
                string pathPurchase = Path.Combine(Environment.CurrentDirectory, @"Data\", FileName);
                List<string> purchases = new List<string>();
                purchases = new List<string>(File.ReadAllLines(pathPurchase));
                List<Purchases> purchaseList = new List<Purchases>();
                Purchases _purchase = new Purchases();
                List<string> PurchaseItems = new List<string>();
                bool isFirstPurchase = true;
                ///
                ///@return content of Purchases file
                ///
                foreach (var purchase in purchases)
                {
                    if (purchase.Contains("CUST"))
                    {
                        if (isFirstPurchase)
                        {
                            isFirstPurchase = false;
                        }
                        else
                        {
                            purchaseList.Add(_purchase);
                        }

                        _purchase = new Purchases() { Customer = purchase.Replace("CUST", "") };
                    }
                    if (purchase.Contains("DATE"))
                    {
                        var date = purchase.Replace("DATE", "");
                        _purchase.Date = DateTime.ParseExact(date, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                        _purchase.Year = _purchase.Date.Date.Year;
                        _purchase.Month = _purchase.Date.Date.Month;
                    }
                    if (purchase.Contains("ITEM"))
                    {
                        var item = purchase.Replace("ITEM", "");

                        _purchase.Items.Add(item);

                    }
                }
                purchaseList = purchaseList.OrderBy(i => i.Customer).ThenBy(i => i.Date).ToList();
                for (int i = 0; i < purchaseList.Count(); i++)
                {
                    for (int j = i + 1; j < purchaseList.Count(); j++)
                    {
                        if (purchaseList[i].Customer == purchaseList[j].Customer &&
                           purchaseList[i].Month == purchaseList[j].Month &&
                           purchaseList[i].Year == purchaseList[j].Year)
                        {
                            purchaseList[i].Items.AddRange(purchaseList[j].Items);
                            purchaseList.Remove(purchaseList[j]);
                            j = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                Console.WriteLine("End method::ReadPurchasesFromFile");
                return purchaseList;
            }
            catch (Exception e)
            {
                Console.WriteLine("method::ReadPurchasesFromFile Exception: {0}", e.Message);
                return new List<Purchases>();
            }
        }
    }

    
}
