using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlirGashi_ConsoleApp.Models
{
    public class Payments
    {
        public string Customer { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Amount { get; set; }

        public Payments() { }

        ///
        ///@return List<Payments>
        ///
        public List<Payments> ReadPaymentsFromFile(string FileName)
        {
            try
            {
                Console.WriteLine("Init method::ReadPaymentsFromFile");
                List<Payments> paymentsList = new List<Payments>();
                if (string.IsNullOrEmpty(FileName))
                    return paymentsList;
                string pathPayment = Path.Combine(Environment.CurrentDirectory, @"Data\", FileName);
                using (StreamReader sr = new StreamReader(pathPayment))
                {
                    string json = sr.ReadToEnd();
                    paymentsList = JsonConvert.DeserializeObject<List<Payments>>(json);
                }
                Console.WriteLine("End method::ReadPaymentsFromFile");
                return paymentsList;
            }
            catch (Exception e)
            {
                Console.WriteLine("method::ReadPaymentsFromFile Exception: {0}", e.Message);
                return new List<Payments>();
            }
        }
    }
}
