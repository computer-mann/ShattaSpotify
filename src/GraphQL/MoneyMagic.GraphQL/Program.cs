using MoneyMagic.GraphQL.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.AddGraphQL().AddTypes().AddInMemorySubscriptions();
builder.Services.AddHostedService<BinanceStreamBackgroundervice>();

var app = builder.Build();

app.UseWebSockets();

app.MapGraphQL();

app.RunWithGraphQLCommands(args);
