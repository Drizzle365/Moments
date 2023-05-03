using Microsoft.AspNetCore.Components.Authorization;
using Moments.Model;
using Moments.Service;

var builder = WebApplication.CreateBuilder(args);
var install = false;

var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "moments.db");
if (!File.Exists(path))
{
    Console.WriteLine("检测到首次运行");
    Console.WriteLine("数据文件初始化");
    install = true;
    if (!File.Exists(path))
    {
        File.Create(path).Close();
        Console.WriteLine("数据文件成功");
    }
}

IFreeSql SqlFactory(IServiceProvider r)
{
    IFreeSql mysql = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=moments.db")
        // .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))
        // .UseAutoSyncStructure(true) 
        .Build();
    return mysql;
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Policy",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .WithOrigins("https://dearain.cn");
        });
});
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
    if (install)
    {
        Console.WriteLine("开始迁移数据库");
        var mysql = serviceScope.ServiceProvider.GetRequiredService<IFreeSql>();
        mysql.CodeFirst.SyncStructure(typeof(Friend));
        mysql.CodeFirst.SyncStructure(typeof(Article));
        mysql.CodeFirst.SyncStructure(typeof(GatherLog));
        Console.WriteLine("数据库结构迁移完成");
    }

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

app.MapGet("/api/friends", () =>
{
    using IServiceScope serviceScope = app.Services.CreateScope();

    var db = serviceScope.ServiceProvider.GetRequiredService<IFreeSql>();
    var temp = db.Select<Friend>()
        .ToList();
    var random = new Random();
    var friends = new List<Friend>();
    foreach (var item in temp)
    {
        friends.Insert(random.Next(friends.Count), item);
    }

    return friends;
});
app.UseCors("Policy");

app.Run();