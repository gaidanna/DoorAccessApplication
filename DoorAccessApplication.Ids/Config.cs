using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;
using System.Text.Json;

namespace DoorAccessApplication.Ids
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
          new[]
          {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
              Name = "role",
              UserClaims = new List<string> {"role"}
            }
          };

        public static IEnumerable<ApiScope> ApiScopes =>
          new[]
          {
            new ApiScope("lockAccess"),
          };
        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource()
            {
                Name = "lockAccess",
                Description = "My API",
                Scopes = new List<string> { "lockAccess" },
            }
    };

        public static IEnumerable<Client> Clients =>
          new[]
          {
              new Client
                {
                    ClientId = "swaggerui",
                    ClientName = "Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    RedirectUris = { $"https://localhost:7224/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"https://localhost:7224/swagger/" },

                    AllowedScopes =
                    {
                        "lockAccess"
                    }
                },
              new Client
                {
                    ClientId = "test",
                    ClientSecrets = new [] { new Secret("test".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "lockAccess" },
                    AlwaysIncludeUserClaimsInIdToken = true,
              }
          };
    }
}
