using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using StreamNote.Database.Commons.Database;
using StreamNote.Database.Commons.Database.Entities;
using StreamNote.GraphQL.Services.Implementations;
using StreamNote.GraphQL.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddScoped<IFirebaseAdminService, FirebaseAdminService>();

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.AddGraphQL().AddTypes();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IServiceProvider>();
var logger = provider.GetRequiredService<ILogger<Program>>();
var firebaseApp = FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("D:\\Projects\\dotnet\\TradingStrategy\\shattaspotify-firebase-adminsdk-dlh3l-00fa7a814b.json")
});

logger.LogInformation("Firebase Admin SDK initialized successfully. {Name}",firebaseApp.Name);

app.MapGraphQL();

app.RunWithGraphQLCommands(args);
