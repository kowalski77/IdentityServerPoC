using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WeatherForecast.API.Identity;

public static class IdentityExtensions
{
    public static AuthenticationBuilder AddJwtBearerAuthentication(this IServiceCollection services) =>
        services
            .ConfigureOptions<ConfigureJwtBearerOptions>()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
}
