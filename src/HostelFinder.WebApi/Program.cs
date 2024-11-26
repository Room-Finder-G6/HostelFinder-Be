using Amazon.S3;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Seeders;
using HostelFinder.WebApi.Extensions;
using HostelFinder.WebApi.Middlewares;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentation();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<PayOS>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    string clientId = configuration["PayOS:ClientId"] ?? throw new InvalidOperationException("PayOS ClientId is not configured.");
    string apiKey = configuration["PayOS:ApiKey"] ?? throw new InvalidOperationException("PayOS ApiKey is not configured.");
    string checksumKey = configuration["PayOS:ChecksumKey"] ?? throw new InvalidOperationException("PayOS ChecksumKey is not configured.");
    string? partnerCode = configuration["PayOS:PartnerCode"]; // Optional

    return new PayOS(clientId, apiKey, checksumKey, partnerCode);
});

async Task RegisterWebhook(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var payOS = scope.ServiceProvider.GetRequiredService<PayOS>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    string webhookUrl = configuration["PayOS:WebhookUrl"];
    if (string.IsNullOrEmpty(webhookUrl))
    {
        Console.WriteLine("Webhook URL is not configured in appsettings.json");
        return;
    }

    try
    {
        await payOS.confirmWebhook("https://tung.akaking.cloud/api/Webhook/HandlePaymentWebhook");
        Console.WriteLine($"Webhook registered successfully: {webhookUrl}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to register webhook: {ex.Message}");
    }
}

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
                .WithOrigins("http://hostel-fe-alb-2090926088.us-east-1.elb.amazonaws.com")
                .WithOrigins("https://tung.akaking.cloud")
                .WithOrigins("http://localhost:5131")
                .WithOrigins("https://27cd-118-71-222-200.ngrok-free.app")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

HostelFinder.Application.ServiceExtensions.ConfigureServices(builder.Services, builder.Configuration);
HostelFinder.Infrastructure.ServiceRegistration.Configure(builder.Services, builder.Configuration);

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IHostelSeeder>();
await seeder.Seed();

app.UseCors("AllowAllOrigins");
app.UseMiddleware<TokenValidationMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeLoggingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
