using MudBlazor.Services;
using CodingDemo.Components;
using CodingDemo.Interfaces.TwelveData;
using CodingDemo.Services.TwelveData;
using CodingDemo.DAL;
using Microsoft.EntityFrameworkCore;
using CodingDemo.Interfaces.DAL;
using CodingDemo.Services.DAL;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
 .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddHttpClient<ITwelveDataService, TwelveDataService>("TwelveData", HttpClient =>
{
    HttpClient.BaseAddress = new Uri("https://twelve-data1.p.rapidapi.com");
    HttpClient.DefaultRequestHeaders.Add("x-rapidapi-key", "ffda5d3ac7msh33bd4a4c7d83c33p1638e5jsn0641ecc0aab1");
    HttpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "twelve-data1.p.rapidapi.com");
});

builder.Services.AddTransient<ITwelveDataService, TwelveDataService>();

builder.Services.AddDbContext<LoggingDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Logging")), ServiceLifetime.Transient);

builder.Services.AddTransient<ILoggingService, LoggingService>();

var serilogColumnOptions = new ColumnOptions();
serilogColumnOptions.Id.DataType = System.Data.SqlDbType.BigInt;

Log.Logger = new LoggerConfiguration()
    .WriteTo
    .MSSqlServer(
        columnOptions: serilogColumnOptions,
        connectionString: builder.Configuration.GetConnectionString("Logging"),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents" })
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
