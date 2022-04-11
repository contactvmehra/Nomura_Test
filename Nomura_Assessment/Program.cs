using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace Nomura_Assessment
{
    class Program
    {
        static HttpClient client = new HttpClient();
        
        
        static void Main(string[] args)
        {
            var path = $"https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/aud.json";
            var result = GetProductAsync(path);
            var myLinkedList = new LinkedList<ExchangeRate>();
            foreach (var elm in result.Result.Aud)
            {
                var exchangeRate = new ExchangeRate();
                exchangeRate.Currency = elm.Key;
                exchangeRate.Rate = elm.Value;
                myLinkedList.AddLast(exchangeRate);

            }
            for (int i = 0; i< myLinkedList.Count; i++)
            {
                var swapped = false;
                for (int j = 0; j< myLinkedList.Count -1-i; j++)
                {
                    if (myLinkedList.ElementAt(j).Rate > myLinkedList.ElementAt(j+1).Rate)
                    {
                        var tempRate = myLinkedList.ElementAt(j).Rate;
                        var tempCurr = myLinkedList.ElementAt(j).Currency;
                        myLinkedList.ElementAt(j).Rate = myLinkedList.ElementAt(j+1).Rate;
                        myLinkedList.ElementAt(j).Currency = myLinkedList.ElementAt(j+1).Currency;
                        myLinkedList.ElementAt(j+1).Rate = tempRate;
                        myLinkedList.ElementAt(j+1).Currency = tempCurr;
                        swapped = true;

                    }
                    else if ((myLinkedList.ElementAt(j).Rate == myLinkedList.ElementAt(j+1).Rate)
                             && string.Compare(myLinkedList.ElementAt(j).Currency, myLinkedList.ElementAt(j+1).Currency) == 1)
                    {
                        var tempRate = myLinkedList.ElementAt(j).Rate;
                        var tempCurr = myLinkedList.ElementAt(j).Currency;
                        myLinkedList.ElementAt(j).Rate = myLinkedList.ElementAt(j+1).Rate;
                        myLinkedList.ElementAt(j).Currency = myLinkedList.ElementAt(j+1).Currency;
                        myLinkedList.ElementAt(j+1).Rate = tempRate;
                        myLinkedList.ElementAt(j+1).Currency = tempCurr;
                        swapped = true;

                    }


                }

                if (!swapped)
                {
                    break;
                }
            }

            foreach (var element in myLinkedList)
            {

                Console.WriteLine(element.Currency + "  " + element.Rate);
            }



        }
        public static async Task<ExchangeData> GetProductAsync(string path)
        {
            ExchangeData result = null;
            var formatters = new List<MediaTypeFormatter>() {
                                new JsonMediaTypeFormatter(),
                                new XmlMediaTypeFormatter()
                            };
            HttpResponseMessage response = client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {

                 result = response.Content.ReadAsAsync<ExchangeData>().Result;
           
                Console.WriteLine(result);

          
            }

            return result;
        }
    }
    public class ExchangeRate
    {
        public string Currency { get; set; }
        public double Rate { get; set; }
    }

    public class ExchangeData
    {
        public DateTime Date { get; set; }
        public Dictionary<string, double> Aud { get; set; }

    }
}
