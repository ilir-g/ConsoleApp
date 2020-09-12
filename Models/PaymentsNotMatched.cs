using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlirGashi_ConsoleApp.Models
{
    public class PaymentsNotMatched
    {
        public string Customer { get; set; }
        public double AmountDue { get; set; }
        public double AmountPayed { get; set; }
        public double DifferenceAmount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
      
        public PaymentsNotMatched() { }
        public PaymentsNotMatched(string _Customer, double _AmountDue,double _AmountPayed,double _DifferenceAmount, int _Year, int _Month)
        {
            Customer = _Customer;
            AmountDue = _AmountDue;
            AmountPayed = _AmountPayed;
            DifferenceAmount = _DifferenceAmount;
            Year = _Year;
            Month = _Month;
           
        }

        ///
        ///@return List<PaymentsNotMatched>
        ///
        public List<PaymentsNotMatched> ReadPaymentsNotMatchedList(List<Payments> paymentsList, List<Purchases> purchaseList, List<Prices> pricesList)
        {
            try
            {
                Console.WriteLine("Init method::ReadPaymentsNotMatchedList");
                List<PaymentsNotMatched> paymentsNotMatchedList = new List<PaymentsNotMatched>();
                var listCustPayments = (from payment in paymentsList
                                        join purchase in purchaseList
                                        on new { payment.Customer, payment.Year, payment.Month }
                                        equals new { purchase.Customer, purchase.Year, purchase.Month }
                                        select new { Customer = payment.Customer, AmountDue = payment.Amount, payment.Month, payment.Year, purchase.Items }
                            ).ToList();

                List<PaymentsNotMatched> paymentsNotMatched = new List<PaymentsNotMatched>();
                foreach (var item in listCustPayments)
                {
                    double AmountPayed = 0, AmountDiff = 0;
                    foreach (var i in item.Items)
                    {
                        double priceItem = pricesList.Where(p => p.Item == i).Select(p => p.Price).Single();
                        AmountPayed += priceItem;
                    }
                    AmountDiff = item.AmountDue - Math.Round(AmountPayed, 2);
                    if (AmountDiff != 0)
                    {
                        paymentsNotMatched.Add(new PaymentsNotMatched(item.Customer, item.AmountDue, Math.Round(AmountPayed, 2), Math.Round(AmountDiff, 2), item.Year, item.Month)); ;

                    }
                }
                paymentsNotMatched = paymentsNotMatched.OrderByDescending(i => i.DifferenceAmount).ToList();
                Console.WriteLine("End method::ReadPaymentsNotMatchedList");
                return paymentsNotMatched;
            }
            catch (Exception e)
            {
                Console.WriteLine("method::ReadPaymentsNotMatchedList Exception: {0}", e.Message);
                return new List<PaymentsNotMatched>();
            }
        }

        ///
        ///@return void
        ///
        public void WritePaymentsToFileJson(List<PaymentsNotMatched> paymentsNotMatched)
        {
            try
            {
                Console.WriteLine("Init method::WritePaymentsToFileJson");
                string PathpaymentsNotMatched = Path.Combine(Environment.CurrentDirectory, @"Data\", "PaymentsNotMatched.json");
                ///
                ///@write Data into new File PaymentsNotMatched.json
                ///
                if (File.Exists(PathpaymentsNotMatched))
                    File.Delete(PathpaymentsNotMatched);

                using (var tw = new StreamWriter(PathpaymentsNotMatched, true))
                {
                    tw.WriteLine(JsonConvert.SerializeObject(paymentsNotMatched).ToString());
                    tw.Close();
                }
                Console.WriteLine("End method::WritePaymentsToFileJson");
            }
            catch (Exception e)
            {
                Console.WriteLine("method::WritePaymentsToFileJson Exception: {0}", e.Message);
                return;
            }
        }
        
        ///
        ///@return void
        ///
        public void WritePaymentsToFileCsv(List<PaymentsNotMatched> paymentsNotMatched)
        {
            try
            {
                Console.WriteLine("Init method::WritePaymentsToFileCsv");
                string PathCsv = Path.Combine(Environment.CurrentDirectory, @"Data\", "PaymentsNotMatched.csv");
                if (File.Exists(PathCsv))
                    File.Delete(PathCsv);

                StringBuilder sbOutPut = new StringBuilder();
                sbOutPut.AppendLine(string.Join(",", new string[] { "Customer", "Year", "Month", "AmountDue", "AmountPayed", "DifferenceAmount" }));

                for (int i = 0; i < paymentsNotMatched.Count(); i++)
                    sbOutPut.AppendLine(string.Join(",", new string[] { paymentsNotMatched[i].Customer.ToString(), paymentsNotMatched[i].Year.ToString(), paymentsNotMatched[i].Month.ToString(), paymentsNotMatched[i].AmountDue.ToString(), paymentsNotMatched[i].AmountPayed.ToString(), paymentsNotMatched[i].DifferenceAmount.ToString() }));

                File.WriteAllText(PathCsv, sbOutPut.ToString());
                Console.WriteLine("End method::WritePaymentsToFileCsv");

            }
            catch (Exception e)
            {
                Console.WriteLine("method::WritePaymentsToFileCsv Exception: {0}", e.Message);
                return;
            }
        }
    }
}
