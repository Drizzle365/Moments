using Microsoft.AspNetCore.Components.Authorization;
using Moments.Model;
using Moments.Service;

var builder = WebApplication.CreateBuilder(args);

IFreeSql SqlFactory(IServiceProvider r)
{
    IFreeSql mysql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=moments.db")
        // .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))
        // .UseAutoSyncStructure(true) 
        .Build();
    return mysql;
}

builder.Services.AddSingleton(SqlFactory);
builder.Services.AddSingleton<Core>();
builder.Services.AddSingleton<TimedTasks>();
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(option =>
    option.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var app = builder.Build();
using (IServiceScope serviceScope = app.Services.CreateScope())
{
    // var mysql = serviceScope.ServiceProvider.GetRequiredService<IFreeSql>();
    // mysql.CodeFirst.SyncStructure(typeof(Friend));
    // mysql.CodeFirst.SyncStructure(typeof(Article));
    // mysql.CodeFirst.SyncStructure(typeof(GatherLog));
    var time = serviceScope.ServiceProvider.GetRequiredService<TimedTasks>();
    var config = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
    time.Start(double.Parse(config["Interval"]!));
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();