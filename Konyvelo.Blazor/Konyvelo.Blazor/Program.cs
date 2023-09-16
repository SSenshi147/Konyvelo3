using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Konyvelo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.ConfigureDbContext();
builder.ConfigureRepos();
builder.ConfigureMediatr();
builder.ConfigureBlazorise();

var app = builder.Build();

app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
