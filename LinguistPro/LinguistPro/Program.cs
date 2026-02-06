using LinguistPro.Models;
using LinguistPro.Services;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=linguist.db"));

builder.Services.AddHttpClient<DictionaryService>();
builder.Services.AddHttpClient<WiktionaryVerbService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
