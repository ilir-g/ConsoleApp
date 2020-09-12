using CsvHelper;
using IlirGashi_ConsoleApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace IlirGashi_ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {


                ///
                ///@get List<Payments>
                ///
                Payments payments = new Payments();
                string FileNamePayments = "Payments.json";
                List<Payments> paymentsList = payments.ReadPaymentsFromFile(FileNamePayments);

                ///
                ///@get List<Prices>
                ///
                Prices prices = new Prices();
                string FileNamePrices = "Prices.xml";
                List<Prices> pricesList = prices.ReadPricesFromFile(FileNamePrices);

                ///
                ///@get List<Purchases>
                ///
                Purchases purchases = new Purchases();
                string FileNamePurchases = "Purchases.dat";
                List<Purchases> purchaseList = purchases.ReadPurchasesFromFile(FileNamePurchases);

                ///
                ///@get List<PaymentsNotMatched>
                ///
                PaymentsNotMatched paymentsNotMatched = new PaymentsNotMatched();
                List<PaymentsNotMatched> paymentsNotMatchedList = paymentsNotMatched.ReadPaymentsNotMatchedList(paymentsList,purchaseList,pricesList);


                ///
                ///@write List<PaymentsNotMatched>
                ///
                paymentsNotMatched.WritePaymentsToFileJson(paymentsNotMatchedList);
                paymentsNotMatched.WritePaymentsToFileCsv(paymentsNotMatchedList);

               
                Console.WriteLine("Press one key to exit");
                Console.ReadKey();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex:{0}", ex.Message);
            }
        }
    }
}
