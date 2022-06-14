using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MRA.Identity
{
    public class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("MRA.API", "MRA API")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("MRA.API", "MRA API", new[] { JwtClaimTypes.Name })
                {
                    Scopes = {"MRA.API"}
                }
            };

        public static IEnumerable<Client> clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "MRA-API",
                    ClientName = "MRA API",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RedirectUris =
                    {
                        "http://.../signin-oidc"    //TODO redirect after authentificate client
                    },
                    AllowedCorsOrigins =
                    {
                        "http://..." //TODO
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://.../signout-oidc"   //TODO redirect after logout client
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MRA.API"
                    },
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}
