using System.Text.RegularExpressions;
using B2S_API_Comm.Domain;
using Models.Handlers;

namespace Algorithms {

    internal partial class ProductFinder {

        List<string> KeyCandidates = new();

        private ProductRequest RequestBase = new();

        B2SHttpClientHandler http = new("https://192.168.200.37:44390/");

        private IEnumerable<Brand>? brands;

        [GeneratedRegex("/[0-9]/")] private static partial Regex NumbersRegex();

        /// <summary>
        /// ProductFinder Algorithm; Is used for processing an input into ProductRequests
        /// </summary>
        public ProductFinder() {
            Init();
        }

        //set up in the initializer because the constructor can't call async code
        private async void Init() {
            brands = await http.GetBrandsAsync();
        }


        /// <summary>
        /// Accepts an input of strings and figures out which of the strings are viable to base a http request on
        /// </summary>
        /// <param name="input"> a <see cref="List{T}"/> of <see cref="string"/>s or a single <see cref="string"/> input from the OCR </param>
        /// <returns> All <see cref="ProductRequest"/> candidates in the form of a <see cref="List{T}"/> </returns>
        /// <exception cref="ArgumentException"></exception>
        public List<ProductRequest> GetProductRequests(object input) {

            List<string> term;

            term = input switch {
                string s => s.Split(' ').ToList(),
                List<string> l => l,
                _ => throw new ArgumentException($"Invalid input. Must be string or List<string>. Received: {input.GetType()}")
            };

            foreach (string s in term) {
                if (s.Length < 4)
                    continue;
                if (IsBrand(s)) {
                    RequestBase.Brand = s;
                    continue;
                }
                if (IsEAN(s)) {
                    RequestBase.EAN = s;
                    KeyCandidates.Add(string.Empty);
                    continue;
                }
                KeyCandidates.Add(s);
            }

            List<ProductRequest> result = new();

            foreach (string k in KeyCandidates) {
                RequestBase.ProductNumber = k;
                result.Add(RequestBase);
            }

            return result;
        }

        private bool IsBrand(string term) {
            if (brands is null) { throw new ArgumentNullException("Brands"); }

            foreach (Brand brand in brands) {
                if (brand.BrdName == term)
                    return true;
            }
            return false;
        }

        private bool IsEAN(string term) {
            if (term.Length is not 8 or 13) {
                return false;
            }
            if (NumbersRegex().IsMatch(term)) {
                return true;
            }
            return false;
        }

    }
}