using Microsoft.AspNetCore.Diagnostics;
using Producer.RabbitMQ;
using web.Models;
using web.Producers;
using web.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IMessageProducer, StoreError>();

var app = builder.Build();

app.UseExceptionHandler(
    options =>
    {
        options.Run(
            async context =>
            {
                var ex = context.Features.Get<IExceptionHandlerFeature>();

                var service = context.RequestServices.GetService<IMessageProducer>();

                if (ex != null && service != null)
                {
                    service.SendMessage(new ErrorModel() { ErrorMessage = ex.Error.Message });
                }
            });
    }
);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notifications");
});

app.Run();
