using Application;
using Application.Common.Models;
using dataservice.Producers;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Producer.RabbitMQ;
using System.Net;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
const string SCHEME = "Bearer";

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            RequireExpirationTime = false,
            RoleClaimType = "auth_role"
        };
    }
 );

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 5 Web API",
        Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"
    });
    // To Enable authorization using Swagger (JWT)    
    swagger.CustomSchemaIds(x => x.FullName);
    swagger.AddSecurityDefinition(SCHEME, new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = SCHEME,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = SCHEME
                                },
                                Scheme = "oauth2",
                                Name = SCHEME,
                                In = ParameterLocation.Header,
                            },
                            new List<string>()

                    }
                });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
