using Moments.Model;

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
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var app = builder.Build();
// using(IServiceScope serviceScope = app.Services.CreateScope())
// {
//     var mysql = serviceScope.ServiceProvider.GetRequiredService<IFreeSql>();
//     mysql.CodeFirst.SyncStructure(typeof(Friend));
//     mysql.CodeFirst.SyncStructure(typeof(Article));
//     mysql.CodeFirst.SyncStructure(typeof(GatherLog));
//
// }
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();