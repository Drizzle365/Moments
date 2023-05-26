using Microsoft.AspNetCore.Components.Authorization;
using Moments.Service;

var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "moments.db");
if (!File.Exists(dbPath))
{
    File.Create(dbPath).Close();
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

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddControllers();
builder.Services.AddSingleton(SqlFactory);
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<GatherService>();
builder.Services.AddSingleton<FriendService>();
builder.Services.AddSingleton<ArticleService>();
builder.Services.AddSingleton<TimedTasksService>();
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(option =>
    option.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddSingleton<StartService>();
var app = builder.Build();
using (IServiceScope serviceScope = app.Services.CreateScope())
{
    serviceScope.ServiceProvider.GetRequiredService<StartService>();
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
app.MapControllers();
app.UseCors("AllowAll");
app.Run();