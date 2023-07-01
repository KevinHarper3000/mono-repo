using System.Security.Claims;
using Duende.IdentityServer.Test;

namespace Data;

public class TestUsers
{
    public static List<TestUser> Users = new()
    {
        new TestUser
        {
            SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
            Username = "Frank",
            Password = "password",

            Claims = new List<Claim>
            {
                new("given_name", "Frank"),
                new("family_name", "Underwood"),
                new("address", "Main Road 1"),
                new("subscriptionLevel", "FreeUser"),
                new("country", "nl")
            }
        },
        new TestUser
        {
            SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
            Username = "Claire",
            Password = "password",

            Claims = new List<Claim>
            {
                new("given_name", "Claire"),
                new("family_name", "Underwood"),
                new("address", "Big Street 2"),
                new("subscriptionLevel", "PayingUser"),
                new("country", "be")
            }
        }
    };
}