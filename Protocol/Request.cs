using B2S_API_Comm.Domain;

namespace Algorithms.Protocol {
    public partial class Request {

        public Status Status { get; set; } = new();

        public List<Product>? Payload { get; set; } = new();

        public Request(Status Status, List<Product>? Payload) {

            this.Status = Status;
            this.Payload = Payload;

        }
    }

}
