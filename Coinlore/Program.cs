using Dynamo_Coinlore.Clients;
using Dynamo_Coinlore.Services;
using Dynamo_Coinlore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICalculateService, CalculateService>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<ICoinLoreClient, CoinLoreClient>();

builder.Services.AddHttpClient("coinlore", (httpClient) =>
{
    httpClient.BaseAddress = new Uri(" https://api.coinlore.net");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
