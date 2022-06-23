using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using ReminderService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./firebasesecret.json"),
});

await host.RunAsync();
