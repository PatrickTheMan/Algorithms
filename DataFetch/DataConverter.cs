//using B2S_API_Comm.Domain;
//using Models.Handlers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataConverter.DataFetch {
//    internal class DataConverter {

//        public DataConverter() { }

//        public static async void Run() {
//            Console.WriteLine($"");

//            Console.WriteLine($"Connecting to database");
//            UnicontaHandler handler = new(true);

//            UnicontaItem[] data = await handler.GetData();
//            List<Product> result = new();

//            Console.WriteLine($"The data is {data.Length} entries.");
//            foreach (UnicontaItem item in data) {
//                bool updated = false;

//                if (item._EAN != null) {
//                    if (item._EAN.Contains("4034106029500")) {

//                        Console.WriteLine("here");
//                    }
//                }

//                if (item._BrandGroup is "Ab" or "SchneiderE" or "Schneider" or "Siemens") {
//                    Console.WriteLine($"Weight: {item.ProductWeight};\tHeight: {item.ProductHeight};\tWidth: {item.ProductWidth}");
//                    Product tmp = new() {
//                        PrdProductNumber = item.ProductNumber,
//                    };
//                    if (item.ProductWeight != 0) {
//                    }

//                    if (updated)
//                        result.Add(tmp);
//                }
//            }
//            Console.WriteLine($"the Results are {result.Count} entries.");

//            Console.WriteLine($"Creating HTTP Client");
//            B2SHttpClientHandler http = new("https://192.168.200.37:44390/");

//            int i;
//            DateTime start = DateTime.Now;
//            for (i = 0; i < result.Count; i++) {
//                if (i % 100 == 0) {
//                    Console.WriteLine(
//                        $"====================================" +
//                        $":Posted {i} entries:" +
//                        $"===================================="
//                        );
//                }

//                Console.WriteLine($"Awaiting BrdName for {data[i]._BrandGroup}");
//                var BrandId = (await http.GetBrandBasedAliasAsync(data[i]._BrandGroup));

//                //Console.WriteLine($"Got BrdId: {BrandId}");

//                Console.WriteLine($"Attempting to create product with Product number: {result[i].PrdId} and " +
//                    $"BrdId: {BrandId}");

//                var post = new Product() {
//                    PrdProductNumber = result[i].PrdId,
//                    BrdId = BrandId,
//                    PrdEanGlr = result[i].EAN
//                };




//                Console.WriteLine(
//                    $"Created product with:" +
//                    $"{post.PrdProductNumber}" +
//                    $"{post.BrdId}"
//                    );
//                await http.PutProductAsync(post);

//            }
//            DateTime end = DateTime.Now;
//            Console.WriteLine($"Done posting results in {(end - start).TotalSeconds} seconds");

//            Console.WriteLine($"Posted {i} results");

//        }


//    }
//}
