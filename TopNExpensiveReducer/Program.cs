using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TopNExpensiveReducer
{
    class Program
    {
        //static List<string[]> reviews = new List<string[]>();
        static Dictionary<string, List<string>> prodId_ReviewText = new Dictionary<string, List<string>>();
        static void Main(string[] args)
        {
            if (!prodId_ReviewText.Any())
                ReadReviews();

            int N = 15;
            if (args.Length > 0 && !int.TryParse(args[0], out N))
                N = 15; //if unable to process arg default N to 15

            //<<category, Product_Id,Title >, rev_count >
            List<KeyValuePair<string, double>> prodDetail_Price = new List<KeyValuePair<string, double>>();

            var expenseTerms = new List<string> { "expensive", "costly", "high-priced", "pricey", "overpriced", "at a premium" };

            string data;
            while ((data = Console.ReadLine()) != null)
            {
                // Data from Hadoop is tab-delimited key/value pairs
                var sArr = data.Split('\t');
                // Get the prodId
                string prodId = sArr[0];
                // Get the Price
                double price = Convert.ToDouble(sArr[1]);
                // Get the Title
                string title = sArr[2];
                // Get the Category
                string category = sArr[3];
                // only print over priced
                if (prodId_ReviewText.ContainsKey(prodId))
                {
                    var reviews = prodId_ReviewText[prodId];
                    foreach (var review in reviews)
                    {
                        if (review.Contains(expenseTerms.ElementAt(4))) //element at 4 is overpriced. As we need only top N=15 over priced products
                        {
                            //<<category, Product_Id,Title >, rev_count >
                            var key = $"<<{category}, {prodId}, {title}>,{reviews.Count()}>";
                            prodDetail_Price.Add(new KeyValuePair<string, double>(key, price));
                        }
                    }
                }
            }

            var orderedDetails = prodDetail_Price.OrderByDescending(x => x.Value).ToList();
            for (int i = 0; i < orderedDetails.Count(); i++)
            {
                if (i == N) return;
                Console.WriteLine($"{orderedDetails[i].Key}");

            }
        }

        private static void ReadReviews()
        {
            using (var fs = File.OpenRead(@"reviews.csv"))
            using (var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(new string[] { "|||" }, StringSplitOptions.None);

                    var prodId = values[1];
                    var reviewText = values[5];

                    if (prodId_ReviewText.ContainsKey(prodId))
                    {
                        prodId_ReviewText[prodId].Add(reviewText);
                    }
                    else
                    {
                        prodId_ReviewText.Add(prodId, new List<string> { reviewText });
                    }
                }
            }
        }
    }
}
