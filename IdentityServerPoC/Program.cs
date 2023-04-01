using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using IdentityServerPoC.Entities;
using IdentityServerPoC.Settings;
using IdentityServerPoC.Support;
using Microsoft.AspNetCore.MiddlewareAnalysis;
using System.Diagnostics;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.Insert(0, ServiceDescriptor.Transient<IStartupFilter, AnalysisStartupFilter>());

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
    (
        connectionString: "mongodb://localhost:27017",
        databaseName: "IdentityDb"
    );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IdentityServerSettings identityServerSettings = builder.Configuration.GetSection(nameof(IdentityServerSettings)).Get<IdentityServerSettings>()!;

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseErrorEvents = true;
})
    .AddAspNetIdentity<ApplicationUser>()
    .AddInMemoryApiScopes(identityServerSettings.ApiScopes)
    .AddInMemoryApiResources(identityServerSettings.ApiResources)
    .AddInMemoryClients(identityServerSettings.Clients)
    .AddInMemoryIdentityResources(identityServerSettings.IdentityResources)
    .AddDeveloperSigningCredential();

builder.Services.AddLocalApiAuthentication();

WebApplication app = builder.Build();

DiagnosticListener listener = app.Services.GetRequiredService<DiagnosticListener>();
// Create an instance of the AnalysisDiagnosticAdapter using the IServiceProvider
// so that the ILogger is injected from DI
AnalysisDiagnosticAdapter observer = ActivatorUtilities.CreateInstance<AnalysisDiagnosticAdapter>(app.Services);
// Subscribe to the listener with the SubscribeWithAdapter() extension method
using var disposable = listener.SubscribeWithAdapter(observer);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

await app.SeedUsersAndRoles().ConfigureAwait(false);
await app.RunAsync().ConfigureAwait(false);
