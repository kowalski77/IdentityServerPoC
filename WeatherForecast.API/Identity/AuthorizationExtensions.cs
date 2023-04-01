namespace WeatherForecast.API.Identity;

public static class AuthorizationExtensions
{
    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityConstants.ReadPolicy, policy =>
            {
                policy.RequireClaim("scope", "weathermanagement.readaccess", "weathermanagement.fullaccess");
            });
            options.AddPolicy(IdentityConstants.WritePolicy, policy =>
            {
                policy.RequireClaim("scope", "weathermanagement.writeaccess", "weathermanagement.fullaccess");
            });
        });
    }
}