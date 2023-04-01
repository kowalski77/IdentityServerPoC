﻿using IdentityServer4.Models;

namespace IdentityServerPoC.Settings;

public class IdentityServerSettings
{
    public IReadOnlyCollection<ApiScope>? ApiScopes { get; init; }

    public IReadOnlyCollection<ApiResource>? ApiResources { get; init; }

    public IReadOnlyCollection<Client>? Clients { get; init; }

    public IReadOnlyCollection<IdentityResource> IdentityResources { get; } = new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
        new IdentityResource("roles", new []{"role" })
    };
}
