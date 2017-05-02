using System;

namespace TopNExpensiveMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            //////////////////////////////
            //Hadoop passes data to the mapper on STDIN
            string line;
            //Hadoop passes data to the mapper on STDIN
            while ((line = Console.ReadLine()) != null)
            {
                var data = line.Split(new string[] { "|||" }, StringSplitOptions.None);
                var productId = data[0];
                var title = data[1];
                var price = data[2];
                var category = data[6];
                //stream productId, price, title and category to the reducer for processing
                Console.WriteLine($"{productId}\t{price}\t{title}\t{category}");
            }
        }
    }
}
