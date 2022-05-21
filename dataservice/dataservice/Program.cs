using Application;
using Application.Common.Models;
using dataservice.Producers;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Producer.RabbitMQ;
using System.Net;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);

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
if (app.Environment.IsDevelopment())
{
    // migrate any database changes on startup (includes initial db creation)
    using (var scope = app.Services.CreateScope())
    {
        var patientContext = scope.ServiceProvider.GetRequiredService<PatientDbContext>();
        var medicineContext = scope.ServiceProvider.GetRequiredService<MedicineDbContext>();
        patientContext.Database.Migrate();
        medicineContext.Database.Migrate();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
