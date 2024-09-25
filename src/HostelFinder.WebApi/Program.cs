using HostelFinder.Infrastructure.Common;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

HostelFinder.Application.ServiceExtentions.ConfigureServices(builder.Services, builder.Configuration);
HostelFinder.Application.ServiceExtentions.ConfigureServices(builder.Services, builder.Configuration);
HostelFinder.Application.ServiceExtentions.ConfigureServices(builder.Services, builder.Configuration);
HostelFinder.Application.ServiceExtentions.ConfigureServices(builder.Services, builder.Configuration);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


//builder.Services.AddSwaggerGen(options =>
//{
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    options.IncludeXmlComments(xmlPath);
//    var securitySchema = new OpenApiSecurityScheme
//    {
//        Name = "JWT Authentication",
//        Description = "Enter JWT Bearer",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = "bearer",
//        Reference = new OpenApiReference
//        {
//            Type = ReferenceType.SecurityScheme,
//            Id = "Bearer"
//        }
//    };
//    options.AddSecurityDefinition("Bearer", securitySchema);
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                { securitySchema, new[] { "Bearer" } }
//            });
//});


HostelFinder.Application.ServiceExtensions.ConfigureServices(builder.Services, builder.Configuration);
HostelFinder.Infrastructure.ServiceRegistration.Configure(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();