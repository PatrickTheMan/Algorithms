using Models.Handlers;
using static LoggingTool.Logger;
using B2S_API_Comm.Domain;
using System.Text.RegularExpressions;
using Algorithms.Protocol;

namespace Algorithms {
    public partial class PostVerification {

        //List of things to filter out:
        //Incomplete Data => Return likely complete data instead
        //Affirm Data Sctructures
        //RegEx's for product numbers
        //etc.

        readonly B2SHttpClientHandler handler;
        List<Product>? data;


        readonly Status OK = new(StatusCode.OK, string.Empty);

        /// <summary>
        /// PostVerification Algorithm; Is used for ensuring a product has not been inserted into the database before
        /// </summary>
        /// <param name="handler">a <see cref="B2SHttpClientHandler"/> instance which will call the API in order to check the data</param>
        public PostVerification(B2SHttpClientHandler handler) {
            this.handler = handler;
        }

        /// <summary>
        /// Verifies the given <see cref="Product"/> based on several conditions;
        /// <br/>
        /// Checks for duplicates, <br/>
        /// Checks for correct EAN syntax if EAN is provided<br/>
        /// </summary>
        /// <param name="p">Product to be verified</param>
        /// <returns>a <see cref="Request"/> with a <see cref="StatusCode"/> that informs of the outcome of the verification</returns>
        public async Task<Request> Verify(Product p) {

            data = await GetProducts(p);

            Request payload;
            Status st = new();
            List<Product>? prods = new();
            st.StatusCode = StatusCode.OK;

            try {
                //p null checks
                if (p is null) {
                    Log($"Cannot Filter NULL Value Product", Color.Failure);
                    st.StatusCode = StatusCode.Bad_Reqest;
                    st.StatusMessage = "Cannot Filter NULL Value Product";
                    prods = null;
                    throw new ArgumentNullException(nameof(p));
                }
                if (p.PrdProductNumber is null) {
                    Log($"Cannot Filter NULL Value ProductNumber", Color.Failure);
                    st.StatusCode = StatusCode.Bad_Reqest;
                    st.StatusMessage = "Provided data included a Null. Ensure ProductNumber has a value";
                    prods = null;
                    throw new ArgumentNullException(nameof(p));
                }


                //define tasks to check by and run them, saving the results to an array
                Log("Running HasDuplicate", Color.Info);
                Request r1 = await HasDuplicate(p);
                Log("Running HasProperEAN", Color.Info);
                Request r2 = await HasProperEAN(p);
                Log("Running HasIncompleteData", Color.Info);
                Request r3 = await HasIncompleteData(p);

                List<Request> _requests = new() {
                   r1,r2,r3
                };

                Log("Running ForEach Loop", Color.Info);
                foreach (var request in _requests) {
                    if (request.Status.StatusCode is not StatusCode.OK) {
                        st.StatusCode = request.Status.StatusCode;
                        st.StatusMessage = request.Status.StatusMessage;
                        prods = request.Payload;
                        Log($"{nameof(request)} Returned a bad status code:", Color.Warning);
                        Log(request.Status.StatusMessage, Color.Warning);
                        break;
                    }
                }
            }
            catch (Exception e) {
                st.StatusCode = StatusCode.Internal_Verification_Error;
                st.StatusMessage = e.Message;
                Log($"Data could not be verified, {e}", Color.Failure);
            }
            finally {
                payload = new(st, prods);
            }
            return payload;
        }

        // Calls the API and gets data based on provided product
        private async Task<List<Product>?> GetProducts(Product p) {

            IEnumerable<Product>? p1 = await handler.GetProductsAsync(B2S_API_Comm.Models.ProductGetOptions.ProductNumber, p.PrdProductNumber);
            IEnumerable<Product>? p2 = new List<Product>(); //instanced and then called because we allow the user to not provide an EAN, which would break the API if called in-line

            if (p.PrdEanGlr is not null) {
                p2 = await handler.GetProductsAsync(B2S_API_Comm.Models.ProductGetOptions.EAN, p.PrdEanGlr) ?? new List<Product>();
            }
            List<Product>? result = new();
            result.AddRange(p1);
            result.AddRange(p2);
            return result;

        }

        // Checks wether the data from the API contains a product with this one's exact data
        private Task<Request> HasDuplicate(Product p) {

            string reason;
            StatusCode status;

            List<Product>? results = new();

            foreach (Product prod in data) {
                if (prod is null || prod.PrdEanGlr is null)
                    continue;
                if (prod.PrdEanGlr.Equals(p.PrdEanGlr) ||
                            prod.PrdProductNumber.Equals(p.PrdProductNumber)) {
                    results.Add(prod);
                }
            }

            if (results.Count > 0) {
                Log("Returning StatusCode.Duplicate", Color.Info);
                status = StatusCode.Duplicate;
                reason = "Found Duplicate of provided product";
            } else {
                Log("Returning StatusCode.OK", Color.Info);
                status = StatusCode.OK;
                reason = string.Empty;
                results = null;
            }
            return Task.FromResult<Request>(new(new(status, reason), results));
        }

        [GeneratedRegex("/[0-9]/")] private static partial Regex OnlyNumbers();
        //checks the validity of this product's EAN
        private Task<Request> HasProperEAN(Product p) {

            string reason;
            StatusCode status;

            if (p.PrdEanGlr is null) {
                Log("Returning StatusCode.OK", Color.Info);
                reason = "Provided Product has no EAN.";
                status = StatusCode.OK;
            } else if (OnlyNumbers().IsMatch(p.PrdEanGlr)) {
                Log("Returning StatusCode.Bad_EAN", Color.Info);
                reason = "Not a number";
                status = StatusCode.Bad_EAN;
            } else if (p.PrdEanGlr.Length is not 8 or 13) {
                reason = $"The length of the product's EAN was {p.PrdEanGlr.Length} which is not a legal value for an EAN number";
                Log("Returning StatusCode.Bad_EAN", Color.Info);
                status = StatusCode.Bad_EAN;
            } else {
                reason = string.Empty;
                Log("Returning StatusCode.OK", Color.Info);
                status = StatusCode.OK;
            }
            return Task.FromResult<Request>(new(new(status, reason), null));
        }

        // Checks wether the data from the API contains a product with similar data to this product
        private Task<Request> HasIncompleteData(Product p) {

            string reason = string.Empty;
            StatusCode status;

            List<Product> results = new();

            foreach (Product prod in data) {
                if (prod is null)
                    continue;
                if (prod.PrdProductNumber.Contains(p.PrdProductNumber)) {
                    results.Add(prod);
                }
            }

            if (results.Count > 0) {
                Log("Returning StatusCode.Duplicate", Color.Info);
                status = StatusCode.Incomplete;
                reason = "Product may include incomplete data";
            } else {
                Log("Returning StatusCode.OK", Color.Info);
                status = StatusCode.OK;
            }

            return Task.FromResult<Request>(new(new(status, reason), results));

        }
    }
}
