using SurfPicksBack;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot/";
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<SurfMapService>();
builder.Services.AddSingleton<LobbyService>();

var app = builder.Build();

app.UseCors("EnableCORS");

app.UseRouting();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
});
app.Run();
