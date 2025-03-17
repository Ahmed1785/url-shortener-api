using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using UrlShortenerAPI.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = "my-cosmos-keyvault";
var secretName = "CosmosConnectionString";
var kvUri = $"https://{keyVaultName}.vault.azure.net";

// Create the KeyVault client
var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

// Retrieve the secret asynchronously
KeyVaultSecret secret;
try
{
    secret = await client.GetSecretAsync(secretName);
}
catch (Exception ex)
{
    Console.WriteLine($"Error retrieving secret: {ex.Message}");
    throw;
}

var cosmosConnectionString = secret.Value;

builder.Services.AddSingleton<CosmosDbService>(sp =>
    new CosmosDbService(cosmosConnectionString,
        "url-shortener-cosmos-db",
        "UrlsContainer"));

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
