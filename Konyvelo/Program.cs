using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Konyvelo.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.AddBlazorise();
builder.Services.AddBootstrapProviders().AddFontAwesomeIcons();

var app = builder.Build();

app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
