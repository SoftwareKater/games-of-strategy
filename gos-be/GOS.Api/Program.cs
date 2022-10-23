using GOS.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSingleton<TournamentService, TournamentService>();

services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseFileServer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
