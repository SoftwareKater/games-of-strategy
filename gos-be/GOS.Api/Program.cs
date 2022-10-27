using GOS.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSingleton<TournamentService, TournamentService>();

services.AddControllers();

// Register the NSwag Swagger services
services.AddOpenApiDocument();

var app = builder.Build();

// Register the NSwag Swagger generator and the Swagger UI middlewares
// Register the middleware before UseRouting()
app.UseOpenApi();
app.UseSwaggerUi3();

// app.UseFileServer();

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
