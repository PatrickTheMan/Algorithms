using static LoggingTool.Logger;
using Algorithms;
using Models.Handlers;
using B2S_API_Comm.Domain;

//Init();
Log("Something");

Run();
Thread.Sleep(10000);

static async void Run() {

    B2SHttpClientHandler http = new("https://192.168.200.37:44390/");
    PostVerification pv = new(http);
    //ostVerification pv = new();

    Product p = new() {
        //PrdEanGlr = "4025515152477",
        PrdProductNumber = "6ES7 360-3AA01-0AA0",
    };
    var r = await pv.Verify(p);
    Log("Final Outcome: " + r.Status.StatusCode);

}