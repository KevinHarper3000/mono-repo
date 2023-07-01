using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace Api;

public static class Config
{
    public static IEnumerable<ApiResource> Apis =>
        new[]
        {
            new ApiResource(
                "imageGalleryApi",
                "Image Gallery API",
                new[] {"subscriptionLevel"})
            {
                ApiSecrets = {new Secret("apiSecret".Sha256())}
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
            new ApiScope("api1", "MyAPI")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // machine to machine client
            new()
            {
                ClientId = "client",
                ClientSecrets = {new Secret("secret".Sha256())},

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // scopes that client has access to
                AllowedScopes = {"api1"}
            },

            // interactive ASP.NET Core Web App
            new()
            {
                ClientId = "web",
                ClientSecrets = {new Secret("secret".Sha256())},

                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect to after login
                RedirectUris = {"https://localhost:5002/signin-oidc"},

                // where to redirect to after logout
                PostLogoutRedirectUris = {"https://localhost:5002/signout-callback-oidc"},

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "verification"
                }
            },

            new()
            {
                AccessTokenLifetime = 1200,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                ClientName = "Image Gallery",
                ClientId = "imageGalleryClient",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireConsent = false,
                RedirectUris = new List<string>
                {
                    "https://localhost:44389/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    "https://localhost:44389/signout-callback-oidc"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Address,
                    "imageGalleryApi",
                    "country",
                    "subscriptionLevel"
                },
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                }
            }
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(),
            new(
                "country",
                "The country you're living in",
                new List<string> {"country"}),
            new(
                "subscriptionLevel",
                "Your subscription level",
                new List<string> {"subscriptionLevel"}),

            new()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            }
        };
}